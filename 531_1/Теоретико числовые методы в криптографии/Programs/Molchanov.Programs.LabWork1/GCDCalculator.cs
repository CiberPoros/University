using System.Numerics;

namespace Molchanov.Programs.LabWork1
{
    internal class GCDCalculator
    {
        public (BigInteger result, List<string> outputInfo) ByEuklid(BigInteger x1, BigInteger x2)
        {
            if (x1 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x1), x1, "Parameter can't be less or equal zero");
            }

            if (x2 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x2), x2, "Parameter can't be less or equal zero");
            }

            List<string> outputInfo = new List<string>();
            return (ByEuklidInternal(x1, x2, outputInfo), outputInfo);
        }

        private BigInteger ByEuklidInternal(BigInteger x1, BigInteger x2, List<string> outputInfo, int deep = 0)
        {
            outputInfo.Add($"r[{deep - 1}] = {x1}; r[{deep}] = {x2}");
            return x2 == 0 ? x1 : ByEuklidInternal(x2, x1 % x2, outputInfo, deep + 1);
        }

        public (BigInteger result, BigInteger u, BigInteger v, List<string> outputInfo) ByEuklidExtend(BigInteger x1, BigInteger x2)
        {
            if (x1 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x1), x1, "Parameter can't be less or equal zero");
            }

            if (x2 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x2), x2, "Parameter can't be less or equal zero");
            }

            List<string> outputInfo = new List<string>();
            BigInteger u = 0, v = 0;
            var result = ByEuklidInternal(x1, x2, outputInfo, 0, ref u, ref v);
            outputInfo.Reverse();
            return (result, u, v, outputInfo);
        }

        private BigInteger ByEuklidInternal(BigInteger x1, BigInteger x2, List<string> outputInfo, int deep, ref BigInteger u, ref BigInteger v)
        {
            if (x1 == 0)
            {
                u = 0; 
                v = 1;
               
                outputInfo.Add($"r[{deep - 1}] = {x1}; r[{deep}] = {x2}; u[{deep}] = {u}; v[{deep}] = {v}");

                return x2;
            }
            BigInteger u1 = 0, v1 = 0;
            BigInteger d = ByEuklidInternal(x2 % x1, x1, outputInfo, deep + 1, ref u1, ref v1);
            u = v1 - (x2 / x1) * u1;
            v = u1;

            outputInfo.Add($"r[{deep - 1}] = {x1}; r[{deep}] = {x2 % x1}; u[{deep}] = {u}; v[{deep}] = {v}");
            
            return d;
        }

        public (BigInteger result, List<string> outputInfo) ByBinaryAlg(BigInteger a, BigInteger b)
        {
            if (a <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(a), a, "Parameter can't be less or equal zero");
            }

            if (b <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(b), b, "Parameter can't be less or equal zero");
            }

            List<string> outputInfo = new List<string>();
            var result = ByBinaryAlgInternal(a, b, outputInfo);

            return (result, outputInfo);
        }

        private BigInteger ByBinaryAlgInternal(BigInteger x, BigInteger y, List<string> outputInfo, int deep = 0)
        { 
            if (x < y)
            {
                (x, y) = (y, x);
            }

            outputInfo.Add($"x[{deep}] = {x}; y[{deep}] = {y}");

            if (y == 0)
            {
                return x;
            }

            var e = 0;
            var deg = 1;

            for (; ; )
            {
                if (deg * y <= x && x < (deg << 1) * y)
                {
                    break;
                }

                e++;
                deg <<= 1;
            }

            var t = BigInteger.Min((deg << 1) * y - x, x - deg * y);

            (BigInteger xNext, BigInteger yNext) = t <= y ? (y, t) : (t, y);

            return ByBinaryAlgInternal(xNext, yNext, outputInfo, deep + 1);
        }
    }
}
