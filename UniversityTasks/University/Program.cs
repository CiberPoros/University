using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HardwareScanner
{
    internal class EntryPoint
    {
        private static readonly string _snapShotFileName = "snapshot";

        public async static Task Main()
        {
            Console.WriteLine("Вычисление текущей аппаратной конфигурации...");
            var hardwareInfo = HardwareInfo.Create();
            Console.WriteLine("Текущая аппаратная конфигурация получена.");
            Console.WriteLine();

            if (File.Exists(_snapShotFileName))
            {
                Console.WriteLine($"Сохраненная информация об аппаратном окружении считана c файла {_snapShotFileName}. Сравнение данных с текущей конфигурацией...");
                var snapshotHash = await File.ReadAllTextAsync(_snapShotFileName);
                await Task.Delay(TimeSpan.FromSeconds(2));
                Console.WriteLine("Сравнение аппаратных конфигураций произведено.");
                Console.WriteLine();

                if (hardwareInfo.GetHashCode().ToString().Equals(snapshotHash))
                {
                    Console.WriteLine("Текущая конфигурация аппаратного окружения совпадает с сохраненной конфигурацией.");
                }
                else
                {
                    Console.WriteLine("Текущая конфигурация аппаратного окружения не совпадает с сохраненной конфигурацией.");
                }

                return;
            }

            var hash = hardwareInfo.GetHashCode();
            await File.WriteAllTextAsync(_snapShotFileName, hash.ToString());

            Console.WriteLine($"Информация о текущем аппаратном окружении успешно сохранена в файл {_snapShotFileName}.");
        }
    }
}