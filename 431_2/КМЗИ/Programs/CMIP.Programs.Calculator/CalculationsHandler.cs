using System;
using System.Collections.Generic;
using CMIP.Programs.Calculator.Operations;

namespace CMIP.Programs.Calculator
{
    internal class CalculationsHandler
    {
        public CalculationsHandler(List<char> alphabet)
        {
            Alphabet = alphabet ?? throw new ArgumentNullException(nameof(alphabet));
            NumbersUtils = new NumbersUtils(alphabet);
        }

        public List<char> Alphabet { get; set; }

        public NumbersUtils NumbersUtils { get; set; }

        public Number Summ(Number left, Number right)
        {
            if (left.IsPositive && right.IsPositive || left.IsNegative && right.IsNegative)
            {
                var summer = new Summer(Alphabet);

                var result = summer.Calculate(left, right);

                if (left.IsNegative && right.IsNegative)
                {
                    result.IsPositive = false;
                }

                return result;
            }

            var substractor = new Substractor(Alphabet);

            return left.IsPositive ? substractor.Calculate(left, right) : substractor.Calculate(right, left);
        }

        public Number Substract(Number left, Number right)
        {
            if (left.IsPositive && right.IsPositive)
            {
                var substractor = new Substractor(Alphabet);

                if (NumbersUtils.Compare(left, right) >= 0)
                {
                    return substractor.Calculate(left, right);
                }
                else
                {
                    var result = substractor.Calculate(right, left);
                    result.IsPositive = false;
                    return result;
                }
            }
            else if (left.IsPositive && right.IsNegative)
            {
                var summer = new Summer(Alphabet);

                return summer.Calculate(left, right);
            }
            else if (left.IsNegative && right.IsPositive)
            {
                var summer = new Summer(Alphabet);

                var result = summer.Calculate(left, right);
                result.IsPositive = false;
                return result;
            }
            else
            {
                var substractor = new Substractor(Alphabet);

                if (NumbersUtils.Compare(right, left) >= 0)
                {
                    return substractor.Calculate(right, left);
                }
                else
                {
                    var result = substractor.Calculate(left, right);
                    result.IsPositive = false;
                    return result;
                }
            }
        }

        public Number Multiply(Number left, Number right)
        {
            var multiplier = new Multiplier(Alphabet);
            var resultIsPositive = left.IsPositive && right.IsPositive || left.IsNegative && right.IsNegative;

            left.IsPositive = true;
            right.IsPositive = true;

            var result = multiplier.Calculate(left, right);
            result.IsPositive = resultIsPositive;

            return result;
        }

        public Number Divide(Number left, Number right)
        {
            var divider = new Divider(Alphabet);
            var resultIsPositive = left.IsPositive && right.IsPositive || left.IsNegative && right.IsNegative;

            left.IsPositive = true;
            right.IsPositive = true;

            var result = divider.Calculate(left, right);
            result.IsPositive = resultIsPositive;

            return result;
        }

        public Number Pow(Number left, Number right)
        {
            var exponenciator = new Exponenciator(Alphabet);
            var resultIsPositive = left.IsPositive;

            left.IsPositive = true;
            right.IsPositive = true;

            var result = exponenciator.Calculate(left, right);
            result.IsPositive = resultIsPositive;

            return result;
        }
    }
}
