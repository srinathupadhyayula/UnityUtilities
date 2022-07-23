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
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ProgressBarAttribute : DrawerAttribute
    {
        public string Name { get; private set; }
        public float MaxValue { get; set; }
        public string MaxValueName { get; private set; }
        public EColor Color { get; private set; }

        public ProgressBarAttribute(string name, float maxValue, EColor color = EColor.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;
        }

        public ProgressBarAttribute(string name, string maxValueName, EColor color = EColor.Blue)
        {
            Name = name;
            MaxValueName = maxValueName;
            Color = color;
        }

        public ProgressBarAttribute(float maxValue, EColor color = EColor.Blue)
            : this("", maxValue, color)
        {
        }

        public ProgressBarAttribute(string maxValueName, EColor color = EColor.Blue)
            : this("", maxValueName, color)
        {
        }
    }
}
