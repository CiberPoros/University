using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.HaffmanCompresser
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Считывание текста с файла...");
            var text = File.ReadAllText(PathSettings.HaffmanTextFileName);
            Console.WriteLine("Текст считан с файла.");
            Console.WriteLine();

            Console.WriteLine("Подсчитаны частоты символов в тексте:");
            var frequencies = Frequencies.CalculateFrequencies(text);
            foreach (var kvp in frequencies)
            {
                Console.WriteLine($"{kvp.Key} {kvp.Value}");
            }
            Console.WriteLine();

            SaveFrequenciesToFile(frequencies);
            Console.WriteLine("Частоты сохранены в файл.");
            Console.WriteLine();

            var compressor = new CompressionByHaffman(frequencies);
            Console.WriteLine("Создана таблица кодов:");
            foreach (var kvp in compressor.Codes)
            {
                Console.WriteLine($"{kvp.Key} {kvp.Value}");
            }
            Console.WriteLine();

            SaveCodesToFile(compressor.Codes);
            Console.WriteLine("Таблица кодов сохранена в файл.");
            Console.WriteLine();

            (var compressedText, var executionTime) = SpeedMeter.Run(text, compressor.Compress);
            File.WriteAllText(PathSettings.HaffmanCompressedTextFileName, compressedText);
            Console.WriteLine("Текст сжат и сохранен в файл.");
            Console.WriteLine("Результат сжатия:");
            Console.WriteLine(compressedText);
            Console.WriteLine();

            Console.WriteLine($"Коеффициент сжатия: {(((compressedText.Length + .0) / 16) / text.Length) * 100}%");
            Console.WriteLine();

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

            File.WriteAllText(PathSettings.HaffmanFrequenciesFileName, sb.ToString());
        }

        private static void SaveCodesToFile(Dictionary<char, string> codes)
        {
            var sb = new StringBuilder();

            foreach (var kvp in codes)
            {
                sb.Append(kvp.Key);
                sb.Append(' ');
                sb.Append(kvp.Value);
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(PathSettings.HaffmanEncodingMapFileName, sb.ToString());
        }
    }
}
