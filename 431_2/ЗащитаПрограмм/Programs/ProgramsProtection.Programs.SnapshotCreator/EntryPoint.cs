using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsProtection.Common;

namespace ProgramsProtection.Programs.SnapshotCreator
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
                if (snapshotsList.Snapshots.Any(x => x.DirectoryPath == directoryPath))
                {
                    if (!NeedReWriteSnapshot())
                    {
                        continue;
                    }
                }

                var snapshot = await Snapshot.CreateAsync(directoryPath);
                foreach (var sn in snapshotsList.Snapshots.Where(x => x.DirectoryPath == directoryPath))
                {
                    File.Delete(sn.DirectoryPath);
                }
                snapshotsList.Snapshots = snapshotsList.Snapshots.Where(x => x.DirectoryPath != directoryPath).ToList();
                snapshotsList.Snapshots.Add(new SnapshotsList.SnapshotShortInfo() { DirectoryPath = directoryPath, Id = snapshot.Id });
                await snapshotsList.WriteToFileAsync();
                await snapshot.WriteToFileAsync();

                Console.WriteLine("Слепок успешно создан!");
                Console.WriteLine();
            }
        }

        private static bool NeedReWriteSnapshot()
        {
            Console.WriteLine("Слепок данной директории уже существует. Перезаписать текущий слепок? (y/n)");
            Console.WriteLine();
            
            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Y:
                        return true;
                    case ConsoleKey.N:
                        return false;
                    default:
                        continue;
                }
            }
        }

        private static string ReadDirectoryPath()
        {
            Console.WriteLine("Введите полный путь к директории для создания слепка...");
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
