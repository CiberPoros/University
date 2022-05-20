using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Analizer.Criteria
{
    internal class IntervalCriteria : ICriteria
    {
        public string Name => "Критерий интервалов";

        public (double val, bool isAccepted) CheckCriteria(IEnumerable<double> values)
        {
            var a = 0.2d;
            var b = 0.6d;

            var t = 2;
            var r = 0;

            var cnt = new int[t + 1];

            var arr = values.ToArray();
            foreach (var val in arr)
            {
                if (val >= a && val <= b)
                {
                    cnt[r >= t ? t : r]++;
                    r = 0;
                }
            }

            var p = (b - a);
            var p_r = p * (1d - p);
            var p_t = (1d - p) * (1d - p);

            var cntSum = cnt.Sum();
            var tmp1 = (cnt[0] / cntSum - p) * (cnt[0] / cntSum - p) / p;
            var tmp2 = (cnt[1] / cntSum - p_r) * (cnt[1] / cntSum - p_r) / p_r;
            var tmp3 = (cnt[2] / cntSum - p_t) * (cnt[2] / cntSum - p_t) / p_t;

            // тут число степеней свободы = 3, а вероятность = 0.4 (ближе всего к 0.5)
            var sum = (tmp1 + tmp2 + tmp3) * 3;
            return (sum, sum < 7.81473d);
        }
    }
}
