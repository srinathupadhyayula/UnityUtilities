using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Types;
using Object = UnityEngine.Object;

namespace Utilities.EditorWindow
{
    public class LayerManager : UnityEditor.EditorWindow
    {
        private const string k_instructions = @"Instructions:
- Rename and reorganise layers as desired using the reorderable list below.
- Applying the modifications will update the physics settings; change the layers on all game objects in all scenes and all prefabs; change any LayerMask serialized properties on game objects and scriptable objects.
- Once the manager has finished processing files, you will be able to save out a layer map which you can use to transform layer filters and masks from the old layout to the new layout.
";
        // State
        private ManagerState m_state       = ManagerState.Complete;
        private Vector2      m_editScroll  = Vector2.zero;
        private bool         m_skipRepaint;

        // Data (serialized object for undo & persistence)
        private LayerManagerData m_data;

        // Reorderable list for layer layout
        private ReorderableList m_layerList;

        // Processing data
        private string[] m_assetPaths;
        private int      m_currentAssetPath = -1;
        private int[]    m_indexSwaps;
        private int[]    m_indexSwapsRedirected;
        private string[] m_fixedLayers;

        // Reporting variables
        private int    m_sceneCount;
        private int    m_prefabCount;
        private int    m_objectCount;
        private int    m_componentCount;
        private int    m_assetCount;
        private int    m_layerMaskCount;
        private bool   m_physicsMatrixCompleted;
        private bool   m_physics2DMatrixCompleted;
        private string m_completionReport = string.Empty;

        // Physics layer collisions
        private uint[] m_physicsMasks;
        private uint[] m_physics2DMasks;

        // Error reporting
        private List<string> m_errors = new();

        // Layout helpers
        private static float LineHeight  => EditorGUIUtility.singleLineHeight;
        private static float LineSpacing => EditorGUIUtility.standardVerticalSpacing;

        private enum ManagerState
        {
            Editing,
            Confirmation,
            Processing,
            Complete
        }

        [MenuItem("Tools/Utilities/Layer Manager")]
        private static void ShowEditor()
        {
            // Get existing open window or if none, make a new one:
            var layerManager = (LayerManager)GetWindow(typeof(LayerManager));
            layerManager.ResetData();
            layerManager.Show();
        }

        public bool Dirty
        {
            get => m_data.Dirty;
            private set => m_data.Dirty = value;
        }

        public bool Valid => m_data.Valid;

        private void Initialise()
        {
            // Create & attach scriptable object (allows undos, etc)
            if (m_data == null)
            {
                m_data = CreateInstance<LayerManagerData>();
                m_data.Initialise();
            }

            // Get fixed layers
            m_fixedLayers = new string[8];
            for (var i = 0; i < 8; ++i)
            {
                m_fixedLayers[i] = LayerMask.LayerToName(i);
            }


            // Create reorderable list
            m_layerList = new ReorderableList(m_data.SerializedObject, m_data.LayerMapProperty,
                                              true, true, false, false)
                          {
                              drawHeaderCallback = DrawLayerMapHeader, drawElementCallback = DrawLayerMapElement,
                              elementHeight      = LineHeight * 2 + LineSpacing * 3
                             ,
                              onReorderCallback = OnLayerMapReorder
                          };

            // Reset state
            m_state = ManagerState.Editing;

            // Reset reporting
            m_sceneCount = 0;
            m_prefabCount = 0;
            m_objectCount = 0;
            m_componentCount = 0;
            m_assetCount = 0;
            m_layerMaskCount = 0;
            m_physicsMatrixCompleted = false;
            m_physics2DMatrixCompleted = false;
            m_errors.Clear();
            m_completionReport = string.Empty;
        }

        private void OnEnable()
        {
            // Set up window
            titleContent = EditorGUIUtility.IconContent("HorizontalSplit");
            titleContent.text = "Layer Manager";
            minSize = new Vector2(400, 320);
            Initialise();
            autoRepaintOnSceneChange = true;
            Undo.undoRedoPerformed += OnUndo;
#if UNITY_2018_1_OR_NEWER
            EditorApplication.quitting += OnQuit;
#endif
        }

        private void OnDestroy()
        {
            // Finish processing if closed mid-way through
            while (m_assetPaths != null)
                IncrementLayerModifications();
            Undo.undoRedoPerformed -= OnUndo;
        }

        private void OnUndo()
        {
            Repaint();
        }

        private void OnQuit()
        {
            Close();
        }

        private void OnGUI()
        {
            if (m_skipRepaint && Event.current.type == EventType.Repaint)
            {
                m_skipRepaint = false;
                return;
            }

            switch (m_state)
            {
                case ManagerState.Editing:
                    OnEditingGUI();
                    break;
                case ManagerState.Confirmation:
                    OnConfirmationGUI();
                    break;
                case ManagerState.Processing:
                    OnProcessingGUI();
                    break;
                case ManagerState.Complete:
                    OnCompleteGUI();
                    break;
            }
        }

