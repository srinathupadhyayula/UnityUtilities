using System;

namespace Utilities.Attributes
{
    /// <summary>
    /// <para>This is a child class of AttachPropertyAttribute.</para>
    /// <para>This attribute finds the component on the children of this gameobject and attaches it to the property field.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    [AttributeUsage(System.AttributeTargets.Field)]
    public class GetComponentInChildrenAttribute : AttachPropertyAttribute
    {
        public bool   IncludeInactive { get; private set; }
        public string ChildName;

        public GetComponentInChildrenAttribute(bool includeInactive = false)
        {
            IncludeInactive = includeInactive;
        }

        public GetComponentInChildrenAttribute(string childName)
        {
            ChildName = childName;
        }
    }
}