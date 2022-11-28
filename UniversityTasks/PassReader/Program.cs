using System;

namespace PassReader
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите пароль для разблокировки приложения:");
            while (true)
            {
                var pass = Console.ReadLine();
                if (string.IsNullOrEmpty(pass) || pass != "12345")
                {
                    Console.WriteLine("Пароль неверен! Повторите попытку...");
                    continue;
                }

                Console.Error.WriteLine(pass);
                return;
            }
        }
    }
}
