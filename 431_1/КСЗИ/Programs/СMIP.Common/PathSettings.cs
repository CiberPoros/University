﻿namespace СMIP.Common
{
    public static class PathSettings
    {
        private const string SandBoxFolderPath = @"..\..\..\..\..\..\..\SandBox\КСЗИ";
        public const string BooksPath = SandBoxFolderPath + "\\" + "Книги";
        private const string Task2_FolderPath = SandBoxFolderPath + "\\" + "Задание_2";
        private const string Task6_FolderPath = SandBoxFolderPath + "\\" + "Задание_6";

        public const string Task2_CodedText = Task2_FolderPath + "\\" + "Шифрограмма.txt";
        public const string Task2_DecodedText = Task2_FolderPath + "\\" + "РасшифрованныйТекст.txt";
        public const string Task2_Bigrams = Task2_FolderPath + "\\" + "Биграммы.txt";
        public const string Task2_Frequencies = Task2_FolderPath + "\\" + "Частоты.txt";
        public const string Task2_Alphabet = Task2_FolderPath + "\\" + "Алфавит.txt";

        public const string Task6_Alphabet = Task6_FolderPath + "\\" + "Алфавит.txt";
        public const string Task6_Key1 = Task6_FolderPath + "\\" + "Ключ1.txt";
        public const string Task6_Key2 = Task6_FolderPath + "\\" + "Ключ2.txt";
        public const string Task6_KeyResult = Task6_FolderPath + "\\" + "КлючСуперпозиции.txt";
        public const string Task6_OpenText = Task6_FolderPath + "\\" + "Текст.txt";
        public const string Task6_CodedText = Task6_FolderPath + "\\" + "Шифрограмма.txt";
        public const string Task6_DecodedText = Task6_FolderPath + "\\" + "РасшифрованныйТекст.txt";
    }
}
