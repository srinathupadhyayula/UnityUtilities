using System;

namespace Utilities.Attributes
{
    /// <summary>
    /// This class is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowAssetPreviewAttribute : DrawerAttribute
    {
        public const int DEFAULT_WIDTH = 64;
        public const int DEFAULT_HEIGHT = 64;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public ShowAssetPreviewAttribute(int width = DEFAULT_WIDTH, int height = DEFAULT_HEIGHT)
        {
            Width = width;
            Height = height;
        }
    }
}
