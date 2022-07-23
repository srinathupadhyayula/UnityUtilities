using System;
using UnityEditor;
using UnityEngine;
using Utilities.Attributes;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// This class is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            Enum targetEnum = PropertyUtility.GetTargetObjectOfProperty(property) as Enum;

            return (targetEnum != null)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            Enum targetEnum = PropertyUtility.GetTargetObjectOfProperty(property) as Enum;
            if (targetEnum != null)
            {
                Enum enumNew = EditorGUI.EnumFlagsField(rect, label.text, targetEnum);
                property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
            }
            else
            {
                string message = attribute.GetType().Name + " can be used only on enums";
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }
    }
}
