using System.Numerics;

namespace Common.IsPrimeChecking
{
    public interface IPrimeChecker
    {
        bool IsPrime(BigInteger value);

        public bool IsProbabilisticByPrimeResult { get; }
        public bool IsProbabilisticByComplexResult { get; }
    }
}
