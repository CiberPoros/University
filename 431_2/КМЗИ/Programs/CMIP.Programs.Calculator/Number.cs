﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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

            Value = value.ToArray();
            IsPositive = isPositive;
        }

        public BigInteger ToBigInteger(List<char> alphabet)
        {
            var baseSystemNumber = alphabet.Count;
            BigInteger currentDegree = 1;

            BigInteger result = 0;
            for (int i = Length - 1; i >= 0; i--)
            {
                result += alphabet.IndexOf(this[i]) * currentDegree;
                currentDegree *= baseSystemNumber;
            }

            return IsPositive ? result : -result;
        }

        public int ToInt32(List<char> alphabet)
        {
            var baseSystemNumber = alphabet.Count;
            int currentDegree = 1;

            int result = 0;
            for (int i = Length - 1; i >= 0; i--)
            {
                result += alphabet.IndexOf(this[i]) * currentDegree;
                currentDegree *= baseSystemNumber;
            }

            return IsPositive ? result : -result;
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

        public bool IsNegative => !IsPositive;

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

        public override string ToString() => IsNegative ? "-" + string.Join("", Value) : string.Join("", Value);
    }
}
