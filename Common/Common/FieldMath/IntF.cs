using System.Numerics;

namespace Common.FieldMath
{
    public class IntF
    {
        public static BigInteger Modulo { get; set; }

        public BigInteger Value { get; set; }

        public IntF(BigInteger value)
        {
            Value = ToField(value);
        }

        public IntF GetInverse()
        {
            var (_, u, _) = GcdExtend(Value, Modulo);
            return new IntF(u);
        }

        public static IntF operator +(IntF left, IntF right)
        {
            return new IntF(left.Value + right.Value);
        }

        public static IntF operator -(IntF left, IntF right)
        {
            return new IntF(left.Value - right.Value);
        }

        public static IntF operator *(IntF left, IntF right)
        {
            return new IntF(left.Value * right.Value);
        }

        public static IntF operator /(IntF left, IntF right)
        {
            var rightInverse = right.GetInverse();

            return left * rightInverse;
        }

        private static (BigInteger result, BigInteger u, BigInteger v) GcdExtend(BigInteger x1, BigInteger x2)
        {
            x1 = ToField(x1);
            x2 = ToField(x2);

            BigInteger u = 0, v = 0;
            var result = ByEuklidInternal(x1, x2, ref u, ref v);
            return (result, u, v);
        }

        private static BigInteger ByEuklidInternal(BigInteger x1, BigInteger x2, ref BigInteger u, ref BigInteger v)
        {
            if (x1 == 0)
            {
                u = 0;
                v = 1;

                return x2;
            }

            BigInteger u1 = 0, v1 = 0;
            BigInteger d = ByEuklidInternal(x2 % x1, x1, ref u1, ref v1);
            u = v1 - (x2 / x1) * u1;
            v = u1;

            return d;
        }

        private static BigInteger ToField(BigInteger value)
        {
            return ((value % Modulo) + Modulo) % Modulo;
        }
    }
}
