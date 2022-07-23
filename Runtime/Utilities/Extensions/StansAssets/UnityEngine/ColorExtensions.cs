using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class ColorExtensions
    {
        public static Color GetRedGreen(this  Color color) => new(color.r, color.g, 0f);
        public static Color GetRedBlue(this   Color color) => new(color.r, 0f, color.b);
        public static Color GetGreenBlue(this Color color) => new(0f, color.g, color.b);
        public static Color SwapRedGreen(this Color color) => new(color.g, color.r, color.b);
        public static Color SwapRedBlue(this  Color color) => new(r:color.b, g:color.g, b:color.r);
        public static Color ReverseRGB(this   Color color) => color.SwapRedBlue();
        
        public static Color GetRandomColor() => new Color(Random.Range(0f, 1f)
                                                        , Random.Range(0f, 1f)
                                                        , Random.Range(0f, 1f), 1f);

        public static Color RandomizeAlpha(this Color color) => new(r: color.r, g: color.g, b: color.b, a: Random.Range(0f, 1f));
    }
}