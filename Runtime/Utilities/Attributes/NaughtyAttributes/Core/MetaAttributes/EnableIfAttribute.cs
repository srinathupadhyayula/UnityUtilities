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
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EnableIfAttribute : ConditionalAttribute
    {
        public EnableIfAttribute(string condition)
            : base(condition)
        {
            Inverted = false;
        }

        public EnableIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = false;
        }

        public EnableIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = false;
        }
    }
}
