using System.Numerics;

namespace Molchanov.Programs.LabWork1;

internal class Program
{
    static void Main()
    {
        while (true)
        {
            var task = ReadTaskNumber();

            switch (task)
            {
                case TaskNumber.GCD:
                    HandleGcd();
                    break;
                case TaskNumber.GCD_EXTEND:
                    HandleGcdExtended();
                    break;
                case TaskNumber.GCD_BINARY:
                    HandleBinaryAlg();
                    break;
                case TaskNumber.COMPARISON_SYSTEM:
                    HandleComparisonSystem();
                    break;
                case TaskNumber.GAUSS:
                    HandleGauss();
                    break;
                case TaskNumber.NONE:
                    return;
                default:
                    break;
            } 
        }
    }

    private static TaskNumber ReadTaskNumber()
    {
        Console.WriteLine("Выберите номер задания: ");
        Console.WriteLine("1. Алгоритм Евклида");
        Console.WriteLine("2. Расширенный алгоритм Евклида");
        Console.WriteLine("3. Бинарный алгоритм");
        Console.WriteLine("4. Решение системы сравнений по Китайской теореме");
        Console.WriteLine("5. Решение системы уравнений над кольцом целых чисел методом Гаусса");
        Console.WriteLine("0. Закрыть программу");
        Console.WriteLine();

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    return TaskNumber.GCD;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    return TaskNumber.GCD_EXTEND;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    return TaskNumber.GCD_BINARY;
                case ConsoleKey.NumPad4:
                case ConsoleKey.D4:
                    return TaskNumber.COMPARISON_SYSTEM;
                case ConsoleKey.NumPad5:
                case ConsoleKey.D5:
                    return TaskNumber.GAUSS;
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    return TaskNumber.NONE;
                default: 
                    continue;
            }
        }
    }

    private static void HandleGcd()
    {
        var (a, b) = ReadPairBigIntegers();

        var gcdCalc = new GCDCalculator();
        var (result, outputInfo) = gcdCalc.ByEuklid(a, b);

        Console.WriteLine($"Result = {result}");
        Console.WriteLine("Process: ");
        Console.WriteLine(string.Join(Environment.NewLine, outputInfo));
        Console.WriteLine();
    }

    private static void HandleGcdExtended()
    {
        var (a, b) = ReadPairBigIntegers();

        var gcdCalc = new GCDCalculator();
        var (result, u, v, outputInfo) = gcdCalc.ByEuklidExtend(a, b);

        Console.WriteLine($"Result = {result}; u = {u}; v = {v}");
        Console.WriteLine("Process: ");
        Console.WriteLine(string.Join(Environment.NewLine, outputInfo));
        Console.WriteLine();
    }

    private static void HandleBinaryAlg()
    {
        var (a, b) = ReadPairBigIntegers();

        var gcdCalc = new GCDCalculator();
        var (result, outputInfo) = gcdCalc.ByBinaryAlg(a, b);

        Console.WriteLine($"Result = {result}");
        Console.WriteLine("Process: ");
        Console.WriteLine(string.Join(Environment.NewLine, outputInfo));
        Console.WriteLine();
    }

    private static void HandleComparisonSystem()
    {
        var (mArr, uArr) = ReadCompSystemArgs();

        var compSystemResolver = new ComparisonSystemResolver();
        var (result, outputInfo) = compSystemResolver.ByChineseTheorem(mArr, uArr);
        Console.WriteLine($"Result = {result}");
        Console.WriteLine("Process: ");
        Console.WriteLine(string.Join(Environment.NewLine, outputInfo));
        Console.WriteLine();
    }

    private static void HandleGauss()
    {
        var n = ReadInt(" N - размер матрицы");
        var mod = ReadBigInt(" mod - размер кольца целых чисел");
        var matrix = ReadMatrix(n);
        var gauss = new Gauss(mod, n, n + 1, matrix);
        var res = gauss.Run();
        Console.WriteLine(string.Join(Environment.NewLine, res.outputInfo));
    }

    private static BigInteger[,] ReadMatrix(int size)
    {
        Console.WriteLine("Введите матрицу N x N+1, состоящую из целых чисел, разделенных пробелами. Последний стоблец матрицы - свободные коеффициенты");
        var res = new BigInteger[size, size + 1];
 
        while (true)
        {
            bool crash = false;
            for (int i = 0; i < size; i++)
            {
                var input = Console.ReadLine()!.Split().Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (input.Length != size + 1)
                {
                    crash = true;
                    break;
                }

                if (input.Any(x => !BigInteger.TryParse(x, out _)))
                {
                    crash = true;
                    break;
                }

                for (int j = 0; j < size + 1; j++)
                {
                    res[i, j] = BigInteger.Parse(input[j]);
                }
            }    

            if (crash)
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            Console.WriteLine();
            return res;
        }
    }

    private static (List<BigInteger> mArr, List<BigInteger> uArr) ReadCompSystemArgs()
    {
        Console.WriteLine("Введите положительное целое число - количество сравнений в системе:");

        BigInteger k = -1;
        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            
            if (!BigInteger.TryParse(input, out k))
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            if (k <= 0)
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            Console.WriteLine();
            break;
        }

        Console.WriteLine($"Введите {k} чисел через пробел - m[0], m[1], ..., m[k - 1]:");

        var mArr = new List<BigInteger>();
        while (true)
        {
            var input = Console.ReadLine()?.Trim().Split()!;

            if (input.Length != k)
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            if (input.Any(x => !BigInteger.TryParse(x, out var _)))
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            mArr = input.Select(x => BigInteger.Parse(x)).ToList();
            Console.WriteLine();
            break;
        }

        Console.WriteLine($"Введите {k} чисел через пробел - u[0], u[1], ..., u[k - 1]:");

        var uArr = new List<BigInteger>();
        while (true)
        {
            var input = Console.ReadLine()?.Trim().Split()!;

            if (input.Length != k)
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            if (input.Any(x => !BigInteger.TryParse(x, out var _)))
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            uArr = input.Select(x => BigInteger.Parse(x)).ToList();
            Console.WriteLine();
            break;
        }

        return (mArr, uArr);
    }

    private static BigInteger ReadBigInt(string addInfo = "")
    {
        Console.WriteLine($"Введите целых положительных число{addInfo}:");

        while (true)
        {
            var input = Console.ReadLine()?.Trim()!;

            if (!BigInteger.TryParse(input, out var res))
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            Console.WriteLine();
            return res;
        }
    }

    private static int ReadInt(string addInfo = "")
    {
        Console.WriteLine($"Введите целых положительных число{addInfo}:");

        while (true)
        {
            var input = Console.ReadLine()?.Trim()!;

            if (!int.TryParse(input, out var res))
            {
                Console.WriteLine("Ошибка формата входных данных. Повторите попытку...");
                continue;
            }

            Console.WriteLine();
            return res;
        }
    }

    private static (BigInteger val1, BigInteger val2) ReadPairBigIntegers(string addInfo = "")
    {
        Console.WriteLine($"Введите 2 целых положительных числа через пробел{addInfo}:");
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

    private enum TaskNumber
    {
        NONE = 0,
        GCD = 1,
        GCD_EXTEND = 2,
        GCD_BINARY = 3,
        COMPARISON_SYSTEM = 4,
        GAUSS = 5
    }
}