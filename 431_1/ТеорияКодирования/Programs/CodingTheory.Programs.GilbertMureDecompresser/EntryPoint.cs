using System;
using System.IO;
using System.Text;
using CodingTheory.Common;

namespace CodingTheory.Programs.GilbertMureDecompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            (var compressedText, var codes) = IOUtils.ReadTextAndCodes(PathSettings.GilbertMureCompressedTextFileName);

            var compressor = new CompressionByGilbertMure
            {
                CodesInverted = codes
            };
            var decompressedText = compressor.Decompress(compressedText);
            File.WriteAllText(PathSettings.GilbertMureDecompressedTextFileName, decompressedText, Encoding.Unicode);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.GilbertMureDecompressedTextFileName)}\"");
        }
    }
}
