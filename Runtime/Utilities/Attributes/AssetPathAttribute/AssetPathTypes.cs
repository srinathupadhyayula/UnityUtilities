namespace Utilities.Attributes
{
    /// /// <summary>
    /// <para> A enum containing all the types of paths we can watch.</para>
    /// </summary>
    /// <remarks>
    /// <para>Author: Byron Mayne</para>
    /// <see cref="https://github.com/ByronMayne"/>
    /// </remarks>
    public enum AssetPathTypes
    {
        /// <summary>
        /// The path will be contained within the 'Asset/*' directory.
        /// </summary>
        Project,

        /// <summary>
        /// The path will be contained within a resources folder.
        /// </summary>
        Resources
    }

}