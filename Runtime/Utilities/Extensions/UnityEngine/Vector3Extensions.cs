using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class Vector3Extensions
    {
        public static Vector3 SetYZ(this Vector3 thisVector, float y, float z)
        {
            return new Vector3(thisVector.x, y, z);
        }

        public static Vector3 SetXZ(this Vector3 thisVector, float x, float z)
        {
            return new Vector3(x, thisVector.y, z);
        }

        public static Vector3 SetXY(this Vector3 thisVector, float x, float y)
        {
            return new Vector3(x, y, thisVector.z);
        }
        
        public static Vector3 SwapYZ(this Vector3 thisVector)
        {
            return new Vector3(thisVector.x, thisVector.z, thisVector.y);
        }

        public static Vector3 SwapXZ(this Vector3 thisVector)
        {
            return new Vector3(thisVector.z, thisVector.y, thisVector.x);
        }

        public static Vector3 SwapXY(this Vector3 thisVector)
        {
            return new Vector3(thisVector.y, thisVector.x, thisVector.z);
        }
        
        public static Vector3 ReverseElements(this Vector3 thisVector)
        {
            return new Vector3(thisVector.z, thisVector.y, thisVector.x);
        }
        
        public static Vector3 GetClosestVectorInList(this Vector3 thisVector, IEnumerable<Vector3> otherVectors)
        {
            return thisVector.GetClosestVectorInList(otherVectors, out float sqrDist);
        }

        public static Vector3 GetClosestVectorInList(this Vector3 thisVector, IEnumerable<Vector3> otherVectors, out float sqrDistance)
        {
            Vector3[] otherVectorsArray = otherVectors as Vector3[] ?? otherVectors.ToArray();

            if (otherVectorsArray.Length == 0)
            {
                Debug.LogException(new Exception("The list of otherVectors is empty"));
                sqrDistance = -1;
                return  Vector3.one.ElementwiseMultiplyBy(-1);
            }
            
            float   minSqrDistance = thisVector.SqrDistance(otherVectorsArray[0]);
            Vector3 minVector      = otherVectorsArray[0];
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
        
        public static Vector3 GetFarthestVectorInList(this Vector3 thisVector, IEnumerable<Vector3> otherVectors)
        {
            return thisVector.GetFarthestVectorInList(otherVectors, out float sqrDist);
        }

        public static Vector3 GetFarthestVectorInList(this Vector3 thisVector, IEnumerable<Vector3> otherVectors, out float sqrDistance)
        {
            Vector3[] otherVectorsArray = otherVectors as Vector3[] ?? otherVectors.ToArray();

            if (otherVectorsArray.Length == 0)
            {
                Debug.LogException(new Exception("The list of otherVectors is empty"));
                sqrDistance = -1;
                return  Vector3.one.ElementwiseMultiplyBy(-1);
            }
            
            float   maxSqrDistance = thisVector.SqrDistance(otherVectorsArray[0]);
            Vector3 maxVector      = otherVectorsArray[0];
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