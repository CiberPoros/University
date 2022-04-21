using System.Collections.Generic;

namespace CMIP.Programs.Calculator.Operations
{
    internal class Exponenciator : AbstractOperator
    {
        public Exponenciator(List<char> alphabet) : base(alphabet)
        {
        }

        protected override bool NeedInversePrev => false;

        protected override bool NeedInverseAfter => false;

        protected override Number CalculateInternal(Number left, Number right)
        {
            var n = right.ToInt32(Alphabet);
            if (n == 0)
            {
                return new Number(new char[] { Alphabet[1] });
            }
            if (n == 1)
            {
                return left;
            }

            var y = new Number(new char[] { Alphabet[1] }); // := 1
            var z = left;
            var multiplier = new Multiplier(Alphabet);

            while (true)
            {
                bool isEven = n % 2 == 0;
                n = n / 2;

                if (!isEven)
                {
                    y = multiplier.Calculate(z, y);

                    if (n == 0)
                    {
                        return y;
                    }
                }

                z = multiplier.Calculate(z, z);
            }
        }
    }
}
