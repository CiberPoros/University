using NUnit.Framework;
using System.Collections.Generic;
using CMIP.Programs.Calculator;
using System;
using System.Linq;

namespace CMIP.CalculatorTests
{
    public class DivisionTests
    {
        private static readonly List<char> _alphabet = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var calculationsHandler = new CalculationsHandler(_alphabet);
            var rnd = new Random();

            for (int i = 0; i < 1000; i++)
            {
                var leftSize = rnd.Next(5, 30);
                var rightSize = rnd.Next(1, leftSize);

                var left = new char[leftSize];
                for (int j = 0; j < left.Length; j++)
                {
                    left[j] = _alphabet[rnd.Next(_alphabet.Count)];
                }

                var right = new char[rightSize];
                while (right.All(x => x == 0))
                {
                    for (int j = 0; j < right.Length; j++)
                    {
                        right[j] = _alphabet[rnd.Next(j == 0 ? 1 : 0, _alphabet.Count)];
                    }
                } 

                var leftNumber = new Number(left);
                var rightNumber = new Number(right);
                var actualResult = calculationsHandler.Divide(leftNumber, rightNumber);

                var expectedResult = Program.CalculateSimple(leftNumber, rightNumber, _alphabet, Programs.Calculator.Operations.OperationType.DIVIDE);

                Assert.AreEqual(expectedResult.ToString(), actualResult.ToString().Split(';').First());
            }
        }
    }
}