        private void OnEditingGUI()
        {
            if (m_layerList == null || m_layerList.serializedProperty == null)
            {
                ResetData();
                if (Event.current.type == EventType.Layout)
                    m_skipRepaint = true;
                return;
            }

            // Use a scroll view to fit it all in
            m_editScroll = EditorGUILayout.BeginScrollView(m_editScroll);

            // Draw instructions
            EditorGUILayout.HelpBox(k_instructions, MessageType.Info);
            EditorGUILayout.Space();

            // Show reorganise list
            m_layerList.DoLayoutList();

            // Apply modifications button
            GUI.enabled = Dirty && Valid;
            if (GUILayout.Button("Apply Layer Modifications"))
                m_state = ManagerState.Confirmation;
            GUI.enabled = true;

            // Reset modifications
            if (GUILayout.Button("Reset Layer Modifications"))
                ResetData();
            EditorGUILayout.Space();

            // End the scroll view
            EditorGUILayout.EndScrollView();

            // Apply changes to data
            m_data.ApplyModifiedProperties();
        }

        private void OnConfirmationGUI()
        {
            // Show warning
            EditorGUILayout.HelpBox("Warning: This process is not reversible and modifies a lot of files.\n\nMake sure all scenes are saved (including the open scene) and you have an up to date backup in case anything gose wrong.", MessageType.Warning);

            // OK
            if (GUILayout.Button("Yes, I have a backup"))
                ApplyLayerModifications();

            // Cancel
            if (GUILayout.Button("No, I'm not ready yet"))
                m_state = ManagerState.Editing;
        }

        private void OnProcessingGUI()
        {
            // Show info
            EditorGUILayout.HelpBox("Processing layer modifications. Do not close this window until completed.", MessageType.Info);

            // Show progress bar
            float helpHeight = GUILayoutUtility.GetLastRect().height + 8;
            Rect r = position;
            r.y = helpHeight;
            r.height = EditorGUIUtility.singleLineHeight;
            r.x = 4;
            r.width -= 8;
            EditorGUI.ProgressBar(r, (float)m_currentAssetPath / (float)m_assetPaths.Length, "Progress");

            // Process
            if (Event.current.type == EventType.Repaint)
            {
                IncrementLayerModifications();
                Repaint();
            }
        }

        private void OnCompleteGUI()
        {
            // Show completion report
            EditorGUILayout.HelpBox(m_completionReport, MessageType.Info);

            EditorGUILayout.Space();

            if (GUILayout.Button("Close the Layer Manager"))
            {
                Close();
            }

            if (GUILayout.Button("Keep making changes"))
            {
                ResetData();
            }

            if (GUILayout.Button("Save Layer Map"))
            {
                CreateMap();
            }

            // Handle errors selection
            if (m_errors.Count == 0)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Handle Errors"))
            {
                var menu = new GenericMenu();

                // Use Debug.LogError
                menu.AddItem(new GUIContent("Output To Console"), false, () =>
                {
                    Debug.LogError(BuildErrorReport(false));
                });

                // Use mailto with support email
                menu.AddItem(new GUIContent("Email Support"), false, () =>
                {
                    Application.OpenURL("mailto:support@yondernauts.games?subject=Layer%20Manager&body=" + BuildErrorReport(true));
                });

                menu.ShowAsContext();
            }
            if (m_errors.Count == 0)
                GUI.enabled = true;
        }

        private string BuildErrorReport(bool url)
        {
            var result = new StringBuilder("Layer Manager failed with the following errors:");
            
            foreach (string err in m_errors)
            {
                result.Append(" - ").AppendLine(err);
            }

            if (url)
            {
                return Uri.EscapeDataString(result.ToString());
            }
            
            return result.ToString();
        }

        private void DrawLayerMapHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Modified Layer Map");
        }

        private void DrawLayerMapElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            LayerManagerData.SerializedLayerMapEntry entry = m_data.GetEntryFromIndex(index);

            rect.height = LineHeight;
            rect.y += LineSpacing;

            // Draw top label (modified & new index)
            EditorGUI.LabelField(rect, new GUIContent($"Modified [{index + 8:D2}]"), EditorStyles.boldLabel);

            // Draw the name entry field
            Color bg = GUI.backgroundColor;
            if (!entry.Valid)
            {
                GUI.backgroundColor = Color.red;
            }
            
            Rect r1 = rect;
            r1.x += 120;
            r1.width -= 204;
            string nameInput = EditorGUI.TextField(r1, entry.Name);
            if (entry.Name != nameInput)
            {
                // Dirty on name change
                entry.Name = nameInput;
                Dirty      = true;
            }
            GUI.backgroundColor = bg;

