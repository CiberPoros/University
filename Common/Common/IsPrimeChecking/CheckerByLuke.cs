using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByLuke : IPrimeChecker
    {
        public int RoundsCount { get; set; }

        public bool IsProbabilisticByPrimeResult => false;

        public bool IsProbabilisticByComplexResult => true;

        public bool IsPrime(BigInteger value)
        {
            if (value == 1 || value == 2)
                return true;

            if (value % 2 == 0)
                return false;

            for (int i = 0; i < RoundsCount; i++)
            {
                var a = Generator.Next(2, value);

                if (BigInteger.ModPow(a, value - 1, value) != 1)
                {
                    return false;
                }

                var isBroken = false;
                for (BigInteger q = 1; q < value - 1; q++)
                {
                    if ((value - 1) / q != 0)
                    {
                        continue;
                    }

                    var checkerByMillerRabin = new CheckerByRabinMiller() { RoundsCount = RoundsCount };
                    if (!checkerByMillerRabin.IsPrime(q))
                    {
                        continue;
                    }

                    if (BigInteger.ModPow(a, (value - 1) / q, value) == 1)
                    {
                        isBroken = true;
                        break;
                    }
                }

                if (!isBroken)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
