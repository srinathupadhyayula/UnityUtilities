using UnityEngine;

namespace Utilities.Extensions
{
    /// <summary>
    /// Unity `Object` extension methods.
    /// </summary>
    /// <remarks>
    /// Author: Stans Assets
    /// <see cref="https://github.com/StansAssets/com.stansassets.foundation"/>
    /// <seealso cref="https://github.com/StansAssets"/>
    /// <seealso cref="https://stansassets.com/"/>
    /// </remarks>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Performs a TRUE null-check.
        /// See http://answers.unity.com/answers/1224404/view.html
        /// </summary>
        /// <param name="obj">An object to check.</param>
        /// <returns>Returns <c>true</c> if object is null, <c>false</c> otherwise.</returns>
        public static bool IsNull(this Object obj)
        {
            var  sysObj = (object) obj;
            bool isNull = sysObj.Equals(null);
            return isNull;
        }

        /// <summary>
        /// Performs a TRUE null-check.
        /// See http://answers.unity.com/answers/1224404/view.html
        /// </summary>
        /// <param name="obj">An object to check.</param>
        /// <returns>Returns <c>false</c> if object is null, <c>true</c> otherwise.</returns>
        public static bool IsNotNull(this Object obj)
        {
            return !IsNull(obj);
        }
    }
}