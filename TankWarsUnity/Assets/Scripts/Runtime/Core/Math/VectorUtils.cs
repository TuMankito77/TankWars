namespace TankWars.Runtime.Core.Math
{
    using Unity.Mathematics;
    using UnityEngine;

    public static class VectorUtils
    {
        public static Vector3 GetAsVector3(this float[,] matrix)
        {
            if (matrix.GetLength(1) > 1)
            {
                Debug.LogError($"{matrix.GetType().Name}:The matrix cannot be convert into a vector since the matrix has more than one dimension. Returning a Vector with the value 0 in each axis.");
                return Vector3.zero;
            }

            if (matrix.GetLength(0) > 3)
            {
                Debug.LogWarning($"{matrix.GetType().Name}:The matrix has more than 3 rows, the vector returned will contain the first three values.");
            }

            Vector3 matrixAsVector = new Vector3(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
            return matrixAsVector;
        }

        //TODO: Create a method for multimplying matrices. 
        //Create static matrices for getting the matrix need to mirror vector in each axis.
        //Use the aforementioned method and the static matrices to calculate the mirrored vector on one of the axis. 
    }
}

