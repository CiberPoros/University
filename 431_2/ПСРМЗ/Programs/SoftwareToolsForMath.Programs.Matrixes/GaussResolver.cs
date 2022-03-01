using System;

namespace SoftwareToolsForMath.Programs.Matrixes
{
    internal class GaussResolver
    {
        internal double[] Solve(double[,] matrix, double[] freeCoeff)
        {
            if (matrix is null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new ArgumentException("The array must be a square matrix.", nameof(matrix));
            }

            if (matrix.GetLength(0) != freeCoeff.Length)
            {
                throw new ArgumentException("Matrix order and free coeff array length must be equals.", nameof(freeCoeff));
            }

            var n = matrix.GetLength(0);
            var result = new double[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    var d = matrix[j, i] / matrix[i, i];
                    for (int k = j; k < n; k++)
                    {
                        matrix[j, k] -= d * matrix[i, k];
                    }
                    freeCoeff[j] -= d * freeCoeff[i];
                }
            }

            for (int i = n - 1; i >= 0; i--)
            {
                var d = 0d;
                for (int j = i + 1; j < n; j++)
                {
                    var s = matrix[i, j] * result[j];
                    d += s;
                }
                result[i] = (freeCoeff[i] - d) / matrix[i, i];
            }

            return result;
        }
    }
}
