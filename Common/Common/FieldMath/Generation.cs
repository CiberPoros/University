using Common.IsPrimeChecking;
using System;
using System.Numerics;

namespace Common.FieldMath
{
    public static class Generation
    {
        private static readonly Random _random = new();

        public static (BigInteger prime, BigInteger primitiveRoot) GetBigPrimeWithPrimitiveElement(int minBitsCount)
        {
            BigInteger prime, primeDivider;

            while (true)
            {
                primeDivider = GetBigPrime(50);

                var degreeOf2 = new BigInteger(1);
                degreeOf2 <<= Math.Max(50, minBitsCount - 50);

                var checher = new CheckerByRabinMiller() { RoundsCount = 30 };
                prime = primeDivider * degreeOf2 + 1;

                for (int i = 0; i < 50 && !checher.IsPrime(prime); i++)
                {
                    degreeOf2 <<= 1;
                    prime = primeDivider * degreeOf2 + 1;
                }

                if (checher.IsPrime(prime))
                {
                    break;
                }
            }
           
            while (true)
            {
                var primitiveRoot = GetRandom(Math.Max(50, minBitsCount - 1));

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
            var checher = new CheckerByRabinMiller() { RoundsCount = 30 };
            var result = new BigInteger(1);

            for (int i = 0; i < Math.Max(minBitsCount / 2, 25); i++)
            {
                result <<= 1;
                result += _random.Next(2);
            }

            if (result % 2 == 0)
            {
                result++;
            }

            while (!checher.IsPrime(result))
            {
                result += 2;
            }

            return result;
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
