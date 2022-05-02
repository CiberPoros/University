using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator
{
    public class NumbersUtils
    {
        public NumbersUtils(List<char> alphabet)
        {
            Alphabet = alphabet ?? throw new ArgumentNullException(nameof(alphabet));
        }

        public List<char> Alphabet { get; set; }

        public int Compare(Number left, Number right)
        {
            left = Normalize(left);
            right = Normalize(right);

            if (left.Length < right.Length)
            {
                return -1;
            }
            else if (right.Length < left.Length)
            {
                return 1;
            }
            else
            { 
                for (int i = 0; i < left.Length; i++)
                {
                    var leftIndex = Alphabet.IndexOf(left[i]);
                    var rigthIndex = Alphabet.IndexOf(right[i]);
                    
                    if (leftIndex.CompareTo(rigthIndex) != 0)
                    {
                        return leftIndex.CompareTo(rigthIndex);
                    }
                }
            }

            return 0;
        }

        public Number Normalize(Number number)
        {
            number = new Number(number.SkipWhile(x => x == Alphabet[0]).ToArray());

            return number.Any() ? number : new Number(new char[] { Alphabet[0] });
        }
    }
}
