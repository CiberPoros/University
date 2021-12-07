using System;
using System.Numerics;

namespace Common
{
    public static class AdditionalMath
    {
        public static BigInteger IntegerSqrt(BigInteger value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Parameter can't be less that 0");
            }

            if (value < 2)
            {
                return value;
            }

            var smallCandidate = IntegerSqrt(value >> 2) << 1;
            var largeCandidate = smallCandidate + 1;

            return largeCandidate * largeCandidate > value ? smallCandidate : largeCandidate;
        }
    }
}
