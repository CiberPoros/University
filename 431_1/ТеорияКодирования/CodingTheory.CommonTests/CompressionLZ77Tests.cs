using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace CodingTheory.Common.Tests
{
    [TestClass()]
    public class CompressionLZ77Tests
    {
        [TestMethod()]
        public void CompressTest()
        {
            var compressor = new CompressionLZ77();

            var etalon = new List<(int, int, char)>
            {
                (0, 0, 'a'),
                (0, 0, 'b'),
                (2, 1, 'c'),
                (4, 7, 'd'),
                (10, 3, ' '),
            };
            var result = compressor.Compress("abacabacabadaca");

            Debug.WriteLine(string.Join(Environment.NewLine, result));

            Assert.IsTrue(etalon.SequenceEqual(result));
        }

        [TestMethod()]
        public void DecompressTest()
        {
            var compressor = new CompressionLZ77();

            var etalon = "abacabacabadaca ";
            var input = new List<(int, int, char)>
            {
                (0, 0, 'a'),
                (0, 0, 'b'),
                (2, 1, 'c'),
                (4, 7, 'd'),
                (10, 3, ' '),
            };
            var result = compressor.Decompress(input);

            Debug.WriteLine(result);

            Assert.AreEqual(etalon, result);
        }
    }
}