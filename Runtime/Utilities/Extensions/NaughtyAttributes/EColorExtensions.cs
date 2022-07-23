using UnityEngine;
using Utilities.Types;

namespace Utilities.Extensions
{
    /// <summary>
    /// Extension methods for <i><b>EColor</b></i> from the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    public static class EColorExtensions
    {
        /// <summary>
        /// Returns a UnityEngine.Color value from the EColor value 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color GetColor(this EColor color)
        {
            return color switch
                   {
                       EColor.Clear  => new Color(0,   0,   0,   0)
                     , EColor.White  => new Color(255, 255, 255, 255)
                     , EColor.Black  => new Color(0,   0,   0,   255)
                     , EColor.Gray   => new Color(128, 128, 128, 255)
                     , EColor.Red    => new Color(255, 0,   63,  255)
                     , EColor.Pink   => new Color(255, 152, 203, 255)
                     , EColor.Orange => new Color(255, 128, 0,   255)
                     , EColor.Yellow => new Color(255, 211, 0,   255)
                     , EColor.Green  => new Color(98,  200, 79,  255)
                     , EColor.Blue   => new Color(0,   135, 189, 255)
                     , EColor.Indigo => new Color(75,  0,   130, 255)
                     , EColor.Violet => new Color(128, 0,   255, 255)
                     , _             => new Color(0,   0,   0,   255)
                   };
        }
    }
}