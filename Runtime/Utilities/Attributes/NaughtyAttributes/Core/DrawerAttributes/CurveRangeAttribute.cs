using System;
using UnityEngine;
using Utilities.Types;

namespace Utilities.Attributes
{
    /// <summary>
    /// This class is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class CurveRangeAttribute : DrawerAttribute
    {
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public EColor Color { get; private set; }

        public CurveRangeAttribute(Vector2 min, Vector2 max, EColor color = EColor.Clear)
        {
            Min = min;
            Max = max;
            Color = color;
        }

        public CurveRangeAttribute(EColor color)
            : this(Vector2.zero, Vector2.one, color)
        {
        }

        public CurveRangeAttribute(float minX, float minY, float maxX, float maxY, EColor color = EColor.Clear)
            : this(new Vector2(minX, minY), new Vector2(maxX, maxY), color)
        {
        }
    }
}
