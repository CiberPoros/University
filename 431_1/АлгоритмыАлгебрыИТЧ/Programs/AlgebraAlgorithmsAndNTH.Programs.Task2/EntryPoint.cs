using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.IsPrimeChecking;

namespace AlgebraAlgorithmsAndNTH.Programs.Task2
{
    class EntryPoint
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            for (; ; )
            {
                var checker = ReadAlgorithm();

                if (checker is null)
                {
                    return;
                }

                var value = ReadValueForCheckIsPrime();
                var executor = new TimeLimitedExecutor();
                var isPrime = await executor.ExecuteAsync(() => checker.IsPrime(value));
                Console.WriteLine(
                    isPrime.IsSuccess 
                    ? (isPrime.Value ? (checker.IsProbabilisticByPrimeResult 
                                            ? "Вероятно простое" 
                                            : "Простое") 
                                     : (checker.IsProbabilisticByComplexResult 
                                            ? "Вероятно составное" 
                                            : "Составное"))
                    : string.Join("; ", isPrime.Errors.Select(x => x.Message)));
                Console.WriteLine();
            }
        }

        private static IPrimeChecker ReadAlgorithm()
        {
            Console.WriteLine("Выберите алгоритм для проверки числа на простоту:");
            Console.WriteLine("1 - Критерий Вильсона,");
            Console.WriteLine("2 - Малая теорема Ферма,");
            Console.WriteLine("3 - Свойства чисел Кармайкла,");
            Console.WriteLine("4 - Тест Соловея-Штрассена,");
            Console.WriteLine("5 - Тест Миллера-Рабина,");
            Console.WriteLine("6 - Полиномиальный тест,");
            Console.WriteLine("0 - Выход из программы;");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return new CheckerByUilson();
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return new CheckerByFerma() { RoundsCount = ReadRoundsCount() };
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return new CheckerByCarmichael() { RoundsCount = ReadRoundsCount() };
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        return new CheckerByStrassen() { RoundsCount = ReadRoundsCount() };
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        return new CheckerByRabinMiller() { RoundsCount = ReadRoundsCount() };
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        return new CheckerByPolinomialTest();
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return null;
                    default:
                        break;
                }
            }
        }

        private static int ReadRoundsCount()
        {
            Console.WriteLine("Введите количество раундов алгоритма: ");
            Console.WriteLine();

            for (; ; )
            {
                var input = Console.ReadLine();

                if (int.TryParse(input, out var result) && result >= 1)
                {
                    Console.WriteLine();
                    return result;
                }

                Console.WriteLine("Неверный ввод! Ожидалось положительное целое число. Повторите попытку...");
                Console.WriteLine();
            }
        }

        private static BigInteger ReadValueForCheckIsPrime()
        {
            Console.WriteLine("Число на проверку: ");
            Console.WriteLine();

            for (; ; )
            {
                var input = Console.ReadLine();

                if (BigInteger.TryParse(input, out var result) && result >= 1)
                {
                    Console.WriteLine();
                    return result;
                }

                Console.WriteLine("Неверный ввод! Ожидалось положительное целое число. Повторите попытку...");
                Console.WriteLine();
            }
        }
    }
}
