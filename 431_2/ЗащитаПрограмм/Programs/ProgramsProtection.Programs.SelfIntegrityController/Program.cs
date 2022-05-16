using System;
using System.IO;
using System.Linq;

namespace ProgramsProtection.Programs.SelfIntegrityController
{
    internal class Program
    {
        private static readonly string _infoFileName = "info.txt";

        static void Main()
        {
            if (!File.Exists(_infoFileName))
            {
                var fileName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var programData = File.ReadAllBytes(fileName);
                var hash = GetHash(programData);

                var info = new ProgramInfo()
                {
                    FileName = fileName,
                    Hash = hash
                };

                WriteToFile(info, _infoFileName);

                Console.WriteLine($"Слепок программы создан и сохранен в файл {_infoFileName}");
                Console.WriteLine();
            }
            else
            {
                var info = ReadFromFile(_infoFileName);

                var fileName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var programData = File.ReadAllBytes(fileName);
                var hash = GetHash(programData);

                var wasChanged = false;
                if (fileName != info.FileName)
                {
                    Console.WriteLine($"Программа была переименована: \"{info.FileName}\" -> \"{fileName}\".");
                    wasChanged = true;
                }
                if (hash != info.Hash)
                {
                    Console.WriteLine("Исполняемый файл был изменен (несоответствие hash-кодов).");
                    wasChanged = true;
                }
                if (!wasChanged)
                {
                    Console.WriteLine("Программа не подвергалась изменениям.");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private static void WriteToFile(ProgramInfo programInfo, string fileName)
        {
            File.WriteAllLines(fileName, new string[] { programInfo.FileName, programInfo.Hash.ToString() });
        }

        private static ProgramInfo ReadFromFile(string fileName)
        {
            var input = File.ReadAllLines(fileName);
            return new ProgramInfo() { FileName = input[0], Hash = uint.Parse(input[1]) };
        }

        private static uint GetHash(byte[] source)
        {
            var size = source.Length / sizeof(uint);
            var ints = new uint[size];
            for (var index = 0; index < size; index++)
            {
                ints[index] = BitConverter.ToUInt32(source, index * sizeof(uint));
            }

            var result = ints.FirstOrDefault();

            for (int i = 1; i < ints.Length; i++)
            {
                result ^= ints[i];
            }

            return result;
        }
    }
}
