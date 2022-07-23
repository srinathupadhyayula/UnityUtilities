using System;

namespace Utilities.Attributes
{
    /// <summary>
    /// <para>This is a child class of AttachPropertyAttribute.</para>
    /// <para>This attribute finds the component on the parent of this gameobject and attaches it to the property field.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    [AttributeUsage(System.AttributeTargets.Field)] public class GetComponentInParent : AttachPropertyAttribute { }
}