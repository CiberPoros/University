using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProgramsProtection.Common
{
    public class DirectoryModel
    {
        public string DirectoryName { get; set; }

        public ICollection<DirectoryModel> InternalDirectories { get; set; }

        public ICollection<FileModel> InternalFiles { get; set; }

        public DateTime CreationDate { get; set; }

        public DirectoryModel()
        {

        }

        public IEnumerable<DirectoryModel> GetAllDirectories()
        {
            yield return this;

            foreach (var directory in InternalDirectories)
            {
                foreach (var internalDirectory in directory.GetAllDirectories())
                {
                    yield return internalDirectory;
                }
            }
        }

        public IEnumerable<FileModel> GetAllFiles()
        {
            foreach (var file in InternalFiles)
            {
                yield return file;
            }

            foreach (var directory in InternalDirectories)
            {
                foreach (var file in directory.GetAllFiles())
                {
                    yield return file;
                }
            }
        }

        public static async Task<DirectoryModel> CreateWithRecursionAsync(string directoryPath)
        {
            if (directoryPath is null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentException("Directory was not found", nameof(directoryPath));
            }

            var result = new DirectoryModel()
            {
                DirectoryName = directoryPath,
                CreationDate = Directory.GetCreationTimeUtc(directoryPath),
                InternalDirectories = new List<DirectoryModel>(),
                InternalFiles = new List<FileModel>()
            };

            var internalDirectories = Directory.GetDirectories(directoryPath);
            foreach (var internalDirectory in internalDirectories)
            {
                result.InternalDirectories.Add(await CreateWithRecursionAsync(internalDirectory));
            }

            var internalFiles = Directory.GetFiles(directoryPath);
            foreach (var internalFile in internalFiles)
            {
                result.InternalFiles.Add(await FileModel.CreateAsync(internalFile));
            }

            return result;
        }
    }
}
