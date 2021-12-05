using System;
using System.IO;
using System.Text;
using СMIP.Common;

namespace CMIP.Programs.Task4
{
    internal class EntryPoint
    {
        static void Main()
        {
            for (; ; )
            {
                var mode = ReadWorkMode();

                switch (mode)
                {
                    case WorkMode.CODE:
                        Code();
                        break;
                    case WorkMode.DECODE:
                        Decode();
                        break;
                    case WorkMode.EXIT_PROGRAM:
                        return;
                }
            }
        }

        private static void Code()
        {
            var alphabet = File.ReadAllText(PathSettings.Task4_Alphabet).ToLower();
            var text = File.ReadAllText(PathSettings.Task4_OpenText).ToLower();

            var key = ReadDeduction(alphabet.Length);

            var resultBuilder = new StringBuilder();
            foreach (var c in text)
            {
                if (alphabet.Contains(c))
                {
                    var index = alphabet.IndexOf(c);
                    var newIndex = (((key - index) % alphabet.Length) + alphabet.Length) % alphabet.Length;
                    resultBuilder.Append(alphabet[newIndex]);
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            File.WriteAllText(PathSettings.Task4_CodedText, resultBuilder.ToString());
            Console.WriteLine($"Текст зашифрован и записан в файл {Path.GetFileName(PathSettings.Task4_CodedText)}");
            Console.WriteLine();
        }

        private static void Decode()
        {
            var alphabet = File.ReadAllText(PathSettings.Task4_Alphabet).ToLower();
            var codedText = File.ReadAllText(PathSettings.Task4_CodedText);
            var key = ReadDeduction(alphabet.Length);

            var resultBuilder = new StringBuilder();
            foreach (var c in codedText)
            {
                if (alphabet.Contains(c))
                {
                    var index = alphabet.IndexOf(c);
                    var newIndex = (((key - index) % alphabet.Length) + alphabet.Length) % alphabet.Length;
                    resultBuilder.Append(alphabet[newIndex]);
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            File.WriteAllText(PathSettings.Task4_DecodedText, resultBuilder.ToString());
            Console.WriteLine($"Текст расшифрован и записан в файл {Path.GetFileName(PathSettings.Task4_DecodedText)}");
            Console.WriteLine();
        }

        private static int ReadDeduction(int ringSize)
        {
            Console.WriteLine($"Введите ключ (элемент кольца вычетов по модулю {ringSize}):");
            Console.WriteLine();

            for (; ; )
            {
                var input = Console.ReadLine();

                if (!int.TryParse(input, out var result))
                {
                    Console.WriteLine("Ожидалось целое число. Повторите попытку...");
                    Console.WriteLine();
                }

                Console.WriteLine();
                return ((result % ringSize) + ringSize) % ringSize;
            }
        }

        private static WorkMode ReadWorkMode()
        {
            Console.WriteLine($"1 - Зашифровать открытый текст,");
            Console.WriteLine($"2 - Расшифровать шифрограмму,");
            Console.WriteLine($"0 - Завершить работу программы;");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return WorkMode.CODE;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return WorkMode.DECODE;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return WorkMode.EXIT_PROGRAM;
                    default:
                        break;
                }
            }
        }
    }
}
