using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Gen.Generators
{
    internal class RC4Generator : IGenerator
    {
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            var k = initialVector is null || !initialVector.Any()
                ? GetDefaultParameters().Select(x => (byte)x).ToList()
                : initialVector.Select(x => (byte)x).ToList();

            var l = k.Count;
            var s = new int[256];
            for (int i = 0; i < 256; i++)
            {
                s[i] = i;
            }
            var j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + s[i] + k[i % l]) % 256;
                (s[j], s[i]) = (s[i], s[j]);
            }

            var ii = 0;
            var jj = 0;
            var currentRes = 0;
            for (int cnt = 0; cnt < numbersCount * 4; cnt++)
            {
                ii = (ii + 1) % 256;
                jj = (jj + s[ii]) % 256;

                (s[jj], s[ii]) = (s[ii], s[jj]);
                var t = (s[ii] + s[jj]) % 256;

                switch (cnt % 4)
                {
                    case 0:
                        currentRes |= t << 24;
                        break;
                    case 1:
                        currentRes |= t << 16;
                        break;
                    case 2:
                        currentRes |= t << 8;
                        break;
                    case 3:
                        currentRes |= t;
                        yield return ((currentRes % maxValue) + maxValue) % maxValue;
                        currentRes = 0;
                        break;
                }
            }
        }

        public IEnumerable<int> GetDefaultParameters()
        {
            for (int i = 0; i < 1000; i++)
            {
                yield return IGenerator.Rnd.Next();
            }
        }
    }
}
