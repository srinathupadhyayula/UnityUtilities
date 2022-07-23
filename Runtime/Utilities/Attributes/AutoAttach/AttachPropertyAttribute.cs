using UnityEngine;

namespace Utilities.Attributes
{
    /// <summary>
    /// <para>The AttachPropertyAttribute is the base class for attaching a component to the property field.</para>
    /// <para>Child classes such as AddComponent and GetComponent, etc.,
    /// do the heavy lifting of finding and adding the component if required.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Alexander</para>
    /// <see cref="https://github.com/Nrjwolf"/>
    /// </remarks>
    public class AttachPropertyAttribute : PropertyAttribute { }
}