using Common.PrimeNumbersGeneration;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Generators.Programs.Gen.Generators
{
    internal class RSAGenerator : IGenerator
    {
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            var generatorPrimeNumbers = new GeneratorByLukeTest();
            var p = generatorPrimeNumbers.Generate(IGenerator.Rnd.Next(50, 70));
            var q = generatorPrimeNumbers.Generate(IGenerator.Rnd.Next(50, 70));
            var n = p * q;
            var f = (p - 1) * (q - 1);

            var e = new BigInteger();

            for (; ; )
            {
                e = IGenerator.Rnd.Next();
                if (BigInteger.GreatestCommonDivisor(e, f) == 1)
                    break;
            }

            BigInteger x = IGenerator.Rnd.Next((int)(n % int.MaxValue));
            var currentResult = 0;
            for (int i = 0; i < numbersCount * 32; i++)
            {
                x = BigInteger.ModPow(x, e, n);
                if ((x & 1) == 1)
                {
                    currentResult |= 1 << (i % 32);
                }

                if (i % 32 == 0)
                {
                    yield return ((currentResult % maxValue) + maxValue) % maxValue;
                    currentResult = 0;
                }
            }
            yield return ((currentResult % maxValue) + maxValue) % maxValue;
        }

        public IEnumerable<int> GetDefaultParameters()
        {
            return Enumerable.Empty<int>();
        }
    }
}
