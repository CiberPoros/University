using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProgramsProtection.Common
{
    public class Snapshot
    {
        public Guid Id { get; set; }

        public string Path { get; set; }

        public DirectoryModel Directory { get; set; }

        public DateTime CreationDate { get; set; }

        private Snapshot()
        {

        }

        public static async Task<Snapshot> CreateAsync(string directoryPath)
        {
            if (directoryPath is null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (!System.IO.Directory.Exists(directoryPath))
            {
                throw new ArgumentException("Directory was not found", nameof(directoryPath));
            }

            var directory = await DirectoryModel.CreateWithRecursionAsync(directoryPath);

            return new Snapshot()
            {
                Id = Guid.NewGuid(),
                Path = directoryPath,
                Directory = directory,
                CreationDate = DateTime.Now
            };
        }

        public async Task WriteToFileAsync()
        {
            var json = JsonSerializer.Serialize(this, options: new JsonSerializerOptions() { WriteIndented = true });
            await File.WriteAllTextAsync($"{Id}.txt", json);
        }

        public static async Task<Snapshot> ReadFromFileAsync(Guid id)
        {
            var fileName = $"{id}.txt";
            var json = await File.ReadAllTextAsync(fileName);
            return JsonSerializer.Deserialize<Snapshot>(json);
        }
    }
}
