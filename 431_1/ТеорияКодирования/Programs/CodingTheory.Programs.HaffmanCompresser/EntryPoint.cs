using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.HaffmanCompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var text = File.ReadAllText(PathSettings.HaffmanTextFileName);
            var frequencies = Frequencies.CalculateFrequencies(text);
            SaveFrequenciesToFile(frequencies);

            var compressor = new CompressionByHaffman(frequencies);
            (var compressedText, var executionTime) = SpeedMeter.Run(text, compressor.Compress);
            SaveCodesAndTextToFile(compressor.Codes, compressedText);
            Console.WriteLine($"Текст сжат и сохранен в файл \"{Path.GetFileName(PathSettings.HaffmanCompressedTextFileName)}\"");
            Console.WriteLine();

            Console.WriteLine($"Коеффициент сжатия: {(((compressedText.Length + .0) / 16) / text.Length) * 100}%");
            Console.WriteLine($"Скорость сжатия: {(text.Length + .0) / executionTime.TotalMilliseconds:n4} символов / мсек.");
        }

        private static void SaveFrequenciesToFile(Dictionary<char, double> frequencies)
        {
            var sb = new StringBuilder();

            foreach (var kvp in frequencies)
            {
                sb.Append(kvp.Key);
                sb.Append(' ');
                sb.Append(kvp.Value);
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(PathSettings.HaffmanFrequenciesFileName, sb.ToString(), Encoding.Unicode);
        }

        private static void SaveCodesAndTextToFile(Dictionary<char, string> codes, string text)
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

            File.WriteAllBytes(PathSettings.HaffmanCompressedTextFileName, result);
        }
    }
}
