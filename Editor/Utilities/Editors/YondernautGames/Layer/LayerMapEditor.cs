using UnityEditor;
using UnityEngine;
using Utilities.Types;
using Utilities.Types.Layer;

namespace Utilities.Editors.Layer
{
    /// <summary>
    /// Custom inspector for the LayerMap class
    /// </summary>
    /// <remarks>
    /// <para>This class is heavily based on a similar class in <see cref="https://github.com/YondernautsGames">YondernautGames</see>'
    /// <see cref="https://github.com/YondernautsGames/LayerManager">LayerManager</see> repository.</para>
    /// </remarks>
    [CustomEditor(typeof(LayerMap))]
    public class LayerMapEditor : Editor
    {
        private int m_indexInput;
        private int m_indexOutput;

        private int m_maskInput;
        private int m_maskOutput;

        const float k_labelWidth = 20;

        public override void OnInspectorGUI()
        {
            var map = serializedObject.targetObject as LayerMap;

            EditorGUILayout.LabelField("Transform Layer Index", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            m_indexInput  = EditorGUILayout.IntField(m_indexInput);
            m_indexInput  = Mathf.Clamp(m_indexInput, 0, 31);
            if (map != null)
            {
                m_indexOutput = map.TransformLayer(m_indexInput);
                EditorGUILayout.LabelField("->", GUILayout.Width(k_labelWidth));
                EditorGUILayout.IntField(m_indexOutput);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("Transform Layer Mask", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                m_maskInput  = EditorGUILayout.IntField(m_maskInput);
                m_maskOutput = map.TransformMask(m_maskInput);
            }

            EditorGUILayout.LabelField("->", GUILayout.Width(k_labelWidth));
            EditorGUILayout.IntField(m_maskOutput);
            EditorGUILayout.EndHorizontal();
        }
    }
}