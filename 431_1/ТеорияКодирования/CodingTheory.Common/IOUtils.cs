using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace CodingTheory.Common
{
    public static class IOUtils
    {
        public static void SaveFrequenciesToFile(Dictionary<char, double> frequencies, string fileName)
        {
            var sb = new StringBuilder();

            foreach (var kvp in frequencies)
            {
                sb.Append(kvp.Key);
                sb.Append(' ');
                sb.Append(kvp.Value);
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(fileName, sb.ToString(), Encoding.Unicode);
        }

        public static void SaveCodesAndTextToFile(Dictionary<char, string> codes, string text, string fileName)
        {
            var sb = new StringBuilder();

            foreach (var kvp in codes)
            {
                sb.Append(kvp.Key);
                sb.Append(' ');
                sb.Append(kvp.Value);
                sb.Append(Environment.NewLine);
            }

            sb.Append(PathSettings.SeparationString);

            var result = Encoding.Unicode.GetBytes(sb.ToString())
                                      .Concat(Utils.ConvertBitsSequenceToByteArray(text))
                                      .ToArray();

            File.WriteAllBytes(fileName, result);
        }

        public static (string compressedText, Dictionary<string, char> codes) ReadTextAndCodes(string fileName)
        {
            var input = File.ReadAllBytes(fileName);
            var codes = new Dictionary<string, char>();

            var tableAndText = Encoding.Unicode.GetString(input).Split(PathSettings.SeparationString, StringSplitOptions.RemoveEmptyEntries);
            var table = tableAndText[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var s in table)
            {
                codes.Add(s[2..], s[0]);
            }

            var byteArray = input.Skip(tableAndText[0].Length * 2 + PathSettings.SeparationString.Length * 2).ToArray();

            var compressedText = Utils.ConvertByteArrayToBitsSequence(byteArray).Trim();

            return (compressedText, codes);
        }
    }
}
