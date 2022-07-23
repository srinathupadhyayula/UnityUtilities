using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities.Types
{
    /// <summary>
    /// Ranged float with values for Min and Max
    ///
    /// <remarks>
    /// <para>Based on the <see cref="https://github.com/Alscenic/shared-unity-utils/blob/main/Runtime/Scripts/Classes/RangedFloat.cs">
    /// RangedFloat</see> class by <see cref="https://github.com/Alscenic">Kyle Lamothe</see> in his
    /// <see cref="https://github.com/Alscenic/shared-unity-utils">shared-unity-utils</see> repository on github</para>
    /// </remarks>
    /// </summary>
    [Serializable]
    public abstract class Ranged<T>
        where T : unmanaged, IComparable, IConvertible
      , IComparable<T>, IEquatable<T>, IFormattable
    {
        protected bool Equals(Ranged<T> other)
        {
            return Min.Equals(other.Min)
                && Max.Equals(other.Max);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Ranged<T>) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        protected abstract T RandomWithin { get; } // => Random.Range(Min, Max);

        /// <summary>
        /// Gets or sets the min.
        /// </summary>
        [field:SerializeField]
        public T Min { get; set; }

        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        [field:SerializeField]
        public T Max {  get; set; }

        public override string ToString()
        {
            return "(" + Min + "/" + Max + ")";
        }
    }

    [Serializable]
    public sealed class RangedFloat : Ranged<float>
    {
        protected override float RandomWithin => Random.Range(Min, Max);
    }

    [Serializable]
    public sealed class RangedDouble : Ranged<double>
    {
        protected override double RandomWithin => Random.Range((float)Min, (float)Max);
    }
    
    [Serializable]
    public sealed class RangedInt : Ranged<int>
    {
        protected override int RandomWithin => Random.Range(Min, Max);
    }

    [Serializable]
    public sealed class RangedShort : Ranged<short>
    {
        protected override short RandomWithin => (short) Random.Range(Min, Max);
    }
    
    [Serializable]
    public sealed class RangedLong : Ranged<long>
    {
        protected override long RandomWithin => (long) Random.Range(Min, Max);
    }
    
    [Serializable]
    public sealed class RangedByte : Ranged<sbyte>
    {
        protected override sbyte RandomWithin => (sbyte) Random.Range(Min, Max);
    }
    
    [Serializable]
    public sealed class RangedUInt : Ranged<uint>
    {
       protected override uint RandomWithin => (uint) Random.Range(Min, Max);
    }

    [Serializable]
    public sealed class RangedUShort : Ranged<ushort>
    {
        protected override ushort RandomWithin => (ushort) Random.Range(Min, Max);
    }
    
    [Serializable]
    public sealed class RangedULong : Ranged<ulong>
    {
        protected override ulong RandomWithin => (ulong) Random.Range(Min, Max);
    }
    
    [Serializable]
    public sealed class RangedUByte : Ranged<byte>
    {
        protected override byte RandomWithin => (byte) Random.Range(Min, Max);
    }
}