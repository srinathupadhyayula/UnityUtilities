using System;
using System.Runtime.CompilerServices;
using Utilities.Types;

namespace Utilities.Extensions
{
    public static partial class MathExtensions
    {
        /// <summary>
        /// Rounds the float 'down' towards 0 by casting the value to int.
        /// </summary>
        /// <param name="floatToFloor">The float parameter to Floor and convert to int</param>
        /// <returns>Returns an int equal to the Ceil of <paramref name="floatToFloor"/></returns>
        /// <example>
        /// <br/>int returnVal = 0;
        /// <br/><paramref name="floatToFloor"/> = 1.2 => returnVal = 1;
        /// <br/><paramref name="floatToFloor"/> = -1.2 => returnVal = -1;
        /// </example>
        /// <remarks>
        /// <para>Casting the float to int truncates the float by removing everything after the decimal point. 
        /// This ensures that the method is always rounding the float towards 0, whether the float is +ve or -ve.</para>
        /// <br/>This method is based on the discussions on the
        /// <i href="https://forum.unity.com/threads/what-is-faster-to-convert-float-to-int-int-randomfloat-or-mathf-roundtoint-randomfloat.268731/">unity forums</i>
        /// and on <i href="https://math.stackexchange.com/questions/344815/how-do-the-floor-and-ceiling-functions-work-on-negative-numbers#:~:text=The%20correct%20answer%20is%20it%20depends%20how%20you,towards%20zero.%20Ceiling%20always%20rounding%20away%20from%20zero.">stackexchange forums</i>
        /// <i href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs"><br/><br/><b>SeeAlso: </b>UnityCSReference</i>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(this float floatToFloor) => (int) floatToFloor;
        
        /// <summary>
        /// Rounds the float 'up' away from 0 and casts the result to int.
        /// </summary>
        /// <param name="floatToCeil">The float parameter to Ceil and convert to int</param>
        /// <returns>Returns an int equal to the Ceil of <paramref name="floatToCeil"/></returns>
        /// <example>
        /// <br/>int returnVal = 0;
        /// <br/><paramref name="floatToCeil"/> = 1.2 => returnVal = 2;
        /// <br/><paramref name="floatToCeil"/> = -1.2 => returnVal = -2;
        /// </example>
        /// <remarks>This method is based on the discussions on the
        /// <i href="https://forum.unity.com/threads/what-is-faster-to-convert-float-to-int-int-randomfloat-or-mathf-roundtoint-randomfloat.268731/">unity forums</i>
        /// and on <i href="https://math.stackexchange.com/questions/344815/how-do-the-floor-and-ceiling-functions-work-on-negative-numbers#:~:text=The%20correct%20answer%20is%20it%20depends%20how%20you,towards%20zero.%20Ceiling%20always%20rounding%20away%20from%20zero.">stackexchange forums</i>
        /// <i href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs"><br/><br/><b>SeeAlso: </b>UnityCSReference</i>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilToInt(this float floatToCeil) => floatToCeil >= 0 ? (int) (floatToCeil + 1f) : (int) (floatToCeil - 1f);

        /// <summary>
        /// Rounds the float to the closets integer value.
        /// </summary>
        /// <param name="floatToRound">The float parameter to Round and convert to int</param>
        /// <returns>Returns an int nearest to the value of <paramref name="floatToRound"/></returns>
        /// <example>
        /// <br/>int returnVal = 0;
        /// <br/><paramref name="floatToRound"/> = 1.2 => returnVal = 1
        /// <br/><paramref name="floatToRound"/> = -1.2 => returnVal = -1
        /// <br/><paramref name="floatToRound"/> = 1.7 => returnVal = 2
        /// <br/><paramref name="floatToRound"/> = -1.7 => returnVal = -2
        /// </example>
        /// <remarks>This method is based on the discussions on the
        /// <i href="https://forum.unity.com/threads/what-is-faster-to-convert-float-to-int-int-randomfloat-or-mathf-roundtoint-randomfloat.268731/">unity forums</i>
        /// and on <i href="https://math.stackexchange.com/questions/344815/how-do-the-floor-and-ceiling-functions-work-on-negative-numbers#:~:text=The%20correct%20answer%20is%20it%20depends%20how%20you,towards%20zero.%20Ceiling%20always%20rounding%20away%20from%20zero.">stackexchange forums</i>
        /// <i href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs"><br/><br/><b>SeeAlso: </b>UnityCSReference</i>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(this float floatToRound) => floatToRound >= 0 ? (int) (floatToRound + 0.5f) : -(int) (0.5f - floatToRound);
        
        
        /// <summary>
        /// Rounds the <see cref="double">double</see> 'down' towards 0 by casting the value to int.
        /// </summary>
        /// <param name="doubleToFloor">The double parameter to Floor and convert to int</param>
        /// <returns>Returns an int equal to the Ceil of <paramref name="doubleToFloor"/></returns>
        /// <example>
        /// <br/>int returnVal = 0;
        /// <br/><paramref name="doubleToFloor"/> = 1.2 => returnVal = 1;
        /// <br/><paramref name="doubleToFloor"/> = -1.2 => returnVal = -1;
        /// </example>
        /// <remarks>
        /// <para>Casting the double to int truncates the double by removing everything after the decimal point. 
        /// This ensures that the method is always rounding the double towards 0, whether the double is +ve or -ve.</para>
        /// <br/>This method is based on the discussions on the
        /// <i href="https://forum.unity.com/threads/what-is-faster-to-convert-float-to-int-int-randomfloat-or-mathf-roundtoint-randomfloat.268731/">unity forums</i>
        /// and on <i href="https://math.stackexchange.com/questions/344815/how-do-the-floor-and-ceiling-functions-work-on-negative-numbers#:~:text=The%20correct%20answer%20is%20it%20depends%20how%20you,towards%20zero.%20Ceiling%20always%20rounding%20away%20from%20zero.">stackexchange forums</i>
        /// <i href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs"><br/><br/><b>SeeAlso: </b>UnityCSReference</i>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(this double doubleToFloor) => (int) doubleToFloor;
        
