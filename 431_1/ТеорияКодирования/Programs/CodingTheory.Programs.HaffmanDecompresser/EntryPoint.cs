using System;
using System.IO;
using System.Text;
using CodingTheory.Common;

namespace CodingTheory.Programs.HaffmanDecompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            (var compressedText, var codes) = IOUtils.ReadTextAndCodes(PathSettings.HaffmanCompressedTextFileName);

            var compressor = new CompressionByHaffman
            {
                CodesInverted = codes
            };
            var decompressedText = compressor.Decompress(compressedText);
            File.WriteAllText(PathSettings.HaffmanDecompressedTextFileName, decompressedText, Encoding.Unicode);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.HaffmanDecompressedTextFileName)}\"");
        }
    }
}
