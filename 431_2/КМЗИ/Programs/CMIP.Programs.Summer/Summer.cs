using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Summer
{
    internal class Summer
    {
        public Summer(List<char> alphabet)
        {
            Alphabet = alphabet ?? throw new ArgumentNullException(nameof(alphabet));
        }

        public List<char> Alphabet { get; set; }

        public char[] Summ(char[] number1, char[] number2)
        {
            if (number1.Any(x => !Alphabet.Contains(x)))
            {
                throw new ArgumentException("Number can contain anly alphabet symbols.", nameof(number1));
            }

            if (number2.Any(x => !Alphabet.Contains(x)))
            {
                throw new ArgumentException("Number can contain anly alphabet symbols.", nameof(number2));
            }

            number1 = number1.Reverse().ToArray();
            number2 = number2.Reverse().ToArray();

            var remains = 0;
            var result = new List<char>();
            for (var index = 0; index < number1.Length || index < number2.Length; index++)
            {
                var current = remains;
                if (index < number1.Length)
                {
                    current += Alphabet.IndexOf(number1[index]);
                }

                if (index < number2.Length)
                {
                    current += Alphabet.IndexOf(number2[index]);
                }

                remains = current / Alphabet.Count;
                result.Add(Alphabet[current % Alphabet.Count]);
            }

            result.Add(Alphabet[remains]);
            result.Reverse();
            return result.SkipWhile(x => x == Alphabet[0]).ToArray();
        }
    }
}
