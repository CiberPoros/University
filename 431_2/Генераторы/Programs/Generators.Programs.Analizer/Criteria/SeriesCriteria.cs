using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators.Programs.Analizer.Criteria
{
    internal class SeriesCriteria : ICriteria
    {
        public string Name => "Критерий серий";

        public (double val, bool isAccepted) CheckCriteria(IEnumerable<double> values)
        {
            // Вероятность = 1 / (3 * 3) = 0.11 (примерно 0.1)
            var k = 3; // Степень свободы = k * k - 1 = 8

            var cnt = new int[k,k];
            var arr = values.ToArray();

            for (int i = 1; i < arr.Length; i++)
            {
                cnt[(int)Math.Floor(arr[i - 1] * k), (int)Math.Floor(arr[i] * k)]++;
            }

            var count = arr.Length - 1;

            var sum = 0d;
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    var val = cnt[i, j];
                    sum += ((val + .0) / count - 1d / (k * k)) * ((val + .0) / count - 1d / (k * k)) / (1d / (k * k));
                }
            }
            sum *= count;

            return (sum, sum < 13.36157d);
        }
    }
}
