using System;

namespace Utilities.Attributes
{
    /// <summary>
    /// <para>This is a child class of AttachPropertyAttribute.</para>
    /// <para>This attribute adds the component to the gameobject.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    [AttributeUsage(System.AttributeTargets.Field)] public class AddComponentAttribute : AttachPropertyAttribute { }
}