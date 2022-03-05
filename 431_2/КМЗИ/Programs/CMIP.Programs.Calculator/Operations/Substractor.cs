using System.Collections.Generic;

namespace CMIP.Programs.Calculator.Operations
{
    internal class Substractor : AbstractOperator
    {
        public Substractor(List<char> alphabet) : base(alphabet)
        {
        }

        internal override char[] CalculateInternal(Number left, Number right)
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
                    current += Alphabet.IndexOf(right[index]);
                }

                remains = current / Alphabet.Count;
                result.Add(Alphabet[current % Alphabet.Count]);
            }

            result.Add(Alphabet[remains]);
            return result.ToArray();
        }
    }
}
