using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators.Programs.Analizer.Criteria
{
    internal class ConflictCriteria : ICriteria
    {
        public string Name => "Критерий конфликтов";

        public (double val, bool isAccepted) CheckCriteria(IEnumerable<double> values)
        {
            var n = 10;
            var m = 100;

            var parr = new double[10];
            for (int i = 0; i < 10; i++)
            {
                parr[i] = ((CalcFact(n) + .0) / (CalcFact(i) * CalcFact(n - i))) * Math.Pow(m, -i) * Math.Pow((1 - 1d / m), n - i);
            }

            var teor = (n + .0) / m - 1 + parr[0];

            var arrCommon = values.ToArray();

            var cntEmp = 0;
            for (int i = 0; i + n - 1 < arrCommon.Length; i++)
            {
                var arr = values.Skip(i).Take(n).ToArray();

                Dictionary<int, int> dict = new Dictionary<int, int>();

                foreach (var val in arr)
                {
                    var mapped = (int)Math.Floor(val * m);

                    if (dict.ContainsKey(mapped))
                    {
                        dict[mapped]++;
                    }
                    else
                    {
                        dict.Add(mapped, 1);
                    }
                }

                cntEmp += dict.Count(x => x.Value >= 2);
            }

            var emp = (cntEmp + .0) / arrCommon.Length;

            return (emp, Math.Abs(teor - emp) < 1d);
        }

        private static int CalcFact(int n)
        {
            if (n == 0)
                return 1;

            var res = 1;
            for (int i = 2; i <= n; i++)
            {
                res *= i;
            }

            return res;
        }
    }
}
