namespace CodingTheory.Common
{
    public static class PathSettings
    {
        public const string SandBoxFolderPath = @"..\..\..\..\..\..\..\SandBox\ТеорияКодирования";

        public const string SeparationString = "#####";

        #region HAFFMAN
        public const string HaffmanFolderPath = SandBoxFolderPath + "\\" + @"АлгоритмХаффмана";

        public const string HaffmanTextFileName = HaffmanFolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string HaffmanCompressedTextFileName = HaffmanFolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string HaffmanDecompressedTextFileName = HaffmanFolderPath + "\\" + @"РазархивированныйТекст.txt";
        public const string HaffmanFrequenciesFileName = HaffmanFolderPath + "\\" + @"Частоты.txt";
        #endregion

        #region FANO
        public const string FanoFolderPath = SandBoxFolderPath + "\\" + @"АлгоритмФано";

        public const string FanoTextFileName = FanoFolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string FanoCompressedTextFileName = FanoFolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string FanoDecompressedTextFileName = FanoFolderPath + "\\" + @"РазархивированныйТекст.txt";
        public const string FanoFrequenciesFileName = FanoFolderPath + "\\" + @"Частоты.txt";
        #endregion

        #region SHANNON
        public const string ShannonFolderPath = SandBoxFolderPath + "\\" + @"АлгоритмШеннона";

        public const string ShannonTextFileName = ShannonFolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string ShannonCompressedTextFileName = ShannonFolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string ShannonDecompressedTextFileName = ShannonFolderPath + "\\" + @"РазархивированныйТекст.txt";
        public const string ShannonFrequenciesFileName = ShannonFolderPath + "\\" + @"Частоты.txt";
        #endregion

        #region MOVE_TO_FRONT
        public const string MoveToFrontFolderPath = SandBoxFolderPath + "\\" + @"АлгоритмСтопкаКниг";

        public const string MoveToFrontTextFileName = MoveToFrontFolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string MoveToFrontCompressedTextFileName = MoveToFrontFolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string MoveToFrontDecompressedTextFileName = MoveToFrontFolderPath + "\\" + @"РазархивированныйТекст.txt";
        public const string MoveToFrontAlphabetFileName = MoveToFrontFolderPath + "\\" + @"Алфавит.txt";
        #endregion
    }
}
