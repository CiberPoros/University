using Common.IsPrimeChecking;
using System;
using System.Numerics;

namespace Common.FieldMath
{
    public static class Generation
    {
        private static readonly Random _random = new();

        public static (BigInteger prime, BigInteger divider) GetBigPrimeWithBigPrimeDivider(int minBitsCount)
        {
            BigInteger prime, primeDivider;
            var limit = (new BigInteger(1)) << minBitsCount;
            var checker = new CheckerByRabinMiller() { RoundsCount = 30 };

            while (true)
            {
                primeDivider = minBitsCount > 30
                    ? GetBigPrime(minBitsCount / 5)
                    : GetLittlePrime(0, 1 << (minBitsCount));

                var degreeOf2 = new BigInteger(1);
                while (true)
                {
                    if (primeDivider * (degreeOf2 << 1) + 1 >= limit)
                        break;

                    degreeOf2 <<= 1;
                }

                prime = primeDivider * degreeOf2 + 1;
                if (prime >= limit)
                {
                    continue;
                }

                if (checker.IsPrime(prime))
                {
                    break;
                }
            }

            return (prime, primeDivider);
        }

        public static (BigInteger prime, BigInteger primitiveRoot) GetBigPrimeWithPrimitiveElement(int minBitsCount)
        {
            BigInteger prime, primeDivider;
            var limit = (new BigInteger(1)) << minBitsCount;
            var checker = new CheckerByRabinMiller() { RoundsCount = 30 };

            while (true)
            {
                primeDivider = minBitsCount > 30 
                    ? GetBigPrime(minBitsCount / 5)
                    : GetLittlePrime(0, 1 << (minBitsCount));

                var degreeOf2 = new BigInteger(1);
                while (true)
                {
                    if (primeDivider * (degreeOf2 << 1) + 1 >= limit)
                        break;

                    degreeOf2 <<= 1;
                }

                prime = primeDivider * degreeOf2 + 1;
                if (prime >= limit)
                {
                    continue;
                }

                if (checker.IsPrime(prime))
                {
                    break;
                }
            }
           
            while (true)
            {
                var primitiveRoot = GetRandom(Math.Min(100, minBitsCount - 1));

                if (BigInteger.ModPow(primitiveRoot, 2, prime - 1) == 1)
                {
                    continue;
                }

                if (BigInteger.ModPow(primitiveRoot, primeDivider, prime - 1) == 1)
                {
                    continue;
                }

                return (prime, primitiveRoot);
            }
        }

        public static BigInteger GetBigPrime(int minBitsCount)
        {
            if (minBitsCount <= 30)
            {
                return GetLittlePrime(0, 1 << minBitsCount);
            }

            var checker = new CheckerByRabinMiller() { RoundsCount = 30 };
            var result = new BigInteger(1);
            result <<= minBitsCount - 1;
            var limit = result << 1;

            while (true)
            {
                result += _random.Next(1 << 25);
                if (result % 2 == 0)
                    result++;
                while (true)
                {
                    if (checker.IsPrime(result))
                    {
                        return result;
                    }

                    result += 2;

                    if (result >= limit)
                    {
                        break;
                    }
                } 
            }
        }

        public static int GetLittlePrime(int minval, int maxVal)
        {
            var checher = new CheckerByRabinMiller() { RoundsCount = 30 };
            
            while (true)
            {
                var result = _random.Next(minval, maxVal);

                if (checher.IsPrime(result))
                {
                    return result;
                }
            }
        }

        public static BigInteger GetRandom(int bitsCount)
        {
            var result = new BigInteger(1);

            for (int i = 0; i < bitsCount; i++)
            {
                result <<= 1;
                result += _random.Next(2);
            }

            return result;
        }
    }
}
