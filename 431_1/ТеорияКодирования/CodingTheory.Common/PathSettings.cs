namespace CodingTheory.Common
{
    public static class PathSettings
    {
        public const string SandBoxFolderPath = @"..\..\..\..\..\..\..\SandBox\ТеорияКодирования";
        public const string HaffmanFolderPath = SandBoxFolderPath + "\\" + @"АлгоритмХаффмана";

        public const string SeparationString = "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$";

        public const string HaffmanTextFileName = HaffmanFolderPath + "\\" + @"ИсходныйТекст.txt";
        public const string HaffmanCompressedTextFileName = HaffmanFolderPath + "\\" + @"АрхивированныйТекст.txt";
        public const string HaffmanDecompressedTextFileName = HaffmanFolderPath + "\\" + @"РазархивированныйТекст.txt";
        public const string HaffmanFrequenciesFileName = HaffmanFolderPath + "\\" + @"Частоты.txt";
        public const string HaffmanEncodingMapFileName = HaffmanFolderPath + "\\" + @"СловарьКодов.txt";
    }
}
