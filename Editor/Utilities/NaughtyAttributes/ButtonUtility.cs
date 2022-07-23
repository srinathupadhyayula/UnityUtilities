using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utilities.Attributes;

namespace Utilities
{
    /// <summary>
    /// This class is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    public static class ButtonUtility
    {
        public static bool IsEnabled(Object target, MethodInfo method)
        {
            // Changed from ShowIfAttributeBase to ConditionalAttribute as per the changes made to the type. - Srinath Upadhyayula
            ConditionalAttribute enableIfAttribute = method.GetCustomAttribute<ConditionalAttribute>();
            if (enableIfAttribute == null)
            {
                return true;
            }

            List<bool> conditionValues = PropertyUtility.GetConditionValues(target, enableIfAttribute.Conditions);
            if (conditionValues.Count > 0)
            {
                bool enabled = PropertyUtility.GetConditionsFlag(conditionValues, enableIfAttribute.ConditionOperator, enableIfAttribute.Inverted);
                return enabled;
            }
            else
            {
                string message = enableIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
                Debug.LogWarning(message, target);

                return false;
            }
        }

        public static bool IsVisible(Object target, MethodInfo method)
        {
            ConditionalAttribute showIfAttribute = method.GetCustomAttribute<ConditionalAttribute>();
            if (showIfAttribute == null)
            {
                return true;
            }

            List<bool> conditionValues = PropertyUtility.GetConditionValues(target, showIfAttribute.Conditions);
            if (conditionValues.Count > 0)
            {
                bool enabled = PropertyUtility.GetConditionsFlag(conditionValues, showIfAttribute.ConditionOperator, showIfAttribute.Inverted);
                return enabled;
            }
            else
            {
                string message = showIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
                Debug.LogWarning(message, target);

                return false;
            }
        }
    }
}
