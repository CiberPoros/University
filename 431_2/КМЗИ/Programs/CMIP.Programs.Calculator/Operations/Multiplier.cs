using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator.Operations
{
    internal class Multiplier : AbstractOperator
    {
        public Multiplier(List<char> alphabet) : base(alphabet)
        {
        }

        protected override bool NeedInversePrev => true;

        protected override bool NeedInverseAfter => false;

        protected override Number CalculateInternal(Number left, Number right)
        {
            var numbers = new List<Number>();
            for (int i = 0; i < right.Length; i++)
            {
                var remains = 0;
                var value = Alphabet.IndexOf(right[i]);
                var currentNumber = Enumerable.Repeat(Alphabet[0], i).ToList();
                for (int j = 0; j < left.Length; j++)
                {
                    var internalValue = Alphabet.IndexOf(left[j]);
                    var currentResult = remains + value * internalValue;
                    currentNumber.Add(Alphabet[(currentResult % Alphabet.Count)]);
                    remains = currentResult / Alphabet.Count;
                }
                currentNumber.Add(Alphabet[remains]);
                currentNumber.Reverse();

                numbers.Add(new Number(currentNumber));
            }

            var summer = new Summer(Alphabet);
            var current = numbers[0];
            for (int i = 1; i < numbers.Count; i++)
            {
                current = summer.Calculate(current, numbers[i]);
            }

            return current;
        }
    }
}
