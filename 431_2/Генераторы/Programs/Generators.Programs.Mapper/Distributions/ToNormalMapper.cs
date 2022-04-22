using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Mapper.Distributions
{
    /// <summary>
    /// Нормальное распределение
    /// </summary>
    internal class ToNormalMapper : AbstractMapper
    {
        // mu
        protected override double GenerateP1()
        {
            return 0d;
        }

        // sigma
        protected override double GenerateP2()
        {
            return 0.3d;
        }

        protected override IEnumerable<double> MapInternal(IEnumerable<double> source, double mu, double sigma)
        {
            var src = source.ToArray();

            for (int i = 1; i < src.Length; i += 2)
            {
                var u1 = src[i - 1];
                var u2 = src[i];

                var z1 = mu + sigma * Math.Sqrt((-2) * Math.Log(1 - u1)) * Math.Cos(2 * Math.PI * u2);
                var z2 = mu + sigma * Math.Sqrt((-2) * Math.Log(1 - u1)) * Math.Sin(2 * Math.PI * u2);
                yield return z1;
                yield return z2;
            }
        }
    }
}
