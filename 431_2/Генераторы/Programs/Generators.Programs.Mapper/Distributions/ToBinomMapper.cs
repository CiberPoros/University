using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Mapper.Distributions
{
    /// <summary>
    /// Биномиальное распределение
    /// </summary>
    internal class ToBinomMapper : AbstractMapper
    {
        // p - вероятность
        protected override double GenerateP1()
        {
            return 0.5d;
        }
        
        // n - количество испытаний
        protected override double GenerateP2()
        {
            return 55d;
        }

        protected override IEnumerable<double> MapInternal(IEnumerable<double> source, double p, double n_double)
        {
            var n = Math.Max(1, (int)Math.Ceiling(n_double));

            var src = source.ToArray();

            for (int i = 0; i < src.Length - n + 1; i += n)
            {
                var cnt = 0d;
                for (int j = i, k = 0; k < n; j++, k++)
                {
                    if (src[j] < p)
                    {
                        cnt++;
                    }
                }

                yield return cnt / n;
            }
        }
    }
}
