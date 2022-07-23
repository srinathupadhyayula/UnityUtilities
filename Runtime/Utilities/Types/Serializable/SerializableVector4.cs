using System;
using UnityEngine;

namespace Utilities.Types.Serializable
{
    [Serializable]
    public struct SerializableVector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public override string ToString() => string.Format($"[{x}, {y}, {z}, {w}]");
        
        public static implicit operator SerializableVector4(Vector4    value) => new(value.x, value.y, value.z, value.w);
        public static implicit operator SerializableVector4(Quaternion value) => new(value.x, value.y, value.z, value.w);

        public static implicit operator Vector4(SerializableVector4    value) => new(value.x, value.y, value.z, value.w);
        public static implicit operator Quaternion(SerializableVector4 value) => new(value.x, value.y, value.z, value.w);
    }
}