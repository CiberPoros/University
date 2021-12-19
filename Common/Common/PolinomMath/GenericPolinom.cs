using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.PolinomMath
{
    public struct GenericPolinom<T>
    {
        private readonly Wrapper<T>[] _values;

        public IReadOnlyList<T> Values => _values.Select(x => x.Value).ToList();

        public int Degree => _values.Length - 1;

        public bool IsEmpty => _values.Length == 0;

        public GenericPolinom(T[] values)
        {
            _values = new Wrapper<T>[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                _values[i] = new Wrapper<T>(values[i]);
            }
        }

        private GenericPolinom(Wrapper<T>[] values)
        {
            _values = new Wrapper<T>[values.Length];
            values.CopyTo(_values, 0);
        }

        T this[int index]
        {
            get => _values[index].Value;
            set => _values[index] = new Wrapper<T>(value);
        }

        public static GenericPolinom<T> operator -(GenericPolinom<T> left, GenericPolinom<T> right)
        {
            var result = new List<Wrapper<T>>();

            for (int i = Math.Max(left.Degree, right.Degree); i >= 0; i--)
            {
                var leftValue = new Wrapper<T>(left.Degree >= i ? left[i] : default);
                var rightValue = new Wrapper<T>(right.Degree >= i ? right[i] : default);
                result.Add(leftValue - rightValue);
            }

            return new GenericPolinom<T>(result.SkipWhile(x => x.Value.Equals(default)).Reverse().ToArray());
        }

        public static GenericPolinom<T> operator *(GenericPolinom<T> polinom, T multiplier)
        {
            var resultArray = new Wrapper<T>[polinom.Degree + 1];
            var wrappedMultiplier = new Wrapper<T>(multiplier);

            for (int i = 0; i < polinom.Degree + 1; i++)
            {
                resultArray[i] = polinom._values[i] * wrappedMultiplier;
            }

            return new GenericPolinom<T>(resultArray);
        }

        public static GenericPolinom<T> operator *(GenericPolinom<T> left, GenericPolinom<T> right)
        {
            var resultArray = new Wrapper<T>[left.Degree + right.Degree + 1];

            for (int i = 0; i < left.Degree + 1; i++)
            {
                for (int j = 0; j < right.Degree + 1; j++)
                {
                    resultArray[i + j] += left._values[i] * right._values[j];
                }
            }

            return new GenericPolinom<T>(resultArray);
        }

        public static GenericPolinom<T> operator /(GenericPolinom<T> left, GenericPolinom<T> right)
        {
            var currentLeft = left;
            Wrapper<T>[] result = null;
            for (; ; )
            {
                var multiplierDegree = currentLeft.Degree - right.Degree;
                if (multiplierDegree < 0)
                {
                    break;
                }

                var multiplierConst = currentLeft._values[currentLeft.Degree] / right._values[right.Degree];
                var multiplierArray = new Wrapper<T>[multiplierDegree + 1];
                multiplierArray[multiplierDegree] = multiplierConst;
                var multiplier = new GenericPolinom<T>(multiplierArray);

                if (multiplierDegree < 0)
                {
                    break;
                }

                if (result is null)
                {
                    result = new Wrapper<T>[multiplierDegree + 1];
                }

                result[multiplierDegree] = multiplierConst;
                currentLeft -= right * multiplier;
            }

            return new GenericPolinom<T>(result ?? Array.Empty<Wrapper<T>>());
        }

        public static bool operator ==(GenericPolinom<T> left, GenericPolinom<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GenericPolinom<T> left, GenericPolinom<T> right)
        {
            return !(left == right);
        }

        public (GenericPolinom<T> result, GenericPolinom<T> remainder) Divide(GenericPolinom<T> polinom)
        {
            var currentLeft = this;
            Wrapper<T>[] result = null;
            for (; ; )
            {
                var multiplierDegree = currentLeft.Degree - polinom.Degree;
                if (multiplierDegree < 0)
                {
                    break;
                }

                var multiplierConst = currentLeft._values[currentLeft.Degree] / polinom._values[polinom.Degree];
                var multiplierArray = new Wrapper<T>[multiplierDegree + 1];
                multiplierArray[multiplierDegree] = multiplierConst;
                var multiplier = new GenericPolinom<T>(multiplierArray);

                if (multiplierDegree < 0)
                {
                    break;
                }

                if (result is null)
                {
                    result = new Wrapper<T>[multiplierDegree + 1];
                }

                result[multiplierDegree] = multiplierConst;
                currentLeft -= polinom * multiplier;
            }

            return (new GenericPolinom<T>(result ?? Array.Empty<Wrapper<T>>()), currentLeft);
        }

        public T CalculateValue(T x)
        {
            var result = new Wrapper<T>(default);
            var xWrapped = new Wrapper<T>(x);
            var current = xWrapped;

            if (_values.Length >= 1)
            {
                result += _values[0];
            }

            for (int i = 1; i < _values.Length; i++)
            {
                result += _values[i] * current;
                current *= xWrapped;
            }

            return result.Value;
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
                if (_values[i] == default)
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
                    _ => string.Concat(_values[i] >= default(Wrapper<T>) ? _values[i] : -_values[i], xStr)
                };

                sb.Append(valStr == string.Empty ? string.Empty : $" {(_values[i] < default(Wrapper<T>) ? '-' : '+')}{(sb.Length > 0 ? " " : string.Empty)}{valStr}");
            }

            return sb.ToString().Trim(' ', '+');
        }

        public override bool Equals(object obj)
        {
            if (obj is not GenericPolinom<T> polinom)
            {
                return false;
            }

            return _values.SequenceEqual(polinom._values);
        }

        public override int GetHashCode()
        {
            var result = _values.Length > 0 ? _values[0].GetHashCode() : 0;

            for (int i = 1; i < _values.Length; i++)
            {
                result ^= _values[i].GetHashCode();
            }

            return result;
        }

        private struct Wrapper<U>
        {
            public readonly U Value;

            private static Type _type;

            private static MethodInfo _mi_op_UnaryPlus;
            private static MethodInfo _mi_op_UnaryNegation;
            private static MethodInfo _mi_op_Increment;
            private static MethodInfo _mi_op_Decrement;
            private static MethodInfo _mi_op_Addition;
            private static MethodInfo _mi_op_Subtraction;
            private static MethodInfo _mi_op_Multiply;
            private static MethodInfo _mi_op_MultiplyConst;
            private static MethodInfo _mi_op_Division;
            private static MethodInfo _mi_op_Equality;
            private static MethodInfo _mi_op_Inequality;
            private static MethodInfo _mi_op_LessThan;
            private static MethodInfo _mi_op_GreaterThan;
            private static MethodInfo _mi_op_LessThanOrEqual;
            private static MethodInfo _mi_op_GreaterThanOrEqual;

            public static Type Type => _type ??= typeof(U);

            public static MethodInfo Mi_op_UnaryPlus => _mi_op_UnaryPlus ??= Type.GetMethod("op_UnaryPlus", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_UnaryNegation => _mi_op_UnaryNegation ??= Type.GetMethod("op_UnaryNegation", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_Increment => _mi_op_Increment ??= Type.GetMethod("op_Increment", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_Decrement => _mi_op_Decrement ??= Type.GetMethod("op_Decrement", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_Addition => _mi_op_Addition ??= Type.GetMethod("op_Addition", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_Subtraction => _mi_op_Subtraction ??= Type.GetMethod("op_Subtraction", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_Multiply => _mi_op_Multiply ??= Type.GetMethod("op_Multiply", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_MultiplyConst => _mi_op_MultiplyConst ??= Type.GetMethod("op_Multiply", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_Division => _mi_op_Division ??= Type.GetMethod("op_Division", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_Equality => _mi_op_Equality ??= Type.GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_Inequality => _mi_op_Inequality ??= Type.GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_LessThan => _mi_op_LessThan ??= Type.GetMethod("op_LessThan", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_GreaterThan => _mi_op_GreaterThan ??= Type.GetMethod("op_GreaterThan", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_LessThanOrEqual => _mi_op_LessThanOrEqual ??= Type.GetMethod("op_LessThanOrEqual", BindingFlags.Static | BindingFlags.Public);
            public static MethodInfo Mi_op_GreaterThanOrEqual => _mi_op_GreaterThanOrEqual ??= Type.GetMethod("op_GreaterThanOrEqual", BindingFlags.Static | BindingFlags.Public);

            public Wrapper(U value)
            {
                Value = value;

                if (_type is null)
                {
                    _type = value.GetType();
                }
            }

            public static Wrapper<U> operator +(Wrapper<U> value)
            {
                return new Wrapper<U>((U)Mi_op_UnaryPlus.Invoke(null, new object[] { value }));
            }

            public static Wrapper<U> operator -(Wrapper<U> value)
            {
                return new Wrapper<U>((U)Mi_op_UnaryNegation.Invoke(null, new object[] { value }));
            }

            public static Wrapper<U> operator ++(Wrapper<U> value)
            {
                return new Wrapper<U>((U)Mi_op_Increment.Invoke(null, new object[] { value }));
            }

            public static Wrapper<U> operator --(Wrapper<U> value)
            {
                return new Wrapper<U>((U)Mi_op_Decrement.Invoke(null, new object[] { value }));
            }

            public static Wrapper<U> operator +(Wrapper<U> left, Wrapper<U> right)
            {
                return new Wrapper<U>((U)Mi_op_Addition.Invoke(null, new object[] { left, right }));
            }

            public static Wrapper<U> operator -(Wrapper<U> left, Wrapper<U> right)
            {
                return new Wrapper<U>((U)Mi_op_Subtraction.Invoke(null, new object[] { left, right }));
            }

            public static Wrapper<U> operator *(Wrapper<U> left, Wrapper<U> right)
            {
                var test = Mi_op_Multiply;
                return new Wrapper<U>((U)Mi_op_Multiply.Invoke(null, new object[] { left, right }));
            }

            public static Wrapper<U> operator *(Wrapper<U> left, T right)
            {
                return new Wrapper<U>((U)Mi_op_Multiply.Invoke(null, new object[] { left, right }));
            }

            public static Wrapper<U> operator /(Wrapper<U> left, Wrapper<U> right)
            {
                return new Wrapper<U>((U)Mi_op_Division.Invoke(null, new object[] { left, right }));
            }

            public static bool operator ==(Wrapper<U> left, Wrapper<U> right)
            {
                return (bool)Mi_op_Equality.Invoke(null, new object[] { left, right });
            }

            public static bool operator !=(Wrapper<U> left, Wrapper<U> right)
            {
                return (bool)Mi_op_Inequality.Invoke(null, new object[] { left, right });
            }

            public static bool operator <(Wrapper<U> left, Wrapper<U> right)
            {
                return (bool)Mi_op_LessThan.Invoke(null, new object[] { left, right });
            }

            public static bool operator >(Wrapper<U> left, Wrapper<U> right)
            {
                return (bool)Mi_op_GreaterThan.Invoke(null, new object[] { left, right });
            }

            public static bool operator <=(Wrapper<U> left, Wrapper<U> right)
            {
                return (bool)Mi_op_LessThanOrEqual.Invoke(null, new object[] { left, right });
            }

            public static bool operator >=(Wrapper<U> left, Wrapper<U> right)
            {
                return (bool)Mi_op_GreaterThanOrEqual.Invoke(null, new object[] { left, right });
            }

            public override bool Equals(object obj)
            {
                return Value.Equals(obj);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}
