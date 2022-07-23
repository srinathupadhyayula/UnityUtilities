using System;
using UnityEditor;
using UnityEngine;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// <para>AttachAttributePropertyDrawer a base class.</para>
    /// <para>Child classes ex: <i>AddComponentAttributeDrawer</i> override the <i>UpdateProperty</i> method.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    public class AttachAttributePropertyDrawer : PropertyDrawer
    {
        private static readonly Color m_GUIColorDefault = new Color(.6f, .6f, .6f, 1);
        private static readonly Color m_GUIColorNull    = new Color(1f,  .5f, .5f, 1);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // turn off attribute if not active or in Play Mode (imitate as build will works)
            if (!AttachAttributesUtils.IsEnabled || Application.isPlaying)
            {
                property.serializedObject.Update();
                EditorGUI.PropertyField(position, property, label, true);
                property.serializedObject.ApplyModifiedProperties();
                return;
            }

            bool isPropertyValueNull = property.objectReferenceValue == null;

            // Change GUI color
            var prevColor = GUI.color;
            GUI.color = isPropertyValueNull ? m_GUIColorNull : m_GUIColorDefault;

            // Default draw
            EditorGUI.PropertyField(position, property, label, true);

            // Get property type and GameObject
            property.serializedObject.Update();
            if (isPropertyValueNull)
            {
                var type = property.GetPropertyType().StringToType();
                var go   = ((MonoBehaviour)(property.serializedObject.targetObject)).gameObject;
                UpdateProperty(property, go, type);
            }

            property.serializedObject.ApplyModifiedProperties();
            GUI.color = prevColor;
        }

        /// Customize it for each attribute
        protected virtual void UpdateProperty(SerializedProperty property, GameObject go, Type type)
        {
            // Do whatever
            // For example to get component 
            // property.objectReferenceValue = go.GetComponent(type);
        }
    }
}