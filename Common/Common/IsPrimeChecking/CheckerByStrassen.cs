using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByStrassen : IPrimeChecker
    {
        public int RoundsCount { get; set; }

        public bool IsProbabilisticByPrimeResult => false;

        public bool IsProbabilisticByComplexResult => true;

        public bool IsPrime(BigInteger value)
        {
            if (value % 2 == 0)
            {
                return false;
            }

            for (int i = 0; i < RoundsCount; i++)
            {
                var a = Generator.Next(2, value);

                if (BigInteger.GreatestCommonDivisor(a, value) > 1)
                {
                    return false;
                }
                
                if (BigInteger.ModPow(a, (value - 1) / 2, value) != (((Jacobian.Calculate(a, value) % value) + value) % value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
