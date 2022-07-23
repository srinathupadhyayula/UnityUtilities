using System;
using UnityEngine;

namespace Utilities.Attributes
{
    /// <summary>
    /// <para> This attribute allows the user to assign assets from inspector while internally converting
    /// the asset to a string containg the asset path.</para>
    /// </summary>
    /// <remarks>
    /// <para>We limit this attributes to fields and only allow one. Should
    /// only be applied to string types.</para>
    /// <para>Author: Byron Mayne</para>
    /// <see cref="https://github.com/ByronMayne"/>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class AssetPath : PropertyAttribute
    {
        private AssetPathTypes m_pathType;
        private Type  m_type;

        /// <summary>
        /// Gets the type of asset path this attribute is watching.
        /// </summary>
        public AssetPathTypes PathType => m_pathType;

        /// <summary>
        /// Gets the type of asset this attribute is expecting.
        /// </summary>
        public Type Type => m_type;

        /// <summary>
        /// Creates the default instance of AssetPathAttribute
        /// </summary>
        public AssetPath(Type type)
        {
            m_type     = type;
            m_pathType = AssetPathTypes.Project;
        }

        private static string SuperProperty => "Complex string example"; 
            

        public void Evaluate()
        {
            string value = SuperProperty;
        }

    }
}