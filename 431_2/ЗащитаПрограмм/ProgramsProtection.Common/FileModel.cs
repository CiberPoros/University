using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ProgramsProtection.Common
{
    public class FileModel
    {
        public string FileName { get; set; }

        public byte[] Hash { get; set; }

        public DateTime CreationDate { get; set; }

        private FileModel()
        {

        }

        public static async Task<FileModel> CreateAsync(string filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File was not found", nameof(filePath));
            }

            var fileInfo = new FileInfo(filePath);
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream fileStream = fileInfo.Open(FileMode.Open))
                {
                    try
                    {
                        fileStream.Position = 0;
                        var hash = await sha256.ComputeHashAsync(fileStream);

                        return new FileModel() { FileName = filePath, Hash = hash, CreationDate = fileInfo.CreationTimeUtc };
                    }
                    catch (IOException)
                    {
                        throw;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
