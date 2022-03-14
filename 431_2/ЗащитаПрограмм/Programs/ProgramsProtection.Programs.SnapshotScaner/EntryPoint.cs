using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsProtection.Common;

namespace ProgramsProtection.Programs.SnapshotScaner
{
    internal class EntryPoint
    {
        static async Task Main()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            for (; ; )
            {
                var directoryPath = ReadDirectoryPath();
                var snapshotsList = await SnapshotsList.ReadFromFileOrCreateAsync();

                var snapshotInfo = snapshotsList.Snapshots.FirstOrDefault(x => x.DirectoryPath == directoryPath);

                if (snapshotInfo is null)
                {
                    Console.WriteLine("Слепка данной директории нет в базе данных.");
                    Console.WriteLine();
                    continue;
                }

                var oldSnapshot = await Snapshot.ReadFromFileAsync(snapshotInfo.Id);
                var newSnapshot = await Snapshot.CreateAsync(directoryPath);
            }
        }

        private static string CompareSnapshots(Snapshot oldSnapshot, Snapshot newSnapshot)
        {

        }

        private static string ReadDirectoryPath()
        {
            Console.WriteLine("Введите полный путь к директории для отслеживания изменений...");
            Console.WriteLine();

            for (; ; )
            {
                var directoryPath = Console.ReadLine();
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    Console.WriteLine("Такой директории не существует. Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }

                return directoryPath;
            }
        }
    }
}
