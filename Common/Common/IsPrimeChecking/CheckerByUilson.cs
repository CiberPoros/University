using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByUilson : IPrimeChecker
    {
        public bool IsProbabilisticByPrimeResult => false;

        public bool IsProbabilisticByComplexResult => false;

        public bool IsPrime(BigInteger value)
        {
            var result = new BigInteger(1);
            for (int i = 2; i < value; i++)
            {
                result = (result * i) % value;
            }

            return result == value - 1;
        }
    }
}
