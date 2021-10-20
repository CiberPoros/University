using System;
using System.IO;
using System.Text;
using CodingTheory.Common;

namespace CodingTheory.Programs.FanoDecompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            (var compressedText, var codes) = IOUtils.ReadTextAndCodes(PathSettings.FanoCompressedTextFileName);

            var compressor = new CompressionByFano
            {
                CodesInverted = codes
            };
            var decompressedText = compressor.Decompress(compressedText);
            File.WriteAllText(PathSettings.FanoDecompressedTextFileName, decompressedText, Encoding.Unicode);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.FanoDecompressedTextFileName)}\"");
        }
    }
}
