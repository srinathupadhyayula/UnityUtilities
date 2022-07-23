using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Utilities.Extensions
{
    /// <summary>
    /// Unity `GameObject` extension methods.
    /// </summary>
    /// <remarks>
    /// Author: Stans Assets
    /// <see cref="https://github.com/StansAssets/com.stansassets.foundation"/>
    /// <seealso cref="https://github.com/StansAssets"/>
    /// <seealso cref="https://stansassets.com/"/>
    /// </remarks>
    public static partial class GameObjectExtensions
    {
        /// <summary>
        /// Set layer to all GameObject children, including inactive.
        /// </summary>
        /// <param name="gameObject">Target GameObject.</param>
        /// <param name="layerNumber">Layer number.</param>
        public static void SetLayerRecursively(this GameObject gameObject, int layerNumber)
        {
            foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layerNumber;
            }
        }
        
        /// <summary>
        /// Gets the local Identifier In File, for the given GameObject
        /// Return 0 in case Game Object wasn't yet saved
        /// </summary>
        /// <param name="go">GameObject you want to check</param>
        public static int GetLocalIdentifierInFile(GameObject go)
        {
#if UNITY_EDITOR
            PropertyInfo inspectorModeInfo = typeof(UnityEditor.SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
            var serializedObject = new UnityEditor.SerializedObject(go);
            inspectorModeInfo?.SetValue(serializedObject, UnityEditor.InspectorMode.Debug, null);
            SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile"); //note the misspelling!
            return localIdProp.intValue;
#else
            return 0;
#endif
        }
        
        /// <summary>
        /// Renderer Bounds of the game object.
        /// </summary>
        /// <param name="go">GameObject you want calculate bounds for.</param>
        /// <returns>Calculated game object bounds.</returns>
        public static Bounds GetRendererBounds(this GameObject go)
        {
            return CalculateBounds(go);
        }

        private static Bounds CalculateBounds(GameObject obj)
        {
            var        hasBounds        = false;
            var        bounds           = new Bounds(Vector3.zero, Vector3.zero);
            Renderer[] childrenRenderer = obj.GetComponentsInChildren<Renderer>();


            Renderer rnd = obj.GetComponent<Renderer>();
            if (rnd != null)
            {
                bounds = rnd.bounds;
                hasBounds = true;
            }

            foreach (Renderer child in childrenRenderer)
                if (!hasBounds)
                {
                    bounds = child.bounds;
                    hasBounds = true;
                }
                else
                {
                    bounds.Encapsulate(child.bounds);
                }

            return bounds;
        }
    }
    
}
