using System;
using System.IO;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.ShannonCompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var text = File.ReadAllText(PathSettings.ShannonTextFileName);
            var frequencies = Frequencies.CalculateFrequencies(text);
            IOUtils.SaveFrequenciesToFile(frequencies, PathSettings.ShannonFrequenciesFileName);

            var compressor = new CompressionByShannon(frequencies);
            (var compressedText, var executionTime) = SpeedMeter.Run(text, compressor.Compress);
            IOUtils.SaveCodesAndTextToFile(compressor.Codes, compressedText, PathSettings.ShannonCompressedTextFileName);
            Console.WriteLine($"Текст сжат и сохранен в файл \"{Path.GetFileName(PathSettings.ShannonCompressedTextFileName)}\"");
            Console.WriteLine();

            Console.WriteLine($"Коеффициент сжатия: {(((compressedText.Length + .0) / 16) / text.Length) * 100}%");
            Console.WriteLine($"Скорость сжатия: {(text.Length + .0) / executionTime.TotalMilliseconds:n4} символов / мсек.");
        }
    }
}
