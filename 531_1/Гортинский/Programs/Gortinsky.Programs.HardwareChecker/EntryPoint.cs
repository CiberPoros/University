using System;
using System.IO;
using System.Threading.Tasks;

namespace HardwareScanner
{
    internal class EntryPoint
    {
        private static readonly string _configurationFileName = "configuration";

        public async static Task Main()
        {
            Console.WriteLine("Вычисление данных об аппаратном окружении...");
            Console.WriteLine();
            var configuration = HardwareInfo.GetHardwareInfoString();

            if (File.Exists(_configurationFileName))
            {
                Console.WriteLine($"Сохраненная конфигурация считана. Сравнение данных с текущей конфигурацией...");
                Console.WriteLine();
                var oldConfiguration = await File.ReadAllTextAsync(_configurationFileName);

                if (configuration.Equals(oldConfiguration))
                {
                    Console.WriteLine("Изменений в аппаратном окрежении не найдено.");
                }
                else
                {
                    Console.WriteLine("Аппаратное окружение было изменено.");
                }

                return;
            }

            await File.WriteAllTextAsync(_configurationFileName, configuration);
            Console.WriteLine($"Информация о аппаратном окружении сохранена в файл {_configurationFileName}.");
        }
    }
}