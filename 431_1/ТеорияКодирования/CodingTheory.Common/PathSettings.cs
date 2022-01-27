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

        #region GILBERT_MURE
        public const string GilbertMureFolderPath = SandBoxFolderPath + "\\" + @"АлгоритмГилбертаМура";

        public const string GilbertMureTextFileName = GilbertMureFolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string GilbertMureCompressedTextFileName = GilbertMureFolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string GilbertMureDecompressedTextFileName = GilbertMureFolderPath + "\\" + @"РазархивированныйТекст.txt";
        public const string GilbertMureFrequenciesFileName = GilbertMureFolderPath + "\\" + @"Частоты.txt";
        #endregion

        #region LZ77
        public const string LZ77FolderPath = SandBoxFolderPath + "\\" + @"АлгоритмLZ77";

        public const string LZ77TextFileName = LZ77FolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string LZ77CompressedTextFileName = LZ77FolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string LZ77DecompressedTextFileName = LZ77FolderPath + "\\" + @"РазархивированныйТекст.txt";
        #endregion

        #region LZ78
        public const string LZ78FolderPath = SandBoxFolderPath + "\\" + @"АлгоритмLZ78";

        public const string LZ78TextFileName = LZ78FolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string LZ78CompressedTextFileName = LZ78FolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string LZ78DecompressedTextFileName = LZ78FolderPath + "\\" + @"РазархивированныйТекст.txt";
        #endregion

        #region RLE
        public const string RLEFolderPath = SandBoxFolderPath + "\\" + @"АлгоритмRLE";

        public const string RLETextFileName = RLEFolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string RLECompressedTextFileName = RLEFolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string RLEDecompressedTextFileName = RLEFolderPath + "\\" + @"РазархивированныйТекст.txt";
        #endregion
    }
}
