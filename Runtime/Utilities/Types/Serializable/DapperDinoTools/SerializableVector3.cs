using System;
using UnityEngine;

namespace Utilities.Types.Serializable
{
    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString() => string.Format($"[{x}, {y}, {z}]");

        public static implicit operator Vector3(SerializableVector3 value) => new(value.x, value.y, value.z);
        public static implicit operator SerializableVector3(Vector3 value) => new(value.x, value.y, value.z);
    }
}
