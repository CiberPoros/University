using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator.Operations
{
    public abstract class AbstractOperator
    {
        public AbstractOperator(List<char> alphabet)
        {
            Alphabet = alphabet ?? throw new ArgumentNullException(nameof(alphabet));
            NumbersUtils = new NumbersUtils(Alphabet);
        }

        protected abstract bool NeedInversePrev { get; }

        protected abstract bool NeedInverseAfter { get; }


        public List<char> Alphabet { get; set; }

        public NumbersUtils NumbersUtils { get; set; }

        /// <summary>
        /// Does not depend on the signs of the number. Signs calculation have to be determined separately.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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

            left = NumbersUtils.Normalize(left);
            right = NumbersUtils.Normalize(right);

            if (NeedInversePrev)
            {
                left = new Number(left.Reverse().ToArray());
                right = new Number(right.Reverse().ToArray());
            }

            var result = CalculateInternal(left, right);
            var remains = result.RemainsByDivision;

            if (NeedInverseAfter)
            {
                result = new Number(result.Reverse().ToArray());
            }

            result = new Number(result.SkipWhile(x => x == Alphabet[0]).ToArray())
            {
                RemainsByDivision = remains
            };

            return result.Any() ? result : new Number(new char[] { Alphabet[0] }) { RemainsByDivision = remains };
        }

        protected abstract Number CalculateInternal(Number left, Number right);
    }
}
