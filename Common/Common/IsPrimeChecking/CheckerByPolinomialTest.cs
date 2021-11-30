using System;
using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByPolinomialTest : IPrimeChecker
    {
        public bool IsProbabilisticByPrimeResult => false;

        public bool IsProbabilisticByComplexResult => false;

        public bool IsPrime(BigInteger value)
        {
            if (value > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Parameter can't be large than {int.MaxValue}.");

            if (value == 1 || value == 2 || value == 3)
                return true;

            if (value % 2 == 0)
                return false;

            var sqrt = Math.Sqrt((int)value);
            
            for (int i = 3; i < sqrt; i += 2)
            {
                if (value % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
