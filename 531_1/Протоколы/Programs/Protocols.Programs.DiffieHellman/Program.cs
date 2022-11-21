using Common.FieldMath;
using Protocols.Programs.DiffieHellman.Steps;
using System.Numerics;

namespace Protocols.Programs.DiffieHellman;

internal class Program
{
    private static readonly GenerateCommonParameters _step1 = new();
    private static readonly GenerateCloseKeyAlice _step2 = new();
    private static readonly GenerateCloseKeyBob _step3 = new();
    private static readonly GenerateOpenKeyAlice _step4 = new();
    private static readonly GenerateOpenKeyBob _step5 = new();
    private static readonly GenerateCommonSecretKeyByAlice _step6 = new();
    private static readonly GenerateCommonSecretKeyByBob _step7 = new();

    private static int _len = -1;

    public async static Task Main()
    {
        for (; ; )
        {
            Console.WriteLine($"1. Генерация общих параметров {nameof(CommonParameters.P)}, {nameof(CommonParameters.G)};");
            Console.WriteLine($"2. Генерация секретного ключа Алисы {nameof(CloseKeyAlice.CloseA)};");
            Console.WriteLine($"3. Генерация секретного ключа Боба {nameof(CloseKeyBob.CloseB)};");
            Console.WriteLine($"4. Генерация открытого ключа Алисы {nameof(OpenKeyAlice.OpenA)};");
            Console.WriteLine($"5. Генерация открытого ключа Боба {nameof(OpenKeyBob.OpenB)};");
            Console.WriteLine($"6. Алиса вычисляет общий секретный ключ {nameof(CommonSecretKeyByAlice.K)};");
            Console.WriteLine($"7. Боб вычисляет общий секретный ключ {nameof(CommonSecretKeyByBob.K)};");
            Console.WriteLine("0. Закрыть программу...");
            Console.WriteLine();

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        await Step1();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        await Step2();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        await Step3();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        await Step4();
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        await Step5();
                        break;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        await Step6();
                        break;
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        await Step7();
                        break;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return;
                    default:
                        break;
                }
            }
        }
    }

    private static async Task Step1()
    {
        Console.WriteLine("Введите длину p в битах");

        while (true)
        {
            var input = Console.ReadLine();

            if (!int.TryParse(input, out _len) || _len <= 2)
            {
                Console.WriteLine("Ожидалось целое положительное число больше 2. Повторите попытку...");
                continue;
            }

            Console.WriteLine();
            break;
        }

        var (p, g) = Generation.GetBigPrimeWithPrimitiveElement(_len);

        await _step1.WriteParameters(new CommonParameters() { P = p, G = g });
        IntF.Modulo = p;

        Console.WriteLine(_step1.Description);
        Console.WriteLine($"Результат сохранен в файл {_step1.FileName}");
        Console.WriteLine();
    }

    private static async Task Step2()
    {
        var (success, commonParams) = await _step1.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step1.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (p, _) = (commonParams.P, commonParams.G);

        var a = Generation.GetRandom(_len - 1) % p;
        while (a == 0)
        {
            a = Generation.GetRandom(_len - 1) % p;
        }

        await _step2.WriteParameters(new CloseKeyAlice() { CloseA = a });

        Console.WriteLine(_step2.Description);
        Console.WriteLine($"Результат сохранен в файл {_step2.FileName}");
        Console.WriteLine();
    }

    private static async Task Step3()
    {
        var (success, commonParams) = await _step1.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step1.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (p, _) = (commonParams.P, commonParams.G);

        var b = Generation.GetRandom(_len - 1) % p;
        while (b == 0)
        {
            b = Generation.GetRandom(_len - 1) % p;
        }

        await _step3.WriteParameters(new CloseKeyBob() { CloseB = b });

        Console.WriteLine(_step3.Description);
        Console.WriteLine($"Результат сохранен в файл {_step3.FileName}");
        Console.WriteLine();
    }

    private static async Task Step4()
    {
        var (success, commonParams) = await _step1.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step1.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (p, g) = (commonParams.P, commonParams.G);

        (success, var closeKeyAlice) = await _step2.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step2.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var closeA = (closeKeyAlice.CloseA);

        var openA = BigInteger.ModPow(g, closeA, p);
        await _step4.WriteParameters(new OpenKeyAlice() { OpenA = openA });

        Console.WriteLine(_step4.Description);
        Console.WriteLine($"Результат сохранен в файл {_step4.FileName}");
        Console.WriteLine();
    }

    private static async Task Step5()
    {
        var (success, commonParams) = await _step1.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step1.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (p, g) = (commonParams.P, commonParams.G);

        (success, var closeKeyBob) = await _step3.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step3.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var closeA = (closeKeyBob.CloseB);

        var openB = BigInteger.ModPow(g, closeA, p);
        await _step5.WriteParameters(new OpenKeyBob() { OpenB = openB });

        Console.WriteLine(_step5.Description);
        Console.WriteLine($"Результат сохранен в файл {_step5.FileName}");
        Console.WriteLine();
    }

    private static async Task Step6()
    {
        var (success, commonParams) = await _step1.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step1.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (p, g) = (commonParams.P, commonParams.G);

        (success, var closeKeyAlice) = await _step2.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step2.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var closeA = (closeKeyAlice.CloseA);

        (success, var openKeyBob) = await _step5.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step5.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var openB = (openKeyBob.OpenB);

        var secretKeyK = BigInteger.ModPow(openB, closeA, p);
        await _step6.WriteParameters(new CommonSecretKeyByAlice() { K = secretKeyK });

        Console.WriteLine(_step6.Description);
        Console.WriteLine($"Результат сохранен в файл {_step6.FileName}");
        Console.WriteLine();
    }

    private static async Task Step7()
    {
        var (success, commonParams) = await _step1.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step1.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (p, g) = (commonParams.P, commonParams.G);

        (success, var closeKeyBob) = await _step3.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step3.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var closeB = (closeKeyBob.CloseB);

        (success, var openKeyAlice) = await _step4.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step4.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var openA = (openKeyAlice.OpenA);

        var secretKeyK = BigInteger.ModPow(openA, closeB, p);
        await _step7.WriteParameters(new CommonSecretKeyByBob() { K = secretKeyK });

        Console.WriteLine(_step7.Description);
        Console.WriteLine($"Результат сохранен в файл {_step7.FileName}");
        Console.WriteLine();
    }
}