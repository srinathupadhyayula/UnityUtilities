using UnityEditor.Graphs;
using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// Attempts to make a color struct from the html color string.
        /// If parsing is failed magenta color will be returned.
        ///
        /// Strings that begin with '#' will be parsed as hexadecimal in the following way:
        /// #RGB (becomes RRGGBB)
        /// #RRGGBB
        /// #RGBA (becomes RRGGBBAA)
        /// #RRGGBBAA
        ///
        /// When not specified alpha will default to FF.
        ///     Strings that do not begin with '#' will be parsed as literal colors, with the following supported:
        /// red, cyan, blue, darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta..
        /// </summary>
        /// <param name="htmlString">Case insensitive html string to be converted into a color.</param>
        /// <returns>The converted color.</returns>
        public static Color MakeColorFromHtml(this string htmlString)
        {
            return MakeColorFromHtml(htmlString, Color.magenta); // Modify this to use a global editable(in editor) constant instead of magenta
        }

        /// <summary>
        /// Attempts to make a color struct from the html color string.
        /// If parsing is failed <see cref="fallbackColor"/> color will be returned.
        ///
        /// Strings that begin with '#' will be parsed as hexadecimal in the following way:
        /// #RGB (becomes RRGGBB)
        /// #RRGGBB
        /// #RGBA (becomes RRGGBBAA)
        /// #RRGGBBAA
        ///
        /// When not specified alpha will default to FF.
        ///     Strings that do not begin with '#' will be parsed as literal colors, with the following supported:
        /// red, cyan, blue, darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta..
        /// </summary>
        /// <param name="htmlString">Case insensitive html string to be converted into a color.</param>
        /// <param name="fallbackColor">Color to fall back to in case the parsing is failed.</param>
        /// <returns>The converted color.</returns>
        public static Color MakeColorFromHtml(this string htmlString, Color fallbackColor)
        {
            return ColorUtility.TryParseHtmlString(htmlString, out Color color) ? color : fallbackColor;
        }
    }
}