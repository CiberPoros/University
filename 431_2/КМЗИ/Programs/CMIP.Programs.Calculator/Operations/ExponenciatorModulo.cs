using System.Collections.Generic;

namespace CMIP.Programs.Calculator.Operations
{
    public class ExponenciatorModulo : AbstractOperator
    {
        public ExponenciatorModulo(List<char> alphabet, Number modulo) : base(alphabet)
        {
            Modulo = modulo;
        }

        public Number Modulo { get; }

        protected override bool NeedInversePrev => false;

        protected override bool NeedInverseAfter => false;

        protected override Number CalculateInternal(Number left, Number right)
        {
            if (Modulo.IsZero)
            {
                return new Number(new char[] { '0' }) { IsUndefined = true };
            }

            if (Modulo.IsSingleOne)
            {
                return new Number(new char[] { '0' });
            }

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
            var divider = new Divider(Alphabet);

            while (true)
            {
                bool isEven = n % 2 == 0;
                n /= 2;

                if (!isEven)
                {
                    y = multiplier.Calculate(z, y);
                    y = divider.Calculate(y, Modulo).RemainsByDivision;

                    if (n == 0)
                    {
                        return y;
                    }
                }

                z = multiplier.Calculate(z, z);
                z = divider.Calculate(z, Modulo).RemainsByDivision;
            }
        }
    }
}
