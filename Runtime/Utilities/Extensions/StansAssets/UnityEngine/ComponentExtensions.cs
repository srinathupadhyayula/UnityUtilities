using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Utilities.Extensions
{
    /// <summary>
    /// Unity `Component` extension methods.
    /// </summary>
    /// <remarks>
    /// Author: Stans Assets
    /// <see cref="https://github.com/StansAssets/com.stansassets.foundation"/>
    /// <seealso cref="https://github.com/StansAssets"/>
    /// <seealso cref="https://stansassets.com/"/>
    /// </remarks>
    public static partial class ComponentExtensions
    {
        /// <summary>
        /// Gets the local Identifier In File, for the given Component
        /// Return 0 in case Component wasn't yet saved
        /// </summary>
        /// <param name="go">Component you want to check</param>
        public static int GetLocalIdentifierInFile(Component go)
        {
#if UNITY_EDITOR
            PropertyInfo inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
            var serializedObject = new SerializedObject(go);
            inspectorModeInfo?.SetValue(serializedObject, InspectorMode.Debug, null);
            SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile"); //note the misspelling!
            return localIdProp.intValue;
#else
            return 0;
#endif
        }
    }
}
