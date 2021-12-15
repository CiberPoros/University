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

        public static int SaveLZ77ResultToFile(IEnumerable<(int offset, int length, char nextSymbol)> lz77Result, string fileName)
        {
            var result = new List<string>();

            foreach (var currentRes in lz77Result)
            {
                var sb = new StringBuilder();
                sb.Append(currentRes.offset);
                sb.Append(' ');
                sb.Append(currentRes.length);
                sb.Append(' ');
                sb.Append(currentRes.nextSymbol);
                result.Add(sb.ToString());
            }

            File.WriteAllLines(fileName, result);

            return result.Sum(x => x.Length) + result.Count - 1;
        }

        public static IEnumerable<(int offset, int length, char nextSymbol)> ReadLZ77ResultFromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName).Where(x => !(string.IsNullOrWhiteSpace(x))).ToList();

            foreach (var line in lines)
            {
                var startIndex = 0;
                var index = 0;
                while (line[index] != ' ')
                {
                    index++;
                }

                var offset = int.Parse(line.Substring(startIndex, index - startIndex));

                startIndex = index + 1;
                index++;
                while (line[index] != ' ')
                {
                    index++;
                }

                var length = int.Parse(line.Substring(startIndex, index - startIndex));
                var nextSymbol = line.Last();

                yield return (offset, length, nextSymbol);
            }
        }
    }
}
