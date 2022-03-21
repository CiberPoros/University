using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Gen.Generators
{
    internal class LinearCongruentGenerator : IGenerator
    {
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            if (initialVector is null || !initialVector.Any())
            {
                initialVector = GetDefaultParameters();
            }
            else if (initialVector.Count() < 3)
            {
                throw new ArgumentException($"{initialVector} must contain at least 3 items.", nameof(initialVector));
            }

            var a = initialVector.ElementAt(0);
            var c = initialVector.ElementAt(1);
            var m = initialVector.ElementAt(2);

            var currentRnd = seed;
            for (int i = 0; i < numbersCount; i++)
            {
                currentRnd = (a * currentRnd + c) % m;
                yield return currentRnd % maxValue;
            }
        }

        public IEnumerable<int> GetDefaultParameters()
        {
            var defaultParameters = new int[33, 3]{
                { 106, 1283, 6075 }, { 625, 6571, 31104 }, { 1277, 24749, 117128 },
	            { 211, 1663, 7875 }, { 1541, 2957, 14000 }, { 2041, 25673, 121500 },
	            { 421, 1663, 7875 }, { 1741, 2731, 12960 }, { 2311, 25367, 120050 },
	            { 430, 2531, 11979 }, { 1291, 4621, 21870 }, { 1597, 51749, 244944 },
	            { 936, 1399, 6655 }, { 205, 29573, 139968 }, { 2661, 36979, 175000 },
	            { 1366, 1283, 6075 }, { 421,  17117, 81000 }, { 4081, 25673, 121500 },
	            { 171, 11213, 53125 }, { 1255, 6173, 29282 }, { 3661, 30809, 145800 },
	            { 859, 2531, 11979 }, { 281, 28411, 134456 }, { 3613, 45289, 214326 },
	            { 419, 6173, 29282 }, { 1093, 18257, 86436 }, { 1366, 150889, 714025 },
	            { 967, 3041, 14406 }, { 421, 54773, 259200 }, { 8121, 28411, 134456 },
	            { 141, 28411, 134456 }, { 1021, 24631, 116640 }, { 4561, 51349, 243000 }
            };

            return defaultParameters.GetRow(IGenerator.Rnd.Next(defaultParameters.GetLength(0)));
        }
    }
}
