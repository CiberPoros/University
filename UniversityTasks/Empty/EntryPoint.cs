using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Empty
{
    internal class EntryPoint
    {
        private static readonly string _hiddenCatalogClearName = "hidden";
        private static readonly string _hiddenCatalog = _hiddenCatalogClearName + "\\";
        private static readonly string _readerName = _hiddenCatalog + "PassReader.exe";

        private static string _mainProgName;

        public static void Main()
        {
            string fullName = Assembly.GetEntryAssembly().Location;
            string progName = Path.GetFileNameWithoutExtension(fullName);

            if (!Directory.Exists(_hiddenCatalogClearName))
            {
                DirectoryInfo di = Directory.CreateDirectory(_hiddenCatalogClearName);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();

            _mainProgName = _hiddenCatalog + progName + ".exe";
            var progData = File.ReadAllBytes(progName + ".exe");

            var index1 = -1;
            for (int i = 0; i < progData.Length - 4; i++)
            {
                if (progData[i] == 0xAB && progData[i + 1] == 0xAC && progData[i + 2] == 0xAD && progData[i + 3] == 0xAE && progData[i + 4] == 0xAF)
                {
                    index1 = i + 5;
                    break;
                }
            }

            var index2 = -1;
            for (int i = index1; i < progData.Length - 4; i++)
            {
                if (progData[i] == 0xAB && progData[i + 1] == 0xAC && progData[i + 2] == 0xAD && progData[i + 3] == 0xAE && progData[i + 4] == 0xAF)
                {
                    index2 = i + 5;
                    break;
                }
            }

            var readerData = new byte[index2 - index1 - 5];
            for (int i = index1, j = 0; j < readerData.Length; i++, j++)
            {
                readerData[j] = progData[i];
            }

            var mainProgData = new byte[progData.Length - index2];
            for (int i = index2, j = 0; j < mainProgData.Length; i++, j++)
            {
                mainProgData[j] = progData[i];
            }

            File.WriteAllBytes(_readerName, readerData);
            Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();

            var readerPassProcess = new Process();
            readerPassProcess.StartInfo.FileName = _readerName;
            readerPassProcess.StartInfo.UseShellExecute = false;
            readerPassProcess.StartInfo.RedirectStandardError = true;
            readerPassProcess.Start();
            string pass;
            try
            {
                pass = readerPassProcess.StandardError.ReadLine();
                if (pass != "12345")
                    return;
            }
            catch
            {
                return;
            }

            try
            {
                File.Delete(_readerName);
                Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();
            }
            catch
            {

            }

            try
            {
                File.Delete(_readerName);
                Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();
            }
            catch
            {

            }

            try
            {
                File.WriteAllBytes(_mainProgName, mainProgData);
                Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();
            }
            catch
            {

            }

            try
            {
                File.WriteAllBytes(_mainProgName, mainProgData);
                Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();
            }
            catch
            {

            }

            while (!File.Exists(_mainProgName))
            {
                Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();
            }

            var mainProgProcess = new Process();
            mainProgProcess.StartInfo.FileName = _mainProgName;
            mainProgProcess.Start();

            mainProgProcess.WaitForExit();
            Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();

            try
            {
                File.Delete(_mainProgName);
                Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();
            }
            catch
            {

            }

            try
            {
                File.Delete(_mainProgName);
                Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();
            }
            catch
            {

            }

            File.Delete(_readerName);
            Task.Delay(TimeSpan.FromMilliseconds(50)).GetAwaiter().GetResult();

            Directory.Delete(_hiddenCatalogClearName);
        }
    }
}
