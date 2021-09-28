using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Common;

namespace AlgebraAlgorithmsAndNTH.Programs.Task1
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            for (; ; )
            {
                var mode = ReadWorkMode();
                if (mode == WorkMode.CLOSE_PROGRAM)
                {
                    return;
                }

                (var a, var b, var m) = ReadParameters();

                switch (mode)
                {
                    case WorkMode.GCD:
                        OutResultValues(SolveComparisionByGCD(a, b, m));
                        break;
                    case WorkMode.EULERS_THEOREM:
                        OutResultValues(SolveComparisionByEulersTheorem(a, b, m));
                        break;
                    case WorkMode.CLOSE_PROGRAM:
                        return;
                    case WorkMode.NONE:
                        break;
                }
            }
        }

        private static WorkMode ReadWorkMode()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Расширенный алгоритм Евклида;");
            Console.WriteLine("2 - Теорема Эйлера;");
            Console.WriteLine("0 - Завершить работу программы...");

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return WorkMode.GCD;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return WorkMode.EULERS_THEOREM;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return WorkMode.CLOSE_PROGRAM;
                    default:
                        break;
                }
            }
        }

        private static void OutResultValues(IEnumerable<BigInteger> result)
        {
            Console.WriteLine("Решения сравнения: ");
            foreach (var ans in result)
                Console.WriteLine(ans);
            Console.WriteLine();
        }

        private static (BigInteger a, BigInteger b, BigInteger m) ReadParameters()
        {
            Console.WriteLine("Решение уравнения вида a * x " + '\u2261' + " b (mod m)");
            Console.WriteLine("Введите через пробел 3 числа: a, b, m...");

            for (; ; )
            {
                var input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (input.Length != 3
                    || !BigInteger.TryParse(input[0], out var a)
                    || !BigInteger.TryParse(input[1], out var b)
                    || !BigInteger.TryParse(input[2], out var m))
                {
                    Console.WriteLine("Некорректный ввод! Повторие попытку...");
                    continue;
                }

                Console.WriteLine();
                return (a, b, m);
            }
        }

        // a * x ≡ b (mod m)
        private static IEnumerable<BigInteger> SolveComparisionByGCD(BigInteger a, BigInteger b, BigInteger m)
        {
            (var gcd, var x, _) = GCD.GetGCDAdvanced(a, m);
            b /= gcd;
            m /= gcd;

            var result = (((x * b) % m) + m) % m;

            for (BigInteger i = 0; i < gcd; i++)
            {
                yield return result;
                result += m;
            }
        }

        // a * x ≡ b (mod m)
        private static IEnumerable<BigInteger> SolveComparisionByEulersTheorem(BigInteger a, BigInteger b, BigInteger m)
        {
            var gcd = GCD.GetGCD(a, m);
            a /= gcd;
            b /= gcd;
            m /= gcd;

            // b * (a^(phi(m) - 1)) mod m
            var result =  (b * BigInteger.ModPow(a, EulersFunction.GetValueBySimpleFactorization((int)m) - 1, m)) % m;

            for (BigInteger i = 0; i < gcd; i++)
            {
                yield return result;
                result += m;
            }
        }
    }
}
