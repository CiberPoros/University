using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Mapper.Distributions
{
    /// <summary>
    /// Треугольное распределение
    /// </summary>
    internal class ToTriangularMapper : AbstractMapper
    {
        // a
        protected override double GenerateP1()
        {
            return 0.5d;
        }

        // b
        protected override double GenerateP2()
        {
            return 0.5d;
        }

        protected override IEnumerable<double> MapInternal(IEnumerable<double> source, double a, double b)
        {
            var src = source.ToArray();

            for (int i = 1; i < src.Length; i += 2)
            {
                var u1 = src[i - 1];
                var u2 = src[i];

                yield return a + b * (u1 + u2 - 1);
            }
        }
    }
}
