using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIP.Programs.Calculator
{
    internal class Program
    {
        private static List<char> _alphabet = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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

        private static Number ReadNumber(List<char> alphabet)
        {
            Console.WriteLine("Веедите число, состоящее только из символов алфавита (можно указать знак)...");
            for (; ; )
            {
                var input = Console.ReadLine();

                var isPositive = input[0] != '-';
                if (input[0] == '-')
                {
                    input = new string(input.Skip(1).ToArray());
                }

                if (input.Any(x => !alphabet.Contains(x)))
                {
                    Console.WriteLine("Число должно состоять только из символов алфавита. Повторите попытку...");
                    continue;
                }

                Console.WriteLine();
                return new Number(input.ToArray(), isPositive);
            }
        }
    }
}
