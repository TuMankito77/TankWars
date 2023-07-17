namespace TankWars.Runtime.Core.Math
{
    using UnityEngine; 

    public static class MatrixUtils 
    {
        public static float[,] xAxisMirorMatrix { get; private set; } =
        {
            {-1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 }
        };

        public static float[,] yAxisMirorMatrix { get; private set; } =
        {
            { 1, 0, 0 },
            { 0,-1, 0 },
            { 0, 0, 1 }
        };

        public static float[,] zAxisMirorMatrix { get; private set; } =
        {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0,-1 }
        };

        public static float[,] GetAsMatrix(this Vector3 vector)
        {
            float[,] vectorAsMatrix = new float[3, 1] { { vector.x }, { vector.y }, { vector.z } };
            return vectorAsMatrix;
        }

        public static float[,] MultiplyMatrices(float[,] matrixA, float[,] matrixB)
        {
            if (matrixA.GetLength(1) != matrixB.GetLength(0))
            {
                Debug.LogError("The matrices could not be multiplied because the number of columns in the first matrix is not of the same length as the number of rows on the second matrix.");
                return new float[matrixA.GetLength(0), matrixB.GetLength(1)];
            }

            int commontLength = matrixA.GetLength(1);
            float[,] result = new float[matrixA.GetLength(0), matrixB.GetLength(1)];

            for (int x = 0; x < matrixA.GetLength(0); x++)
            {
                for (int y = 0; y < matrixB.GetLength(1); y++)
                {
                    float currentCellValue = 0;

                    for (int i = 0; i < commontLength; i++)
                    {
                        currentCellValue += matrixA[x, i] * matrixB[i, y];
                    }

                    result[x, y] = currentCellValue;
                }
            }

            return result;
        }
    }
}
