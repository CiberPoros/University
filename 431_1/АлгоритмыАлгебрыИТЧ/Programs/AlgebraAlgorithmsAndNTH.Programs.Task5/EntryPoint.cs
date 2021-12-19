using System;
using System.Text;
using AlgebraAlgorithmsAndNTH.Common;
using Common.PolinomMath;

namespace AlgebraAlgorithmsAndNTH.Programs.Task5
{
    class EntryPoint
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            for (; ; )
            {
                var mode = ReadWorkMode();

                switch (mode)
                {
                    case WorkMode.DEVIDE:
                        Devide();
                        break;
                    case WorkMode.GCD:
                        GCD();
                        break;
                    case WorkMode.CLOSE_PROGRAM:
                        return;
                }
            }
        }

        private static void Devide()
        {
            var divisible = IOUtils.ReadPolinomFromConsole("делимое");
            var divider = IOUtils.ReadPolinomFromConsole("делитель");

            (var result, var remainder) = divisible.Divide(divider);

            Console.WriteLine($"Результат деления: {result}");
            Console.WriteLine($"Остаток от деления: {remainder}");
            Console.WriteLine();
        }

        private static void GCD()
        {
            var left = IOUtils.ReadPolinomFromConsole("");
            var right = IOUtils.ReadPolinomFromConsole("");

            var result = PolinomDouble.GCD(left, right);

            Console.WriteLine($"Наибольший общий делитель: {result}");
            Console.WriteLine();
        }

        private static WorkMode ReadWorkMode()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Деление полиномов;");
            Console.WriteLine("2 - Нахождение наибольшего общего делителя;");
            Console.WriteLine("0 - Завершить работу программы...");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return WorkMode.DEVIDE;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return WorkMode.GCD;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return WorkMode.CLOSE_PROGRAM;
                    default:
                        break;
                }
            }
        }
    }
}
