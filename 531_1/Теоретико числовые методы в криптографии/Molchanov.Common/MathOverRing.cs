using System.Numerics;

namespace Molchanov.Common
{
    public static class MathOverRing
    {
        public static BigInteger GetInverse(BigInteger value, BigInteger mod)
        {
			BigInteger x = 0, y = 0;

			_ = GCD(value, mod, ref x, ref y);

			return x;
        }

        private static BigInteger GCD(BigInteger a, BigInteger b, ref BigInteger x, ref BigInteger y)
		{
			if (a == 0)
			{
				x = 0; y = 1;
				return b;
			}
			BigInteger x1 = 0, y1 = 0;
			BigInteger d = GCD(b % a, a, ref x1, ref y1);
			x = y1 - (b / a) * x1;
			y = x1;
			return d;
		}
	}
}