using System;
using System.IO;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.HaffmanCompresser
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            var text = ReadText();

            var frequencies = Frequencies.CalculateFrequencies(text);

            var compressor = new CompressionByHaffman(frequencies);

            var code = compressor.Compress(text);

            Console.WriteLine("Результат сжатия (битовая строка): ");
            Console.WriteLine(code);
            Console.WriteLine();

            var decode = compressor.DeCompress(code);

            Console.WriteLine("Результат разархивирования: ");
            Console.WriteLine(decode);
            Console.WriteLine();

            Console.WriteLine("Изменения!");
        }

        private static string ReadText()
        {
            Console.WriteLine("Введите название файла с текстом:");
            var path = Console.ReadLine();
            Console.WriteLine();

            return File.ReadAllText(path);
        }
    }
}
