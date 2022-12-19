using System.Numerics;

namespace Eliptic
{
    public static class Jacobian
    {
        public static BigInteger Calculate(BigInteger a, BigInteger b)
        {
            if (BigInteger.GreatestCommonDivisor(a, b) != 1)
            {
                return 0;
            }

            var r = new BigInteger(1);

            if (a < 0)
            {
                a = -a;

                if (b % 4 == 3)
                {
                    r = -r;
                }
            }

            for (; ; )
            {
                var t = 0;

                while (a % 2 == 0)
                {
                    t++;
                    a /= 2;
                }

                if (t % 2 == 1 && (b % 8 == 3 || b % 8 == 5))
                {
                    r = -r;
                }

                if (a % 4 == b % 4 && a % 4 == 3)
                {
                    r = -r;
                }

                var c = a;
                a = b % c;
                b = c;

                if (a == 0)
                {
                    return r;
                }
            }
        }
    }
}
