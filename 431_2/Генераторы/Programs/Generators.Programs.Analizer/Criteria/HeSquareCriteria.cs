using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Analizer.Criteria
{
    internal class HeSquareCriteria : ICriteria
    {
        public string Name => "Критерий Хи-квадрат";

        public (double val, bool isAccepted) CheckCriteria(IEnumerable<double> values)
        {
            // Вероятность 1 / 14 = 0.7 (примерно 0.5)
            var k = 14; // Степень свободы = k - 1 = 13

            var cnt = new int[k];

            foreach (var val in values)
            {
                cnt[(int)Math.Floor(val * k)]++;
            }

            var length = values.Count();
            var result = cnt.Sum(x => ((((x + .0) / length) - (1d / k)) * (((x + .0) / length) - (1d / k))) / (1d / k)) * length;

            return (result, result < 22.36203d);
        }
    }
}
