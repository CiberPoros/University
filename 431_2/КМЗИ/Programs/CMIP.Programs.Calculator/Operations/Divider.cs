using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator.Operations
{
    public class Divider : AbstractOperator
    {
        public Divider(List<char> alphabet) : base(alphabet)
        {
        }

        protected override bool NeedInversePrev => false;

        protected override bool NeedInverseAfter => false;

        // TODO: retest it, works incorrectly
        protected override Number CalculateInternal(Number left, Number right)
        {
            var u = left.Select(x => Alphabet.IndexOf(x)).Reverse().ToArray();
            var v = right.Select(x => Alphabet.IndexOf(x)).Reverse().ToArray();
            var n = v.Length;
            var b = Alphabet.Count;
            var d = b / (v[n - 1] + 1);

            int[] qArr = CalculateInternal(ref u, ref v);

            var result = new Number(qArr.Select(x => Alphabet[x]).Reverse());

            var vTemp = new int[n];
            Array.Copy(u, 0, vTemp, 0, n);
            var dTemp = new int[] { d };
            var remains = CalculateInternal(ref vTemp, ref dTemp).Reverse().SkipWhile(x => x == 0).Reverse().ToArray();

            if (!remains.Any())
            {
                remains = new int[] { 0 };
            }

            result.RemainsByDivision = new Number(remains.Select(x => Alphabet[x]).Reverse());

            return result;
        }

        private int[] CalculateInternal(ref int[] u, ref int[] v)
        {
            var m = u.Length - v.Length;
            var n = v.Length;
            var b = Alphabet.Count;
            var d = b / (v[n - 1] + 1);

            u = MultyplyOnConstant(u, d, b);
            v = MultyplyOnConstant(v, d, b, true);

            var qArr = new int[m + 1];
            for (var j = m; j >= 0; j--) // D2
            {
                // D3
                var q = (u[j + n] * b + u[j + n - 1]) / v[n - 1];
                var r = (u[j + n] * b + u[j + n - 1]) % v[n - 1];

                do
                {
                    if (q == b || n > 1 && q * v[n - 2] > b * r + u[j + n - 2])
                    {
                        q--;
                        r += v[n - 1];
                    }
                }
                while (r < b && (q == b || n > 1 && q * v[n - 2] > b * r + u[j + n - 2]));

                // D4
                var uArr = new int[n + 1];
                Array.Copy(u, j, uArr, 0, n + 1);
                var vArr = new int[n];
                Array.Copy(v, 0, vArr, 0, n);

                var qMult = MultyplyOnConstant(vArr, q, b, true); //  vArr * q
                var isLeftGreaterOrEquals = IsLeftGreaterOrEquals(uArr, qMult);
                if (isLeftGreaterOrEquals)
                {
                    var minusResult = MinusInternal(uArr, qMult, b);
                    Array.Copy(minusResult, 0, u, j, n + 1);
                }
                else
                {
                    var inverseMinusResult = MinusInternal(qMult, uArr, b);
                    var bPow = new int[n + 2];
                    bPow[^1] = 1;
                    var currentRes = MinusInternal(bPow, inverseMinusResult, b);
                    Array.Copy(currentRes, 0, u, j, n + 1);
                }

                // D5
                qArr[j] = q;

                // D6
                if (!isLeftGreaterOrEquals)
                {
                    qArr[j]--;
                    var uTemp = new int[n + 1];
                    Array.Copy(u, j, uTemp, 0, n + 1);

                    var vTemp = new int[n + 1];
                    Array.Copy(v, 0, vTemp, 0, n);

                    var summ = SummInternalSkipRemains(uTemp, vTemp, b);
                    Array.Copy(summ, 0, u, j, n + 1);
                }
            }

            return qArr;
        }

        private static int[] MultyplyOnConstant(IList<int> value, int multiplier, int modulo, bool skipZero = false)
        {
            var remains = 0;
            var result = new List<int>();

            for (int i = 0; i < value.Count; i++)
            {
                result.Add((value[i] * multiplier + remains) % modulo);
                remains = (value[i] * multiplier + remains) / modulo;
            }

            if (skipZero && remains == 0)
            {
                return result.ToArray();
            }

            result.Add(remains);
            return result.ToArray();
        }

        private static int[] MinusInternal(IList<int> left, IList<int> right, int modulo, bool skipZero = false)
        {
            var result = left.ToList();
            var remains = 0;

            var r = right.ToArray().Reverse().SkipWhile(x => x == 0).Reverse().ToList();
            for (int i = 0; i < r.Count; i++)
            {
                var current = (((result[i] - r[i] - remains) % modulo) + modulo) % modulo;
                remains = result[i] >= r[i] + remains ? 0 : 1;
                result[i] = current;
            }

            if (remains > 0)
            {
                result[r.Count] -= remains;
            }

            if (skipZero)
            {
                return result.ToArray().Reverse().SkipWhile(x => x == 0).Reverse().ToArray();
            }

            return result.ToArray();
        }

        private static int[] SummInternalSkipRemains(IList<int> left, IList<int> right, int modulo)
        {
            var result = left.ToArray();
            var remains = 0;

            for (int i = 0; i < right.Count; i++)
            {
                var current = (result[i] + right[i] + remains) % modulo;
                remains = result[i] + right[i] + remains < modulo ? 0 : 1;
                result[i] = current;
            }

            return result;
        }

        private static bool IsLeftGreaterOrEquals(IList<int> left, IList<int> right)
        {
            var l = left.Reverse().SkipWhile(x => x == 0).Reverse().ToList();
            var r = right.Reverse().SkipWhile(x => x == 0).Reverse().ToList();

            if (l.Count > r.Count)
            {
                return true;
            }
            else if (r.Count > l.Count)
            {
                return false;
            }

            for (int i = l.Count - 1; i >= 0; i--)
            {
                if (l[i] > r[i])
                {
                    return true;
                }
                else if (r[i] > l[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
