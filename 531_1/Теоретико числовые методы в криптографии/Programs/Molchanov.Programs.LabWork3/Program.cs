using Common.IsPrimeChecking;
using System.Numerics;

namespace Molchanov.Programs.LabWork3;

internal class Program
{
    static void Main()
    {
        while (true)
        {
            var testType = ReadAdditionalTaskNumber();

            if (testType == TestType.NONE)
                return;

            var (value, roundsCount) = ReadPair("число на проверку и количество раундов алгоритма");
            var checker = CreateChecker(testType, roundsCount);

            var isPrime = checker.IsPrime(value);
            Console.WriteLine($"Результат: {(isPrime ? "вероятно простое" : "составное")}");
            Console.WriteLine();
        }
    }

    private static IPrimeChecker CreateChecker(TestType testType, int roundsCount) =>
        testType switch
        {
            TestType.FERMA => new CheckerByFerma() { RoundsCount = roundsCount },
            TestType.STRASSEN => new CheckerByStrassen() { RoundsCount = roundsCount },
            TestType.MILLER_RABIN => new CheckerByRabinMiller() { RoundsCount = roundsCount },
            _ => throw new NotImplementedException(),
        };

    private static (BigInteger val1, int val2) ReadPair(string addInfo = "")
    {
        Console.WriteLine($"Введите 2 целых положительных числа через пробел {addInfo}:");
        while (true)
        {
            var input = Console.ReadLine()!;
            var splited = input.Split().Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (splited.Length != 2)
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            if (!BigInteger.TryParse(splited[0], out var val1))
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            if (!int.TryParse(splited[1], out var val2))
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            if (val1 <= 0 || val2 <= 0)
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            Console.WriteLine();
            return (val1, val2);
        }
    }

    private static TestType ReadAdditionalTaskNumber()
    {
        Console.WriteLine("Выберите алгоритм проверки на простоту: ");
        Console.WriteLine("1. Тест Ферма");
        Console.WriteLine("2. Тест Соловея-Штрассена");
        Console.WriteLine("3. Тест Миллера-Рабина");
        Console.WriteLine("0. Завершить работу программы");
        Console.WriteLine();

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    return TestType.FERMA;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    return TestType.STRASSEN;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    return TestType.MILLER_RABIN;
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    return TestType.NONE;
                default:
                    continue;
            }
        }
    }

    public enum TestType
    {
        NONE = 0,
        FERMA = 1,
        STRASSEN = 2,
        MILLER_RABIN = 3
    }
}