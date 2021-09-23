using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.HaffmanDecompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            (var compressedText, var codes) = ReadTextAndCodes();

            var compressor = new CompressionByHaffman
            {
                CodesInverted = codes
            };
            var decompressedText = compressor.Decompress(compressedText);
            File.WriteAllText(PathSettings.HaffmanDecompressedTextFileName, decompressedText, Encoding.Unicode);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.HaffmanDecompressedTextFileName)}\"");
        }

        private static (string compressedText, Dictionary<string, char> codes) ReadTextAndCodes()
        {
            var input = File.ReadAllBytes(PathSettings.HaffmanCompressedTextFileName);
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
