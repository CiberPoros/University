using System.Numerics;

namespace Common
{
    public static class GCD
    {
        public static int GetGCD(int a, int b) => b != 0 ? GetGCD(b, a % b) : a;

        public static BigInteger GetGCD(BigInteger a, BigInteger b) => b != 0 ? GetGCD(b, a % b) : a;

        /// <summary>
        /// resolve a * x + b * y = GCD(a, b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static (int gcd, int x, int y) GetGCDAdvanced(int a, int b)
        {
            var gcd = GetGCDAdvancedInternal(a, b, out var x, out var y);
            return (gcd, x, y);
        }

        /// <summary>
        /// resolve a * x + b * y = GCD(a, b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static (BigInteger gcd, BigInteger x, BigInteger y) GetGCDAdvanced(BigInteger a, BigInteger b)
        {
            var gcd = GetGCDAdvancedInternal(a, b, out var x, out var y);
            return (gcd, x, y);
        }

        private static BigInteger GetGCDAdvancedInternal(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            var d = GetGCDAdvancedInternal(b % a, a, out var x1, out var y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }

        private static int GetGCDAdvancedInternal(int a, int b, out int x, out int y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            var d = GetGCDAdvancedInternal(b % a, a, out var x1, out var y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }
    }
}
