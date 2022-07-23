using System;
using System.Collections.Generic;
using UnityEditor;
using Utilities.Attributes;

namespace Utilities.Validators.PropertyValidators
{
    /// <summary>
    /// This class is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    public abstract class PropertyValidatorBase
    {
        public abstract void ValidateProperty(SerializedProperty property);
    }

    public static class ValidatorAttributeExtensions
    {
        private static Dictionary<Type, PropertyValidatorBase> s_validatorsByAttributeType;

        static ValidatorAttributeExtensions()
        {
            s_validatorsByAttributeType = new Dictionary<Type, PropertyValidatorBase>();
            s_validatorsByAttributeType[typeof(MinValueAttribute)] = new MinValuePropertyValidator();
            s_validatorsByAttributeType[typeof(MaxValueAttribute)] = new MaxValuePropertyValidator();
            s_validatorsByAttributeType[typeof(RequiredAttribute)] = new RequiredPropertyValidator();
            s_validatorsByAttributeType[typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator();
        }

        public static PropertyValidatorBase GetValidator(this ValidatorAttribute attr)
        {
            PropertyValidatorBase validator;
            if (s_validatorsByAttributeType.TryGetValue(attr.GetType(), out validator))
            {
                return validator;
            }
            else
            {
                return null;
            }
        }
    }
}
