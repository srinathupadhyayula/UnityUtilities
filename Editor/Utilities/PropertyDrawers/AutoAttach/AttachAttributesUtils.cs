using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// <para>AUtility class for AttachAttribute.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    public static class AttachAttributesUtils
    {
        private const string k_editorPrefsAttachAttributesGlobal = "IsAttachAttributesActive";

        public static bool IsEnabled
        {
            get => EditorPrefs.GetBool(k_editorPrefsAttachAttributesGlobal, true);
            set
            {
                if (value)
                    EditorPrefs.DeleteKey(k_editorPrefsAttachAttributesGlobal);
                else
                    EditorPrefs.SetBool(k_editorPrefsAttachAttributesGlobal, value); // clear value if it's equals defaultValue
            }
        }

        public static string GetPropertyType(this SerializedProperty property)
        {
            string type  = property.type;
            Match  match = Regex.Match(type, @"PPtr<\$(.*?)>");
            if (match.Success)
                type = match.Groups[1].Value;
            return type;
        }

        public static Type StringToType(this string aClassName) =>
            System.AppDomain.CurrentDomain.GetAssemblies()
                  .SelectMany(x => x.GetTypes())
                  .First(x => x.IsSubclassOf(typeof(Component)) && x.Name == aClassName);
    }
}