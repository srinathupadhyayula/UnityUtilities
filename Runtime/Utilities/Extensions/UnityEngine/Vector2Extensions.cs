using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Extensions
{
    public partial class Vector2Extensions
    {
        public static Vector2 SwapElements(this Vector2 thisVector)
        {
            return new Vector2(thisVector.y, thisVector.x);
        }

        public static Vector2 ReverseElements(this Vector2 thisVector) => thisVector.SwapElements();
        
        
        public static Vector2 GetClosestVectorInList(this Vector2 thisVector, IEnumerable<Vector2> otherVectors)
        {
            return thisVector.GetClosestVectorInList(otherVectors, out float sqrDist);
        }

        public static Vector2 GetClosestVectorInList(this Vector2 thisVector, IEnumerable<Vector2> otherVectors, out float sqrDistance)
        {
            Vector2[] otherVectorsArray = otherVectors as Vector2[] ?? otherVectors.ToArray();

            if (otherVectorsArray.Length == 0)
            {
                Debug.LogException(new Exception("The list of otherVectors is empty"));
                sqrDistance = -1;
                return  Vector2.negativeInfinity;
            }
            
            float   minSqrDistance = thisVector.SqrDistance(otherVectorsArray[0]);
            Vector2 minVector   = otherVectorsArray[0];
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
        
        public static Vector2 GetFarthestVectorInList(this Vector2 thisVector, IEnumerable<Vector2> otherVectors)
        {
            return thisVector.GetFarthestVectorInList(otherVectors, out float sqrDist);
        }

        public static Vector2 GetFarthestVectorInList(this Vector2 thisVector, IEnumerable<Vector2> otherVectors, out float sqrDistance)
        {
            Vector2[] otherVectorsArray = otherVectors as Vector2[] ?? otherVectors.ToArray();

            if (otherVectorsArray.Length == 0)
            {
                Debug.LogException(new Exception("The list of otherVectors is empty"));
                sqrDistance = -1;
                return  Vector2.one.ElementwiseMultiplyBy(-1);
            }
            
            float   maxSqrDistance = thisVector.SqrDistance(otherVectorsArray[0]);
            Vector2 maxVector      = otherVectorsArray[0];
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