using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class Vector3IntExtensions
    {
        /// <summary>
        /// Calculates a squared distance between current and given vectors.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="otherVector">The given vector.</param>
        /// <returns>Returns squared distance between current and given vectors.</returns>
        public static float SqrDistance(this in Vector3Int thisVector, in Vector3Int otherVector)
        {
            int x = otherVector.x - thisVector.x;
            int y = otherVector.y - thisVector.y;
            int z = otherVector.z - thisVector.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// Multiplies each element in Vector3 by the given scalar.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="scalarToMultiply">The given scalar.</param>
        /// <returns>Returns new Vector3 containing the multiplied components.</returns>
        public static Vector3Int ElementwiseMultiplyBy(this in Vector3Int thisVector, int scalarToMultiply)
        {
            return new Vector3Int(thisVector.x * scalarToMultiply,
                                  thisVector.y * scalarToMultiply,
                                  thisVector.z * scalarToMultiply);
        }

        /// <summary>
        /// Multiplies each element in Vector3 by the given scalar.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="scalarToMultiply">The given scalar.</param>
        /// <returns>Returns new Vector3 containing the multiplied components.</returns>
        public static Vector3Int ElementwiseMultiplyBy(this in Vector3Int thisVector, float scalarToMultiply) =>
            thisVector.ElementwiseMultiplyBy(scalarToMultiply.RoundToInt());

        /// <summary>
        /// Multiplies each element in Vector3 a by the corresponding element of b.
        /// </summary>
        /// <param name="thisVector">The current vector.</param>
        /// <param name="vectorToMultiply">The given vector.</param>
        /// <returns>Returns new Vector3 containing the multiplied components of the given vectors.</returns>
        public static Vector3Int ElementwiseMultiplyBy(this in Vector3Int thisVector, Vector3Int vectorToMultiply)
        {
            vectorToMultiply.x *= thisVector.x;
            vectorToMultiply.y *= thisVector.y;
            vectorToMultiply.z *= thisVector.z;

            return vectorToMultiply;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisVector"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3Int SetYZ(this Vector3Int thisVector, int y, int z)
        {
            return new Vector3Int(thisVector.x, y, z);
        }

        public static Vector3Int SetXZ(this Vector3Int thisVector, int x, int z)
        {
            return new Vector3Int(x, thisVector.y, z);
        }

        public static Vector3Int SetXY(this Vector3Int thisVector, int x, int y)
        {
            return new Vector3Int(x, y, thisVector.z);
        }

        public static Vector3Int SwapYZ(this Vector3Int thisVector)
        {
            return new Vector3Int(thisVector.x, thisVector.z, thisVector.y);
        }

        public static Vector3Int SwapXZ(this Vector3Int thisVector)
        {
            return new Vector3Int(thisVector.z, thisVector.y, thisVector.x);
        }

        public static Vector3Int SwapXY(this Vector3Int thisVector)
        {
            return new Vector3Int(thisVector.y, thisVector.x, thisVector.z);
        }

        public static Vector3Int ReverseElements(this Vector3Int thisVector)
        {
            return new Vector3Int(thisVector.z, thisVector.y, thisVector.x);
        }

        public static Vector3Int GetClosestVectorInList(this Vector3Int thisVector, IEnumerable<Vector3Int> otherVectors)
        {
            return thisVector.GetClosestVectorInList(otherVectors, out float sqrDist);
        }

        public static Vector3Int GetClosestVectorInList(this Vector3Int thisVector, IEnumerable<Vector3Int> otherVectors
                                                      , out  float      sqrDistance)
        {
            Vector3Int[] otherVectorsArray = otherVectors as Vector3Int[] ?? otherVectors.ToArray();

            if (otherVectorsArray.Length == 0)
            {
                Debug.LogException(new Exception("The list of otherVectors is empty"));
                sqrDistance = -1;
                return Vector3Int.one.ElementwiseMultiplyBy(-1);
            }

            float      minSqrDistance = thisVector.SqrDistance(otherVectorsArray[0]);
            Vector3Int minVector      = otherVectorsArray[0];
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

        public static Vector3Int GetFarthestVectorInList(this Vector3Int thisVector, IEnumerable<Vector3Int> otherVectors)
        {
            return thisVector.GetFarthestVectorInList(otherVectors, out float sqrDist);
        }

        public static Vector3Int GetFarthestVectorInList(this Vector3Int thisVector, IEnumerable<Vector3Int> otherVectors
                                                       , out  float      sqrDistance)
        {
            Vector3Int[] otherVectorsArray = otherVectors as Vector3Int[] ?? otherVectors.ToArray();

            if (otherVectorsArray.Length == 0)
            {
                Debug.LogException(new Exception("The list of otherVectors is empty"));
                sqrDistance = -1;
                return Vector3Int.one.ElementwiseMultiplyBy(-1);
            }

            float      maxSqrDistance = thisVector.SqrDistance(otherVectorsArray[0]);
            Vector3Int maxVector      = otherVectorsArray[0];
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