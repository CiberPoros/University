using CMIP.Programs.Calculator.Operations;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CMIP.Programs.Calculator
{
    internal class Program
    {
        private static readonly List<char> _alphabet = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        static void Main()
        {
            var baseOfNumberSystem = ReadBaseOfNumberSystem();
            var operation = ReadOperationType();
            var alphabet = _alphabet.Take(baseOfNumberSystem).ToList();
            var calculationsHandler = new CalculationsHandler(alphabet);

            var number1 = ReadNumber(alphabet);
            var number2 = ReadNumber(alphabet);
            
            var result = operation switch
            {
                OperationType.PLUS => SpeedMeter.Run(number1, number2, calculationsHandler.Summ),
                OperationType.MINUS => SpeedMeter.Run(number1, number2, calculationsHandler.Substract),
                OperationType.MULTIPLY => SpeedMeter.Run(number1, number2, calculationsHandler.Multiply),
                OperationType.DIVIDE => SpeedMeter.Run(number1, number2, calculationsHandler.Divide),
                OperationType.NONE => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };

            Console.WriteLine($"Результат: {result.Item1}");
            Console.WriteLine($"Время подсчета в миллисекундах: {result.Item2.TotalMilliseconds}");
            Console.WriteLine();

            var simpleResult = SpeedMeter.Run(number1, number2, alphabet, operation, CalculateSimple);
            Console.WriteLine($"Результат встроенной операции (10 - я система счисления): {simpleResult.Item1}");
            Console.WriteLine($"Время подсчета в миллисекундах для встроенной операции: {simpleResult.Item2.TotalMilliseconds}");
            Console.WriteLine();
        }

        private static BigInteger CalculateSimple(Number left, Number right, List<char> alphabet, OperationType operationType)
        {
            var leftParsed = left.ToBigInteger(alphabet);
            var rightParsed = right.ToBigInteger(alphabet);

            return operationType switch
            {
                OperationType.PLUS => leftParsed + rightParsed,
                OperationType.MINUS => leftParsed - rightParsed,
                OperationType.MULTIPLY => leftParsed * rightParsed,
                OperationType.DIVIDE => leftParsed / rightParsed,
                OperationType.NONE => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
        }

        private static OperationType ReadOperationType()
        {
            Console.WriteLine("Выберите операцию:");
            Console.WriteLine("1. Сложение;");
            Console.WriteLine("2. Вычитание;");
            Console.WriteLine("3. Умножение;");
            Console.WriteLine("4. Деление...");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        return OperationType.PLUS;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                        return OperationType.MINUS;
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3:
                        return OperationType.MULTIPLY;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4:
                        return OperationType.DIVIDE;
                    default:
                        continue;
                }
            }
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

                if (!input.Any())
                {
                    Console.WriteLine("Ожидалось некоторое число. Повторите попытку...");
                    continue;
                }

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
