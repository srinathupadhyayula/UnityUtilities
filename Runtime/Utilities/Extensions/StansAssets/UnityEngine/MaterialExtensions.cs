using UnityEngine;

namespace Utilities.Extensions
{
    /// <summary>
    /// Unity `Material` extension methods.
    /// </summary>
    /// <remarks>
    /// Author: Stans Assets
    /// <see cref="https://github.com/StansAssets/com.stansassets.foundation"/>
    /// <seealso cref="https://github.com/StansAssets"/>
    /// <seealso cref="https://stansassets.com/"/>
    /// </remarks>
    public static partial class MaterialExtensions
    {
        /// <summary>
        /// Set's alpha channel for the Material `_Color` property
        /// </summary>
        /// <param name="material">Material to operate with.</param>
        /// <param name="value">Alpha channel value.</param>
        public static void SetAlpha(this Material material, float value)
        {
            if (material.HasProperty("_Color"))
            {
                var color = material.color;
                color.a = value;
                material.color = color;
            }
        }
    }
}
