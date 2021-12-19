using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.PolinomMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Common.Tests
{
    [TestClass()]
    public class PolinomTests
    {
        [TestMethod()]
        public void Subtraction_IsEmptyResult()
        {
            var left = new PolinomDouble(new double[] { 5, 5, 5 });
            var right = new PolinomDouble(new double[] { 5, 5, 5 });

            var result = left - right;

            Debug.WriteLine(result);

            Assert.IsTrue(result.IsEmpty);
        }

        [TestMethod()]
        public void Subtraction_LeftDegreeGreater()
        {
            var left = new PolinomDouble(new double[] { 5, 5, 5, 5 });
            var right = new PolinomDouble(new double[] { 4, 4, 4 });

            var expected = new PolinomDouble(new double[] { 1, 1, 1, 5 });
            var result = left - right;

            Debug.WriteLine(result);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void Subtraction_LeftDegreeLess()
        {
            var left = new PolinomDouble(new double[] { 5, 5, 5 });
            var right = new PolinomDouble(new double[] { 4, 4, 4, 4 });

            var expected = new PolinomDouble(new double[] { 1, 1, 1, -4 });
            var result = left - right;

            Debug.WriteLine(result);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void MultiplicationConst_Positive()
        {
            var polinom = new PolinomDouble(new double[] { 5, 5, 5 });

            var expected = new PolinomDouble(new double[] { 10, 10, 10 });
            var result = polinom * 2;

            Debug.WriteLine(result);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void MultiplicationPolinom_Positive()
        {
            var left = new PolinomDouble(new double[] { 2, 3, 4 });
            var right = new PolinomDouble(new double[] { -1, 3, 6 });

            var expected = new PolinomDouble(new double[] { -2, 3, 17, 30, 24 });
            var result = left * right;

            Debug.WriteLine(result);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void DivisionPolinom_WikiExample()
        {
            var polinom = new PolinomDouble(new double[] { -42, 0, -12, 1 });
            var divider = new PolinomDouble(new double[] { -3, 1 });

            var expected = new PolinomDouble(new double[] { -27, -9, 1 });
            var result = polinom / divider;

            Debug.WriteLine(result);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void DivisionPolinomWithRemainder_Positive()
        {
            var polinom = new PolinomDouble(new double[] { -1, 1, 0, -2, 3, 1 });
            var divider = new PolinomDouble(new double[] { -3, 7, 2 });

            var expectedResult = new PolinomDouble(new double[] { -41d / 16d, 5d / 8d, -1d / 4d, 1d / 2d});
            var expectedRemainder = new PolinomDouble(new double[] { -139d / 16d, 333d / 16d });
            (var result, var remainder) = polinom.Divide(divider);

            Debug.WriteLine(result);
            Debug.WriteLine(remainder);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedRemainder, remainder);
        }
    }
}