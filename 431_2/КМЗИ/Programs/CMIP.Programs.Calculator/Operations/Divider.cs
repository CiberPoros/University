using System;
using System.Collections.Generic;

namespace CMIP.Programs.Calculator.Operations
{
    internal class Divider : AbstractOperator
    {
        public Divider(List<char> alphabet) : base(alphabet)
        {
        }

        protected override bool NeedInversePrev => false;

        protected override bool NeedInverseAfter => false;

        protected override Number CalculateInternal(Number left, Number right)
        {
            throw new NotImplementedException();
        }
    }
}
