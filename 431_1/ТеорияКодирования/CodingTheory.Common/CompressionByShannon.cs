using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTheory.Common
{
    public class CompressionByShannon : CompressionAbstract
    {
        public CompressionByShannon()
        {

        }

        public CompressionByShannon(Dictionary<char, double> frequencies)
        {
            _frequencies = frequencies;

            MakeCodes();
        }

        protected override void MakeCodes()
        {
            Codes = new Dictionary<char, string>();
            CodesInverted = new Dictionary<string, char>();

            var frequencies = _frequencies.Select(x => (x.Key, x.Value)).OrderByDescending(x => x.Value).ToList();
            var frequenciesSumm = 0D;
            foreach (var pair in frequencies)
            {
                var l = CalculateCeiling(pair.Value);
                var bitsCode = new StringBuilder();
                var currentFrequency = frequenciesSumm;

                for (int i = 0; i < l; i++)
                {
                    currentFrequency *= 2;

                    bitsCode.Append(currentFrequency >= 1 ? '1' : '0');

                    if (currentFrequency >= 1)
                    {
                        currentFrequency -= 1; 
                    }
                }

                var resultBits = bitsCode.ToString();
                Codes.Add(pair.Key, resultBits);
                CodesInverted.Add(resultBits, pair.Key);

                frequenciesSumm += pair.Value;
            }
        }

        private static int CalculateCeiling(double frequency)
        {
            var result = 1;
            var current = 1D;
            for (; ; )
            {
                current /= 2;

                if (frequency >= current)
                {
                    return result;
                }

                result++;
            }
        }
    }
}
