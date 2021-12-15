using System;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.LZ77Compresser
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var text = new string(File.ReadAllLines(PathSettings.LZ77TextFileName).SelectMany(x => x).ToArray());
            var compresser = new CompressionLZ77();

            (var compressionResult, var executionTime) = SpeedMeter.Run(text, compresser.Compress);
            var resultLength = IOUtils.SaveLZ77ResultToFile(compressionResult, PathSettings.LZ77CompressedTextFileName);
            Console.WriteLine($"Текст сжат и сохранен в файл \"{Path.GetFileName(PathSettings.LZ77CompressedTextFileName)}\"");
            Console.WriteLine();

            Console.WriteLine($"Коеффициент сжатия: {(((resultLength + .0) / 16) / text.Length) * 100}%");
            Console.WriteLine($"Скорость сжатия: {(text.Length + .0) / executionTime.TotalMilliseconds:n4} символов / мсек.");
        }
    }
}
