using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByRabinMiller : IPrimeChecker
    {
        public int RoundsCount { get; set; }

        public bool IsProbabilisticByPrimeResult => true;

        public bool IsProbabilisticByComplexResult => false;

        public bool IsPrime(BigInteger value)
        {
            if (value == 1 || value == 3)
            {
                return true;
            }

            if (value % 2 == 0)
            {
                return false;
            }

            var t = value - 1;
            var degree2 = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                degree2++;
            }

            for (int i = 0; i < RoundsCount; i++)
            {
                var a = Generator.Next(2, value - 1);
                var x = BigInteger.ModPow(a, t, value);

                if (x == 1 || x == value - 1)
                {
                    continue;
                }

                bool isBroken = false;
                for (int j = 0; j < degree2 - 1; j++)
                {
                    x = (x * x) % value;

                    if (x == 1)
                    {
                        return false;
                    }

                    if (x == value - 1)
                    {
                        isBroken = true;
                        break;
                    }
                }

                if (!isBroken)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
