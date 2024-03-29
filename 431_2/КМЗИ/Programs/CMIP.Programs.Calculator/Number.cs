﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CMIP.Programs.Calculator
{
    public class Number : IEnumerable<char>
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

        public static Number FromBigInteger(BigInteger value)
        {
            return new Number(value.ToString(), true);
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

        public Number RemainsByDivision { get; set; }

        public bool IsUndefined { get; set; }

        public char[] Value { get; set; }

        public bool IsPositive { get; set; }

        public bool IsZero => !Value.Any() || Value.Length == 1 && Value[0] == '0';

        public bool IsSingleOne => Value.Length == 1 && Value[0] == '1';

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

        public override string ToString()
        {
            if (IsUndefined)
            {
                return "Undefined";
            }

            if (RemainsByDivision is null)
                return IsNegative ? "-" + string.Join("", Value) : string.Join("", Value);
            else
                return IsNegative ? "-" + string.Join("", Value) : string.Join("", Value) + "; Остаток от деления: " + string.Join("", RemainsByDivision.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Number right)
            {
                return false;
            }

            return Value.SequenceEqual(right.Value);
        }

        public override int GetHashCode()
        {
            var result = 0;
            foreach (var current in Value)
            {
                result ^= current;
            }

            return result;
        }
    }
}
