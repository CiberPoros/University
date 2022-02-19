using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Summer
{
    internal class Program
    {
        static void Main()
        {
            var alphabet = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            var numbersSystemBase = ReadBaseOfNumberSystem();
            alphabet = alphabet.Take(numbersSystemBase).ToList();

            var number1 = ReadNumber(alphabet);
            var number2 = ReadNumber(alphabet);

            var summer = new Summer(alphabet);
            var result = summer.Summ(number1, number2);

            var outputList = new List<string>
            {
                new string(number1),
                new string(number2),
                new string('-', Math.Max(Math.Max(number1.Length, number2.Length), result.Length)),
                new string(result)
            };

            var padding = outputList.Max(x => x.Length);
            for (int i = 0; i < outputList.Count; i++)
            {
                outputList[i] = outputList[i].PadLeft(padding);
            }
            outputList[0] = "+" + outputList[0];
            outputList[1] = "+" + outputList[1];
            padding = outputList.Max(x => x.Length);
            for (int i = 0; i < outputList.Count; i++)
            {
                outputList[i] = outputList[i].PadLeft(padding);
            }

            Console.WriteLine(string.Join(Environment.NewLine, outputList));
        }

        private static int ReadBaseOfNumberSystem()
        {
            Console.WriteLine("Введите основание системы счисления (число от 2 до 16)...");

            for (; ; )
            {
                var input = Console.ReadLine();

                if (!int.TryParse(input, out var result) || result < 2 || result > 16)
                {
                    Console.WriteLine("Ожидалось целое число из отрезка [2..16]. Повторите попытку...");
                    continue;
                }

                Console.WriteLine();
                return result;
            }
        }

        private static char[] ReadNumber(List<char> alphabet)
        {
            Console.WriteLine("Веедите число, состоящее только из символов алфавита...");
            for (; ; )
            {
                var input = Console.ReadLine();

                if (input.Any(x => !alphabet.Contains(x)))
                {
                    Console.WriteLine("Число должно состоять только из символов алфавита. Повторите попытку...");
                    continue;
                }

                Console.WriteLine();
                return input.ToArray();
            }
        }
    }
}
