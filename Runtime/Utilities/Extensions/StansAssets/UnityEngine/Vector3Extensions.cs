using UnityEngine;

namespace Utilities.Extensions
{
    /// <summary>
    /// Unity `Vector3` extension methods.
    /// </summary>
    /// <remarks>
    /// Author: Stans Assets
    /// <see cref="https://github.com/StansAssets/com.stansassets.foundation"/>
    /// <seealso cref="https://github.com/StansAssets"/>
    /// <seealso cref="https://stansassets.com/"/>
    /// </remarks>
    public static partial class Vector3Extensions
    {
        /// <summary>
        /// Calculates a squared distance between current and given vectors.
        /// </summary>
        /// <param name="a">The current vector.</param>
        /// <param name="b">The given vector.</param>
        /// <returns>Returns squared distance between current and given vectors.</returns>
        private static float SqrDistance(this in Vector3 a, in Vector3 b)
        {
            float x = b.x - a.x;
            float y = b.y - a.y;
            float z = b.z - a.z;
            return ((x * x) + (y * y) + (z * z));
        }

        /// <summary>
        /// Multiplies each element in Vector3 by the given scalar <paramref name="scalarToMultiplyBy"/>
        /// and returns a new Vector3 containing the result.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="scalarToMultiplyBy">The given scalar.</param>
        /// <returns>Returns new Vector3 containing the multiplied components.</returns>
        public static Vector3 ElementwiseMultiplyBy(this in Vector3 thisVector, float scalarToMultiplyBy)
        {
            return new Vector3(thisVector.x * scalarToMultiplyBy,
                               thisVector.y * scalarToMultiplyBy);
        }

        /// <summary>
        /// Multiplies each element in current Vector3 <paramref name="thisVector"/>
        /// by the corresponding element of the other Vector3 <paramref name="otherVector"/>
        /// and returns a new Vector3 containing the multiplied values.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="otherVector">The given vector.</param>
        /// <returns>Returns new Vector3 containing the multiplied components of the given vectors.</returns>
        public static Vector3 ElementwiseMultiplyBy(this in Vector3 thisVector, Vector3 otherVector)
        {
            otherVector.x *= thisVector.x;
            otherVector.y *= thisVector.y;
            
            return otherVector;
        }

        /// <summary>
        /// Smooths a Vector3 that represents euler angles.
        /// </summary>
        /// <param name="current">The current Vector3 value.</param>
        /// <param name="target">The target Vector3 value.</param>
        /// <param name="velocity">A reference Vector3 used internally.</param>
        /// <param name="smoothTime">The time to smooth, in seconds.</param>
        /// <returns>The smoothed Vector3 value.</returns>
        public static Vector3 SmoothDampEuler(this in Vector3 current, Vector3 target, ref Vector3 velocity, float smoothTime)
        {
            Vector3 v;

            v.x = Mathf.SmoothDampAngle(current.x, target.x, ref velocity.x, smoothTime);
            v.y = Mathf.SmoothDampAngle(current.y, target.y, ref velocity.y, smoothTime);
            v.z = Mathf.SmoothDampAngle(current.z, target.z, ref velocity.z, smoothTime);

            return v;
        }
    }
}