using System;
using System.IO;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.LZ77Decompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var codes = IOUtils.ReadLZ77ResultFromFile(PathSettings.LZ77CompressedTextFileName);
            var compresser = new CompressionLZ77();
            (var decompressedText, var executionTime) = SpeedMeter.Run(codes, compresser.Decompress);
            File.WriteAllText(PathSettings.LZ77DecompressedTextFileName, decompressedText);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.LZ77DecompressedTextFileName)}\"");
        }
    }
}
