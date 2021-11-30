using System.Numerics;
using Common.IsPrimeChecking;

namespace Common.PrimeNumbersGeneration
{
    public class GeneratorByPoklington : IPrimeNumberGenerator
    {
        public int RoundsCount { get; set; }

        public BigInteger Generate(int minBitsCount)
        {
            for (; ; )
            {
                var currentPrime = GenerateStartRandomNumber();

                for (BigInteger i = currentPrime - 1; i > 1; i--)
                {
                    var n = currentPrime * i + 1;

                    var isCurrentPrime = false;
                    for (int j = 0; j < RoundsCount; j++)
                    {
                        var a = Generator.Next(2, n);

                        if (BigInteger.ModPow(a, n - 1, n) == 1 && BigInteger.GreatestCommonDivisor(n, (((BigInteger.ModPow(a, (n - 1) / currentPrime, n) - 1) % n) + n) % n) == 1)
                        {
                            isCurrentPrime = true;
                            break;
                        }
                    }

                    if (!isCurrentPrime)
                    {
                        continue;
                    }

                    if (n >= (new BigInteger(1) << minBitsCount))
                    {
                        return n;
                    }

                    currentPrime = n;
                }
            }
        }

        private BigInteger GenerateStartRandomNumber()
        {
            var result = Generator.Next(10000);
            var primeChecker = new CheckerByRabinMiller() { RoundsCount = RoundsCount };

            while (!primeChecker.IsPrime(result))
            {
                result++;
            }

            return result;
        }
    }
}
