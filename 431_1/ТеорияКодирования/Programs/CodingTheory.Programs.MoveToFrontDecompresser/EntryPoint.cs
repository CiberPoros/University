using System;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;

namespace CodingTheory.Programs.MoveToFrontDecompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var alphabet = File.ReadAllText(PathSettings.MoveToFrontAlphabetFileName);
            var compressedText = File.ReadAllText(PathSettings.MoveToFrontCompressedTextFileName);
            var code = compressedText.Split(',').Select(x => int.Parse(x)).ToList();

            var compressor = new CompressionMoveToFront();
            var decompressedText = new string(compressor.Decompress(alphabet, code).ToArray());

            File.WriteAllText(PathSettings.MoveToFrontDecompressedTextFileName, decompressedText);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.MoveToFrontDecompressedTextFileName)}\"");
        }
    }
}
