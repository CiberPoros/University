using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator.Operations
{
    public class Multiplier : AbstractOperator
    {
        public Multiplier(List<char> alphabet) : base(alphabet)
        {
        }

        protected override bool NeedInversePrev => true;

        protected override bool NeedInverseAfter => false;

        protected override Number CalculateInternal(Number left, Number right)
        {
            var result = Enumerable.Repeat(0, left.Length + right.Length).ToList();
            for (int i = 0; i < right.Length; i++)
            {
                var remains = 0;
                var value = Alphabet.IndexOf(right[i]);
                for (int j = 0; j < left.Length; j++)
                {
                    var internalValue = Alphabet.IndexOf(left[j]);
                    var currentResult = value * internalValue + remains + result[j + i];
                    result[i + j] = currentResult % Alphabet.Count;
                    remains = currentResult / Alphabet.Count;
                }
                result[i + left.Length] = remains;
            }

            return new Number(result.Select(x => Alphabet[x]).Reverse().ToList());
        }
    }
}
