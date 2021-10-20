using System;
using System.IO;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.FanoCompresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var text = File.ReadAllText(PathSettings.FanoTextFileName);
            var frequencies = Frequencies.CalculateFrequencies(text);
            IOUtils.SaveFrequenciesToFile(frequencies, PathSettings.FanoFrequenciesFileName);

            var compressor = new CompressionByFano(frequencies);
            (var compressedText, var executionTime) = SpeedMeter.Run(text, compressor.Compress);
            IOUtils.SaveCodesAndTextToFile(compressor.Codes, compressedText, PathSettings.FanoCompressedTextFileName);
            Console.WriteLine($"Текст сжат и сохранен в файл \"{Path.GetFileName(PathSettings.FanoCompressedTextFileName)}\"");
            Console.WriteLine();

            Console.WriteLine($"Коеффициент сжатия: {(((compressedText.Length + .0) / 16) / text.Length) * 100}%");
            Console.WriteLine($"Скорость сжатия: {(text.Length + .0) / executionTime.TotalMilliseconds:n4} символов / мсек.");
        }
    }
}
