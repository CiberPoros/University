using System;
using System.IO;
using System.Threading.Tasks;

namespace ProgramsProtection.Common
{
    public class FileModel
    {
        public string FileName { get; set; }

        public byte[] Hash { get; set; }

        public DateTime CreationDate { get; set; }

        public FileModel()
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
            var source = await File.ReadAllBytesAsync(filePath);
            var sha = new CustomSha256();
            var hash = sha.GetHash(source);
            return new FileModel() 
            { 
                FileName = filePath, 
                Hash = hash, 
                CreationDate = fileInfo.CreationTimeUtc 
            };
        }
    }
}
