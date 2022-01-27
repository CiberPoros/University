using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.RLEDecompresser
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            var codes = ReadCodesFromFile(PathSettings.RLECompressedTextFileName);
            var compresser = new CompressionRLE();
            (var decompressedText, var executionTime) = SpeedMeter.Run(codes, compresser.Decompress);
            File.WriteAllText(PathSettings.RLEDecompressedTextFileName, decompressedText);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.RLEDecompressedTextFileName)}\"");
        }

        private static List<(int, string)> ReadCodesFromFile(string fileName)
        {
            var input = File.ReadAllText(fileName);

            var splited = input.Split('$');

            return splited.Select(x => (int.Parse(x.Split('#')[0]), x.Split('#')[1])).ToList();
        }
    }
}
