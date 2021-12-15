using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTheory.Common
{
    public class CompressionLZ77
    {
        public IEnumerable<(int offset, int length, char nextSymbol)> Compress(string text)
        {
            for (int i = 0; i < text.Length; )
            {
                var currentString = text[i].ToString();
                var offset = 0;
                var length = 0;

                for (; ; )
                {
                    var subText = text.Substring(0, Math.Min(text.Length, i + length));
                    var index = subText.IndexOf(currentString);

                    if (index == -1)
                    {
                        break;
                    }

                    offset = i - index;
                    length++;

                    if (i + length >= text.Length)
                    {
                        currentString += ' ';
                        break;
                    }

                    currentString += text[i + length];
                }

                yield return (offset, length, currentString.Last());
                i += length + 1;
            }
        }

        public string Decompress(IEnumerable<(int offset, int length, char nextSymbol)> codes)
        {
            var resultBuilder = new StringBuilder();

            foreach (var code in codes)
            {
                if (code.offset != 0)
                {
                    for (int i = resultBuilder.Length - code.offset, j = 0; j < code.length; i++, j++)
                    {
                        resultBuilder.Append(resultBuilder[i]);
                    }
                }

                resultBuilder.Append(code.nextSymbol);
            }

            return resultBuilder.ToString();
        }
    }
}
