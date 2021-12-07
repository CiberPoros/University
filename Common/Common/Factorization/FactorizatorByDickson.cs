using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Common.IsPrimeChecking;

namespace Common.Factorization
{
    public class FactorizatorByDickson : IFactorizator
    {
        private static readonly int[] _primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79 };
        private static readonly Random _random = new();

        public IEnumerable<BigInteger> Factorize(BigInteger value)
        {
            if (value > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Parameter can't be greater than {int.MaxValue}");
            }

            var n = (int)value;
            for (; ; )
            {
                if (n < int.MaxValue)
                {
                    var simpleFactorizator = new FactorizatorByBruteForce();
                    foreach (var currentResult in simpleFactorizator.Factorize(n))
                    {
                        yield return currentResult;
                    }

                    yield break;
                }

                var currentDivider = FactorizeInternal(n);

                if (currentDivider != n)
                {
                    yield return currentDivider;
                    n /= currentDivider;
                    continue;
                }

                break;
            }

            yield return n;
        }

        private static int FactorizeInternal(int n)
        {
            for (; ; )
            {
                try
                {
                    StepOne(n, out List<int> B);

                    if (n > 1000000)
                    {
                        return CalcAns(n);
                    }

                    StepFive(B, n, out Dictionary<(int, int), Dictionary<int, int>> table);

                    int x = -1, y = -1;
                    while (!StepSix(n, table, B, out x, out y))
                    {
                        StepFive(B, n, out table);
                    }

                    var ans = StepSeven(x, y, n);

                    if (n == ans.Item1 * ans.Item2)
                    {
                        return 0;
                    }
                }
                catch
                {

                }
            }
        }

        private static void StepOne(int n, out List<int> B)
        {
            double L = Math.Exp(Math.Sqrt(Math.Log(n) * Math.Log(Math.Log(n))));
            double M = Math.Sqrt(L);

            B = new List<int>();
            for (int i = 2; i <= M; i++)
                if (_primes.Contains(i))
                    B.Add(i);
        }

        private static void StepTwo(int n, out int b) => b = _random.Next((int)Math.Ceiling(Math.Sqrt(n)) + 1, n);

        private static void StepThree(int b, int n, out int a) => a = (int)BigInteger.ModPow(b, 2, n);

        private static void StepFour(int a, int b, List<int> B, ref Dictionary<(int, int), Dictionary<int, int>> table)
        {
            int _a = a;
            Dictionary<int, int> dividers = new Dictionary<int, int>();
            for (int i = 0; i < B.Count; i++)
                dividers.Add(B[i], 0);

            for (int i = 0; i < B.Count; i++)
            {
                if (_a % B[i] == 0)
                {
                    _a = _a / B[i];
                    dividers[B[i]]++;
                    i = -1;
                }
            }

            if (_a == 1)
                table.Add((b, a), dividers);
        }

        private static void StepFive(List<int> B, int n, out Dictionary<(int, int), Dictionary<int, int>> table, bool f = false)
        {
            table = new Dictionary<(int, int), Dictionary<int, int>>();

            while (!f || table.Count < B.Count + 1)
            {
                f = true;
                StepTwo(n, out int b);
                StepThree(b, n, out int a);
                StepFour(a, b, B, ref table);
            }
        }

