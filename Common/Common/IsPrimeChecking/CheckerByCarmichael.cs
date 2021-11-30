using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByCarmichael : IPrimeChecker
    {
        public int RoundsCount { get; set; }

        public bool IsProbabilisticByPrimeResult => true;

        public bool IsProbabilisticByComplexResult => false;

        public bool IsPrime(BigInteger value)
        {
            var isCarmichaelNumber = false;
            if (value >= 561)
            {
                isCarmichaelNumber = true;
                for (int i = 2; i < value; i++)
                {
                    if (BigInteger.GreatestCommonDivisor(value, i) == 1 && BigInteger.ModPow(i, value - 1, value) != 1)
                    {
                        isCarmichaelNumber = false;
                    }
                }
            }

            if (isCarmichaelNumber)
            {
                return false;
            }

            var checkerByFerma = new CheckerByFerma() { RoundsCount = RoundsCount };
            return checkerByFerma.IsPrime(value);
        }
    }
}
