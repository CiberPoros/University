using System.Linq;

namespace Common
{
    public static class EulersFunction
    {
        // TODO: to non-static
        public static int GetValueBySimpleFactorization(int value)
        {
            var divisors = Factorization.FactorizationByBruteForce(value).ToArray();

            var result = 1;
            foreach (var divisor in divisors)
            {
                result *= divisor - 1;
            }

            return result;
        }
    }
}
