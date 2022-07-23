﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Utilities.Attributes;
using Object = UnityEngine.Object;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// <para> PropertyDrawer class for the AssetPath attribute.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Byron Mayne</para>
    /// <see cref="https://github.com/ByronMayne"/>
    /// </remarks>
    [CustomPropertyDrawer(typeof(AssetPath))]
    public class AssetPathDrawer : PropertyDrawer
    {
        // A helper warning label when the user puts the attribute above a non string type.
        private const           string     k_invalidTypeLabel  = "Attribute invalid for type ";
        private const           string     k_typeMustBeString  = " Type must be string!";
        private const           float      k_buttonWidth       = 80f;
        private static readonly int        PPtrHash            = "PPtrHash".GetHashCode();
        private                 int        m_pickerControlID   = -1;
        private static          GUIContent s_missingAssetLabel = new("Missing");
        
        private string m_activePickerPropertyPath;
        // A shared array of references to the objects we have loaded
        private IDictionary<string, Object> m_references;


        /// <summary>
        /// Invoked when unity creates our drawer. 
        /// </summary>
        public AssetPathDrawer()
        {
            m_references = new Dictionary<string, Object>();
        }

        /// <summary>
        /// Invoked when we want to try our property. 
        /// </summary>
        /// <param name="position">The position we have allocated on screen</param>
        /// <param name="property">The field our attribute is over</param>
        /// <param name="label">The nice display label it has</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property = GetProperty(property);
            if (property.propertyType != SerializedPropertyType.String)
            {
                // Create a rect for our label
                Rect labelPosition = position;
                // Set it's width 
                labelPosition.width = EditorGUIUtility.labelWidth;
                // Draw it
                GUI.Label(labelPosition, label);
                // Create a rect for our content
                Rect contentPosition = position;
                // Move it over by the x
                contentPosition.x += labelPosition.width;
                // Shrink it in width since we moved it over
                contentPosition.width -= labelPosition.width;
                // Draw our content warning;
                EditorGUI.HelpBox(contentPosition, k_invalidTypeLabel + this.fieldInfo.FieldType.Name + k_typeMustBeString, MessageType.Error);
            }
            else
            {
                HandleObjectReference(position, property, label);
            }

        }

        /// <summary>
        /// Due to the fact that ShowObjectPicker does not have a non generic version we
        /// have to use reflection to create and invoke it.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        private void ShowObjectPicker(Type type, Rect position)
        {
            // Get the type
            Type classType = typeof(EditorGUIUtility);
            // Get the method
            MethodInfo showObjectPickerMethod = classType.GetMethod("ShowObjectPicker", BindingFlags.Public | BindingFlags.Static);
            // Make the generic version
            MethodInfo genericObjectPickerMethod = showObjectPickerMethod?.MakeGenericMethod(type);
            // We have no starting target
            Object target = null;
            // We are not allowing scene objects 
            var allowSceneObjects = false;
            // An empty filter
            string searchFilter = string.Empty;
            // Make a control ID
            m_pickerControlID = GUIUtility.GetControlID(PPtrHash, FocusType.Passive, position);
            // Save our property path
            // Invoke it (We have to do this step since there is only a generic version for showing the asset picker.
            genericObjectPickerMethod?.Invoke(null, new object[] { target, allowSceneObjects, searchFilter, m_pickerControlID });
        }

        protected virtual SerializedProperty GetProperty(SerializedProperty rootProperty)
        {
            return rootProperty;
        }


        protected virtual Type ObjectType()
        {
            // Get our attribute
            var assetPathAttribute = attribute as AssetPath;
            // Return back the type.
            return assetPathAttribute?.Type;
        }

        private void HandleObjectReference(Rect position, SerializedProperty property, GUIContent label)
        {

            Type objectType = ObjectType();
            // First get our value
            Object propertyValue = null;
            // Save our path
            string assetPath = property.stringValue;
            // Have a label to say it's missing
            //bool isMissing = false;
            // Check if we have a key
            if (m_references.ContainsKey(property.propertyPath))
            {
                // Get the value. 
                propertyValue = m_references[property.propertyPath];
            }
            // Now if its null we try to load it
            if (propertyValue == null && !string.IsNullOrEmpty(assetPath))
            {
                // Try to load our asset
                propertyValue = AssetDatabase.LoadAssetAtPath(assetPath, objectType);

                if (propertyValue == null)
                {
                    //isMissing = true;
                }
                else
                {
                    m_references[property.propertyPath] = propertyValue;
                }
            }

            EditorGUI.BeginChangeCheck();
            {
                // Draw our object field.
                propertyValue = EditorGUI.ObjectField(position, label, propertyValue, objectType, false);
            }
            if (EditorGUI.EndChangeCheck())
            {
                OnSelectionMade(propertyValue, property);
            }
        }

        protected virtual void OnSelectionMade(Object newSelection, SerializedProperty property)
        {
            string assetPath = string.Empty;

            if (newSelection != null)
            {
                // Get our path
                assetPath = AssetDatabase.GetAssetPath(newSelection);
            }

            // Save our value.
            m_references[property.propertyPath] = newSelection;
            // Save it back
            property.stringValue = assetPath;
        }
    }
}