using System;
using System.IO;
using System.Text;
using СMIP.Common;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Task8
{
    class EntryPoint
    {
        private static readonly Random _rnd = new();

        static void Main()
        {
            for (; ; )
            {
                var mode = ReadWorkMode();

                switch (mode)
                {
                    case WorkMode.GENERATE_RUS_TEXT:
                        File.WriteAllText(PathSettings.Task8_Text_Rus, GenerateText(ReadRusAlphabet(), ReadLength()));
                        Console.WriteLine($"Русский текст сгенерирован и сохранен в файл {Path.GetFileName(PathSettings.Task8_Text_Rus)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.GENERATE_ENG_TEXT:
                        File.WriteAllText(PathSettings.Task8_Text_Eng, GenerateText(ReadEngAlphabet(), ReadLength()));
                        Console.WriteLine($"Английский текст сгенерирован и сохранен в файл {Path.GetFileName(PathSettings.Task8_Text_Eng)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.CALC_INDEX_FOR_RUS_TEXT:
                        Console.WriteLine($"Индекс для русского текста подсчитан и равен: {CalcIndex(ReadRusText())}");
                        Console.WriteLine();
                        break;
                    case WorkMode.CALC_INDEX_FOR_ENG_TEXT:
                        Console.WriteLine($"Индекс для английского текста подсчитан и равен: {CalcIndex(ReadEngText())}");
                        Console.WriteLine();
                        break;
                    case WorkMode.CLOSE_PROGRAM:
                        return;
                }
            }
        }

        private static double CalcIndex(string text)
        {
            var occurrencesCount = new Dictionary<char, int>();

            foreach (var c in text)
            {
                if (occurrencesCount.ContainsKey(c))
                {
                    occurrencesCount[c]++;
                    continue;
                }

                occurrencesCount.Add(c, 1);
            }

            return occurrencesCount.Sum(x => ((x.Value + .0) / text.Length) * ((x.Value + .0) / text.Length));
        }

        private static int ReadLength()
        {
            Console.WriteLine("Введите одно целое положительное число - длину генерируемого текста:");
            Console.WriteLine();

            for (; ; )
            {
                if (!int.TryParse(Console.ReadLine(), out var result) || result <= 0)
                {
                    Console.WriteLine("Ожидалось целое положительное число. Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine();
                return result;
            }
        }

        private static string ReadRusAlphabet()
        {
            return File.ReadAllText(PathSettings.Task8_Alphabet_Rus);
        }

        private static string ReadEngAlphabet()
        {
            return File.ReadAllText(PathSettings.Task8_Alphabet_Eng);
        }

        private static string ReadRusText()
        {
            return File.ReadAllText(PathSettings.Task8_Text_Rus);
        }

        private static string ReadEngText()
        {
            return File.ReadAllText(PathSettings.Task8_Text_Eng);
        }

        private static string GenerateText(string alphabet, int length)
        {
            var result = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                result.Append(alphabet[_rnd.Next(alphabet.Length)]);
            }

            return result.ToString();
        }

        private static WorkMode ReadWorkMode()
        {
            Console.WriteLine($"1 - Сгенерировать случайный бессмысленный текст из русских символов,");
            Console.WriteLine($"2 - Сгенерировать случайный бессмысленный текст из английских символов,");
            Console.WriteLine($"3 - Посчитать индекс совпадения для текста из русских символов,");
            Console.WriteLine($"4 - Посчитать индекс совпадения для текста из английских символов,");
            Console.WriteLine($"0 - Завершить работу программы;");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return WorkMode.GENERATE_RUS_TEXT;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return WorkMode.GENERATE_ENG_TEXT;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return WorkMode.CALC_INDEX_FOR_RUS_TEXT;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        return WorkMode.CALC_INDEX_FOR_ENG_TEXT;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return WorkMode.CLOSE_PROGRAM;
                    default:
                        break;
                }
            }
        }
    }
}
