using System;
using System.Linq;
using System.Text.RegularExpressions;
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
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class ScenePropertyDrawer : PropertyDrawerBase
    {
        private const string k_sceneListItem = "{0} ({1})";
        private const string k_scenePattern = @".+\/(.+)\.unity";
        private const string k_typeWarningMessage = "{0} must be an int or a string";
        private const string k_buildSettingsWarningMessage = "No scenes in the build settings";

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            bool validPropertyType = property.propertyType == SerializedPropertyType.String || property.propertyType == SerializedPropertyType.Integer;
            bool anySceneInBuildSettings = GetScenes().Length > 0;

            return (validPropertyType && anySceneInBuildSettings)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            string[] scenes = GetScenes();
            bool anySceneInBuildSettings = scenes.Length > 0;
            if (!anySceneInBuildSettings)
            {
                DrawDefaultPropertyAndHelpBox(rect, property, k_buildSettingsWarningMessage, MessageType.Warning);
                return;
            }

            string[] sceneOptions = GetSceneOptions(scenes);
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    DrawPropertyForString(rect, property, label, scenes, sceneOptions);
                    break;
                case SerializedPropertyType.Integer:
                    DrawPropertyForInt(rect, property, label, sceneOptions);
                    break;
                default:
                    string message = string.Format(k_typeWarningMessage, property.name);
                    DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
                    break;
            }

            EditorGUI.EndProperty();
        }

        private string[] GetScenes()
        {
            return EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => Regex.Match(scene.path, k_scenePattern).Groups[1].Value)
                .ToArray();
        }

        private string[] GetSceneOptions(string[] scenes)
        {
            return scenes.Select((s, i) => string.Format(k_sceneListItem, s, i)).ToArray();
        }

        private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label, string[] scenes, string[] sceneOptions)
        {
            int index = IndexOf(scenes, property.stringValue);
            int newIndex = EditorGUI.Popup(rect, label.text, index, sceneOptions);
            string newScene = scenes[newIndex];

            if (!property.stringValue.Equals(newScene, StringComparison.Ordinal))
            {
                property.stringValue = scenes[newIndex];
            }
        }

        private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, string[] sceneOptions)
        {
            int index = property.intValue;
            int newIndex = EditorGUI.Popup(rect, label.text, index, sceneOptions);

            if (property.intValue != newIndex)
            {
                property.intValue = newIndex;
            }
        }

        private static int IndexOf(string[] scenes, string scene)
        {
            var index = Array.IndexOf(scenes, scene);
            return Mathf.Clamp(index, 0, scenes.Length - 1);
        }
    }
}
