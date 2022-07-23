using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Utilities.Attributes;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// <para>This is a property drawer for AddComponentAttribute.</para>
    /// <para>This is a child class of AttachAttributePropertyDrawer.</para>
    /// <para>This attribute FindObjectOfType and assigns it to the property.</para>
    /// </summary>
    /// <remarks>
    /// <para>Renamed the class from <i>XYZ<b>Editor</b></i> to <i>XYZ<b>Drawer</b></i> to clearly show that this is a <i>PropertyDrawer</i> class and <b>not</b> an <i>Editor</i> class</para>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    [CustomPropertyDrawer(typeof(FindObjectOfTypeAttribute))]
    public class FindObjectOfTypeAttributeDrawer : AttachAttributePropertyDrawer
    {
        protected override void UpdateProperty(SerializedProperty property, GameObject go, Type type)
        {
            property.objectReferenceValue = FindObjectsOfTypeByName(property.GetPropertyType());
        }

        public UnityEngine.Object FindObjectsOfTypeByName(string aClassName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                for (int n = 0; n < types.Length; n++)
                {
                    if (typeof(UnityEngine.Object).IsAssignableFrom(types[n]) && aClassName == types[n].Name)
                        return UnityEngine.Object.FindObjectOfType(types[n]);
                }
            }
            return new UnityEngine.Object();
        }
    }
}