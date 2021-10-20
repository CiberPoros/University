using System;
using System.Numerics;

namespace Common
{
    public class Generator
    {
        private static readonly Random _random = new();

        public static BigInteger Next(BigInteger maxValue)
        {
            var log = Convert.ToInt32(Math.Ceiling(BigInteger.Log(maxValue, 2)));

            for (; ; )
            {
                var result = new BigInteger(0);
                for (int i = 0; i < log; i++)
                {
                    result |= _random.Next(0, 2);
                    result <<= 1;
                }

                if (result < maxValue)
                {
                    return result;
                }
            }    
        }

        public static BigInteger Next(BigInteger minValue, BigInteger maxValue) => Next(maxValue - minValue) + minValue;
    }
}
