using System.Collections.Generic;
using System.Text;

namespace CodingTheory.Common
{
    public abstract class CompressionAbstract
    {
        protected Dictionary<char, double> _frequencies;

        public Dictionary<char, string> Codes { get; set; }
        public Dictionary<string, char> CodesInverted { get; set; }

        public CompressionAbstract()
        {

        }

        public CompressionAbstract(Dictionary<char, double> frequencies)
        {
            _frequencies = frequencies;

            MakeCodes();
        }

        public virtual string Compress(string text)
        {
            var sb = new StringBuilder();

            foreach (var c in text)
            {
                sb.Append(Codes[c]);
            }

            return sb.ToString();
        }

        public virtual string Decompress(string compressedText)
        {
            var sb = new StringBuilder();
            var currentString = string.Empty;

            for (int i = 0; i < compressedText.Length; i++)
            {
                currentString += compressedText[i];

                if (CodesInverted.TryGetValue(currentString, out var value))
                {
                    sb.Append(value);
                    currentString = string.Empty;
                }
            }

            return sb.ToString();
        }

        protected abstract void MakeCodes();
    }
}