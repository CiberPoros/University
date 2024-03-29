﻿using CMIP.Programs.Calculator.Operations;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CMIP.Programs.Calculator
{
    public class Program
    {
        private static readonly List<char> _alphabet = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        static void Main()
        {
            for (; ; )
            {
                var baseOfNumberSystem = ReadBaseOfNumberSystem();
                var operation = ReadOperationType();
                if (operation == OperationType.EXIT_PROGRAM)
                {
                    return;
                }

                var alphabet = _alphabet.Take(baseOfNumberSystem).ToList();
                var calculationsHandler = new CalculationsHandler(alphabet);

                var number1 = ReadNumber(alphabet);
                var number2 = ReadNumber(alphabet);
                Number number3 = default;

                if (operation == OperationType.POW_MOD)
                {
                    number3 = ReadNumber(alphabet);
                }

                (Number, TimeSpan) result = default;
                try
                {
                    result = operation switch
                    {
                        OperationType.PLUS => SpeedMeter.Run(number1, number2, calculationsHandler.Summ),
                        OperationType.MINUS => SpeedMeter.Run(number1, number2, calculationsHandler.Substract),
                        OperationType.MULTIPLY => SpeedMeter.Run(number1, number2, calculationsHandler.Multiply),
                        OperationType.DIVIDE => SpeedMeter.Run(number1, number2, calculationsHandler.Divide),
                        OperationType.POW => SpeedMeter.Run(number1, number2, calculationsHandler.Pow),
                        OperationType.POW_MOD => SpeedMeter.Run(number1, number2, number3, calculationsHandler.PowMod),
                        OperationType.NONE => throw new NotImplementedException(),
                        _ => throw new NotImplementedException()
                    };
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine($"Неверный формат входных данных. Подробности: {e.Message}");
                    return;
                }

                Console.WriteLine($"Результат: {result.Item1}");
                Console.WriteLine($"Время подсчета в миллисекундах: {result.Item2.TotalMilliseconds}");
                Console.WriteLine();

                var simpleResult = SpeedMeter.Run(number1, number2, number3, alphabet, operation, CalculateSimple);
                Console.WriteLine($"Результат встроенной операции (10 - я система счисления): {(simpleResult.Item1.HasValue ? simpleResult.Item1.ToString() : "Undefined")}");
                Console.WriteLine($"Время подсчета в миллисекундах для встроенной операции: {simpleResult.Item2.TotalMilliseconds}");
                Console.WriteLine();

                if (operation == OperationType.DIVIDE)
                {
                    var remainsSimpleResult = SpeedMeter.Run(number1, number2, number3, alphabet, OperationType.GET_REMAINS, CalculateSimple);
                    Console.WriteLine($"Результат встроенной операции остатка от деления (10 - я система счисления): {(remainsSimpleResult.Item1.HasValue ? remainsSimpleResult.Item1.ToString() : "Undefined")}");
                    Console.WriteLine($"Время подсчета в миллисекундах для встроенной операции остатка от деления: {remainsSimpleResult.Item2.TotalMilliseconds}");
                    Console.WriteLine();
                } 
            }
        }

        public static BigInteger? CalculateSimple(Number left, Number right, Number modulo, List<char> alphabet, OperationType operationType)
        {
            var leftParsed = left.ToBigInteger(alphabet);
            var rightParsed = right.ToBigInteger(alphabet);
            var moduloParsed = modulo?.ToBigInteger(alphabet);

            return operationType switch
            {
                OperationType.PLUS => leftParsed + rightParsed,
                OperationType.MINUS => leftParsed - rightParsed,
                OperationType.MULTIPLY => leftParsed * rightParsed,
                OperationType.DIVIDE => leftParsed / rightParsed,
                OperationType.POW => BigInteger.Pow(leftParsed, (int)rightParsed),
                OperationType.GET_REMAINS => leftParsed % rightParsed,
                OperationType.POW_MOD => moduloParsed.Value == 0 ? null : BigInteger.ModPow(leftParsed, rightParsed, moduloParsed.Value),
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
            Console.WriteLine("4. Деление;");
            Console.WriteLine("5. Возведение в степень;");
            Console.WriteLine("6. Возведение в степень по модулю...");
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
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.D5:
                        return OperationType.POW;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.D6:
                        return OperationType.POW_MOD;
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
