using Generators.Programs.Analizer.Criteria;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Generators.Programs.Analizer
{
    internal class Program
    {
        private const string RandomNumbersFileName = "Random.txt";
        private const string MathematicalExpectationListFileName = "MathExp.txt";
        private const string StandardDeviationListFileName = "StandardDeviation.txt";
        private const string DoubleValueFormat = "0.0000";

        private static readonly Random _random = new();
        private static readonly IEnumerable<ICriteria> _criteria = new List<ICriteria>
        {
            new HeSquareCriteria(),
            new SeriesCriteria(),
            new IntervalCriteria(),
            new PartitioningCriteria(),
            new PermutationCriteria(),
            new MonoCriteria(),
            new ConflictCriteria()
        };

        static void Main()
        {
            for (; ; )
            {
                var workMode = ReadWorkMode();

                switch (workMode)
                {
                    case WorkMode.GENERATE_RANDOM_AND_CAL_STAT:
                        HandleCase1();
                        break;
                    case WorkMode.CALCULATE_MATHEMATICAL_EXPECTATION_AND_STANDART_DEVIATION:
                        HandleCase2();
                        break;
                    case WorkMode.GENERATION_AND_COMPARATION_VALUES_FOR_GRAPHICS:
                        HandleCase3();
                        break;
                    case WorkMode.CHECK_CRITERIA:
                        HandleCase4();
                        break;
                    case WorkMode.EXIT_PROGRAM:
                        return;
                    default:
                        continue;
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void HandleCase1()
        {
            var numbers = GenerateAndWriteNumbers();

            Console.WriteLine($"Сгенерировано 10000 случайных чисел, числа записаны в файл {RandomNumbersFileName}.");
            Console.WriteLine();

            double mathExp = CalculateMathematicalExpectation(numbers);
            Console.WriteLine($"Посчитано эмпирическое мат. ожидание: {mathExp.ToString(DoubleValueFormat)}");
            Console.WriteLine();

            double standartDeviation = CalculateStandardDeviation(numbers);
            Console.WriteLine($"Посчитано эмпирическое среднеквадратичное отклонение: {standartDeviation.ToString(DoubleValueFormat)}");
            Console.WriteLine();

            // эти значения просто по формулам считаются
            Console.WriteLine("Теоретические значения:");
            Console.WriteLine($"Мат. ожидание = {0.5.ToString(DoubleValueFormat)}");
            Console.WriteLine($"Среднеквадратичное отклонение = {0.2886d.ToString(DoubleValueFormat)}");
        }

        private static void HandleCase2()
        {
            double[] numbers;
            try
            {
                numbers = ReadNumbersFromFile();
            }
            catch (IOException)
            {
                Console.WriteLine($"Ошибка при чтении из файла {RandomNumbersFileName}.");
                return;
            }
            catch (InvalidCastException)
            {
                Console.WriteLine($"Ошибка формата данных в файле {RandomNumbersFileName}.");
                return;
            }
            catch
            {
                Console.WriteLine($"Неизвестная ошибка Повторите попытку...");
                return;
            }
            Console.WriteLine("Последовательность чисел считана с файла...");
            Console.WriteLine();

            double mathExp = CalculateMathematicalExpectation(numbers);
            Console.WriteLine($"Посчитано эмпирическое мат. ожидание: {mathExp.ToString(DoubleValueFormat)}");
            Console.WriteLine();

            double standartDeviation = CalculateStandardDeviation(numbers);
            Console.WriteLine($"Посчитано эмпирическое среднеквадратичное отклонение: {standartDeviation.ToString(DoubleValueFormat)}");
            Console.WriteLine();

            // эти значения просто по формулам считаются
            Console.WriteLine("Теоретические значения:");
            Console.WriteLine($"Мат. ожидание = {0.5.ToString(DoubleValueFormat)}");
            Console.WriteLine($"Среднеквадратичное отклонение = {0.2886d.ToString(DoubleValueFormat)}");
        }

        private static void HandleCase3()
        {
            var mathExpList = new List<(double val, int size)>();
            var standardDeviationList = new List<(double val, int size)>();
            for (int i = 0, size = 1000; i < 100; i++, size += 1000)
            {
                var numbers = GenerateRandomEnumerables(size).ToArray();

                var mathExp = CalculateMathematicalExpectation(numbers);
                var standardDeviation = CalculateStandardDeviation(numbers);

                mathExpList.Add((mathExp, size));
                standardDeviationList.Add((standardDeviation, size));
            }

            File.WriteAllText(MathematicalExpectationListFileName, string.Join(" ", mathExpList.Select(x => $"({x.val.ToString(DoubleValueFormat)};{x.size})")));
            Console.WriteLine($"Математические ожидания посчитаны и записаны в файл {MathematicalExpectationListFileName}");
            Console.WriteLine();

            File.WriteAllText(StandardDeviationListFileName, string.Join(" ", standardDeviationList.Select(x => $"({x.val.ToString(DoubleValueFormat)};{x.size})")));
            Console.WriteLine($"Среднеквадратичные отклонения посчитаны и записаны в файл {StandardDeviationListFileName}");
            Console.WriteLine();
        }

        private static void HandleCase4()
        {
            var values = ReadNumbersFromFile();

            foreach (var criteria in _criteria)
            {
                var res = criteria.CheckCriteria(values);
                Console.WriteLine($"Критерий \"{criteria.Name}\"");
                Console.WriteLine($"Результат: {(res.isAccepted ? "критерий подтвержден" : "критерий не подтвержден")}. Значение: {res.val}{(criteria is ConflictCriteria ? "; Теоретическое значение: 0.004355" : string.Empty)}");
                Console.WriteLine();
            }
        }

        private static IEnumerable<double> GenerateRandomEnumerables(int size)
        {
            var generator = new Gen.Generators.MersenneVortexGenerator();
            var maxVal = 100000;

            foreach (var random in generator.Generate(Enumerable.Empty<int>(), _random.Next(10, 50), size, maxVal))
            {
                yield return (random + .0) / maxVal;
            }
        }

        private static WorkMode ReadWorkMode()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Выберите режим работы: ");
            Console.ResetColor();
            Console.WriteLine("1. Сгенерировать 10000 чисел и посчитать мат. ожидание и среднеквадратичное отклонение (пункт1),");
            Console.WriteLine($"2. Вычислить мат. ожидание и среднеквадратичное отклонение для чисел из файла {RandomNumbersFileName},");
            Console.WriteLine($"3. Автоматическая генерация чисел и подсчет параметров для выборок разных размеров,");
            Console.WriteLine($"4. Проверка по критериям для заданной выборки,");
            Console.WriteLine("0. Завершить работу программы;");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        return WorkMode.GENERATE_RANDOM_AND_CAL_STAT;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return WorkMode.CALCULATE_MATHEMATICAL_EXPECTATION_AND_STANDART_DEVIATION;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return WorkMode.GENERATION_AND_COMPARATION_VALUES_FOR_GRAPHICS;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        return WorkMode.CHECK_CRITERIA;
                    default:
                        continue;
                }
            }
        }

        private static double[] GenerateAndWriteNumbers()
        {
            var res = new double[10000];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = _random.NextDouble();
            }

            File.WriteAllLines(RandomNumbersFileName, res.Select(x => x.ToString(DoubleValueFormat)).ToArray());
            
            return res;
        }

        // Мат. ожидание
        private static double CalculateMathematicalExpectation(IEnumerable<double> values)
        {
            return values.Sum() / values.Count();
        }

        // Среднеквадратичное отклонение
        private static double CalculateStandardDeviation(IEnumerable<double> values)
        {
            return values.Sum(x => Math.Sqrt((x - 0.5d) * (x - 0.5d))) / values.Count();
        }

        private static double[] ReadNumbersFromFile()
        {
            return File.ReadAllLines(RandomNumbersFileName).Select(x => double.Parse(x)).ToArray();
        }
    }
}
