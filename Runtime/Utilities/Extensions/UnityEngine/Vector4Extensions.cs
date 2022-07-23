using UnityEngine;
using Utilities.Types.Serializable;

namespace Utilities.Extensions
{
    /// <summary>
    /// Internal extension methods for <see cref="Vector4"/>.
    /// </summary>
    public static class Vector4Extensions
    {
        /// <summary>
        /// Returns a <see cref="Quaternion"/> instance where the component values are equal to this
        /// <see cref="Vector4"/>'s components.
        /// </summary>
        /// <param name="thisVector"></param>
        /// <returns></returns>
        public static Quaternion ToQuaternion(this Vector4 thisVector) => (SerializableQuaternion) thisVector;
    }
}
