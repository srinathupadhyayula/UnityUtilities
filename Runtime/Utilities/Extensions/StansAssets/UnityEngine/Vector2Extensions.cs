using UnityEngine;

namespace Utilities.Extensions
{
    /// <summary>
    /// Unity `Vector2` extension methods.
    /// </summary>
    /// <remarks>
    /// Author: Stans Assets
    /// <see cref="https://github.com/StansAssets/com.stansassets.foundation"/>
    /// <seealso cref="https://github.com/StansAssets"/>
    /// <seealso cref="https://stansassets.com/"/>
    /// </remarks>
    public static partial class Vector2Extensions
    {
        /// <summary>
        /// Calculates a squared distance between current and given vectors.
        /// </summary>
        /// <param name="a">The current vector.</param>
        /// <param name="b">The given vector.</param>
        /// <returns>Returns squared distance between current and given vectors.</returns>
        private static float SqrDistance(this in Vector2 a, in Vector2 b)
        {
            float x = b.x - a.x;
            float y = b.y - a.y;
            return ((x * x) + (y * y));
        }
        
        /// <summary>
        /// Multiplies each element in Vector2 by the given scalar <paramref name="scalarToMultiplyBy"/>
        /// and returns a new Vector2 containing the result.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="scalarToMultiplyBy">The given scalar.</param>
        /// <returns>Returns new Vector2 containing the multiplied components.</returns>
        public static Vector2 ElementwiseMultiplyBy(this in Vector2 thisVector, float scalarToMultiplyBy)
        {
            return new Vector2(thisVector.x * scalarToMultiplyBy,
                               thisVector.y * scalarToMultiplyBy);
        }
        
        /// <summary>
        /// Multiplies each element in current Vector2 <paramref name="thisVector"/>
        /// by the corresponding element of the other Vector2 <paramref name="otherVector"/>
        /// and returns a new Vector2 containing the multiplied values.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="otherVector">The given vector.</param>
        /// <returns>Returns new Vector2 containing the multiplied components of the given vectors.</returns>
        public static Vector2 ElementwiseMultiplyBy(this in Vector2 thisVector, Vector2 otherVector)
        {
            otherVector.x *= thisVector.x;
            otherVector.y *= thisVector.y;

            return otherVector;
        }
        
        /// <summary>
        /// Smooths a Vector2 that represents euler angles.
        /// </summary>
        /// <param name="current">The current Vector2 value.</param>
        /// <param name="target">The target Vector2 value.</param>
        /// <param name="velocity">A reference Vector2 used internally.</param>
        /// <param name="smoothTime">The time to smooth, in seconds.</param>
        /// <returns>The smoothed Vector2 value.</returns>
        public static Vector2 SmoothDampEuler(this in Vector2 current, Vector2 target, ref Vector2 velocity, float smoothTime)
        {
            Vector2 v;

            v.x = Mathf.SmoothDampAngle(current.x, target.x, ref velocity.x, smoothTime);
            v.y = Mathf.SmoothDampAngle(current.y, target.y, ref velocity.y, smoothTime);

            return v;
        }
    }
}