        /// <summary>
        /// Rounds the <see cref="double">double</see> 'up' away from 0 and casts the result to int.
        /// </summary>
        /// <param name="doubleToCeil">The double parameter to Ceil and convert to int</param>
        /// <returns>Returns an int equal to the Ceil of <paramref name="doubleToCeil"/></returns>
        /// <example>
        /// <br/>int returnVal = 0;
        /// <br/><paramref name="doubleToCeil"/> = 1.2 => returnVal = 2;
        /// <br/><paramref name="doubleToCeil"/> = -1.2 => returnVal = -2;
        /// </example>
        /// <remarks>This method is based on the discussions on the
        /// <i href="https://forum.unity.com/threads/what-is-faster-to-convert-float-to-int-int-randomfloat-or-mathf-roundtoint-randomfloat.268731/">unity forums</i>
        /// and on <i href="https://math.stackexchange.com/questions/344815/how-do-the-floor-and-ceiling-functions-work-on-negative-numbers#:~:text=The%20correct%20answer%20is%20it%20depends%20how%20you,towards%20zero.%20Ceiling%20always%20rounding%20away%20from%20zero.">stackexchange forums</i>
        /// <i href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs"><br/><br/><b>SeeAlso: </b>UnityCSReference</i>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilToInt(this double doubleToCeil) => doubleToCeil >= 0 ? (int) (doubleToCeil + 1) : (int) (doubleToCeil - 1);

        /// <summary>
        /// Rounds the <see cref="double">double</see> to the closets integer value.
        /// </summary>
        /// <param name="doubleToRound">The double parameter to Round and convert to int</param>
        /// <returns>Returns an int nearest to the value of <paramref name="doubleToRound"/></returns>
        /// <example>
        /// <br/>int returnVal = 0;
        /// <br/><paramref name="doubleToRound"/> = 1.2 => returnVal = 1
        /// <br/><paramref name="doubleToRound"/> = -1.2 => returnVal = -1
        /// <br/><paramref name="doubleToRound"/> = 1.7 => returnVal = 2
        /// <br/><paramref name="doubleToRound"/> = -1.7 => returnVal = -2
        /// </example>
        /// <remarks>This method is based on the discussions on the
        /// <i href="https://forum.unity.com/threads/what-is-faster-to-convert-float-to-int-int-randomfloat-or-mathf-roundtoint-randomfloat.268731/">unity forums</i>
        /// and on <i href="https://math.stackexchange.com/questions/344815/how-do-the-floor-and-ceiling-functions-work-on-negative-numbers#:~:text=The%20correct%20answer%20is%20it%20depends%20how%20you,towards%20zero.%20Ceiling%20always%20rounding%20away%20from%20zero.">stackexchange forums</i>
        /// <i href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs"><br/><br/><b>SeeAlso: </b>UnityCSReference</i>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(this double doubleToRound) => doubleToRound >= 0 ? (int) (doubleToRound + 0.5) : -(int) (0.5 - doubleToRound);
        
        /// <summary>
        /// Rounds the <see cref="float">float</see> value to <see cref="int">int</see> using the rounding method
        /// specified by the <paramref name="roundingMethod"/> parameter.
        /// </summary>
        /// <param name="floatToRound">The float value to be rounded to int</param>
        /// <param name="roundingMethod">Specifies the <see cref="RoundingMethod"/> to use.
        /// Default value is <i>RoundingMethod.Floor</i></param>
        /// <returns>integer equal to the rounded value of <paramref name="floatToRound"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">This should never happen.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Round(this float floatToRound, RoundingMethod roundingMethod = RoundingMethod.Floor)
        {
            return roundingMethod switch
                   {
                       RoundingMethod.Floor => floatToRound.FloorToInt()
                     , RoundingMethod.Ceil  => floatToRound.CeilToInt()
                     , RoundingMethod.Round => floatToRound.RoundToInt()
                     , _                    => throw new ArgumentOutOfRangeException(nameof(roundingMethod))
                   };
        }
        
