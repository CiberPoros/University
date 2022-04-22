using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Mapper.Distributions
{
    internal class ToGammaMapper : AbstractMapper
    {
        // a
        protected override double GenerateP1()
        {
            return 0d;
        }

        // b
        protected override double GenerateP2()
        {
            return 2d;
        }

        // with const k = 5
        protected override IEnumerable<double> MapInternal(IEnumerable<double> source, double a, double b)
        {
            var k = 5;
            var src = source.ToArray();

            for (int i = 0; i < src.Length - k + 1; i++)
            {
                var u = src.Skip(i).Take(k).ToArray();

                var mult = 1 - u[0];
                for (int j = 1; j < u.Length; j++)
                {
                    mult *= 1 - u[j];
                }

                yield return a - b * Math.Log(mult);
            }
        }
    }
}
