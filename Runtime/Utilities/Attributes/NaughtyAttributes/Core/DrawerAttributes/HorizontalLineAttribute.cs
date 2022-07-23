using System;
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
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class HorizontalLineAttribute : DrawerAttribute
    {
        public const float DEFAULT_HEIGHT = 2.0f;
        public const EColor DEFAULT_COLOR = EColor.Gray;

        public float Height { get; private set; }
        public EColor Color { get; private set; }

        public HorizontalLineAttribute(float height = DEFAULT_HEIGHT, EColor color = DEFAULT_COLOR)
        {
            Height = height;
            Color = color;
        }
    }
}