        /// <summary>
        /// Rounds the <see cref="double">double</see> value to <see cref="int">int</see> using the rounding method
        /// specified by the <paramref name="roundingMethod"/> parameter.
        /// </summary>
        /// <param name="doubleToRound">The double value to be rounded to int</param>
        /// <param name="roundingMethod">Specifies the <see cref="RoundingMethod"/> to use.
        /// Default value is <i>RoundingMethod.Floor</i></param>
        /// <returns>integer equal to the rounded value of <paramref name="doubleToRound"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">This should never happen.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Round(this double doubleToRound, RoundingMethod roundingMethod = RoundingMethod.Floor)
        {
            return roundingMethod switch
                   {
                       RoundingMethod.Floor => doubleToRound.FloorToInt()
                     , RoundingMethod.Ceil  => doubleToRound.CeilToInt()
                     , RoundingMethod.Round => doubleToRound.RoundToInt()
                     , _                    => throw new ArgumentOutOfRangeException(nameof(roundingMethod))
                   };
        }
        
        /// <summary>
        /// Gets the Factorial number of the given number -> <paramref name="valueToGetFactorialOf"/>
        /// </summary>
        /// <param name="valueToGetFactorialOf">Number</param>
        /// <returns>The factorial</returns>
        public static int GetFactorial(this int valueToGetFactorialOf)
        {
            var factorial = 1;
            if (valueToGetFactorialOf <= 1) return factorial;
            
            for (int n = valueToGetFactorialOf; n > 1; n--)
            {
                factorial *= n;
            }

            return factorial;
        }

        /// <summary>
        /// Gets the factorial of a the given floating point number -> <paramref name="valueToGetFactorialOf"/>
        /// by rounding the value to an int and then calculating the factorial value.
        /// </summary>
        /// <param name="valueToGetFactorialOf">float Number</param>
        /// <param name="roundingMethod">Specifies the <see cref="RoundingMethod"/> to use.
        /// Default value is <i>RoundingMethod.Floor</i></param>
        /// <returns>The factorial</returns>
        public static int GetFactorial(this float valueToGetFactorialOf, RoundingMethod roundingMethod = RoundingMethod.Floor)
        {
            return GetFactorial(valueToGetFactorialOf.Round(roundingMethod));
        }

        /// <summary>
        /// Gets the Fibonacci number of the given number -> <paramref name="valueToGetFibonacciOf"/>
        /// </summary>
        /// <param name="valueToGetFibonacciOf">Number</param>
        /// <returns>The Fibonacci number</returns>
        public static int GetFibonacci(this int valueToGetFibonacciOf)
        {
            int val1 =0, val2 = 1, val = 0;
            if (valueToGetFibonacciOf < 1 )
                return 0;
            if (valueToGetFibonacciOf < 2)
                return 1;
            for (var n = 2; n <= valueToGetFibonacciOf; n++)
            {
                val  = val1 + val2;
                val1 = val2;
                val2 = val;
            }
            return val;
        }

        /// <summary>
        /// Gets the Fibonacci number of the given floating point number -> <paramref name="valueToGetFibonacciOf"/>
        /// by rounding it to int and then calculating the fibonacci value.
        /// </summary>
        /// <param name="valueToGetFibonacciOf">Number</param>
        /// <param name="roundingMethod">Specifies the <see cref="RoundingMethod"/> to use.
        /// Default value is <i>RoundingMethod.Floor</i></param>
        /// <returns>The Fibonacci number</returns>
        public static int GetFibonacci(this float valueToGetFibonacciOf, RoundingMethod roundingMethod = RoundingMethod.Floor)
        {
            return GetFibonacci(valueToGetFibonacciOf.Round(roundingMethod));
        }

        /// <summary>
        /// Gets the binomial coefficient from two numbers
        /// </summary>
        /// <param name="firstValue">First number</param>
        /// <param name="secondValue">Second number</param>
        /// <returns>The binomial coefficient of two numbers</returns>
        public static int GetBinomialCoefficient(int firstValue, int secondValue)
        {
            if (firstValue == secondValue || secondValue == 0) return 1;

            int aFactorial       = firstValue.GetFactorial();
            int bFactorial       = secondValue.GetFactorial();
            int aMinusBFactorial = (firstValue - secondValue).GetFactorial();
            
            return aFactorial / (bFactorial * aMinusBFactorial);
        }

        /// <summary>
        /// Gets the binomial coefficient from two floating point numbers by down
        /// rounding them to ints and then calculating the BinomialCoefficient value.
        /// </summary>
        /// <param name="firstValue">First number</param>
        /// <param name="secondValue">Second number</param>
        /// <param name="roundingMethod">Specifies the <see cref="RoundingMethod"/> to use.
        /// Default value is <i>RoundingMethod.Floor</i></param>
        /// <returns>The binomial coefficient of two numbers</returns>
        public static int GetBinomialCoefficient(float firstValue, float secondValue, RoundingMethod roundingMethod = RoundingMethod.Floor)
        {
            return GetBinomialCoefficient(firstValue.Round(roundingMethod), secondValue.Round(roundingMethod));
        }
    }
}