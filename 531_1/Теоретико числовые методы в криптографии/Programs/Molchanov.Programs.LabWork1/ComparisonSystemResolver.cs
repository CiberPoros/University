using System.Numerics;
using System.Text;

namespace Molchanov.Programs.LabWork1
{
    internal class ComparisonSystemResolver
    {
        public (BigInteger result, List<string> outputInfo) ByChineseTheorem(IEnumerable<BigInteger> mValues, IEnumerable<BigInteger> uValues)
        {   
            var (result, val1, val2) = IsMutuallySimple(mValues);
            if (!result)
            {
                throw new ArgumentException($"Числа должны быть взаимнопростыми. {val1} и {val2} имеют наименьший общий делитель, отличный от 1.", nameof(mValues));
            }

            var outputInfo = new List<string>();
            var mArr = mValues.ToArray();
            var uArr = uValues.ToArray();

            BigInteger m = 1;
            var sb = new StringBuilder();
            sb.Append("m = ");
            for (int i = 0; i < mArr.Length; i++)
            {
                m *= mArr[i];
                sb.Append($"{mArr[i]} + ");
            }
            outputInfo.Add(sb.ToString().Trim(' ', '+'));

            var cArr = new BigInteger[mArr.Length];
            for (int i = 0; i < cArr.Length; i++)
            {
                cArr[i] = m / mArr[i];
                outputInfo.Add($"c[{i}] = M / m[i] = {m} / {mArr[i]} = {cArr[i]}");
            }

            var dArr = new BigInteger[mArr.Length];
            for (int i = 0; i < dArr.Length; i++)
            {
                dArr[i] = GetInverse(cArr[i], mArr[i]);
                outputInfo.Add($"d[{i}] = c[i]^-1 mod m[i] = {cArr[i]}^-1 mod {mArr[i]} = {dArr[i]}");
            }

            BigInteger u = 0;
            {
                for (int i = 0; i < mArr.Length; i++)
                {
                    u += (cArr[i] * dArr[i] * uArr[i]) % m;
                }
            }

            return (u % m, outputInfo);
        }

        private (bool result, BigInteger val1, BigInteger val2) IsMutuallySimple(IEnumerable<BigInteger> values)
        {
            var arr = values.ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (BigInteger.GreatestCommonDivisor(arr[i], arr[j]) != 1)
                    {
                        return (false, arr[i], arr[j]);
                    }
                }
            }

            return (true, 0 , 0);
        }

        private static BigInteger GetInverse(BigInteger value, BigInteger mod)
        {
            var gcdCalc = new GCDCalculator();
            var gcdResult = gcdCalc.ByEuklidExtend(value, mod);

            var result = ((gcdResult.u % mod) + mod) % mod;
            return result;
        }
    }
}
