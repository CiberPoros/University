using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;

namespace CodingTheory.Programs.HaffmanDecompresser
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Считывание архивированного текста с файла...");
            var compressedText = File.ReadAllText(PathSettings.HaffmanCompressedTextFileName);
            Console.WriteLine("Архивированный текст считан с файла.");
            Console.WriteLine();

            Console.WriteLine("Считывание таблицы кодов символов...");
            var codes = ReadCodesFromFile();
            Console.WriteLine("Таблица кодов символов считана.");
            Console.WriteLine();

            var compressor = new CompressionByHaffman
            {
                CodesInverted = codes
            };
            var decompressedText = compressor.Decompress(compressedText);
            File.WriteAllText(PathSettings.HaffmanDecompressedTextFileName, decompressedText);
            Console.WriteLine("Текст разархивирован и результат сохранен в файл.");
            Console.WriteLine("Результат разархивирования: ");
            Console.WriteLine(decompressedText);
        }

        private static Dictionary<string, char> ReadCodesFromFile()
        {
            var result = new Dictionary<string, char>();

            var input = File.ReadAllLines(PathSettings.HaffmanEncodingMapFileName)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .ToArray();

            foreach (var s in input)
            {
                var splited = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                result.Add(splited[1], splited[0].First());
            }

            return result;
        }
    }
}
