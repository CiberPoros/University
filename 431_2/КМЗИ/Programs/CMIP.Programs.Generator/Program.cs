using CMIP.Programs.Calculator;
using CMIP.Programs.Calculator.Operations;
using Common.IsPrimeChecking;
using Common.PrimeNumbersGeneration;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CMIP.Programs.Generator
{
    internal class Program
    {
        private static Random _random = new Random();
        private static readonly IPrimeChecker _primeChecker = new CheckerByRabinMiller() { RoundsCount = 30 };
        private static readonly IPrimeNumberGenerator _primeNumberGenerator = new GeneratorByLukeTest();

        private static readonly List<char> _alphabet = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        static void Main(string[] args)
        {
            var bitsCount = ReadBitsCount();
            var (p, q) = GenerateByCustomArithmetic(bitsCount);

            Console.WriteLine($"p: {p}");
            Console.WriteLine($"q: {q}");
        }

        private static int ReadBitsCount()
        {
            Console.WriteLine("Введите минимальное количество бит генерируемых чисел (от 15 до 100): ");
            Console.WriteLine();
            for (; ; )
            {
                var input = Console.ReadLine();
                if (!int.TryParse(input, out var res) || res < 15 || res > 100)
                {
                    Console.WriteLine("Некорректный ввод. Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine();
                return res;
            }
        }

        private static (BigInteger p, BigInteger q) GenerateByCustomArithmetic(int k)
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
                var sNum = divider.Calculate(lowerBoundNum, qNum); // lowerBound / q
                sNum = summer.Calculate(sNum, new Number(new char[] { '1' })); // lowerBound / q + 1
                sNum = summer.Calculate(sNum, Number.FromBigInteger(_random.Next((int)BigInteger.Min(int.MaxValue, 100000))));

                var pNum = multiplier.Calculate(qNum, sNum);
                pNum = summer.Calculate(pNum, new Number(new char[] { '1' }));
                var p = pNum.ToBigInteger(_alphabet);

                if (p >= upperBound)
                {
                    continue;
                }

                var firstConditionMember = multiplier.Calculate(qNum, new Number(new char[] { '2' }));
                firstConditionMember = summer.Calculate(firstConditionMember, new Number(new char[] { '1' }));
                firstConditionMember = multiplier.Calculate(firstConditionMember, firstConditionMember);
                if (p >= firstConditionMember.ToBigInteger(_alphabet))
                {
                    continue;
                }

                var exponenciator = new ExponenciatorModulo(_alphabet, pNum);
                var secondConditionMember = exponenciator.Calculate(new Number(new char[] { '2' }), multiplier.Calculate(qNum, sNum));
                if (!secondConditionMember.IsSingleOne)
                {
                    continue;
                }

                exponenciator = new ExponenciatorModulo(_alphabet, pNum);
                var thirdConditionMember = exponenciator.Calculate(new Number(new char[] { '2' }), sNum);
                if (thirdConditionMember.IsSingleOne)
                {
                    continue;
                }

                return (p, q);
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
