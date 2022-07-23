using UnityEngine;
using Utilities.Types.Serializable;

namespace Utilities.Extensions
{
    /// <summary>
    /// Internal extension methods for <see cref="Quaternion"/>.
    /// </summary>
    public static class QuaternionExtensions
    {
        /// <summary>
        /// Returns a <see cref="Vector4"/> instance where the component values are equal to this
        /// <see cref="Quaternion"/>'s components.
        /// </summary>
        /// <param name="thisQuaternion"></param>
        /// <returns></returns>
        public static Vector4 ToVector4(this Quaternion thisQuaternion) => (SerializableVector4) thisQuaternion;
    }
}