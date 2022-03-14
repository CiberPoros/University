namespace ProgramsProtection.Common
{
    public static class PathSettings
    {
        private const string SandBoxFolderPath = @"..\..\..\..\..\..\..\SandBox\ЗащитаПрограмм";
        public const string DirectoryScanerPath = SandBoxFolderPath + "\\" + "DirectoryScaner";
        public const string SnapshotsListPath = DirectoryScanerPath + "\\" + "ShanshotsInfo.txt";
    }
}
