using System.Collections.Generic;
using System.Numerics;
using Common.IsPrimeChecking;

namespace Common.Factorization
{
    public class FactorizatorByFerma : IFactorizator
    {
        public IEnumerable<BigInteger> Factorize(BigInteger value)
        {
            var isPrimeChecker = new CheckerByRabinMiller() { RoundsCount = 15 };

            while (!isPrimeChecker.IsPrime(value))
            {
                var divider = GetDivider(value);
                yield return divider;

                value /= divider;
            }

            yield return value;
        }

        private static BigInteger GetDivider(BigInteger value)
        {
            if (value % 2 == 0)
            {
                return 2;
            }

            var s = AdditionalMath.IntegerSqrt(value);
            if (s * s < value)
            {
                s++;
            }

            if (s * s == value)
            {
                return s;
            }

            var x = s;
            var l = x * x - value;
            for (int k = 1; ; k++, x++)
            {
                var y = AdditionalMath.IntegerSqrt(l);
                if (y * y == l)
                {
                    return x + y;
                }

                l = (s + k) * (s + k) - value;
            }
        }
    }
}
