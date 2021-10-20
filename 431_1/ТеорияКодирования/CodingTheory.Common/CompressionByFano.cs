using System.Collections.Generic;
using System.Linq;

namespace CodingTheory.Common
{
    public class CompressionByFano : CompressionAbstract
    {
        public CompressionByFano()
        {

        }

        public CompressionByFano(Dictionary<char, double> frequencies)
        {
            _frequencies = frequencies;

            MakeCodes();
        }

        protected override void MakeCodes()
        {
            Codes = new Dictionary<char, string>();
            CodesInverted = new Dictionary<string, char>();

            var frequencies = new List<(char, double)>();
            foreach (var kvp in _frequencies)
            {
                frequencies.Add((kvp.Key, kvp.Value));
            }

            frequencies.OrderByDescending(x => x.Item2).ToList();
            frequencies = frequencies.Where((_, index) => index % 2 == 0).Concat(frequencies.Where((_, index) => index % 2 == 1)).ToList();

            MakeCodesRecursion(frequencies, 0, frequencies.Count - 1, string.Empty);
        }

        private void MakeCodesRecursion(List<(char key, double value)> frequencies, int left, int right, string code)
        {
            if (left == right)
            {
                Codes.Add(frequencies[left].key, code);
                CodesInverted.Add(code, frequencies[left].key);
                return;
            }

            var length = right - left + 1;

            var l1 = left;
            var r1 = left + length / 2 - 1;
            var l2 = r1 + 1;
            var r2 = right;

            MakeCodesRecursion(frequencies, l1, r1, code + "1");
            MakeCodesRecursion(frequencies, l2, r2, code + "0");
        }
    }
}
