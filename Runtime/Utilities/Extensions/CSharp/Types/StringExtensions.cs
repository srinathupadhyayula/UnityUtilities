using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// Return the text ready to be displayed on the UI as the supplied colour
        /// </summary>
        /// <param name="thisString">The current string</param>
        /// <param name="colour">The color to set for the string</param>
        /// <returns>Html string with the specified color</returns>
        /// <remarks>
        /// <para>This method is heavily based on a similar method in <see cref="https://github.com/DapperDino">DapperDino</see>'s
        /// <see cref="https://github.com/DapperDino/Dapper-Tools">Dapper-Tools</see> package.</para>
        /// </remarks>
        public static string WithColour(this string thisString, Color colour)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(colour)}>{thisString}</color>";
        }

    }
}