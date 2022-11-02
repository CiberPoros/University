using Common;
using System.Numerics;

namespace Molchanov.Programs.LabWork2;

internal class Program
{
    static void Main()
    {
        while (true)
        {
            var task = ReadTaskNumber();

            switch (task)
            {
                case TaskNumber.АлгоритмРазложенияВЦепнуюДробь:
                    HandleTask1();
                    break;
                case TaskNumber.АлгоритмВычисленияСимволаЯкоби:
                    HandleTask3();
                    break;
                case TaskNumber.АлгоритмВычисленияСимволаЛежандра:
                    HandleTask4();
                    break;
                case TaskNumber.ИзвленениеКвадратногоКорняВКольцеВычетов:
                    HandleTask5();
                    break;
                case TaskNumber.None:
                    return;
                default:
                    break;
            }
        }
    }

    private static void HandleTask1()
    {
        var (a, b) = ReadPairBigIntegers("делитель и делимое");
        Console.WriteLine($"Результат: {Task1(a, b)}");
    }

    private static void HandleTask3()
    {
        var (a, b) = ReadPairBigIntegers("2 числа a и b");
        Console.WriteLine($"Результат: {Task3(a, b)}");
    }

    private static void HandleTask4()
    {
        var (a, b) = ReadPairBigIntegers("2 числа a и b");
        Console.WriteLine($"Результат: {Task4(a, b)}");
    }

    private static void HandleTask5()
    {
        var (a, b) = ReadPairBigIntegers("2 числа a и p");
        Console.WriteLine($"Результат: {Task5(a, b)}");
    }

    private static string Task1(BigInteger a, BigInteger b)
    {
        var list = new List<BigInteger>();
        Gcd(list, a, b);

        return $"[{list[0]};{string.Join(",", list.Skip(1))}]";
    }

    private static BigInteger Task3(BigInteger a, BigInteger b)
    {
        return Jacobian.Calculate(a, b);
    }

    private static BigInteger Task4(BigInteger a, BigInteger b)
    {
        var res = BigInteger.ModPow(a, (b - 1) / 2, b);
        if (res == b - 1)
            res = -1;

        return res;
    }

    private static BigInteger Task5(BigInteger a, BigInteger p)
    {
        var fInt = new F_int(a, p);

        var sqrt = F_int.Sqrt(fInt);

        return sqrt._val;
    }

    private static BigInteger Gcd(List<BigInteger> list, BigInteger a, BigInteger b)
    {
        if (b > 0)
            list.Add(a / b);

        return b > 0 ? Gcd(list, b, a % b) : a;
    }

    private static (BigInteger val1, BigInteger val2) ReadPairBigIntegers(string addInfo = "")
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

            if (!BigInteger.TryParse(splited[1], out var val2))
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

    private static TaskNumber ReadTaskNumber()
    {
        Console.WriteLine("Выберите номер задания: ");
        Console.WriteLine("1. Алгоритм разложения в цепную дробь");
        Console.WriteLine("2. Алгоритм приложений цепных дробей");
        Console.WriteLine("3. Алгоритм вычисления символа якоби");
        Console.WriteLine("4. Алгоритм вычисления символа лагранжа");
        Console.WriteLine("5. Извленение квадратного корня в кольце вычетов");
        Console.WriteLine("0. Закрыть программу");
        Console.WriteLine();

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    return TaskNumber.АлгоритмРазложенияВЦепнуюДробь;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    return TaskNumber.АлгоритмПриложенийЦепныхДробей;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    return TaskNumber.АлгоритмВычисленияСимволаЯкоби;
                case ConsoleKey.NumPad4:
                case ConsoleKey.D4:
                    return TaskNumber.АлгоритмВычисленияСимволаЛежандра;
                case ConsoleKey.NumPad5:
                case ConsoleKey.D5:
                    return TaskNumber.ИзвленениеКвадратногоКорняВКольцеВычетов;
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    return TaskNumber.None;
                default:
                    continue;
            }
        }
    }

    public enum TaskNumber
    {
        None = 0,
        АлгоритмРазложенияВЦепнуюДробь = 1,
        АлгоритмПриложенийЦепныхДробей = 2,
        АлгоритмВычисленияСимволаЯкоби = 3,
        АлгоритмВычисленияСимволаЛежандра = 4,
        ИзвленениеКвадратногоКорняВКольцеВычетов = 5
    }
}