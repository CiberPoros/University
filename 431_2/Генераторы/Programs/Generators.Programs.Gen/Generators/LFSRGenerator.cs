using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                : Utils.ConvertIntArrayToByteArray(initialVector.ToArray()).Concat(Utils.ConvertIntArrayToByteArray(GetDefaultParameters().ToArray()).Take(seed / 8 - initialVector.Count())).ToList();

            throw new NotImplementedException();
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
