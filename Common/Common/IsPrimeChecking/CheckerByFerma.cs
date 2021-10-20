using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByFerma : IPrimeChecker
    {
        public int RoundsCount { get; set; }

        public bool IsProbabilisticByPrimeResult => true;

        public bool IsProbabilisticByComplexResult => false;

        public bool IsPrime(BigInteger value)
        {
            for (int i = 0; i < RoundsCount; i++)
            {
                BigInteger a;
                do
                {
                    a = Generator.Next(value);
                } while (a % value == 0);

                if (BigInteger.ModPow(a, value - 1, value) != 1)
                {
                    return false;
                }    
            }

            return true;
        }
    }
}
