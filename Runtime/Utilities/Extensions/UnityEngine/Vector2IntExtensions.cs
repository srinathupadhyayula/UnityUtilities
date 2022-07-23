using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class Vector2IntExtensions
    {
        private static Vector2Int SwapElements(this Vector2Int thisVector)
        {
            return new Vector2Int(thisVector.y, thisVector.x);
        }
        
        public static Vector2Int ReverseElements(this Vector2Int thisVector) => thisVector.SwapElements();
        
        /// <summary>
        /// Calculates a squared distance between current and given vectors.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="otherVector">The given vector.</param>
        /// <returns>Returns squared distance between current and given vectors.</returns>
        /// <remarks> This method is based on the method in Vector2Extensions class in StansAssets, rewritten for Vector2Int</remarks>
        private static float SqrDistance(this in Vector2Int thisVector, in Vector2Int otherVector)
        {
            int x = otherVector.x - thisVector.x;
            int y = otherVector.y - thisVector.y;
            return ((x * x) + (y * y));
        }

        /// <summary>
        /// Multiplies each element in Vector2 by the given scalar.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="scalarToMultiplyBy">The given scalar.</param>
        /// <returns>Returns new Vector2Int containing the multiplied components.</returns>
        /// <remarks> This is a overloaded method with an int scalar value</remarks>
        public static Vector2Int ElementwiseMultiplyBy(this in Vector2Int thisVector, int scalarToMultiplyBy)
        {
            return new Vector2Int(thisVector.x * scalarToMultiplyBy,
                                  thisVector.y * scalarToMultiplyBy);
        }

        /// <summary>
        /// Multiplies each element in Vector2 by the given scalar.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="scalarToMultiplyBy">The given scalar.</param>
        /// <returns>Returns new Vector2Int containing the multiplied components.</returns>
        /// <remarks> This method is based on the method in Vector2Extensions class in StansAssets, rewritten for Vector2Int</remarks>
        public static Vector2Int ElementwiseMultiplyBy(this in Vector2Int thisVector, float scalarToMultiplyBy) => thisVector.ElementwiseMultiplyBy(scalarToMultiplyBy.RoundToInt());
        
        /// <summary>
        /// Multiplies each element in Vector2 a by the corresponding element of b.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="vectorToMultiplyBy">The given vector.</param>
        /// <returns>Returns new Vector2 containing the multiplied components of the given vectors.</returns>
        /// <remarks> This method is based on the method in Vector2Extensions class in StansAssets, rewritten for Vector2Int</remarks>
        public static Vector2Int ElementwiseMultiplyBy(this in Vector2Int thisVector, Vector2Int vectorToMultiplyBy)
        {
            vectorToMultiplyBy.x *= thisVector.x;
            vectorToMultiplyBy.y *= thisVector.y;

            return vectorToMultiplyBy;
        }
        
        public static Vector2Int GetClosestVectorInList(this Vector2Int thisVector, IEnumerable<Vector2Int> otherVectors)
        {
            return thisVector.GetClosestVectorInList(otherVectors, out float sqrDist);
        }

        public static Vector2Int GetClosestVectorInList(this Vector2Int thisVector, IEnumerable<Vector2Int> otherVectors, out float sqrDistance)
        {
            Vector2Int[] otherVectorsArray = otherVectors as Vector2Int[] ?? otherVectors.ToArray();

            if (otherVectorsArray.Length == 0)
            {
                Debug.LogException(new Exception("The list of otherVectors is empty"));
                sqrDistance = -1;
                return  Vector2Int.one.ElementwiseMultiplyBy(-1);
            }
            
            float      minSqrDistance = thisVector.SqrDistance(otherVectorsArray[0]);
            Vector2Int minVector      = otherVectorsArray[0];
            for (int i = otherVectorsArray.Length - 1; i > 0; i--)
            {
                float newDistance = thisVector.SqrDistance(otherVectorsArray[i]);
                if (newDistance >= minSqrDistance)
                {
                    continue;
                }
                minSqrDistance = newDistance;
                minVector      = otherVectorsArray[i];
            }

            sqrDistance = minSqrDistance;
            return minVector;
        }
        
        public static Vector2Int GetFarthestVectorInList(this Vector2Int thisVector, IEnumerable<Vector2Int> otherVectors)
        {
            return thisVector.GetFarthestVectorInList(otherVectors, out float sqrDist);
        }

        public static Vector2Int GetFarthestVectorInList(this Vector2Int thisVector, IEnumerable<Vector2Int> otherVectors, out float sqrDistance)
        {
            Vector2Int[] otherVectorsArray = otherVectors as Vector2Int[] ?? otherVectors.ToArray();

            if (otherVectorsArray.Length == 0)
            {
                Debug.LogException(new Exception("The list of otherVectors is empty"));
                sqrDistance = -1;
                return  Vector2Int.one.ElementwiseMultiplyBy(-1);
            }
            
            float      maxSqrDistance = thisVector.SqrDistance(otherVectorsArray[0]);
            Vector2Int maxVector      = otherVectorsArray[0];
            for (int i = otherVectorsArray.Length - 1; i > 0; i--)
            {
                float newDistance = thisVector.SqrDistance(otherVectorsArray[i]);
                if (newDistance <= maxSqrDistance)
                {
                    continue;
                }
                maxSqrDistance = newDistance;
                maxVector      = otherVectorsArray[i];
            }

            sqrDistance = maxSqrDistance;
            return maxVector;
        }
    }
}