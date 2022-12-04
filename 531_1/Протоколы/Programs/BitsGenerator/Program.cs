using BitsGenerator.Steps;
using Common.FieldMath;
using System.Numerics;

namespace BitsGenerator;

internal class Program
{
    private static readonly GenerateCommonParameters _step1 = new();
    private static readonly BobRandomNumbersGeneration _step2 = new();
    private static readonly AliceRandomNumberGenerate _step3 = new();
    private static readonly AliceCalculateModPow _step4 = new();
    private static readonly BobBitGeneration _step5 = new();
    private static readonly AliceShareResult _step6 = new();
    private static readonly Random _random = new();

    private static int _len = -1;

    public async static Task Main()
    {
        for (; ; )
        {
            Console.WriteLine($"1. Генерация общего параметра {nameof(CommonParameters.P)};");
            Console.WriteLine($"2. Генерация случайных чисел {nameof(BobRandomNumbers.T)}, {nameof(BobRandomNumbers.H)} Бобом;");
            Console.WriteLine($"3. Генерация случайного числа {nameof(AliceRandomNumber.X)} Алисой;");
            Console.WriteLine($"4. Вычисление числа {nameof(AliceRandomModPowNumber.Y)} Алисой, используя {nameof(BobRandomNumbers.T)} или {nameof(BobRandomNumbers.H)};");
            Console.WriteLine($"5. Предположение Боба о том, какое число использовала Алиса {nameof(BobRandomNumbers.T)} или {nameof(BobRandomNumbers.H)};");
            Console.WriteLine($"6. Объявление результата протокола Алисой;");
            Console.WriteLine($"7. Боб проверяет, что бросок монеты был честен;");
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

        var (p, divider) = Generation.GetBigPrimeWithBigPrimeDivider(_len);

        await _step1.WriteParameters(new CommonParameters() { P = p, Divider = divider });
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
        var (p, divider) = (commonParams.P, commonParams.Divider);

        var h = new BigInteger(1);
        while (true)
        {
            h = Generation.GetRandom(_len - 1) % p;

            if (BigInteger.ModPow(h, 2, p - 1) == 1)
            {
                continue;
            }

            if (BigInteger.ModPow(h, divider, p - 1) == 1)
            {
                continue;
            }

            break;
        }

        var t = new BigInteger(1);
        while (true)
        {
            t = Generation.GetRandom(_len - 1) % p;

            if (BigInteger.ModPow(t, 2, p - 1) == 1)
            {
                continue;
            }

            if (BigInteger.ModPow(t, divider, p - 1) == 1)
            {
                continue;
            }

            break;
        }

        await _step2.WriteParameters(new BobRandomNumbers { H = h, T = t });

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
        var (p, divider) = (commonParams.P, commonParams.Divider);

        (success, var bobRandomNumbers) = await _step2.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step2.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (h, t) = (bobRandomNumbers.H, bobRandomNumbers.T);

        if (BigInteger.ModPow(h, 2, p - 1) == 1)
        {
            Console.WriteLine($"{nameof(bobRandomNumbers.H)} не является примитивным элементом в GF({nameof(commonParams.P)})");
            Console.WriteLine();
            return;
        }

        if (BigInteger.ModPow(h, divider, p - 1) == 1)
        {
            Console.WriteLine($"{nameof(bobRandomNumbers.H)} не является примитивным элементом в GF({nameof(commonParams.P)})");
            Console.WriteLine();
            return;
        }

        if (BigInteger.ModPow(t, 2, p - 1) == 1)
        {
            Console.WriteLine($"{nameof(bobRandomNumbers.T)} не является примитивным элементом в GF({nameof(commonParams.P)})");
            Console.WriteLine();
            return;
        }

        if (BigInteger.ModPow(t, divider, p - 1) == 1)
        {
            Console.WriteLine($"{nameof(bobRandomNumbers.T)} не является примитивным элементом в GF({nameof(commonParams.P)})");
            Console.WriteLine();
            return;
        }

        var x = Generation.GetRandom(_len - 1) % p;
        while (BigInteger.GreatestCommonDivisor(x, p - 1) != 1)
        {
            x = Generation.GetRandom(_len - 1) % p;
        }

        await _step3.WriteParameters(new AliceRandomNumber { X = x });

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
        var (p, divider) = (commonParams.P, commonParams.Divider);

        (success, var bobRandomNumbers) = await _step2.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step2.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (h, t) = (bobRandomNumbers.H, bobRandomNumbers.T);

        (success, var aliceRandomNumbers) = await _step3.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step2.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var x = aliceRandomNumbers.X;

        var random = _random.Next(2);

        var y = random == 0 
            ? BigInteger.ModPow(h, x, p) 
            : BigInteger.ModPow(t, x, p);

        await _step4.WriteParameters(new AliceRandomModPowNumber { Y = y, UsedValue = random == 0 ? nameof(BobRandomNumbers.H) : nameof(BobRandomNumbers.T) });

        Console.WriteLine(_step4.Description);
        Console.WriteLine($"Результат сохранен в файл {_step4.FileName}");
        Console.WriteLine();
    }

    private static async Task Step5()
    {
        var random = _random.Next(2);
        await _step5.WriteParameters(new BitCalculatedByBob { BobsGuess = random });

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
        var (p, _) = (commonParams.P, commonParams.Divider);

        (success, var bobRandomNumbers) = await _step2.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step2.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (h, _) = (bobRandomNumbers.H, bobRandomNumbers.T);

        (success, var aliceRandomNumbers) = await _step3.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step3.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var x = aliceRandomNumbers.X;

        (success, var aliceModPowNumber) = await _step4.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step4.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var y = aliceModPowNumber.Y;

        (success, var bobsGuess) = await _step5.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step5.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var bobsGuessBit = bobsGuess.BobsGuess;

        var hUsed = BigInteger.ModPow(h, x, p) == y;

        var resultBit = hUsed && bobsGuessBit == 1 || !hUsed && bobsGuessBit == 0 ? 1 : 0;

        await _step6.WriteParameters(new ResultBit() { Bit = resultBit });

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
        var (p, _) = (commonParams.P, commonParams.Divider);

        (success, var bobRandomNumbers) = await _step2.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step2.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var (h, t) = (bobRandomNumbers.H, bobRandomNumbers.T);

        (success, var aliceRandomNumbers) = await _step3.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step3.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var x = aliceRandomNumbers.X;

        (success, var aliceModPowNumber) = await _step4.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step4.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var y = aliceModPowNumber.Y;

        (success, var bobsGuess) = await _step5.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step5.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var bobsGuessBit = bobsGuess.BobsGuess;

        (success, var resultBit) = await _step6.ReadParameters();
        if (!success)
        {
            Console.WriteLine($"Не удалось считать данные из файла {_step6.FileName}. Файл отсутствует или поврежден");
            Console.WriteLine();
            return;
        }
        var result = resultBit.Bit;

        var hMod = BigInteger.ModPow(h, x, p);
        var tMod = BigInteger.ModPow(t, x, p);

        if (tMod != y && hMod != y)
        {
            Console.WriteLine($"H^X != Y mop P и T^X != Y mop P, следовательно Алиса - жулик!");
            Console.WriteLine();
            return;
        }

        var hUsed = BigInteger.ModPow(h, x, p) == y;
        if ((hUsed && bobsGuessBit == 1 || !hUsed && bobsGuessBit == 0) && result == 0)
        {
            Console.WriteLine($"Результирующий бит должен быть равен 1, а равен 0. Алиса - жулик!");
            Console.WriteLine();
            return;
        }

        if ((!hUsed && bobsGuessBit == 1 || hUsed && bobsGuessBit == 0) && result == 1)
        {
            Console.WriteLine($"Результирующий бит должен быть равен 0, а равен 1. Алиса - жулик!");
            Console.WriteLine();
            return;
        }

        if (BigInteger.GreatestCommonDivisor(x, p - 1) != 1)
        {
            Console.WriteLine("Числа X и P-1 не являются взаимнопростыми. Алиса - жулик!");
            Console.WriteLine();
            return;
        }

        Console.WriteLine("Боб выполнил все проверки и убедился, что Алиса не жульничает");
        Console.WriteLine();
    }
}