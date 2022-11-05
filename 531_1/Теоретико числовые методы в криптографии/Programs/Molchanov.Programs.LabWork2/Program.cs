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
                case TaskNumber.АлгоритмПриложенийЦепныхДробей:
                    HandleTask2();
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

    private static void HandleTask2()
    {
        while (true)
        {
            var task = ReadAdditionalTaskNumber();

            switch (task)
            {
                case AdditionalTaskNumber.РешениеЛинейныхСравнений:
                    HandleAdditionalTask1();
                    break;
                case AdditionalTaskNumber.ПоискОбратногоЭлементаВКольцеВычетов:
                    HandleAdditionalTask2();
                    break;
                case AdditionalTaskNumber.РешениеЛинейныхДиофановыхУравнений:
                    HandleAdditionalTask3();
                    break;
                case AdditionalTaskNumber.None:
                    return;
                default:
                    break;
            }
        }
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

    private static (BigInteger val1, BigInteger val2, BigInteger val3) ReadThreeBigIntegers(string addInfo = "")
    {
        Console.WriteLine($"Введите 3 целых положительных числа через пробелы {addInfo}:");
        while (true)
        {
            var input = Console.ReadLine()!;
            var splited = input.Split().Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (splited.Length != 3)
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

            if (!BigInteger.TryParse(splited[2], out var val3))
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            if (val1 <= 0 || val2 <= 0 || val3 <= 0)
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            Console.WriteLine();
            return (val1, val2, val3);
        }
    }

    private static (bool success, BigInteger result) SolveComparison(BigInteger a, BigInteger b, BigInteger c, BigInteger x, BigInteger y, BigInteger g)
    {
        if (a == 0 && b == 0)
        {
            x = 0;
            return (c == 0, x);
        }
        (g, x, y) = GCD.GetGCDAdvanced(a > 0 ? a : -a, b > 0 ? b : -b);

        if (c % g != 0)
            return (false, default);

        x *= c / g;
        y *= c / g;

        if (a < 0)
            x *= -1;

        if (b < 0)
            y *= -1;

        return (true, x);
    }

    private static void HandleAdditionalTask1()
    {
        var (a, b, m) = ReadThreeBigIntegers("a, b - элементы, m - модуль");

        var (success, result) = SolveComparison(a, m, b, 0, 0, 0);

        if (!success)
        {
            Console.WriteLine("Решений нет!");
            Console.WriteLine();
            return;
        }

        Console.WriteLine($"x = {result}");
        Console.WriteLine();
    }

    private static void HandleAdditionalTask2()
    {
        var (a, m) = ReadPairBigIntegers("a - элемент, m - модуль");

        var u1 = m;
        var u2 = new BigInteger(0);
        var v1 = a;
        var v2 = new BigInteger(1);

        while (v1 != 0)
        {
            var q = u1 / v1;
            var t1 = u1 - q * v1;
            var t2 = u2 - q * v2;
            u1 = v1;
            u2 = v2;
            v1 = t1;
            v2 = t2;
        }

        var ans = u1 == 1 ? (u2 + m) % m : -1;

        if (ans == -1)
        {
            Console.WriteLine("У введенного элемента не существует обратного в этом кольце!");
            Console.WriteLine();
            return;
        }

        Console.WriteLine($"Обратный элемент: {ans}");
        Console.WriteLine();
    }

    private static void HandleAdditionalTask3()
    {
        var (a, b, c) = ReadThreeBigIntegers("a, b - коэффициенты, c - свободный коэффициент");

        var (d, x, y) = GCD.GetGCDAdvanced(a, b);

        if (c % d != 0)
        {
            Console.WriteLine("Решение не может быть получено");
            Console.WriteLine();
            return;
        }

        var t = (c / d) * x;
        var t2 = b / d;

        if (t == 0)
        {
            (x, y) = (0, (c / d) * y);
        }
        else if (t > 0)
        {
            (x, y) = (t + t2 * (-(t / t2)), c / d * y - a / d * (-(t / t2)));
        }
        else
        {
            (x, y) = (t + t2 * (-((t - t2 + 1) / t2)), c / d * y - a / d * ((-((t - t2 + 1) / t2))));
        }

        Console.WriteLine($"x = {x}; y = {y}");
        Console.WriteLine();
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

    private static AdditionalTaskNumber ReadAdditionalTaskNumber()
    {
        Console.WriteLine("Выберите номер задания: ");
        Console.WriteLine("1. Решение линейных сравнений");
        Console.WriteLine("2. Поиск обратного элемента в кольце вычетов");
        Console.WriteLine("3. Решение линейных диофановых уравнений");
        Console.WriteLine("0. Вернуться в предыдущее меню");
        Console.WriteLine();

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    return AdditionalTaskNumber.РешениеЛинейныхСравнений;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    return AdditionalTaskNumber.ПоискОбратногоЭлементаВКольцеВычетов;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    return AdditionalTaskNumber.РешениеЛинейныхДиофановыхУравнений;
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    return AdditionalTaskNumber.None;
                default:
                    continue;
            }
        }
    }

    public enum AdditionalTaskNumber
    {
        None = 0,
        РешениеЛинейныхСравнений = 1,
        ПоискОбратногоЭлементаВКольцеВычетов = 2,
        РешениеЛинейныхДиофановыхУравнений = 3
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