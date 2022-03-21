using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Gen.Generators
{
    internal class AdditiveGenerator : IGenerator
    {
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            var values = initialVector is null 
                ? GetDefaultParameters().ToList() 
                : initialVector.ToList().Concat(GetDefaultParameters().Take(55 - initialVector.Count())).ToList();

            for (int i = 0; i < numbersCount; i++)
            {
                values.Add((values[values.Count - 24] + values[values.Count - 55]) % maxValue);
            }

            return values.Skip(55).Select(x => x >= 0 ? x : x + maxValue).ToList();
        }

        public IEnumerable<int> GetDefaultParameters()
        {
            for (int i = 0; i < 55; i++)
            {
                yield return IGenerator.Rnd.Next();
            }
        }
    }
}
