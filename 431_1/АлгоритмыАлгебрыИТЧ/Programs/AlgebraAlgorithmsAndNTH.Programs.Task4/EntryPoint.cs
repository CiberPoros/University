using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Common;
using Common.Factorization;

namespace AlgebraAlgorithmsAndNTH.Programs.Task4
{
    class EntryPoint
    {
        static async Task Main()
        {
            for (; ; )
            {
                var factorizator = ReadFactorizator();
                if (factorizator is null)
                {
                    return;
                }

                var value = ReadFactorizableValue();

                var executor = new TimeLimitedExecutor();
                var result = await executor.ExecuteAsync(() => factorizator.Factorize(value).ToList());
                if (result.IsFailed)
                {
                    Console.WriteLine(string.Join("; ", result.Errors.Select(x => x.Message)));
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine("Результат факторизации: ");
                Console.WriteLine(string.Join(", ", result.Value));
                Console.WriteLine();
            }
        }

        private static BigInteger ReadFactorizableValue()
        {
            Console.WriteLine("Введите целое положительное число - факторизуемое значение:");
            Console.WriteLine();

            for (; ; )
            {
                if (!BigInteger.TryParse(Console.ReadLine(), out var result) || result <= 0)
                {
                    Console.WriteLine("Ожидалось целое положительное число. Повторите попытку...");
                    Console.WriteLine();
                }

                Console.WriteLine();
                return result;
            }
        }

        private static IFactorizator ReadFactorizator()
        {
            Console.WriteLine("Выберите алгоритм факторизации:");
            Console.WriteLine("1. Метод факторизации Ферма;");
            Console.WriteLine("2. Метод факторизации Диксона;");
            Console.WriteLine("0. Завершить работу программы.");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return new FactorizatorByFerma();
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return new FactorizatorByDickson();
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return null;
                    default:
                        break;
                }
            }
        }
    }
}
