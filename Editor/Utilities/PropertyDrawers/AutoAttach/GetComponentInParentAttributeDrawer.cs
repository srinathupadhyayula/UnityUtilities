using System;
using UnityEditor;
using UnityEngine;
using Utilities.Attributes;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// <para>This is a property drawer for AddComponentAttribute.</para>
    /// <para>This is a child class of AttachAttributePropertyDrawer.</para>
    /// <para>This attribute gets the component in parent object by calling GetComponent method on the  gameobject's transform.parent; and assigns it to the property.</para>
    /// </summary>
    /// <remarks>
    /// <para>Renamed the class from <i>XYZ<b>Editor</b></i> to <i>XYZ<b>Drawer</b></i> to clearly show that this is a <i>PropertyDrawer</i> class and <b>not</b> an <i>Editor</i> class</para>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    [CustomPropertyDrawer(typeof(GetComponentInParent))]
    public class GetComponentInParentAttributeDrawer : AttachAttributePropertyDrawer
    {
        protected override void UpdateProperty(SerializedProperty property, GameObject go, Type type)
        {
            if (go.transform.parent != null)
                property.objectReferenceValue = go.transform.parent.gameObject.GetComponent(type);
        }
    }
}