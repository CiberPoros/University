﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
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
                var snapshotBinary = await File.ReadAllBytesAsync(_snapShotFileName);
                var snapshotJson = Encoding.Unicode.GetString(snapshotBinary);
                var snapshot = JsonConvert.DeserializeObject<HardwareInfo>(snapshotJson);
                await Task.Delay(TimeSpan.FromSeconds(2));
                Console.WriteLine("Сравнение аппаратных конфигураций произведено.");
                Console.WriteLine();

                var compareResult = hardwareInfo.CompareInfo(snapshot);

                if (!compareResult.Any())
                {
                    Console.WriteLine("Текущая конфигурация аппаратного окружения совпадает с сохраненной конфигурацией.");
                }
                else
                {
                    Console.WriteLine("Текущая конфигурация аппаратного окружения не совпадает с сохраненной конфигурацией. Подробности:");

                    foreach (var ci in compareResult)
                    {
                        Console.WriteLine(ci.Key);

                        foreach (var item in ci.Value)
                        {
                            Console.WriteLine(item);
                        }

                        Console.WriteLine();
                    }
                }

                return;
            }

            string json = JsonConvert.SerializeObject(hardwareInfo);
            var binaryData = Encoding.Unicode.GetBytes(json);

            await File.WriteAllBytesAsync(_snapShotFileName, binaryData);

            Console.WriteLine($"Информация о текущем аппаратном окружении успешно сохранена в файл {_snapShotFileName}.");
        }
    }
}