            // Draw the redirect control
            r1.x += r1.width + 4;
            r1.width = 80;
            GUI.enabled = !string.IsNullOrEmpty(entry.OldName);
            if (EditorGUI.DropdownButton(r1, new GUIContent("Redirect"), FocusType.Passive))
            {
                var menu = new GenericMenu();

                // Add None option
                menu.AddItem(new GUIContent("None"), false, () =>
                {
                    entry.Redirect = -1;
                    Dirty = true;
                });
                menu.AddSeparator("");

                // Add options for fixed layers
                for (var i = 0; i < 8; ++i)
                {
                    if (!string.IsNullOrEmpty(m_fixedLayers[i]))
                    {
                        // Do stuff
                        int id = i;
                        menu.AddItem(new GUIContent(m_fixedLayers[i]), false, () =>
                        {
                            entry.Redirect = id;
                            Dirty = true;
                        });
                    }
                }
                menu.AddSeparator("");

                // Add options for valid layers
                LayerManagerData.SerializedLayerMapEntry[] allEntries = m_data.GetAllEntries();
                for (var i = 0; i < allEntries.Length; ++i)
                {
                    if (i == index)
                    {
                        continue;
                    }

                    LayerManagerData.SerializedLayerMapEntry targetEntry = allEntries[i];
                    
                    if (targetEntry.Redirect != -1)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(targetEntry.Name))
                    {
                        continue;
                    }

                    menu.AddItem(new GUIContent(targetEntry.Name), false, () =>
                                                                          {
                                                                              entry.Redirect = targetEntry.OldIndex;
                                                                              Dirty          = true;
                                                                          });
                }

