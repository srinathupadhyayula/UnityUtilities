using System;
using UnityEditor;
using UnityEngine;

namespace Utilities.Types
{
    public class LayerManagerData : ScriptableObject
    {
        // Map array hidden from public access
        [SerializeField] private LayerMapEntry[] m_layerMap;
        [SerializeField] private bool m_dirty;

        private SerializedLayerMapEntry[] m_serializedEntries;

        [Serializable]
        public class LayerMapEntry
        {
            public string name;
            public string oldName;
            public int oldIndex;
            public int redirect = -1;

            public LayerMapEntry(string n, int i)
            {
                name = n;
                oldName = n;
                oldIndex = i;
            }

            public bool Valid => redirect != -1 || !string.IsNullOrEmpty(name) || string.IsNullOrEmpty(oldName);
        }

        public class SerializedLayerMapEntry
        {
            private LayerManagerData m_data;

            public SerializedProperty SerializedProperty
            {
                get;
                private set;
            }

            public SerializedLayerMapEntry(LayerManagerData data, int index)
            {
                m_data = data;
                SerializedProperty = data.LayerMapProperty.GetArrayElementAtIndex(index);
            }

            public string Name
            {
                get { return SerializedProperty.FindPropertyRelative("name").stringValue; }
                set
                {
                    Undo.RecordObject(m_data, "Change Layer Name");
                    SerializedProperty.FindPropertyRelative("name").stringValue = value;
                    // if empty, wipe dependent redirects
                    if (string.IsNullOrEmpty (value))
                    {
                        var entries = m_data.GetAllEntries();
                        foreach (var entry in entries)
                        {
                            if (entry == this)
                                continue;
                            if (entry.Redirect == OldIndex)
                                entry.Redirect = -1;
                        }
                    }
                    EditorUtility.SetDirty(m_data);
                }
            }

            public int Redirect
            {
                get { return SerializedProperty.FindPropertyRelative("redirect").intValue; }
                set
                {
                    Undo.RecordObject(m_data, "Redirect Layer");
                    int undo = Undo.GetCurrentGroup();
                    SerializedProperty.FindPropertyRelative("redirect").intValue = value;
                    // If check dependent redirects and set to new value
                    if (value == -1)
                    {
                        if (string.IsNullOrEmpty (OldName))
                        {
                            var entries = m_data.GetAllEntries();
                            foreach (var entry in entries)
                            {
                                if (entry == this)
                                    continue;
                                if (entry.Redirect == OldIndex)
                                {
                                    Undo.RecordObject(m_data, "Redirect Layer");
                                    entry.SerializedProperty.FindPropertyRelative("redirect").intValue = -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        var entries = m_data.GetAllEntries();
                        foreach (var entry in entries)
                        {
                            if (entry == this)
                                continue;
                            if (entry.Redirect == OldIndex)
                            {
                                Undo.RecordObject(m_data, "Redirect Layer");
                                entry.SerializedProperty.FindPropertyRelative("redirect").intValue = value;
                            }
                        }
                    }
                    Undo.CollapseUndoOperations(undo);
                    EditorUtility.SetDirty(m_data);
                }
            }

            public string OldName
            {
                get { return SerializedProperty.FindPropertyRelative("oldName").stringValue; }
            }

            public int OldIndex
            {
                get { return SerializedProperty.FindPropertyRelative("oldIndex").intValue; }
            }

            public bool Valid
            {
                get
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        var entries = m_data.GetAllEntries();
                        foreach (var entry in entries)
                        {
                            if (entry == this)
                                continue;
                            if (entry.Name == Name)
                                return false;
                        }
                    }
                    return Redirect != -1 || !string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(OldName);
                }
            }

            public string GetRedirectName()
            {
                foreach (var entry in m_data.m_layerMap)
                {
                    if (entry.oldIndex == OldIndex)
                        return entry.name;
                }
                return string.Empty;
            }
        }

        public bool Dirty
        {
            get { return m_dirty; }
            set { SerializedObject.FindProperty("m_dirty").boolValue = value; }
        }

        public bool Valid
        {
            get
            {
                foreach (var entry in m_layerMap)
                {
                    if (!entry.Valid)
                        return false;
                }
                return true;
            }
        }

        public SerializedObject SerializedObject
        {
            get;
            private set;
        }

        public SerializedProperty LayerMapProperty
        {
            get;
            private set;
        }

        public void Initialise()
        {
            // Build layer map from current settings
            m_layerMap = new LayerMapEntry[24/*32*/];
            for (var i = 0; i < 24/*32*/; ++i)
            {
                int oldIndex = i + 8;
                m_layerMap[i] = new LayerMapEntry(LayerMask.LayerToName(oldIndex), oldIndex);
            }

            // Set as not dirty
            m_dirty = false;

            // Get serialization objects
            SerializedObject = new SerializedObject(this);
            LayerMapProperty = SerializedObject.FindProperty("m_layerMap");

            // Build serialized entries
            m_serializedEntries = new SerializedLayerMapEntry[24/*32*/];
            RebuildSerializedEntries();

            // Set to not save
            hideFlags = HideFlags.DontSave;
        }

        public void RebuildSerializedEntries()
        {
            for (var i = 0; i < 24/*32*/; ++i)
                m_serializedEntries[i] = new SerializedLayerMapEntry(this, i);
        }

        public SerializedLayerMapEntry GetEntryFromIndex(int index)
        {
            return m_serializedEntries[index];
        }

        public SerializedLayerMapEntry GetEntryFromOldIndex(int oldIndex)
        {
            for (var i = 0; i < m_layerMap.Length; ++i)
            {
                if (m_layerMap[i].oldIndex == oldIndex)
                    return m_serializedEntries[i];
            }
            return null;
        }

        public SerializedLayerMapEntry[] GetAllEntries()
        {
            return m_serializedEntries;
        }

        public void ApplyModifiedProperties()
        {
            SerializedObject.ApplyModifiedProperties();
        }
    }
}