﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProgramsProtection.Common
{
    public class SnapshotsList
    {
        public ICollection<SnapshotShortInfo> Snapshots { get; set; }

        public class SnapshotShortInfo
        {
            public Guid Id { get; set; }

            public string DirectoryPath { get; set; }
        }

        public static async Task<SnapshotsList> ReadFromFileOrCreateAsync()
        {
            if (!File.Exists(PathSettings.SnapshotsListPath))
            {
                return new SnapshotsList() { Snapshots = new List<SnapshotShortInfo>() };
            }

            var json = await File.ReadAllTextAsync(PathSettings.SnapshotsListPath);
            return JsonSerializer.Deserialize<SnapshotsList>(json);
        }

        public async Task WriteToFileAsync()
        {
            var json = JsonSerializer.Serialize(this, options: new JsonSerializerOptions() { WriteIndented = true });
            await File.WriteAllTextAsync(PathSettings.SnapshotsListPath, json);
        }
    }
}
