using System;

namespace Utilities.Attributes
{
    /// <summary>
    /// <para>This is a child class of AttachPropertyAttribute.</para>
    /// <para>This attribute finds a gameobject with the component and attaches to the property.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    [AttributeUsage(System.AttributeTargets.Field)] public class FindObjectOfTypeAttribute : AttachPropertyAttribute { }
}