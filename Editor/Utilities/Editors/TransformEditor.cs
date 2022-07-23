using System;
using UnityEditor;
using UnityEngine;
using Utilities.Patterns;
using Object = UnityEngine.Object;

namespace Utilities.Editors
{
    /// <summary>
    /// A Custom Inspector for Transforms
    /// </summary>
    /// <remarks>
    /// <para>This class is heavily based on a similar class in <b cref="https://github.com/thomue00">Thomas Müller</b>'s
    /// <i cref="https://github.com/thomue00/Unity3D-Editor-Extensions-and-Tools/blob/master/Assets/Scripts/Editor/TransformEditor.cs">
    /// Unity3D Editor Extensions and Tools</i> repository.</para>
    /// </remarks>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    public class TransformEditor : EditorSingleton<TransformEditor>
    {
        private static Vector3?    s_copyPosition = Vector3.zero;
        private static Quaternion? s_copyRotation = Quaternion.identity;
        private static Vector3?    s_copyScale    = Vector3.zero;

        private Transform          m_transform;
        private SerializedProperty m_pos;
        private SerializedProperty m_rot;
        private SerializedProperty m_scale;
        private Space              m_space = Space.Local;
        private int                m_selectionCount;

        private GUIStyle m_btnSmall;

        private const string k_propertyIDPosition = "m_LocalPosition";
        private const string k_propertyIDRotation = "m_LocalRotation";
        private const string k_propertyIDScale    = "m_LocalScale";

        private bool IsLocal => m_space == Space.Local;

        private new void OnEnable()
        {

            base.OnEnable();
            m_pos            = serializedObject.FindProperty(k_propertyIDPosition);
            m_rot            = serializedObject.FindProperty(k_propertyIDRotation);
            m_scale          = serializedObject.FindProperty(k_propertyIDScale);
            m_transform      = (Transform) serializedObject.targetObject;
            m_selectionCount = Selection.gameObjects.Length;
        }


        public override void OnInspectorGUI()
        {
            EnsureStyles();

            DrawTransformHeader();
            GUILayout.Space(1);
            serializedObject.Update();

            float resetLabelWith = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 15f;

            DrawTransformPosition();
            DrawTransformRotation();
            DrawTransformScale();

            if (m_selectionCount > 1)
            {
                EditorGUILayout.LabelField("Selected: " + m_selectionCount + " Objects");
            }

            EditorUtility.SetDirty(m_transform);
            serializedObject.ApplyModifiedProperties();
            EditorGUIUtility.labelWidth = resetLabelWith;

            EditorGUILayout.Space();
        }

        private void EnsureStyles()
        {
            m_btnSmall = new GUIStyle(EditorStyles.toolbarButton) {fixedHeight = 16, fixedWidth = 20};
        }

        private void DrawTransformHeader()
        {
            using (new EditorGUILayout.HorizontalScope())
            {

                string val = m_selectionCount > 1 ? ("ID: Multiple") : ("ID: " + m_transform.GetInstanceID());
                EditorGUILayout.LabelField(val, GUILayout.MinWidth(0));

                GUILayout.FlexibleSpace();

                m_space = (Space) EnumToolbar(m_space);
            }
        }

        private void DrawTransformPosition()
        {
            using (new EditorGUILayout.HorizontalScope())
            {

                bool reset = GUILayout.Button("R", m_btnSmall);

                GUILayoutOption option = GUILayout.MinWidth(63);
                if (IsLocal)
                {
                    EditorGUILayout.PropertyField(m_pos.FindPropertyRelative("x"), option);
                    EditorGUILayout.PropertyField(m_pos.FindPropertyRelative("y"), option);
                    EditorGUILayout.PropertyField(m_pos.FindPropertyRelative("z"), option);
                }
                else
                {
                    Vector3 pos = m_transform.position;
                    FloatField("X", ref pos.x, option);
                    FloatField("Y", ref pos.y, option);
                    FloatField("Z", ref pos.z, option);
                    m_transform.position = pos;
                }

                if (GUILayout.Button("C", m_btnSmall))
                {
                    s_copyPosition = IsLocal ? Selection.activeTransform.localPosition : Selection.activeTransform.position;
                }

                if (GUILayout.Button("V", m_btnSmall))
                {
                    if (s_copyPosition.HasValue)
                    {
                        EditorTools.RegisterUndo("Paste Position", serializedObject.targetObjects);
                        if (IsLocal)
                        {
                            Selection.activeTransform.localPosition = s_copyPosition.Value;
                        }
                        else
                        {
                            Selection.activeTransform.position = s_copyPosition.Value;
                        }
                    }
                }

                if (reset)
                {
                    EditorTools.RegisterUndo("Reset Position", serializedObject.targetObjects);
                    if (IsLocal)
                    {
                        m_pos.vector3Value = Vector3.zero;
                    }
                    else
                    {
                        m_transform.position = Vector3.zero;
                    }

                    GUIUtility.keyboardControl = 0;
                }
            }
        }

        private void DrawTransformRotation()
        {
            using (new EditorGUILayout.HorizontalScope())
            {

                bool reset = GUILayout.Button("R", m_btnSmall);

                GUILayoutOption option = GUILayout.MinWidth(63);
                Vector3 euler = IsLocal
                                    ? ((Transform) serializedObject.targetObject).localRotation.eulerAngles
                                    : ((Transform) serializedObject.targetObject).rotation.eulerAngles;
                euler = WrapAngle(euler);

                CheckDifference(m_rot);
                Axes altered = Axes.None;

                if (FloatField("X", ref euler.x, option))
                {
                    altered |= Axes.X;
                }

                if (FloatField("Y", ref euler.y, option))
                {
                    altered |= Axes.Y;
                }

                if (FloatField("Z", ref euler.z, option))
                {
                    altered |= Axes.Z;
                }

                if (GUILayout.Button("C", m_btnSmall))
                {
                    s_copyRotation =
                        IsLocal ? Selection.activeTransform.localRotation : Selection.activeTransform.rotation;
                }

                if (GUILayout.Button("V", m_btnSmall))
                {

                    if (s_copyRotation.HasValue)
                    {

                        EditorTools.RegisterUndo("Paste Rotation", serializedObject.targetObjects);
                        if (IsLocal)
                        {
                            Selection.activeTransform.localRotation = s_copyRotation.Value;
                        }
                        else
                        {
                            Selection.activeTransform.rotation = s_copyRotation.Value;
                        }
                    }
                }

                if (reset)
                {

                    EditorTools.RegisterUndo("Reset Rotation", serializedObject.targetObjects);
                    if (IsLocal)
                    {

                        m_rot.quaternionValue        = Quaternion.identity;
                        m_transform.localEulerAngles = Vector3.zero;
                    }
                    else
                    {

                        m_transform.rotation    = Quaternion.identity;
                        m_transform.eulerAngles = Vector3.zero;
                    }

                    GUIUtility.keyboardControl = 0;
                }

                if (altered != Axes.None)
                {

                    EditorTools.RegisterUndo("Changed Rotation", serializedObject.targetObjects);

                    foreach (Object obj in serializedObject.targetObjects)
                    {

                        var t = obj as Transform;
                        if (t != null)
                        {
                            Vector3 v = IsLocal ? t.localEulerAngles : t.eulerAngles;
                            if ((altered & Axes.X) != 0)
                            {
                                v.x = euler.x;
                            }

                            if ((altered & Axes.Y) != 0)
                            {
                                v.y = euler.y;
                            }

                            if ((altered & Axes.Z) != 0)
                            {
                                v.z = euler.z;
                            }

                            if (IsLocal)
                            {
                                t.localEulerAngles = v;
                            }
                            else
                            {
                                t.eulerAngles = v;
                            }
                        }
                    }
                }
            }

        }

        private void DrawTransformScale()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                bool reset = GUILayout.Button("R", m_btnSmall);

                GUILayoutOption option = GUILayout.MinWidth(63);
                if (IsLocal)
                {
                    EditorGUILayout.PropertyField(m_scale.FindPropertyRelative("x"), option);
                    EditorGUILayout.PropertyField(m_scale.FindPropertyRelative("y"), option);
                    EditorGUILayout.PropertyField(m_scale.FindPropertyRelative("z"), option);
                }
                else
                {
                    Vector3 scale = m_transform.lossyScale;
                    FloatField("X", ref scale.x, option);
                    FloatField("Y", ref scale.y, option);
                    FloatField("Z", ref scale.z, option);
                }

                if (GUILayout.Button("C", m_btnSmall) && IsLocal)
                {
                    s_copyScale = Selection.activeTransform.localScale;
                }

                if (GUILayout.Button("V", m_btnSmall) && IsLocal)
                {

                    if (s_copyScale.HasValue)
                    {

                        EditorTools.RegisterUndo("Paste Scale", serializedObject.targetObjects);
                        Selection.activeTransform.localScale = s_copyScale.Value;
                    }
                }

                if (reset)
                {
                    if (IsLocal)
                    {
                        EditorTools.RegisterUndo("Reset Scale", serializedObject.targetObjects);
                        m_scale.vector3Value = Vector3.one;
                    }

                    GUIUtility.keyboardControl = 0;
                }
            }

        }

        private bool FloatField(string floatFieldName, ref float val, params GUILayoutOption[] options)
        {
            float newVal = val;
            EditorGUI.BeginChangeCheck();
            newVal = EditorGUILayout.FloatField(floatFieldName, newVal, options);
            if (EditorGUI.EndChangeCheck() && Differs(val, newVal))
            {
                val = newVal;
                return true;
            }

            return false;
        }

        private Enum EnumToolbar(Enum selected)
        {
            string[]     items    = Enum.GetNames(selected.GetType());
            int          count    = items.Length;
            Array        vals     = Enum.GetValues(selected.GetType());
            GUIContent[] contents = new GUIContent[count];
            for (int i = 0; i < count; i++)
            {
                contents[i] = new GUIContent(items[i], "");
            }

            int selectedIdx = 0;
            while (selectedIdx < count)
            {
                if (selected.ToString() == vals.GetValue(selectedIdx).ToString())
                {
                    break;
                }

                selectedIdx++;
            }

            selectedIdx = GUILayout.Toolbar(selectedIdx, contents);
            return (Enum) vals.GetValue(selectedIdx);

        }

        private Axes CheckDifference(SerializedProperty rotProperty)
        {
            Axes axes = Axes.None;
            if (rotProperty.hasMultipleDifferentValues)
            {
                Vector3 original = rotProperty.quaternionValue.eulerAngles;
                foreach (Object obj in serializedObject.targetObjects)
                {
                    axes |= CheckDifference(obj as Transform, original);
                    if (axes == Axes.All)
                    {
                        break;
                    }
                }
            }

            return axes;
        }

        private Axes CheckDifference(Transform t, Vector3 original)
        {
            Vector3 next = IsLocal ? t.localEulerAngles : t.eulerAngles;

            Axes axes = Axes.None;
            if (Differs(next.x, original.x))
            {
                axes |= Axes.X;
            }

            if (Differs(next.y, original.y))
            {
                axes |= Axes.Y;
            }

            if (Differs(next.z, original.z))
            {
                axes |= Axes.Z;
            }

            return axes;
        }

        private static bool Differs(float a, float b)
        {
            return Math.Abs(a - b) > 0.0001f;
        }

        private static float WrapAngle(float angle)
        {
            while (angle > 180f)
            {
                angle -= 360f;
            }

            while (angle < -180f)
            {
                angle += 360f;
            }

            return angle;
        }

        private static Vector3 WrapAngle(Vector3 angle)
        {
            angle.x = WrapAngle(angle.x);
            angle.y = WrapAngle(angle.y);
            angle.z = WrapAngle(angle.z);
            return angle;
        }
    }

    internal enum Space
    {
        Local  = 0
      , Global = 1
    }

    [Flags]
    internal enum Axes
    {
        None = 0
      , X    = 1
      , Y    = 2
      , Z    = 4
      , All  = 7
       ,
    }
}