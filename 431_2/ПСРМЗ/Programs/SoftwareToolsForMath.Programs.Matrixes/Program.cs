using System;

namespace SoftwareToolsForMath.Programs.Matrixes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var solver = new GaussResolver();

            var matrix = new double[,]
            {
                { 2d, 4d, 1d },
                { 5d, 2d, 1d },
                { 2d, 3d, 4d }
            };
            var freeCoeff = new double[] { 36d, 47d, 37d };

            var result = solver.Solve(matrix, freeCoeff);

            Console.WriteLine(String.Join(" ", result));
        }
    }
}