        private static bool StepSix(int n, Dictionary<(int, int), Dictionary<int, int>> table, List<int> B, out int x, out int y)
        {
            int[,] matrix = new int[B.Count + 1, B.Count];
            int i = 0;
            foreach (var ind in table)
            {
                int j = 0;
                foreach (var kvp in table[ind.Key])
                {
                    matrix[i, j] = kvp.Value % 2;

                    j++;
                }

                i++;
            }

            int[] values = new int[B.Count + 1];

            for (int it = 0; it < values.Length; it++)
            {
                int val = 0;
                int k = 1;

                for (int j = B.Count - 1; j >= 0; j--)
                {
                    if (matrix[it, j] == 1)
                        val += k;

                    k *= 2;
                }

                values[it] = val;
            }

            List<int> res = new List<int>();
            for (int it = 0; it < values.Length; it++)
            {
                List<int> result = new List<int>() { it };
                bool f = false;
                Rec(values, values[it], it + 1, result, ref res, ref f);
                if (f)
                    break;
            }

            x = 1;
            i = 0;

            int[] _y = new int[B.Count];
            foreach (var ind in table)
            {
                if (res.Contains(i))
                {
                    x = (x * ind.Key.Item1) % n;

                    int _k = 0;
                    foreach (var kvp in ind.Value)
                    {
                        _y[_k] += kvp.Value;
                        _k++;
                    }
                }

                i++;
            }

            y = 1;

            for (i = 0; i < B.Count; i++)
            {
                _y[i] /= 2;

                y = (int)((BigInteger.ModPow(B[i], _y[i], n) * new BigInteger(y)) % n);
            }

            if (x == y || x == (n - y))
                return false;
            else
                return true;
        }

        private static(int, int) StepSeven(int x, int y, int n)
        {
            if (x < y)
            {
                int z = x;
                x = y;
                y = z;
            }

            return (InternalGCD(x + y, n), InternalGCD(x - y, n));
        }

        private static void Rec(int[] values, int prev, int index, List<int> result, ref List<int> res, ref bool f)
        {
            if (f)
                return;

            for (int i = index; i < values.Length; i++)
            {
                int k = prev ^ values[i];

                if (k == 0)
                {
                    for (int j = 0; j < result.Count; j++)
                        res.Add(result[j]);

                    res.Add(i);

                    f = true;
                    return;
                }

                result.Add(i);
                Rec(values, k, i + 1, result, ref res, ref f);
                result.RemoveAt(result.Count - 1);
            }
        }

        private static int CalcAns(int n)
        {
            P2method(n);
            return 0;
        }

        private static int InternalGCD(int a, int b)
        {
            return b == 0 ? a : InternalGCD(b, a % b);
        }

        private static void P1method(ref BigInteger n, ref BigInteger c, int constant)
        {
            BigInteger a = c, b = c, p;

            while (true)
            {
                a = (a * a + constant) % n;
                b = (b * b + constant) % n;
                b = (b * b + constant) % n;
                (BigInteger d, _, _) = GCD.GetGCDAdvanced(a - b, n);

                if (d > 1 && d < n)
                {
                    p = d;
                    return;
                }

                if (d == n)
                {
                    return;
                }

                if (d == 1)
                {
                    continue;
                }
            }
        }

        private static void P2method(BigInteger n)
        {
            BigInteger[] primes = new BigInteger[1000];
            int _k = 1;
            primes[0] = 2;
            for (int _i = 3; ; _i++)
            {
                var isPrimeChecker = new CheckerByRabinMiller() { RoundsCount = 15 };
                if (_i % 2 == 1 && isPrimeChecker.IsPrime(_i))
                    primes[_k] = _i;
                else
                    continue;

                _k++;

                BigInteger p, a;

                while (true)
                {
                    a = _random.Next(2, (int)(n - 1));
                    if (BigInteger.GreatestCommonDivisor(a, n) == 1)
                        break;
                }

                BigInteger d = BigInteger.GreatestCommonDivisor(a, n);
                if (d >= 2)
                {
                    p = d;
                    return;
                }

                for (int i = 0; i < primes.Length; ++i)
                {
                    BigInteger l = (BigInteger)(Math.Log((double)n) / Math.Log((double)primes[i]));
                    a = BigInteger.ModPow(a, BigInteger.Pow(primes[i], (int)l), n);
                }

                d = BigInteger.GreatestCommonDivisor(a - 1, n);

                if (d == n || d == 1)
                {
                    continue;
                }
                else
                {
                    p = d;
                    return;
                }
            }
        }
    }
}
