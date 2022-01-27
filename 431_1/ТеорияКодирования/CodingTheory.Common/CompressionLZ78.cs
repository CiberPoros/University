using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodingTheory.Common
{
    public class CompressionLZ78
    {
        public List<(int, char)> Compress(string text)
        {
            var dictionary = new Dictionary<string, int>();
            var number = 1;
            var result = new List<(int, char)>();

            for (int i = 0; i < text.Length;)
            {
                var current = string.Empty;

                bool isBroken = false;
                int j = i;
                for (; j < text.Length; j++)
                {
                    if (!dictionary.ContainsKey(current + text[j]))
                    {
                        dictionary.Add(current + text[j], number);
                        result.Add((current == string.Empty ? 0 : dictionary[current], text[j]));
                        number++;
                        isBroken = true;
                        break;
                    }
                    current += text[j];
                }
                i = j + 1;

                if (!isBroken)
                {
                    result.Add((dictionary[current], ' '));
                }
            }

            return result;
        }

        public string Decompress(List<(int, char)> codes)
        {
            var result = new StringBuilder();
            var dictionary = new Dictionary<string, int>();
            var number = 1;

            foreach (var code in codes)
            {
                if (code.Item1 == 0)
                {
                    result.Append(code.Item2);
                    dictionary.Add(code.Item2.ToString(), number);
                }
                else
                {
                    result.Append(dictionary.Where(kvp => kvp.Value == code.Item1).First().Key + code.Item2);
                    dictionary.Add(dictionary.Where(kvp => kvp.Value == code.Item1).First().Key + code.Item2, number);
                }

                number++;
            }

            return result.ToString();
        }
    }
}
