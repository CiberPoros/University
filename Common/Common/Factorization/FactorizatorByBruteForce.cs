using System;
using System.Collections.Generic;
using System.Numerics;

namespace Common.Factorization
{
    public class FactorizatorByBruteForce : IFactorizator
    {
        public IEnumerable<BigInteger> Factorize(BigInteger value)
        {
            if (value > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Parameter can't be greater than {int.MaxValue}");
            }

            var n = (int)value;

            bool isPrime;
            do
            {
                isPrime = true;
                var sqrt = Math.Sqrt(n);

                for (int i = 2; i <= sqrt; i++)
                {
                    if (n % i == 0)
                    {
                        yield return i;
                        n /= i;
                        isPrime = false;
                        break;
                    }
                }
            }
            while (!isPrime);
            yield return n;
        }
    }
}
