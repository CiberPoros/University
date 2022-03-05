using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator.Operations
{
    internal abstract class AbstractOperator
    {
        public AbstractOperator(List<char> alphabet)
        {
            Alphabet = alphabet ?? throw new ArgumentNullException(nameof(alphabet));
            NumbersUtils = new NumbersUtils(Alphabet);
        }

        public List<char> Alphabet { get; set; }

        public NumbersUtils NumbersUtils { get; set; }

        public virtual Number Calculate(Number left, Number right)
        {
            if (left.Any(x => !Alphabet.Contains(x)))
            {
                throw new ArgumentException("Number can contain anly alphabet symbols.", nameof(left));
            }

            if (right.Any(x => !Alphabet.Contains(x)))
            {
                throw new ArgumentException("Number can contain anly alphabet symbols.", nameof(right));
            }

            left = new Number(left.Reverse().ToArray());
            right = new Number(right.Reverse().ToArray());

            left = NumbersUtils.Normalize(left);
            right = NumbersUtils.Normalize(right);

            var result = CalculateInternal(left, right);

            result = new Number(result.Reverse().SkipWhile(x => x == Alphabet[0]).ToArray());

            return result.Any() ? result : new Number(new char[] { Alphabet[0] });
        }

        internal abstract Number CalculateInternal(Number left, Number right);
    }
}
