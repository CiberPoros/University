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
            if (value == 1)
            {
                return true;
            }

            for (int i = 2; i * i <= value; i++)
            {
                for (int j = 2; ; j++)
                {
                    var x = new BigInteger(i);
                    x = BigInteger.Pow(x, j);

                    if (x == value)
                    {
                        return false;
                    }

                    if (x > value)
                    {
                        break;
                    }    
                }
            }

            var log = (long)Math.Floor(BigInteger.Log(value, 2)) + 1;
            log *= log;

            int r = -1;

            for (int r_current = 0; r == -1; r_current++)
            {
                var current = value % r_current; 
                for (int i = 1; r == -1; i++)
                {
                    if (current == 1)
                    {
                        if (i > log)
                        {
                            r = r_current;
                        }
                    }

                    current = (current * value) % r_current;
                }
            }

            for (int i = 2; i <= r; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, value) != 1)
                {
                    return false;
                }
            }

            var limit = Math.Floor(Math.Sqrt(EulersFunction.GetValueBySimpleFactorization(r)) * BigInteger.Log(value, 2));

            // TODO: end this shit

            return false;
        }
    }
}
