using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators.Programs.Gen.Generators
{
    internal class MersenneVortexGenerator : IGenerator
    {
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            throw new NotImplementedException();
        }

        // 12 numbers total count
        public IEnumerable<int> GetDefaultParameters()
        {
            yield return 624; // n
            yield return 32; // w
            yield return 31; // r
            yield return 397; // m
            yield return 624; // n
            unchecked
            {
                yield return (int)0x9908B0DF; // a
            }
            yield return 11; // u
            yield return 7; // s
            yield return 15; // t
            yield return 18; // l
            unchecked
            {
                yield return (int)0x9D2C5680; // b
            }
            unchecked
            {
                yield return (int)0xEFC60000; // c
            }
        }
    }
}
