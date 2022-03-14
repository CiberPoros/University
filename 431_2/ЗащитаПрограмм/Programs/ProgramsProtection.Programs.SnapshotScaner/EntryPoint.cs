using System;
using System.Collections.Generic;
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

                var result = CompareSnapshots(oldSnapshot, newSnapshot);

                foreach (var info in result)
                {
                    Console.WriteLine(info);
                }
                Console.WriteLine();
            }
        }

        private static List<string> CompareSnapshots(Snapshot oldSnapshot, Snapshot newSnapshot)
        {
            var result = new List<string>();

            var oldDirectories = oldSnapshot.Directory.GetAllDirectories().ToList();
            var newDirectories = newSnapshot.Directory.GetAllDirectories().ToList();
            foreach (var oldDirectory in oldDirectories)
            {
                var newDirectory = newDirectories.FirstOrDefault(x => x.CreationDate == oldDirectory.CreationDate);
                if (newDirectory is not null)
                {
                    if (newDirectory.DirectoryName != oldDirectory.DirectoryName)
                    {
                        result.Add($"Директория {oldDirectory.DirectoryName} была переименована в {newDirectory.DirectoryName}.");
                    }

                    continue;
                }

                result.Add($"Директория {oldDirectory.DirectoryName} была удалена.");
            }

            foreach (var newDirectory in newDirectories)
            {
                if (oldDirectories.Any(x => x.CreationDate == newDirectory.CreationDate))
                {
                    continue;
                }

                result.Add($"Добавлена новая директория {newDirectory.DirectoryName}.");
            }

            var oldFiles = oldSnapshot.Directory.GetAllFiles().ToList();
            var newFiles = newSnapshot.Directory.GetAllFiles().ToList();
            foreach (var oldFile in oldFiles)
            {
                var newFile = newFiles.FirstOrDefault(x => x.FileName == oldFile.FileName && x.CreationDate == oldFile.CreationDate);
                if (newFile is not null)
                {
                    if (!newFile.Hash.SequenceEqual(oldFile.Hash))
                    {
                        result.Add($"Файл {oldFile.FileName} был изменен.");
                    }

                    continue;
                }

                var newFileByHash = newFiles.FirstOrDefault(x => x.Hash.SequenceEqual(oldFile.Hash) && x.CreationDate == oldFile.CreationDate);
                if (newFileByHash is not null)
                {
                    result.Add($"Файл {oldFile.FileName} был переименован в {newFileByHash.FileName}.");
                    continue;
                }

                var newFileByCreationDate = newFiles.FirstOrDefault(x => x.CreationDate == oldFile.CreationDate);
                if (newFileByCreationDate is not null)
                {
                    result.Add($"Файл {oldFile.FileName} был изменен и переименован в {newFileByCreationDate.FileName}.");
                    continue;
                }

                result.Add($"Файл {oldFile.FileName} был удален.");
            }

            foreach (var newFile in newFiles)
            {
                if (oldFiles.Any(x => x.CreationDate == newFile.CreationDate))
                {
                    continue;
                }

                result.Add($"Добавлен новый файл {newFile.FileName}.");
            }

            return result;
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
