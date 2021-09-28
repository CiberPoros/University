namespace СMIP.Common
{
    public static class PathSettings
    {
        private const string SandBoxFolderPath = @"..\..\..\..\..\..\..\SandBox\КСЗИ";
        public const string BooksPath = SandBoxFolderPath + "\\" + "Книги";
        private const string Task2_FolderPath = SandBoxFolderPath + "\\" + "Задание_2";

        public const string Task2_CodedText = Task2_FolderPath + "\\" + "Шифрограмма.txt";
        public const string Task2_DecodedText = Task2_FolderPath + "\\" + "РасшифрованныйТекст.txt";
        public const string Task2_Bigrams = Task2_FolderPath + "\\" + "Биграммы.txt";
        public const string Task2_Frequencies = Task2_FolderPath + "\\" + "Частоты.txt";
        public const string Task2_Alphabet = Task2_FolderPath + "\\" + "Алфавит.txt";
    }
}
