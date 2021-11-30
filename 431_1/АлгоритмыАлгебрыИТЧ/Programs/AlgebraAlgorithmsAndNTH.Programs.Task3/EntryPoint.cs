using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.PrimeNumbersGeneration;

namespace AlgebraAlgorithmsAndNTH.Programs.Task3
{
    class EntryPoint
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            for (; ; )
            {
                var generator = ReadAlgorithm();

                if (generator is null)
                {
                    return;
                }

                var bitsCount = ReadBitsCount();
                var executor = new TimeLimitedExecutor();
                var primeNumberResult = await executor.ExecuteAsync(() => generator.Generate(bitsCount));
                Console.WriteLine(
                    primeNumberResult.IsSuccess
                    ? $"Сгенерированное число: {primeNumberResult.Value}"
                    : string.Join("; ", primeNumberResult.Errors.Select(x => x.Message)));
                Console.WriteLine();
            }
        }

        private static IPrimeNumberGenerator ReadAlgorithm()
        {
            Console.WriteLine("Выберите алгоритм для построения большого простого числа:");
            Console.WriteLine("1 - Критерий Люка,");
            Console.WriteLine("2 - Теорема Поклинктона,");
            Console.WriteLine("0 - Выход из программы;");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return new GeneratorByLukeTest();
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return new GeneratorByPoklington() { RoundsCount = ReadRoundsCount() };
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return null;
                    default:
                        break;
                }
            }
        }

        private static int ReadBitsCount()
        {
            Console.WriteLine("Введите целое положительное число: минимальный порог бит в генерируемом числе: ");
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
    }
}
