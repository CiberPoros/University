using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTheory.Common
{
    public class CompressionRLE
    {
        public List<(int, string)> Compress(string text)
        {
            var result = new List<(int, string)>();

            for (int i = 0; i < text.Length;)
            {
                var startIndex = i;
                var endIndex = startIndex + 1;

                if (endIndex >= text.Length)
                {
                    result.Add((1, text[i].ToString()));
                    break;
                }

                for (; endIndex < text.Length; endIndex++)
                {
                    if (text[endIndex] != text[startIndex])
                    {
                        break;
                    }
                }

                if (endIndex - startIndex > 1)
                {
                    result.Add((endIndex - startIndex, text[startIndex].ToString()));
                    i = endIndex == startIndex ? endIndex + 1 : endIndex;
                    continue;
                }

                var j = endIndex;
                for (; j < text.Length; j++)
                {
                    if (text[j] == text[j - 1])
                    {
                        break;
                    }
                }

                result.Add((-(j - startIndex), text[startIndex..j]));
                i = j + 1;
            }

            return result;
        }

        public string Decompress(List<(int, string)> codes)
        {
            var result = new StringBuilder();

            foreach (var code in codes)
            {
                if (code.Item1 > 0)
                {
                    result.Append(code.Item2.First(), code.Item1);
                }
                else
                {
                    result.Append(code.Item2);
                }
            }

            return result.ToString();
        }
    }
}
