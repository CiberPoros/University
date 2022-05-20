using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators.Programs.Analizer.Criteria
{
    internal class PartitioningCriteria : ICriteria
    {
        public string Name => "Критерий разбиений";

        public (double val, bool isAccepted) CheckCriteria(IEnumerable<double> values)
        {
            var maxVal = 5;
            var arr = new List<int>();

            foreach (var val in values)
            {
                var value = (int)Math.Floor(val * maxVal);

                arr.Add(value);
            }

            var hitsCnt = new int[maxVal];
            for (int i = 0; i < arr.Count - maxVal + 1; i++)
            {
                hitsCnt[arr.Skip(0).Take(maxVal).Distinct().Count() - 1]++; // Количество различных чисел из текущего кусочка массива
            }

            // посчитал для 5 руками
            var hitsP = new double[]
            {
                0.0015864d, // все одинаковые
                0.0960201d,
                0.4799081d,
                0.384056d,
                0.0384294d, // все разные
            };

            var cntSum = hitsCnt.Sum();
            var sum = 0d;
            for (int i = 0; i < maxVal; i++)
            {
                sum += ((hitsCnt[i] + .0) / cntSum - hitsP[i]) * ((hitsCnt[i] + .0) / cntSum - hitsP[i]) / hitsP[i];
            }
            sum *= maxVal;

            // не очень понятно, какую тут вероятность брать. Беру самую маленькую
            // число степеней свободы maxVal + 1 = 6
            return (sum, sum > 18.54758);
        }
    }
}
