using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator
{
    internal class Number : IEnumerable<char>
    {
        public Number(IEnumerable<char> value, bool isPositive)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = Value.ToArray();
            IsPositive = isPositive;
        }

        public Number(IEnumerable<char> value) : this(value, true)
        {

        }

        public char this[int index]
        {
            get => Value[index];
            set => Value[index] = value;
        }

        public char[] Value { get; set; }

        public bool IsPositive { get; set; }

        public int Length => Value.Length;

        public IEnumerator<char> GetEnumerator()
        {
            foreach (var val in Value)
            {
                yield return val;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var val in Value)
            {
                yield return val;
            }
        }
    }
}
