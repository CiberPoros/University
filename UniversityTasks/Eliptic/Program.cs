using Eliptic;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace EllipsCons
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = FindSetPoint();
            var order = points.Count + 1;
            var primitivePoints = GetGenerativePoints(points);
            var matrix = GetMatrix(points);

            Console.WriteLine("Точки эл. кривой:");
            Console.WriteLine($"{string.Join(", ", points)}, (0, 0)");
            Console.WriteLine();

            Console.WriteLine($"Порядок: {order}");
            Console.WriteLine();

            Console.WriteLine("Генерация точек:");
            foreach (var arr in primitivePoints)
            {
                Console.WriteLine($"{string.Join(", ", arr)}");
            }
            Console.WriteLine();

            Console.WriteLine("Матрица сложения:");
            foreach (var arr in matrix)
            {
                Console.WriteLine($"{string.Join(", ", arr)}");
            }
            Console.WriteLine();
        }

        private static List<List<(F_int x, F_int y)>> GetGenerativePoints(List<(F_int x, F_int y)> points)
        {
            var result = new List<List<(F_int x, F_int y)>>();
            var n = 5;

            foreach (var leftPoint in points)
            {
                var currentRusult = new List<(F_int x, F_int y)>();
                currentRusult.Add(leftPoint);

                (F_int x, F_int y) currentLeftPoint = (new F_int(leftPoint.x._val, n), new F_int(leftPoint.y._val, n));

                foreach (var rightPoint in points)
                {
                    var summ = Generator.MultiPointOnPoint(currentLeftPoint.x, currentLeftPoint.y, leftPoint.x, leftPoint.y, out var resX, out var resY, new F_int(2, n));
                    currentRusult.Add((resX, resY));

                    currentLeftPoint = (new F_int(resX._val, n), new F_int(resY._val, n));
                }

                result.Add(currentRusult);
            }

            return result;
        }

        private static List<(F_int x, F_int y)> FindSetPoint()
        {
            var n = 5;
            var result = new List<(F_int x, F_int y)>();

            for (int i = 0; i < n; i++)
            {
                var y_2 = (((i * i * i + 2 * i + 1) % n) + n) % n;

                var yArr = new List<int>();
                for (int j = 0; j < n; j++)
                {
                    if ((((j * j) % n) + n) % n == y_2)
                    {
                        yArr.Add(j);
                    }
                }

                if (Jacobian.Calculate(y_2, n) == 1)
                {
                    result.Add((new F_int(i, n), new F_int(yArr[0], n)));
                    result.Add((new F_int(i, n), new F_int(yArr[1], n)));
                }
                else if (Jacobian.Calculate(y_2, n) == 0)
                {
                    result.Add((new F_int(i, n), new F_int(yArr[0], n)));
                }
            }

            return result;
        }

        private static List<List<(F_int x, F_int y)>> GetMatrix(List<(F_int x, F_int y)> points)
        {
            var result = new List<List<(F_int x, F_int y)>>();
            var n = 5;

            foreach (var leftPoint in points)
            {
                var currentRes = new List<(F_int x, F_int y)>();
                foreach (var rightPoint in points)
                {
                    var summ = Generator.MultiPointOnPoint(leftPoint.x, leftPoint.y, rightPoint.x, rightPoint.y, out var resX, out var resY, new F_int(2, n));
                    currentRes.Add((new F_int(resX._val, n), new F_int(resY._val, n)));
                }

                result.Add(currentRes);
            }

            return result;
        }
    }
}