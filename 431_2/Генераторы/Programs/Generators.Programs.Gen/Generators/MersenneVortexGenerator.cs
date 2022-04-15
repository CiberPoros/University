using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Gen.Generators
{
    internal class MersenneVortexGenerator : IGenerator
    {
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            var vector = initialVector is null
                ? GetDefaultParameters().ToList()
                : initialVector.ToList().Concat(GetDefaultParameters().Take(11 - initialVector.Count())).ToList();

            var n = vector[0];
            var w = vector[1];
            var r = vector[2];
            var m = vector[3];
            var a = vector[4];
            var u = vector[5];
            var s = vector[6];
            var t = vector[7];
            var l = vector[8];
            var b = vector[9];
            var c = vector[10];

            uint u1 = 0;
            uint h1 = 0;
            for (uint mask = 1, i = 1; i <= w; i++, mask <<= 1)
            {
                if (i > r)
                {
                    u1 |= mask;
                }
                if (i <= r)
                {
                    h1 |= mask;
                }
            }
            
            var x = new List<int>();
            for (int i = 0; i < n; i++)
            {
                x.Add(IGenerator.Rnd.Next());
            }

            for (int i = 0, cnt = 0; cnt < numbersCount; i = (i + 1) % n, cnt++)
            {
                var y = (x[i] & u1) | (x[(i + 1) % n] & h1);
                x[i] = (int)((y & 1) == 1 ? x[(i + m) % n] ^ (y >> 1) ^ a : x[(i + m) % n] ^ (y >> 1) ^ 0);
                y = x[i];
                y ^= (y >> u);
                y ^= ((y << s) & b);
                y ^= ((y << t) & c);
                yield return ((((int)(y ^ (y >> l)) % maxValue) + maxValue) % maxValue);
            }
        }

        // 11 numbers total count
        public IEnumerable<int> GetDefaultParameters()
        {
            yield return 624; // n
            yield return 32; // w
            yield return 31; // r
            yield return 397; // m
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
