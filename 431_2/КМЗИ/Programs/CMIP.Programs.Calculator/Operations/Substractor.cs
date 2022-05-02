using System.Collections.Generic;

namespace CMIP.Programs.Calculator.Operations
{
    public class Substractor : AbstractOperator
    {
        public Substractor(List<char> alphabet) : base(alphabet)
        {
        }

        protected override bool NeedInversePrev => true;

        protected override bool NeedInverseAfter => true;
        
        protected override Number CalculateInternal(Number left, Number right)
        {
            var remains = 0;
            var result = new List<char>();
            for (var index = 0; index < left.Length || index < right.Length; index++)
            {
                var current = remains;
                if (index < left.Length)
                {
                    current += Alphabet.IndexOf(left[index]);
                }

                if (index < right.Length)
                {
                    current -= Alphabet.IndexOf(right[index]);
                }

                remains = current >= 0 ? 0 : -1;
                result.Add(Alphabet[((current % Alphabet.Count) + Alphabet.Count) % Alphabet.Count]);
            }

            return new Number(result.ToArray());
        }
    }
}
