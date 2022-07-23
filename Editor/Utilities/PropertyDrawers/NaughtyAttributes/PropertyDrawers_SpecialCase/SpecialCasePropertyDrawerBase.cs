using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities.Attributes;
using Utilities.Validators.PropertyValidators;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// This class is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    public abstract class SpecialCasePropertyDrawerBase
    {
        public void OnGUI(Rect rect, SerializedProperty property)
        {
            // Check if visible
            bool visible = PropertyUtility.IsVisible(property);
            if (!visible)
            {
                return;
            }

            // Validate
            ValidatorAttribute[] validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
            foreach (var validatorAttribute in validatorAttributes)
            {
                validatorAttribute.GetValidator().ValidateProperty(property);
            }

            // Check if enabled and draw
            EditorGUI.BeginChangeCheck();
            bool enabled = PropertyUtility.IsEnabled(property);

            using (new EditorGUI.DisabledScope(disabled: !enabled))
            {
                OnGUI_Internal(rect, property, PropertyUtility.GetLabel(property));
            }

            // Call OnValueChanged callbacks
            if (EditorGUI.EndChangeCheck())
            {
                PropertyUtility.CallOnValueChangedCallbacks(property);
            }
        }

        public float GetPropertyHeight(SerializedProperty property)
        {
            return GetPropertyHeight_Internal(property);
        }

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);
        protected abstract float GetPropertyHeight_Internal(SerializedProperty property);
    }

    public static class SpecialCaseDrawerAttributeExtensions
    {
        private static Dictionary<Type, SpecialCasePropertyDrawerBase> s_drawersByAttributeType;

        static SpecialCaseDrawerAttributeExtensions()
        {
            s_drawersByAttributeType = new Dictionary<Type, SpecialCasePropertyDrawerBase>();
            s_drawersByAttributeType[typeof(ReorderableListAttribute)] = ReorderableListPropertyDrawer.Instance;
        }

        public static SpecialCasePropertyDrawerBase GetDrawer(this SpecialCaseDrawerAttribute attr)
        {
            SpecialCasePropertyDrawerBase drawer;
            if (s_drawersByAttributeType.TryGetValue(attr.GetType(), out drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }
    }
}
