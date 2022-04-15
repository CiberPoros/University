using Common.PrimeNumbersGeneration;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Generators.Programs.Gen.Generators
{
    internal class BbsGenerator : IGenerator
    {
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            var generatorPrimeNumbers = new GeneratorByLukeTest();

            var p = generatorPrimeNumbers.Generate(IGenerator.Rnd.Next(80, 100));
            while (p % 4 != 3)
            {
                p = generatorPrimeNumbers.Generate(IGenerator.Rnd.Next(80, 100));
            }

            var q = generatorPrimeNumbers.Generate(IGenerator.Rnd.Next(80, 100));
            while (q % 4 != 3)
            {
                q = generatorPrimeNumbers.Generate(IGenerator.Rnd.Next(80, 100));
            }

            var n = p * q;

            BigInteger x = IGenerator.Rnd.Next();
            while (BigInteger.GreatestCommonDivisor(x, n) != 1)
            {
                x = IGenerator.Rnd.Next();
            }

            x = (x * x) % n;
            var currentResult = 0;
            for (int i = 0; i < numbersCount * 32; i++)
            {
                x = (x * x) % n;
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
