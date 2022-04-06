using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Gen.Generators
{
    internal class LFSRGenerator : IGenerator
    {
        private int _seed;

        // seed here is count of bits in initial vector
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            _seed = seed;
            var values = initialVector is null
                ? Utils.ConvertIntArrayToByteArray(GetDefaultParameters().ToArray()).ToList()
                : Utils.ConvertIntArrayToByteArray(initialVector.ToArray()).Concat(Utils.ConvertIntArrayToByteArray(GetDefaultParameters().ToArray()).Take(seed / 4 - initialVector.Count())).ToList();

            // names as in the algorithm
            var x = values.Take(values.Count / 2).ToList();
            var a = values.Skip(values.Count / 2).ToArray();
            var n = x.Count * 8;

            for (int i = n + 1, j = 0; j < numbersCount; i++, j++)
            {
                var currentResultNumber = 0;
                for (int k = 0, mask = 1; k < 31; k++, mask <<= 1)
                {
                    var currentBit = Utils.GetBitFromByteArray(x, j * 31 + k) && Utils.GetBitFromByteArray(a, 0);

                    for (int index = j * 31 + k + 1, cnt = 1; cnt < n; index++, cnt++)
                    {
                        currentBit ^= (Utils.GetBitFromByteArray(x, index) && Utils.GetBitFromByteArray(a, cnt));
                    }

                    var adjustedIndex = n + j * 31 + k + 1;
                    Utils.AddLast(x, currentBit, adjustedIndex);
                    if (currentBit)
                    {
                        currentResultNumber |= mask;
                    }
                }

                yield return currentResultNumber % maxValue;
            }
        }

        public IEnumerable<int> GetDefaultParameters()
        {
            for (int i = 0; i < _seed / 32 + 1; i++)
            {
                yield return IGenerator.Rnd.Next();
            }
        }
    }
}
