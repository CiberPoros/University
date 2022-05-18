using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Gen.Generators
{
    internal class NFSRGenerator : IGenerator
    {
        private int _seed;

        // seed here is count of bits in initial vector
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            _seed = seed;
            var lfsr = new LFSRGenerator();

            var initial = initialVector.ToArray();
            var gen1 = lfsr.Generate(initialVector.Take(initial.Length / 3).ToArray(), seed, numbersCount, maxValue).ToArray(); // первая треть
            var gen2 = lfsr.Generate(initialVector.Skip(initial.Length / 3).Take(initial.Length / 3).ToArray(), seed, numbersCount, maxValue).ToArray(); // вторая треть
            var gen3 = lfsr.Generate(initialVector.Skip((initial.Length / 3) * 2).Take(initial.Length / 3).ToArray(), seed, numbersCount, maxValue).ToArray(); // последняя треть

            var length = Math.Min(Math.Min(gen1.Length, gen2.Length), gen3.Length);
            for (int i = 0; i < length; i++)
            {
                yield return gen1[i] & gen2[i] ^ gen2[i] & gen3[i] ^ gen3[i]; // L1 * L2 + L2 * L3 + L3 формула из методички
            }
        }

        public IEnumerable<int> GetDefaultParameters()
        {
            for (int i = 0; i < (_seed / 32 + 1) * 3; i++)
            {
                yield return IGenerator.Rnd.Next();
            }
        }
    }
}
