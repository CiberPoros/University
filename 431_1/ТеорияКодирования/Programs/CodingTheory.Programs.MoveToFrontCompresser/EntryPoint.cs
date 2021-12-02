using System;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.MoveToFrontCompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var text = File.ReadAllText(PathSettings.MoveToFrontTextFileName).ToLower();
            var alphabet = new string(text.ToLower().Distinct().ToArray());
            File.WriteAllText(PathSettings.MoveToFrontAlphabetFileName, alphabet);

            var compressor = new CompressionMoveToFront();
            (var compressedText, var executionTime) = SpeedMeter.Run(text, alphabet, compressor.Compress);
            var compressedTextString = string.Join(",", compressedText);

            File.WriteAllText(PathSettings.MoveToFrontCompressedTextFileName, compressedTextString);
            Console.WriteLine($"Текст сжат и сохранен в файл \"{Path.GetFileName(PathSettings.MoveToFrontCompressedTextFileName)}\"");
            Console.WriteLine();

            Console.WriteLine($"Коеффициент сжатия: {(((compressedTextString.Length + .0) / 16) / text.Length) * 100}%");
            Console.WriteLine($"Скорость сжатия: {(text.Length + .0) / executionTime.TotalMilliseconds:n4} символов / мсек.");
        }
    }
}
