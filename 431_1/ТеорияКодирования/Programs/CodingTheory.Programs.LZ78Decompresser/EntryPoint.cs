using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.LZ78Decompresser
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            var codes = ReadCodesFromFile(PathSettings.LZ78CompressedTextFileName);
            var compresser = new CompressionLZ78();
            (var decompressedText, var executionTime) = SpeedMeter.Run(codes, compresser.Decompress);
            File.WriteAllText(PathSettings.LZ78DecompressedTextFileName, decompressedText);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.LZ78DecompressedTextFileName)}\"");
        }

        private static List<(int, char)> ReadCodesFromFile(string fileName)
        {
            var input = File.ReadAllText(fileName);

            var splited = input.Split('$');

            return splited.Select(x => (int.Parse(x[0..^1]), x.Last())).ToList();
        }
    }
}