                menu.ShowAsContext();
            }
            GUI.enabled = true;

            // Draw bottom label (original & old index)
            rect.y += LineHeight + LineSpacing;
            EditorGUI.LabelField(rect, new GUIContent($"Original [{entry.OldIndex:D2}]"));

            // Draw old name
            Rect r2 = rect;
            r2.x += 120;
            r2.width -= 204;
            EditorGUI.LabelField(r2, entry.OldName);

            // Draw redirect
            r2.x += r2.width + 4;
            r2.width = 80;
            int redirect = entry.Redirect;
            if (redirect == -1)
            {
                EditorGUI.LabelField(r2, "No Redirect");
            }
            else
            {
                if (redirect < 8)
                {
                    EditorGUI.LabelField(r2, m_fixedLayers[redirect]);
                }
                else
                {
                    EditorGUI.LabelField(r2, m_data.GetEntryFromOldIndex(redirect).Name);
                }
            }
        }

        private void OnLayerMapReorder(ReorderableList list)
        {
            Dirty = true;
            m_data.RebuildSerializedEntries();
        }

        private void ResetData()
        {
            if (m_data != null)
            {
                DestroyImmediate(m_data);
                m_data = null;
            }
            Initialise();
        }

        private void ApplyLayerModifications()
        {
            // Get the layer collision matrix before altering the layers
            GetLayerCollisionMatrix();
            Get2DLayerCollisionMatrix();

            // Get Tags and Layers settings
            Object asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0];
            if (asset == null)
            {
                // Complete with error
                m_state = ManagerState.Complete;
                m_completionReport = "Failed to process layer modifications. Asset not found: ProjectSettings/TagManager.asset";
                return;
            }

            var tagManager = new SerializedObject(asset);

            // Get layer properties
            SerializedProperty layerProps = tagManager.FindProperty("layers");
            if (layerProps == null)
            {
                // Complete with error
                m_state = ManagerState.Complete;
                m_completionReport = "Failed to process layer modifications. No layers property found in tag manager asset.";
                return;
            }

            // Modify layer settings
            LayerManagerData.SerializedLayerMapEntry[] allEntries = m_data.GetAllEntries();
            try
            {
                for (var i = 0; i < 24; ++i)
                    layerProps.GetArrayElementAtIndex(i + 8).stringValue = allEntries[i].Name;
                tagManager.ApplyModifiedPropertiesWithoutUndo();
            }
            catch (Exception e)
            {
                // Complete with error
                m_state = ManagerState.Complete;
                m_completionReport = "Failed to process layer modifications. Exception when updating layer settings: " + e.Message;
                return;
            }

            // Build reverse array of index swaps (old to new)
            m_indexSwaps = new int[32];
            m_indexSwapsRedirected = new int[32];
            for (var i = 0; i < 8; ++i)
            {
                m_indexSwaps[i] = i;
                m_indexSwapsRedirected[i] = i;
            }

            for (var i = 0; i < 24; ++i)
            {
                m_indexSwaps[allEntries[i].OldIndex] = i + 8;
            }
            
            for (var i = 0; i < 24; ++i)
            {
                if (allEntries[i].Redirect == -1)
                {
                    m_indexSwapsRedirected[allEntries[i].OldIndex] = i + 8;
                }
                else
                {
                    m_indexSwapsRedirected[allEntries[i].OldIndex] = TransformLayer(allEntries[i].Redirect, false);
                }
            }

            // Apply new layers to collision matrix
            ProcessLayerCollisionMatrix();
            Process2DLayerCollisionMatrix();

            // Set up for incremental processing.
            m_assetPaths = AssetDatabase.GetAllAssetPaths();
            m_currentAssetPath = 0;
            m_state = ManagerState.Processing;
        }

        private void IncrementLayerModifications()
        {
            if (m_assetPaths == null)
            {
                return;
            }
            if (m_currentAssetPath >= m_assetPaths.Length)
            {
                m_assetPaths = null;
                m_currentAssetPath = 0;
                return;
            }

            string path = m_assetPaths[m_currentAssetPath];
            try
            {
                if (path.StartsWith("Assets/"))
                {
                    // Process prefab
                    if (path.EndsWith(".prefab"))
                    {
                        try
                        {
                            // Record object & component counts to check if modified
                            int objCount = m_objectCount;
                            int compCount = m_componentCount;

                            // Load the prefab asset and modify
                            var obj = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);
                            if (obj != null)
                            {
                                ProcessGameObject(obj, false);

                                // Prefab was modified
                                if (m_objectCount > objCount || m_componentCount > compCount)
                                    ++m_prefabCount;
                            }
                        }
                        catch (Exception e)
                        {
                            m_errors.Add($"Encountered error processing prefab: \"{path}\", message: {e.Message}");
                        }
                    }
                    else
                    {
                        // Process ScriptableObject asset
                        if (path.EndsWith(".asset"))
                        {
                            try
                            {
                                // Load the scriptable object (and children) and modify
                                Object[] objects = AssetDatabase.LoadAllAssetsAtPath(path);
                                if (objects != null)
                                {
                                    foreach (Object obj in objects)
                                    {
                                        if (obj == null)
                                            continue;

                                        var so = new SerializedObject(obj);
                                        if (ProcessSerializedObject(so))
                                            ++m_assetCount;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                m_errors.Add($"Encountered error processing Scriptable Object: \"{path}\", message: {e.Message}");
                            }
                        }
                        else
                        {
                            // Process scene
                            if (path.EndsWith(".unity"))
                            {
                                try
                                {
                                    // Record object & component counts to check if modified
                                    int objCount = m_objectCount;
                                    int compCount = m_componentCount;

                                    // Load the scene
                                    Scene scene = EditorSceneManager.OpenScene(path);

                                    // Iterate through objects and modify
                                    GameObject[] objects = scene.GetRootGameObjects();
                                    foreach (GameObject obj in objects)
                                    {
                                        if (obj != null)
                                            ProcessGameObject(obj, true);
                                    }

                                    // Scene was modified
                                    if (m_objectCount > objCount || m_componentCount > compCount)
                                    {
                                        ++m_sceneCount;
                                        //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                                        if (!EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ""))
                                            Debug.LogWarning("Failed to save scene: " + path);
                                    }
                                }
                                catch (Exception e)
                                {
                                    m_errors.Add($"Encountered error processing scene: \"{path}\", message: {e.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_errors.Add($"Encountered error processing asset: \"{path}\", message: {e.Message}");
            }

            // Increment or complete
            ++m_currentAssetPath;
            if (m_currentAssetPath >= m_assetPaths.Length)
            {
                m_assetPaths = null;
                m_currentAssetPath = 0;
                AssetDatabase.SaveAssets();
                m_state = ManagerState.Complete;
                BuildReport();
            }
        }

#if UNITY_2018_3_OR_NEWER

        private void ProcessGameObject(GameObject go, bool inScene)
        {
            if (go == null)
                return;

            try
            {
                if (inScene)
                {
                    if (PrefabUtility.IsPartOfPrefabInstance(go) && !PrefabUtility.IsPrefabAssetMissing(go))
                    {
                        // Checking prefab in scene - only process unapplied modifications
                        // The rest will be done through the prefabs in the project hierarchy pass
                        ProcessPrefabModifications(go);
                    }
                    else
                    {
                        // Process children
                        Transform t = go.transform;
                        int childCount = t.childCount;
                        for (var i = 0; i < childCount; ++i)
                        {
                            try
                            {
                                Transform child = t.GetChild(i);
                                if (child == null)
                                    continue;

                                ProcessGameObject(child.gameObject, inScene);
                            }
                            catch (Exception e)
                            {
                                m_errors.Add($"Encountered error processing GameObject child in scene: \"{go.name}\", child index: {i}, message: {e.Message}");
                            }
                        }

                        // Swap layer
                        try
                        {
                            var so = new SerializedObject(go);
                            SerializedProperty layerProp = so.FindProperty("m_layer");
                            int oldLayer = layerProp.intValue;
                            int transformedLayer = TransformLayer(oldLayer, true);
                            if (transformedLayer != oldLayer)
                            {
                                layerProp.intValue = transformedLayer;
                                so.ApplyModifiedPropertiesWithoutUndo();
                                ++m_objectCount;
                            }
                        }
                        catch (Exception e)
                        {
                            m_errors.Add($"Encountered error processing layer property on GameObject in scene: \"{go.name}\", message: {e.Message}");
                        }

                        // Process Components
                        Component[] components = go.GetComponents<Component>();
                        for (var i = 0; i < components.Length; ++i)
                        {
                            try
                            {
                                if (components[i] == null)
                                    continue;

                                if (ProcessSerializedObject(new SerializedObject(components[i])))
                                    ++m_componentCount;
                            }
                            catch (Exception e)
                            {
                                m_errors.Add($"Encountered error processing component on GameObject in scene: \"{go.name}\", component index: {i}, message: {e.Message}");
                            }
                        }
                    }
                }
                else
                {
                    if (PrefabUtility.IsPartOfVariantPrefab(go))
                        ProcessVariantPrefab(go, go);
                    else
                        ProcessProjectPrefab(go);
                }
            }
            catch (Exception e)
            {
                m_errors.Add($"Encountered error processing GameObject: \"{go.name}\", message: {e.Message}");
            }
        }

        private void ProcessProjectPrefab(GameObject go)
        {
            ProcessPrefabGameObject(go);

            // Process children
            Transform t = go.transform;
            int childCount = t.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                try
                {
                    GameObject child = t.GetChild(i).gameObject;
                    if (child == null)
                        continue;

                    GameObject childRoot = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                    if (childRoot != null)
                        ProcessPrefabModifications(child);
                    else
                        ProcessProjectPrefab(child);
                }
                catch (Exception e)
                {
                    m_errors.Add($"Encountered error processing prefab child object: \"{go.name}\", child index: {i}, message: {e.Message}");
                }
            }
        }

        private void ProcessVariantPrefab(GameObject go, GameObject root)
        {
            if (go == root)
                ProcessPrefabModifications(go);

            // PROCESSING CHILDREN NOT REQUIRED
            /*
            // Process children
            Transform t = go.transform;
            int childCount = t.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                var child = t.GetChild(i).gameObject;
                var childRoot = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                if (childRoot != root)
                    ProcessVariantPrefab(child, childRoot);
                else
                    ProcessVariantPrefab(child, root);
            }
            */
        }

        private void ProcessPrefabGameObject(GameObject go)
        {
            if (go == null)
                return;

            // Swap layer
            try
            {
                var so = new SerializedObject(go);
                SerializedProperty layerProp = so.FindProperty("m_layer");
                int oldLayer = layerProp.intValue;
                int transformedLayer = TransformLayer(oldLayer, true);
                if (transformedLayer != oldLayer)
                {
                    layerProp.intValue = transformedLayer;
                    so.ApplyModifiedPropertiesWithoutUndo();
                    ++m_objectCount;
                }
            }
            catch (Exception e)
            {
                m_errors.Add($"Encountered error processing layer property on prefab object: \"{go.name}\", message: {e.Message}");
            }

            // Process Components
            Component[] components = go.GetComponents<Component>();
            for (var i = 0; i < components.Length; ++i)
            {
                try
                {
                    if (components[i] == null)
                        continue;

                    if (ProcessSerializedObject(new SerializedObject(components[i])))
                        ++m_componentCount;
                }
                catch (Exception e)
                {
                    m_errors.Add($"Encountered error processing components on prefab object: \"{go.name}\", component index: {i}, message: {e.Message}");
                }
            }
        }

        private void ProcessPrefabModifications(GameObject go)
        {
            PropertyModification[] mods = PrefabUtility.GetPropertyModifications(go);
            if (mods == null)
            {
                return;
            }

            var found = false;

            try
            {
                foreach (PropertyModification mod in mods)
                {
                    if (mod.target == null)
                    {
                        continue;
                    }

                    var so = new SerializedObject(mod.target);
                    SerializedProperty itr = so.GetIterator();
                    string prev = string.Empty;
                    while (itr.Next(true))
                    {
                        if (itr.propertyPath == mod.propertyPath)
                        {
                            if (so.targetObject is GameObject)
                            {
                                if (itr.name == "m_layer")
                                {
                                    //Debug.Log("Found modified object layer on object: " + so.targetObject.name + ", type: " + itr.type);
                                    int oldLayer = itr.intValue;
                                    int transformedLayer = TransformLayer(oldLayer, true);
                                    if (transformedLayer != oldLayer)
                                    {
                                        found = true;
                                        mod.value = transformedLayer.ToString();
                                    }
                                }
                                else
                                {
                                    if (prev == "LayerMask")
                                    {
                                        found = true;
                                        //Debug.Log("Found modified LayerMask property: " + itr.propertyPath);
                                        int oldMask = itr.intValue;
                                        int transformedMask = TransformLayer(oldMask, true);
                                        if (transformedMask != oldMask)
                                        {
                                            mod.value = transformedMask.ToString();
                                        }
                                    }
                                }
                                so.ApplyModifiedProperties();
                                break;
                            }
                            prev = itr.type;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_errors.Add($"Encountered error processing property modifications on prefab: \"{go.name}\", message: {e.Message}");
            }

            if (found)
                PrefabUtility.SetPropertyModifications(go, mods);
        }

#else
        void ProcessGameObject (GameObject go, bool inScene)
        {
            try
            {
                if (inScene && PrefabUtility.GetPrefabObject(go) != null)
                    return;

                // Process children
                Transform t = go.transform;
                int childCount = t.childCount;
                for (int i = 0; i < childCount; ++i)
                {
                    try
                    {
                        var child = t.GetChild(i);
                        if (child == null)
                            continue;

                        ProcessGameObject(child.gameObject, inScene);
                    }
                    catch (Exception e)
                    {
                        m_Errors.Add(string.Format("Encountered error processing GameObject child: \"{0}\", child index: {1}, in scene: {2}, message: {3}", go.name, i, inScene, e.Message));
                    }
                }

                // Swap layer
                SerializedObject so = new SerializedObject(go);
                var layerProp = so.FindProperty("m_layer");
                int oldLayer = layerProp.intValue;
                int transformedLayer = TransformLayer(oldLayer, true);
                if (transformedLayer != oldLayer)
                {
                    layerProp.intValue = transformedLayer;
                    so.ApplyModifiedPropertiesWithoutUndo();
                    ++m_ObjectCount;
                }

                // Process Components
                Component[] components = go.GetComponents<Component>();
                for (int i = 0; i < components.Length; ++i)
                {
                    try
                    {
                        if (components[i] == null)
                            continue;

                        if (ProcessSerializedObject(new SerializedObject(components[i])))
                            ++m_ComponentCount;
                    }
                    catch (Exception e)
                    {
                        m_Errors.Add(string.Format("Encountered error processing component on GameObject: \"{0}\", component index: {1}, in scene: {2}, message: {3}", go.name, i, inScene, e.Message));
                    }
                }
            }
            catch (Exception e)
            {
                m_Errors.Add(string.Format("Encountered error processing GameObject: \"{0}\", message: {1}", go.name, e.Message));
            }
        }
#endif

        private bool ProcessSerializedObject (SerializedObject so)
        {
            try
            {
                int oldMaskCount = m_layerMaskCount;

                // Get property iterator
                SerializedProperty itr = so.GetIterator();
                if (itr != null)
                {
                    // Iterate through properties
                    var complete = false;
                    while (!complete)
                    {
                        // Only process LayerMask properties
                        if (itr.type == "LayerMask")
                        {
                            int old = itr.intValue;
                            int transformed = TransformMask(old);

                            // Record modifications
                            if (old != transformed)
                            {
                                itr.intValue = transformed;
                                ++m_layerMaskCount;
                            }
                        }

                        complete = !itr.Next(true);
                    }
                }

                // Apply changes if there are any
                if (m_layerMaskCount > oldMaskCount)
                {
                    so.ApplyModifiedPropertiesWithoutUndo();
                    return true;
                }
            }
            catch (Exception e)
            {
                m_errors.Add($"Encountered error processing SerializedObject: \"{so.targetObject.name}\", message: {e.Message}");
            }

            return false;
        }

        private int TransformLayer (int old, bool redirected)
        {
            if (redirected)
                return m_indexSwapsRedirected[old];
            else
                return m_indexSwaps[old];
        }

        private int TransformMask (int old)
        {
            var result = 0;

            // Iterate through each old layer
            for (var i = 0; i < 32; ++i)
            {
                // Get old flag for layer
                int flag = (old >> i) & 1;
                // Assign flag to new layer
                result |= flag << TransformLayer(i, true);
            }

            return result;
        }

        private uint TransformMatrix (uint old)
        {
            uint result = 0;

            // Iterate through each old layer
            for (var i = 0; i < 32; ++i)
            {
                // Get old flag for layer
                uint flag = (old >> i) & 1;
                // Assign flag to new layer
                result |= flag << TransformLayer(i, false);
            }

            return result;
        }

        private void CreateMap ()
        {
            // Create map scriptable object
            LayerMap map = CreateInstance<LayerMap>();

            // Get the map serialied property
            var mapSo = new SerializedObject(map);
            SerializedProperty mapSp = mapSo.FindProperty("m_map");

            // Build the layer map
            mapSp.arraySize = 32;
            for (var i = 0; i < 32; ++i)
            {
                mapSp.GetArrayElementAtIndex(i).intValue = m_indexSwapsRedirected[i];
            }

            // Save the map asset
            mapSo.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.CreateAsset(map, AssetDatabase.GenerateUniqueAssetPath("Assets/LayerMap.asset"));
            AssetDatabase.SaveAssets();
        }


        private void GetLayerCollisionMatrix ()
        {
            m_physicsMasks = null;

            try
            {
                // Get dynamics manager asset
                Object asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/DynamicsManager.asset")[0];
                if (asset == null)
                {
                    m_errors.Add("Failed to read physics layer collision matrix. Asset file was missing or null");
                    m_physicsMasks = null;
                    return;
                }

                var dynamicsManager = new SerializedObject(asset);
                if (dynamicsManager == null)
                    return;

                // Get collision matrix property
                SerializedProperty matrixProp = dynamicsManager.FindProperty("m_layerCollisionMatrix");
                if (matrixProp == null)
                    return;

                // Get layer masks
                m_physicsMasks = new uint[32];
                for (var i = 0; i < 32; ++i)
                {
                    m_physicsMasks[i] = (uint)matrixProp.GetArrayElementAtIndex(i).longValue;
                }

                // Fix layer masks by setting empty layers to everything and cross-referencing
                for (var i = 0; i < 32; ++i)
                {
                    if (string.IsNullOrEmpty(LayerMask.LayerToName(i)))
                    {
                        m_physicsMasks[i] = uint.MaxValue;
                    }
                }
                for (var i = 0; i < 32; ++i)
                {
                    for (var j = 0; j < 32; ++j)
                    {
                        if (i == j)
                            continue;

                        // Cross reference here
                        uint referenced = (m_physicsMasks[j] >> i) & 1;
                        m_physicsMasks[i] |= referenced << j;
                    }
                    
                }

                // Print out binary for checking against
                //string total = "Old Layer Collision Matrix:\n";
                //for (int i = 0; i < 32; ++i)
                //    total += string.Format("{0}{1:D2}: {2} \n", LayerMask.LayerToName(i).PadRight(16, ' '), i, Convert.ToString(m_PhysicsMasks[i], 2).PadLeft(32, '0'));
                //Debug.Log(total);
            }
            catch (Exception e)
            {
                m_errors.Add("Failed to read physics layer collision matrix. Exception when updating settings: " + e.Message);
                m_physicsMasks = null;
            }
        }

        private void ProcessLayerCollisionMatrix ()
        {
            if (m_physicsMasks == null)
                return;
            
            try
            {
                // Get dynamics manager asset
                var dynamicsManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/DynamicsManager.asset")[0]);

                // Get collision matrix property
                SerializedProperty matrixProp = dynamicsManager.FindProperty("m_layerCollisionMatrix");
                if (matrixProp == null)
                {
                    m_errors.Add("Failed to process physics layer collision matrix. Matrix property not found in dynamics manager asset.");
                    return;
                }

                // Process layer masks
                for (var i = 0; i < 32; ++i)
                {
                    uint oldLayerMask = m_physicsMasks[i];
                    uint newLayerMask = TransformMatrix(oldLayerMask);

                    // Apply and record onlly if changed
                    matrixProp.GetArrayElementAtIndex(TransformLayer(i, false)).longValue = newLayerMask;
                }

                // Print out binary for checking against
                //string total = "New Layer Collision Matrix:\n";
                //for (int i = 0; i < 32; ++i)
                //    total += string.Format("{0}{1:D2}: {2} \n", LayerMask.LayerToName(i).PadRight(16, ' '), i, Convert.ToString(matrixProp.GetArrayElementAtIndex(i).intValue, 2).PadLeft(32, '0'));
                //Debug.Log(total);

                // Apply modifications
                dynamicsManager.ApplyModifiedPropertiesWithoutUndo();

                m_physicsMatrixCompleted = true;
            }
            catch (Exception e)
            {
                m_errors.Add("Failed to process physics layer collision matrix. Exception when updating settings: " + e.Message);
            }
        }

        private void Get2DLayerCollisionMatrix()
        {
            m_physics2DMasks = null;

            try
            {
                // Get dynamics manager asset
                var dynamicsManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Physics2DSettings.asset")[0]);
                if (dynamicsManager == null)
                    return;

                // Get collision matrix property
                SerializedProperty matrixProp = dynamicsManager.FindProperty("m_layerCollisionMatrix");
                if (matrixProp == null)
                {
                    return;
                }

                // Get layer masks
                m_physics2DMasks = new uint[32];
                for (var i = 0; i < 32; ++i)
                {
                    m_physics2DMasks[i] = (uint)matrixProp.GetArrayElementAtIndex(i).longValue;
                }
                
                // Fix layer masks by setting empty layers to everything and cross-referencing
                for (var i = 0; i < 32; ++i)
                {
                    if (string.IsNullOrEmpty(LayerMask.LayerToName(i)))
                    {
                        m_physics2DMasks[i] = uint.MaxValue;
                    }
                }
                for (var i = 0; i < 32; ++i)
                {
                    for (var j = 0; j < 32; ++j)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        // Cross reference here
                        uint referenced = (m_physics2DMasks[j] >> i) & 1;
                        m_physics2DMasks[i] |= referenced << j;
                    }
                    
                }
            }
            catch (Exception e)
            {
                m_errors.Add("Failed to read physics 2D layer collision matrix. Exception when updating settings: " + e.Message);
                m_physics2DMasks = null;
            }
        }

        private void Process2DLayerCollisionMatrix()
        {
            if (m_physics2DMasks == null)
            {
                return;
            }

            try
            {
                // Get dynamics manager asset
                Object asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Physics2DSettings.asset")[0];
                if (asset == null)
                {
                    m_errors.Add("Failed to process physics 2D layer collision matrix. Asset not found: ProjectSettings/Physics2DSettings.asset");
                    return;
                }

                var dynamicsManager = new SerializedObject(asset);

                // Get collision matrix property
                SerializedProperty matrixProp = dynamicsManager.FindProperty("m_layerCollisionMatrix");
                if (matrixProp == null)
                {
                    m_errors.Add("Failed to process physics 2D layer collision matrix. Matrix property not found in dynamics manager asset.");
                    return;
                }

                // Process layer masks
                for (var i = 0; i < 32; ++i)
                {
                    uint oldLayerMask = m_physics2DMasks[i];
                    uint newLayerMask = TransformMatrix(oldLayerMask);

                    // Apply and record onlly if changed
                    matrixProp.GetArrayElementAtIndex(TransformLayer(i, false)).longValue = newLayerMask;
                }

                // Apply modifications
                dynamicsManager.ApplyModifiedPropertiesWithoutUndo();

                m_physics2DMatrixCompleted = true;
            }
            catch (Exception e)
            {
                m_errors.Add("Failed to process physics 2D layer collision matrix. Exception when updating settings: " + e.Message);
            }
        }

        private void BuildReport ()
        {
            m_completionReport = $"Layer Modification Completed\n\n- Modified tags and layers settings.\n"
                               + $"- {GetCollisionMatrixReport()}\n- {Get2DCollisionMatrixReport()}\n- {GetObjectsReport()}\n- {GetMasksReport()}\n"
                               + $"- Errors encountered: {m_errors.Count}.";
        }

        private string GetCollisionMatrixReport ()
        {
            if (m_physicsMatrixCompleted)
                return "Physics layer collision matrix modifications succeeded.";
            else
                return "Physics layer collision matrix modifications failed with errors.";
        }

        private string Get2DCollisionMatrixReport()
        {
            if (m_physics2DMatrixCompleted)
            {
                return "Physics 2D layer collision matrix modifications succeeded.";
            }
            else
            {
                return "Physics 2D layer collision matrix modifications failed with errors.";
            }
        }

        private string GetObjectsReport ()
        {
            // Get string dependent on numbers of prefabs and scene objects
            if (m_sceneCount > 0 && m_prefabCount > 0)
            {
                return $"Modified layer property for {m_objectCount} GameObjects across {m_sceneCount} scenes and {m_prefabCount} prefabs.";
            }

            if (m_sceneCount > 0)
            {
                return $"Modified layer property for {m_objectCount} GameObjects across {m_sceneCount} scenes.";
            }

            if (m_prefabCount > 0)
            {
                return $"Modified layer property for {m_objectCount} GameObjects across {m_prefabCount} prefabs.";
            }

            // Objects that didn't belong to a scene or prefab (this should never be reached)
            if (m_objectCount > 0)
            {
                return $"Modified layer property for {m_objectCount} GameObjects.";
            }

            // No changes to game objects
            return "No GameObject layers affected by changes.";
        }

        private string GetMasksReport ()
        {
            // Get string dependent on numbers of game object components and scriptable object assets
            if (m_componentCount > 0 && m_assetCount > 0)
            {
                return $"Modified {m_layerMaskCount} LayerMask properties on {m_componentCount} "
                     + $"components and {m_assetCount} scriptable object assets.";}

            if (m_componentCount > 0)
            {
                return $"Modified {m_layerMaskCount} LayerMask properties on {m_componentCount} components.";
            }

            if (m_assetCount > 0)
            {
                return $"Modified {m_layerMaskCount} LayerMask properties on {m_assetCount} scriptable object assets.";
            }

            // Properties that did not belong to a scriptable object or component (this should never be reached)
            if (m_layerMaskCount > 0)
            {
                return $"Modified {m_layerMaskCount} LayerMask properties.";
            }

            // No layermask properties found
            return "No LayerMask properties found on components or scriptable object assets.";
        }
    }
}