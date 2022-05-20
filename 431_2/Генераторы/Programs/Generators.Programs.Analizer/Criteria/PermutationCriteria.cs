using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generators.Programs.Analizer.Criteria
{
    internal class PermutationCriteria : ICriteria
    {
        public string Name => "Критерий перестановок";

        public (double val, bool isAccepted) CheckCriteria(IEnumerable<double> values)
        {
            Dictionary<string, int> cnt = new Dictionary<string, int>();

            var t = 4;
            var arr = values.ToArray();

            for (int i = 0; i < arr.Length - t; i++)
            {
                var str = GetStr(arr.Skip(i).Take(t + 1).ToArray());

                if (cnt.ContainsKey(str))
                {
                    cnt[str]++;
                }
                else
                {
                    cnt.Add(str, 1);
                }
            }

            var sum = 0d;
            var p = 1d / (24);
            foreach (var kvp in cnt)
            {
                sum += ((kvp.Value + .0) / (arr.Length - t) - p) * ((kvp.Value + .0) / (arr.Length - t) - p) / (p * arr.Length);
            }
            sum *= (arr.Length - t + 1);

            return (sum, sum < 40.64647d);
        }

        private static string GetStr(double[] values)
        {
            var sb = new StringBuilder();

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(values[i] > values[i - 1] ? '1' : '0');
            }

            return sb.ToString();
        }
    }
}
