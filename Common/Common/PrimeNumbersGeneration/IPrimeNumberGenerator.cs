using System.Numerics;

namespace Common.PrimeNumbersGeneration
{
    public interface IPrimeNumberGenerator
    {
        BigInteger Generate(int minBitsCount);
    }
}
