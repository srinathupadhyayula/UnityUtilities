using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [CustomPropertyDrawer(typeof(InputAxisAttribute))]
    public class InputAxisPropertyDrawer : PropertyDrawerBase
    {
        private static readonly string AssetPath = Path.Combine("ProjectSettings", "InputManager.asset");
        private const string k_axesPropertyPath = "m_Axes";
        private const string k_namePropertyPath = "m_Name";

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return (property.propertyType == SerializedPropertyType.String)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                var inputManagerAsset = AssetDatabase.LoadAssetAtPath(AssetPath, typeof(object));
                var inputManager = new SerializedObject(inputManagerAsset);

                var axesProperty = inputManager.FindProperty(k_axesPropertyPath);
                var axesSet = new HashSet<string>();
                axesSet.Add("(None)");

                for (var i = 0; i < axesProperty.arraySize; i++)
                {
                    var axis = axesProperty.GetArrayElementAtIndex(i).FindPropertyRelative(k_namePropertyPath).stringValue;
                    axesSet.Add(axis);
                }

                var axes = axesSet.ToArray();

                string propertyString = property.stringValue;
                int index = 0;
                // check if there is an entry that matches the entry and get the index
                // we skip index 0 as that is a special custom case
                for (int i = 1; i < axes.Length; i++)
                {
                    if (axes[i].Equals(propertyString, System.StringComparison.Ordinal))
                    {
                        index = i;
                        break;
                    }
                }

                // Draw the popup box with the current selected index
                int newIndex = EditorGUI.Popup(rect, label.text, index, axes);

                // Adjust the actual string value of the property based on the selection
                string newValue = newIndex > 0 ? axes[newIndex] : string.Empty;

                if (!property.stringValue.Equals(newValue, System.StringComparison.Ordinal))
                {
                    property.stringValue = newValue;
                }
            }
            else
            {
                string message = string.Format("{0} supports only string fields", typeof(InputAxisAttribute).Name);
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }
    }
}
