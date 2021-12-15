using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByCarmichael : IPrimeChecker
    {
        public CheckerByCarmichael(bool withOutputToConsole)
        {
            WithOutputToConsole = withOutputToConsole;
        }

        public CheckerByCarmichael()
        {

        }

        public int RoundsCount { get; set; }

        public bool IsProbabilisticByPrimeResult => true;

        public bool IsProbabilisticByComplexResult => false;

        public bool WithOutputToConsole { get; set; }

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
                        break;
                    }
                }
            }

            if (isCarmichaelNumber)
            {
                if (WithOutputToConsole)
                {
                    System.Console.WriteLine("Число вероятно является числом Кармайкла");
                }
                return false;
            }

            var checkerByFerma = new CheckerByFerma() { RoundsCount = RoundsCount };
            if (WithOutputToConsole)
            {
                System.Console.WriteLine("Число не является числом Кармайкла");
            }
            return checkerByFerma.IsPrime(value);
        }
    }
}
