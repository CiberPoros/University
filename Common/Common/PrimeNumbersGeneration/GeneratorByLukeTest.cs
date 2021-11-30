using System.Numerics;
using Common.IsPrimeChecking;

namespace Common.PrimeNumbersGeneration
{
    public class GeneratorByLukeTest : IPrimeNumberGenerator
    {
        public BigInteger Generate(int minBitsCount)
        {
            var startValue = new BigInteger(1) << minBitsCount;
            var degree = minBitsCount;
            var IsPrimeChecker = new CheckerByRabinMiller() { RoundsCount = 100 };

            for (var i = startValue; ; i <<= 1, degree++)
            {
                if (!IsPrimeChecker.IsPrime(degree))
                {
                    continue;
                }

                var s = new BigInteger(4);
                var k = new BigInteger(1);
                var m = i - 1;

                while (k != degree - 1)
                {
                    s = ((((s * s) - 2) % m) + m) % m;
                    k++;
                }

                if (s == 0)
                {
                    return m;
                }
            }
        }
    }
}
