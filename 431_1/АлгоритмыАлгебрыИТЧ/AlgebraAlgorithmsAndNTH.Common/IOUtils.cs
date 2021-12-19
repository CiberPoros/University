using System;
using System.Linq;
using Common.PolinomMath;

namespace AlgebraAlgorithmsAndNTH.Common
{
    public static class IOUtils
    {
        public static PolinomDouble ReadPolinomFromConsole(string polinomName)
        {
            Console.WriteLine($"Введите полином {polinomName} в виде коэффициентов многочлена от старшего к младшему через пробел:");
            Console.WriteLine();

            for (; ; )
            {
                var input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var resultArray = new double[input.Length];
                var success = true;
                for (int i = 0; i < input.Length; i++)
                {
                    if (!double.TryParse(input[i], out var doubleVal))
                    {
                        success = false;
                        break;
                    }

                    resultArray[i] = doubleVal;
                }

                if (!success)
                {
                    Console.WriteLine("Неверный формат входных данных! Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine();
                return new PolinomDouble(resultArray.Reverse().ToArray());
            }
        }
    }
}
