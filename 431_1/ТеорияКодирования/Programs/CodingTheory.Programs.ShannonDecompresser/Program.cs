﻿using System;
using System.IO;
using System.Text;
using CodingTheory.Common;

namespace CodingTheory.Programs.ShannonDecompresser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            (var compressedText, var codes) = IOUtils.ReadTextAndCodes(PathSettings.ShannonCompressedTextFileName);

            var compressor = new CompressionByShannon
            {
                CodesInverted = codes
            };
            var decompressedText = compressor.Decompress(compressedText);
            File.WriteAllText(PathSettings.ShannonDecompressedTextFileName, decompressedText, Encoding.Unicode);
            Console.WriteLine($"Текст разархивирован и результат сохранен в файл \"{Path.GetFileName(PathSettings.ShannonDecompressedTextFileName)}\"");
        }
    }
}
