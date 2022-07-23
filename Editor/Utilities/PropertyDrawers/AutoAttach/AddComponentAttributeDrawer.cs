using System;
using UnityEditor;
using UnityEngine;
using Utilities.Attributes;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// <para>This is a property drawer for AddComponentAttribute.</para>
    /// <para>This is a child class of AttachAttributePropertyDrawer.</para>
    /// <para>This attribute adds the component to the gameobject, and is called from
    /// within the AttachAttributePropertyDrawer if the component is not attached.</para>
    /// </summary>
    /// <remarks>
    /// <para>Renamed the class from <i>XYZ<b>Editor</b></i> to <i>XYZ<b>Drawer</b></i> to clearly show that this is a <i>PropertyDrawer</i> class and <b>not</b> an <i>Editor</i> class</para>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    [CustomPropertyDrawer(typeof(AddComponentAttribute))]
    public class AddComponentAttributeDrawer : AttachAttributePropertyDrawer
    {
        /// <summary>
        /// This method adds the component to the gameobject
        /// </summary>
        /// <param name="property">The serialized property of the component</param>
        /// <param name="go"> the gameobject to which the component belongs to</param>
        /// <param name="type">the type of the property field</param>
        protected override void UpdateProperty(SerializedProperty property, GameObject go, Type type)
        {
            property.objectReferenceValue = go.AddComponent(type);
        }
    }
}