using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolinomMath
{
    public struct PolinomDouble
    {
        private readonly double[] _values;

        public IReadOnlyList<double> Values => _values;

        public int Degree => _values.Length - 1;

        public bool IsEmpty => _values.Length == 0;

        public PolinomDouble(double[] values)
        {
            values = values.Reverse().SkipWhile(x => x == 0).Reverse().ToArray();
            _values = new double[values.Length];
            values.CopyTo(_values, 0);
        }

        double this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public static PolinomDouble GCD(PolinomDouble left, PolinomDouble right)
        {
            return right.IsDegree0OrValuesToSmall() ? left : GCD(right, left % right);
        }

        public static PolinomDouble operator -(PolinomDouble left, PolinomDouble right)
        {
            var result = new List<double>();

            for (int i = Math.Max(left.Degree, right.Degree); i >= 0; i--)
            {
                var leftValue = left.Degree >= i ? left[i] : 0;
                var rightValue = right.Degree >= i ? right[i] : 0;
                result.Add(leftValue - rightValue);
            }

            return new PolinomDouble(result.SkipWhile(x => x == 0).Reverse().ToArray());
        }

        public static PolinomDouble operator *(PolinomDouble polinom, double multiplier)
        {
            var resultArray = new double[polinom.Degree + 1];

            for (int i = 0; i < polinom.Degree + 1; i++)
            {
                resultArray[i] = polinom[i] * multiplier;
            }

            return new PolinomDouble(resultArray);
        }

        public static PolinomDouble operator *(PolinomDouble left, PolinomDouble right)
        {
            var resultArray = new double[left.Degree + right.Degree + 1];

            for (int i = 0; i < left.Degree + 1; i++)
            {
                for (int j = 0; j < right.Degree + 1; j++)
                {
                    resultArray[i + j] += left[i] * right[j];
                }
            }

            return new PolinomDouble(resultArray);
        }

        public static PolinomDouble operator /(PolinomDouble left, PolinomDouble right)
        {
            (var result, _) = left.Divide(right);
            return result;
        }

        public static PolinomDouble operator %(PolinomDouble left, PolinomDouble right)
        {
            (_, var remainder) = left.Divide(right);
            return remainder;
        }

        public static bool operator ==(PolinomDouble left, PolinomDouble right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PolinomDouble left, PolinomDouble right)
        {
            return !(left == right);
        }

        public (PolinomDouble result, PolinomDouble remainder) Divide(PolinomDouble polinom)
        {
            var currentLeft = this;
            double[] result = null;
            for (; ; )
            {
                var multiplierDegree = currentLeft.Degree - polinom.Degree;
                if (multiplierDegree < 0)
                {
                    break;
                }

                var multiplierConst = currentLeft[currentLeft.Degree] / polinom[polinom.Degree];
                var multiplierArray = new double[multiplierDegree + 1];
                multiplierArray[multiplierDegree] = multiplierConst;
                var multiplier = new PolinomDouble(multiplierArray);

                if (multiplierDegree < 0)
                {
                    break;
                }

                if (result is null)
                {
                    result = new double[multiplierDegree + 1];
                }

                result[multiplierDegree] = multiplierConst;
                currentLeft -= polinom * multiplier;
            }

            return (new PolinomDouble(result ?? Array.Empty<double>()), currentLeft);
        }

        public double CalculateValue(double x)
        {
            var result = 0d;

            for (int i = 0; i < _values.Length; i++)
            {
                result += Math.Pow(_values[i] * x, i);
            }

            return result;
        }

        public override string ToString()
        {
            if (IsEmpty)
            {
                return "0";
            }

            var sb = new StringBuilder();

            for (int i = Degree; i >= 0; i--)
            {
                if (this[i] == 0)
                {
                    continue;
                }

                var xStr = i switch
                {
                    0 => string.Empty,
                    1 => "x",
                    _ => $"x^{i}"
                };
                var valStr = this[i] switch
                {
                    0 => string.Empty,
                    1 => i == 0 ? "1" : xStr,
                    _ => string.Concat(Math.Abs(this[i]), xStr)
                };

                sb.Append(valStr == string.Empty ? string.Empty : $" {(this[i] < 0 ? '-' : '+')}{(sb.Length > 0 ? " " : string.Empty)}{valStr}");
            }

            return sb.ToString().Trim(' ', '+');
        }

        public override bool Equals(object obj)
        {
            if (obj is not PolinomDouble polinom)
            {
                return false;
            }

            return _values.SequenceEqual(polinom._values);
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(Math.Ceiling(CalculateValue(1)));
        }

        private bool IsDegree0OrValuesToSmall()
        {
            return Degree <= 0 || _values.Skip(1).All(x => Math.Abs(x) < 0.00001d);
        }
    }
}
