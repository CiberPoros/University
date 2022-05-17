using CMIP.Programs.Calculator;
using CMIP.Programs.Calculator.Operations;
using Common.IsPrimeChecking;
using Common.PrimeNumbersGeneration;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace CMIP.Programs.Generator
{
    internal class Program
    {
        private static readonly Random _random = new Random();
        private static readonly IPrimeNumberGenerator _primeNumberGenerator = new GeneratorByLukeTest();

        private static readonly List<char> _alphabet = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        static async Task Main()
        {
            for (; ; )
            {
                var bitsCount = ReadBitsCount();

                var task1 = Task.Run(() => GenerateByCustomArithmetic(bitsCount));
                var task2 = Task.Delay(TimeSpan.FromSeconds(5));

                await Task.WhenAny(task1, task2);
                if (task1.IsCompleted)
                {
                    var (p, q, s) = task1.Result;
                    Console.WriteLine($"p: {p}");
                    Console.WriteLine($"q: {q}");
                    Console.WriteLine($"s: {s}");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Числа с заданной длиной бит не найдены");
                    Console.WriteLine();
                }
            }
        }

        private static int ReadBitsCount()
        {
            Console.WriteLine("Введите минимальное количество бит генерируемых чисел (от 0 до 100): ");
            Console.WriteLine();
            for (; ; )
            {
                var input = Console.ReadLine();
                if (!int.TryParse(input, out var res) || res < 0 || res > 100)
                {
                    Console.WriteLine("Некорректный ввод. Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine();
                return res;
            }
        }

        private static (BigInteger p, BigInteger q, BigInteger s) GenerateByCustomArithmetic(int k)
        {
            var q = _primeNumberGenerator.Generate(k / 2);
            var lowerBound = new BigInteger(1) << (k - 1);
            var upperBound = lowerBound << 1;

            var qNum = Number.FromBigInteger(q);
            var lowerBoundNum = Number.FromBigInteger(lowerBound);

            var summer = new Summer(_alphabet);
            var divider = new Divider(_alphabet);
            var multiplier = new Multiplier(_alphabet);

            while (true)
            {
                // Генерация s
                var sNum = divider.Calculate(lowerBoundNum, qNum); // lowerBound / q
                sNum = summer.Calculate(sNum, Number.FromBigInteger(_random.Next((int)BigInteger.Min(int.MaxValue, 100000))));

                var pNum = multiplier.Calculate(qNum, sNum); // q * s
                pNum = summer.Calculate(pNum, new Number(new char[] { '1' })); // q * s + 1
                var p = pNum.ToBigInteger(_alphabet);

                if (p >= upperBound) // чтобы было строго k bit
                {
                    continue;
                }

                var firstConditionMember = multiplier.Calculate(qNum, new Number(new char[] { '2' })); // q * 2
                firstConditionMember = summer.Calculate(firstConditionMember, new Number(new char[] { '1' })); // q * 2 + 1
                firstConditionMember = multiplier.Calculate(firstConditionMember, firstConditionMember); // (q * 2 + 1) * (q * 2 + 1)
                if (p >= firstConditionMember.ToBigInteger(_alphabet))
                {
                    continue;
                }

                var exponenciator = new ExponenciatorModulo(_alphabet, pNum);
                var secondConditionMember = exponenciator.Calculate(new Number(new char[] { '2' }), multiplier.Calculate(qNum, sNum)); // 2 ^ (q * s) mod p
                if (!secondConditionMember.IsSingleOne) // res != 1
                {
                    continue;
                }

                exponenciator = new ExponenciatorModulo(_alphabet, pNum);
                var thirdConditionMember = exponenciator.Calculate(new Number(new char[] { '2' }), sNum); // 2 ^ s mod p
                if (thirdConditionMember.IsSingleOne) // res == 1
                {
                    continue;
                }

                return (p, q, sNum.ToBigInteger(_alphabet));
            }
        }

        private static (BigInteger p, BigInteger q) Generate(int k)
        {
            var q = _primeNumberGenerator.Generate(k / 2);
            var lowerBound = new BigInteger(1) << (k - 1);
            var upperBound = lowerBound << 1;

            while (true)
            {
                var s = lowerBound / q + 1 + _random.Next((int)BigInteger.Min(int.MaxValue, lowerBound / q));
                var p = q * s + 1;

                if (p >= upperBound)
                {
                    continue;
                }

                if (p >= (2 * q + 1) * (2 * q + 1))
                {
                    continue;
                }

                if (BigInteger.ModPow(2, q * s, p) != 1)
                {
                    continue;
                }

                if (BigInteger.ModPow(2, s, p) == 1)
                {
                    continue;
                }

                return (p, q);
            }
        }
    }
}
