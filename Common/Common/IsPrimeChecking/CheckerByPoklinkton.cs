using System;
using System.Numerics;

namespace Common.IsPrimeChecking
{
    public class CheckerByPoklinkton : IPrimeChecker
    {
        public bool IsProbabilisticByPrimeResult => false;

        public bool IsProbabilisticByComplexResult => false;

        public bool IsPrime(BigInteger value)
        {
            throw new NotImplementedException();
        }
    }
}
