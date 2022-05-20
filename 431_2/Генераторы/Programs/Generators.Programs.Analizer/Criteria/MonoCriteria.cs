using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators.Programs.Analizer.Criteria
{
    internal class MonoCriteria : ICriteria
    {
        public string Name => "Критерий монотонности";

        public (double val, bool isAccepted) CheckCriteria(IEnumerable<double> values)
        {
            var arr = values.ToArray();

            var lens = new List<int>();
            var currentLen = 1;
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] >= arr[i - 1])
                    currentLen++;
                else
                {
                    lens.Add(currentLen);
                    currentLen = 1;
                }
            }

            lens.Add(currentLen);

            var a = new double[,]
            {
                { 4529.4, 9044.9, 13568, 22615, 22615, 27892 },
                { 9044.9, 18097, 27139, 36187, 45234, 55789 },
                { 13568, 27139, 40721, 54281, 67582, 83685 },
                { 18091, 36187, 54281, 72414, 90470, 111580 },
                { 22615, 45234, 67852, 90470, 113262, 139476 },
                { 27892, 55789, 83685, 111580, 139476, 172860 }
            };

            var b = new double[] { 1d / 6, 5d / 24, 11d / 120, 19d / 720, 29d / 5040, 1d / 840 };

            var c = new int[6];
            for (int i = 0; i < lens.Count; i++)
            {
                if (lens[i] < 6)
                {
                    c[lens[i]]++;
                }
            }

            var M = 1d / (arr.Length - 6);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    M += (c[i] - arr.Length * b[i]) * (c[j] - arr.Length * b[j]) / a[i, j];
                }
            }

            // ваще хз, какие тут вероятности, их тут нет
            return (M, M > 18.54758);
        }
    }
}
