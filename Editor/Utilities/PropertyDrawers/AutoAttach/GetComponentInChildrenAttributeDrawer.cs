using System;
using UnityEditor;
using UnityEngine;
using Utilities.Attributes;

namespace Utilities.PropertyDrawers
{
    #region Attribute Editors

    /// <summary>
    /// <para>This is a property drawer for AddComponentAttribute.</para>
    /// <para>This is a child class of AttachAttributePropertyDrawer.</para>
    /// <para>This attribute uses the GetComponentInChildren method of the game object and assigns it to the property.</para>
    /// </summary>
    /// <remarks>
    /// <para>Renamed the class from <i>XYZ<b>Editor</b></i> to <i>XYZ<b>Drawer</b></i> to clearly show that this is a <i>PropertyDrawer</i> class and <b>not</b> an <i>Editor</i> class</para>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    [CustomPropertyDrawer(typeof(GetComponentInChildrenAttribute))]
    public class GetComponentInChildrenAttributeDrawer : AttachAttributePropertyDrawer
    {
        protected override void UpdateProperty(SerializedProperty property, GameObject go, Type type)
        {
            GetComponentInChildrenAttribute labelAttribute = (GetComponentInChildrenAttribute)attribute;
            if (labelAttribute.ChildName == null)
            {
                property.objectReferenceValue = go.GetComponentInChildren(type, labelAttribute.IncludeInactive);
            }
            else
            {
                var child = go.transform.Find(labelAttribute.ChildName);
                if (child != null)
                {
                    property.objectReferenceValue = child.GetComponent(type);
                }
            }
        }
    }

    #endregion
}