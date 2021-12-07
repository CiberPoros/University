using System;
using System.Collections.Generic;

namespace Common
{
    // TODO: to non-static
    public static class SimpleFactorization
    {
        public static IEnumerable<int> FactorizationByBruteForce(int value)
        {
            bool isPrime;
            do
            {
                isPrime = true;
                var sqrt = Math.Sqrt(value);

                for (int i = 2; i <= sqrt; i++)
                {
                    if (value % i == 0)
                    {
                        yield return i;
                        value /= i;
                        isPrime = false;
                        break;
                    }
                }
            }
            while (!isPrime);
            yield return value;
        }
    }
}
