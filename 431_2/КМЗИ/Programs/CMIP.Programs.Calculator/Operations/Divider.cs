using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator.Operations
{
    internal class Divider : AbstractOperator
    {
        public Divider(List<char> alphabet) : base(alphabet)
        {
        }

        protected override bool NeedInversePrev => false;

        protected override bool NeedInverseAfter => false;

        // TODO: retest it, works incorrectly
        protected override Number CalculateInternal(Number left, Number right)
        {
            var divisible = left.Select(x => Alphabet.IndexOf(x)).Reverse().ToList();
            var divider = right.Select(x => Alphabet.IndexOf(x)).Reverse().ToList();
            var m = divisible.Count - divider.Count;
            var n = divider.Count;
            var b = Alphabet.Count;

            var d = b / (divider[n - 1] + 1);

            divisible = MultyplyOnConstant(divisible, d, b);
            divider = MultyplyOnConstant(divider, d, b, true);

            var qArr = new int[m + 1];
            for (var j = m; j >=0; j--) // D2
            {
                // D3
                var q = (divisible[j + n] * b + divisible[j + n - 1]) / divider[n - 1];
                var r = (divisible[j + n] * b + divisible[j + n - 1]) % divider[n - 1];
                if (q == b || q * divider[n - 2] > b * r + divisible[j + n - 2])
                {
                    q--;
                    r += divider[n - 1];

                    if (r < b && (q == b || q * divider[n - 2] > b * r + divisible[j + n - 2]))
                    {
                        q--;
                        r += divider[n - 1];
                    }
                }

                // D4
                var handler = new CalculationsHandler(Alphabet);
                var currentRes = handler.Substract(
                    new Number(divisible.Select(x => Alphabet[x])),
                    new Number(MultyplyOnConstant(divider, q, Alphabet.Count).Select(x => Alphabet[x])));

                // D5
                if (currentRes.IsPositive)
                {
                    divisible = currentRes.Select(x => Alphabet.IndexOf(x)).ToList();
                    qArr[j] = q;
                }
                else
                {
                    var sumRes = handler.Summ(
                        new Number(divisible.Select(x => Alphabet[x])),
                        new Number(Enumerable.Repeat(0, n + 1).ToList().Concat(new List<int> { 1 }).Reverse().Select(x => Alphabet[x]).ToList()));
                    divisible = sumRes.Select(x => Alphabet.IndexOf(x)).ToList();
                    qArr[j] = q;

                    // D6
                    qArr[j]--;
                    var sumD6 = handler.Summ(
                        new Number(divisible.Select(x => Alphabet[x])),
                        new Number(divider.Select(x => Alphabet[x])));

                    divisible = sumD6.Select(x => Alphabet.IndexOf(x)).ToList();
                }
            }

            return new Number(qArr.Select(x => Alphabet[x]));
        }

        public int[] Divide(int[] u, int[] v)
        {
            var b = Alphabet.Count;
            var n = v.Length;
            var m = u.Length - v.Length;

            var d = b / (v[n - 1] + 1);

            u = MultiplyOnConst(u, d);
            v = MultiplyOnConst(v, d, false);
            var qArr = new int[m];

            for (int j = m; j >= 0; j--)
            {
                var q = (u[j + n] * b + u[j + n - 1]) / v[n - 1];
                var r = (u[j + n] * b + u[j + n - 1]) % v[n - 1];

                if (q == b || q * v[n - 2] > b * r + u[j + n - 2])
                {
                    q--;
                    r += v[n - 1];

                    if (r < b && (q == b || q * v[n - 2] > b * r + u[j + n - 2]))
                    {
                        q--;
                        r += v[n - 1];
                    }
                }

                var temp = MultiplyOnConst(v, q);
                u = Minus(u, temp); // TODO: check is positive

                qArr[j] = q;
            }

            return qArr;
        }

        private int[] Minus(int[] left, int[] right)
        {
            var b = Alphabet.Count;
            var remains = 0;

            var result = left.ToArray();

            for (int l = left.Length - 1, r = right.Length - 1; r >= 0; l--, r--)
            {
                result[l] = (((left[l] - right[r] - remains) % b) + b) % b;
                remains = left[l] - right[r] - remains < 0 ? 1 : 0;
            }

            if (remains == 1)
            {
                result[result.Length - right.Length - 1]--;
            }

            return result;
        }

        private int[] MultiplyOnConst(int[] value, int constant, bool extend = true)
        {
            var b = Alphabet.Count;
            var remains = 0;

            var result = new int[value.Length + 1];
            for (int i = value.Length - 1; i >= 0; i--)
            {
                result[i] = (value[i] * constant + remains) % b;
                remains = (value[i] * constant + remains) / b;
            }

            result[0] = remains;

            if (!extend && result[0] == 0)
            {
                result = result.Skip(1).ToArray();
            }

            return result;
        }

        private List<int> MultyplyOnConstant(List<int> value, int multiplier, int modulo, bool skipZero = false)
        {
            var remains = 0;
            ICollection<int> result = new List<int>();

            for (int i = value.Count - 1; i >= 0; i--)
            {
                result.Add((value[i] * multiplier + remains) % modulo);
                remains = (value[i] * multiplier + remains) / modulo;
            }

            result.Add(remains);

            return skipZero ? result.Reverse().SkipWhile(x => x == 0).ToList() : result.Reverse().ToList();
        }
    }
}
