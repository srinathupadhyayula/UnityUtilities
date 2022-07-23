using System;
using UnityEngine;

namespace Utilities.Types.Serializable
{
    [Serializable]
    public struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public override string ToString() => string.Format($"[{x}, {y}, {z}, {w}]");

        public static implicit operator Vector4(SerializableQuaternion             value) => new(value.x, value.y, value.z, value.w);
        public static implicit operator Quaternion(SerializableQuaternion          value) => new(value.x, value.y, value.z, value.w);
        public static implicit operator SerializableVector4(SerializableQuaternion value) => new(value.x, value.y, value.z, value.w);
        
        public static implicit operator SerializableQuaternion(Vector4             value) => new(value.x, value.y, value.z, value.w);
        public static implicit operator SerializableQuaternion(Quaternion          value) => new(value.x, value.y, value.z, value.w);
        public static implicit operator SerializableQuaternion(SerializableVector4 value) => new(value.x, value.y, value.z, value.w);
    }
}