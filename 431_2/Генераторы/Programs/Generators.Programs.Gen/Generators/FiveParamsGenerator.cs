using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Gen.Generators
{
    internal class FiveParamsGenerator : IGenerator
    {
        public IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue)
        {
            var parameters = initialVector is null
                ? GetDefaultParameters().ToList()
                : initialVector.Concat(GetDefaultParameters().Take(4 - initialVector.Count())).ToList();

            // names as in the algorithm
            int q1 = parameters[0], q2 = parameters[1], q3 = parameters[2], p = parameters[3];

            var bits = Utils.GenerateInitialBitVector(p);
            for (int i = p + 1, j = 0; j < numbersCount; i++, j++)      
            {
                var currentResultNumber = 0;
                for (int k = 0, mask = 1; k < 31; k++, mask <<= 1)
                {
                    var currentBit =
                        Utils.GetBitFromByteArray(bits, q1 + j * 31 + k)
                        ^ Utils.GetBitFromByteArray(bits, q2 + j * 31 + k)
                        ^ Utils.GetBitFromByteArray(bits, q3 + j * 31 + k)
                        ^ Utils.GetBitFromByteArray(bits, j * 31 + k);

                    var adjustedIndex = p + j * 31 + k;
                    Utils.AddLast(bits, currentBit, adjustedIndex);
                    if (currentBit)
                    {
                        currentResultNumber |= mask;
                    }
                }

                yield return currentResultNumber % maxValue;
            }
        }

        // from table in the algorithm
        public IEnumerable<int> GetDefaultParameters()
        {
            var defaultParameters = new int[10, 5];

            defaultParameters[0, 0] = 20; defaultParameters[0, 1] = 40; defaultParameters[0, 2] = 69; defaultParameters[0, 3] = 89;
            defaultParameters[1, 0] = 31; defaultParameters[1, 1] = 57; defaultParameters[1, 2] = 82; defaultParameters[1, 3] = 107;
            defaultParameters[2, 0] = 22; defaultParameters[2, 1] = 63; defaultParameters[2, 2] = 83; defaultParameters[2, 3] = 127;
            defaultParameters[3, 0] = 86; defaultParameters[3, 1] = 197; defaultParameters[3, 2] = 447; defaultParameters[3, 3] = 521;
            defaultParameters[4, 0] = 167; defaultParameters[4, 1] = 307; defaultParameters[4, 2] = 461; defaultParameters[4, 3] = 607;
            defaultParameters[5, 0] = 339; defaultParameters[5, 1] = 630; defaultParameters[5, 2] = 998; defaultParameters[5, 3] = 1279;
            defaultParameters[6, 0] = 585; defaultParameters[6, 1] = 1197; defaultParameters[6, 2] = 1656; defaultParameters[6, 3] = 2203;
            defaultParameters[7, 0] = 577; defaultParameters[7, 1] = 1109; defaultParameters[7, 2] = 1709; defaultParameters[7, 3] = 2281;
            defaultParameters[8, 0] = 809; defaultParameters[8, 1] = 1621; defaultParameters[8, 2] = 2381; defaultParameters[8, 3] = 3217;
            defaultParameters[9, 0] = 1093; defaultParameters[9, 1] = 2254; defaultParameters[9, 2] = 3297; defaultParameters[9, 3] = 4253;

            return defaultParameters.GetRow(IGenerator.Rnd.Next(defaultParameters.GetLength(0)));
        }
    }
}
