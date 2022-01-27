﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodingTheory.Common;
using Common;

namespace CodingTheory.Programs.RLECompresser
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            var text = new string(File.ReadAllLines(PathSettings.RLETextFileName).SelectMany(x => x).ToArray());
            var compresser = new CompressionRLE();

            (var compressionResult, var executionTime) = SpeedMeter.Run(text, compresser.Compress);
            var resultLength = SaveCodeToFile(compressionResult, PathSettings.RLECompressedTextFileName);
            Console.WriteLine($"Текст сжат и сохранен в файл \"{Path.GetFileName(PathSettings.RLECompressedTextFileName)}\"");
            Console.WriteLine();

            Console.WriteLine($"Коеффициент сжатия: {(((resultLength + .0) / 16) / text.Length) * 100}%");
            Console.WriteLine($"Скорость сжатия: {(text.Length + .0) / executionTime.TotalMilliseconds:n4} символов / мсек.");
        }

        private static int SaveCodeToFile(List<(int, string)> codes, string fileName)
        {
            var result = new StringBuilder();

            foreach (var code in codes)
            {
                result.Append(code.Item1);
                result.Append('#');
                result.Append(code.Item2);
                result.Append('$');
            }

            File.WriteAllText(fileName, result.ToString().Trim('$'));

            return result.Length;
        }
    }
}
