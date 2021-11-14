using System;
using System.IO;
using System.Linq;
using СMIP.Common;

namespace CMIP.Programs.Task6
{
    class EntryPoint
    {
        static void Main()
        {
            for (; ; )
            {
                var mode = ReadWorkMode();

                switch (mode)
                {
                    case WorkMode.ENCRYPT_KEY1:
                        File.WriteAllText(PathSettings.Task6_CodedText, Encrypt(ReadText(), ReadAlphabet(), ReadKey1()));
                        Console.WriteLine($"Текст зашифрован ключом 1 и записан в файл {Path.GetFileName(PathSettings.Task6_CodedText)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.ENCRYPT_KEY2:
                        File.WriteAllText(PathSettings.Task6_CodedText, Encrypt(ReadText(), ReadAlphabet(), ReadKey2()));
                        Console.WriteLine($"Текст зашифрован ключом 2 и записан в файл {Path.GetFileName(PathSettings.Task6_CodedText)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.CALCULATE_SUPER_KEY:
                        File.WriteAllText(PathSettings.Task6_KeyResult, CalcSuperKey(ReadAlphabet(), ReadKey1(), ReadKey2()));
                        Console.WriteLine($"Супер ключ посчитан и записан в файл {Path.GetFileName(PathSettings.Task6_KeyResult)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.ENCRYPT_SUPER_KEY:
                        File.WriteAllText(PathSettings.Task6_CodedText, Encrypt(ReadText(), ReadAlphabet(), ReadSuperKey()));
                        Console.WriteLine($"Текст зашифрован супер ключом и записан в файл {Path.GetFileName(PathSettings.Task6_CodedText)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.DECRYPT_KEY1:
                        File.WriteAllText(PathSettings.Task6_DecodedText, Decrypt(ReadCodedText(), ReadAlphabet(), ReadKey1()));
                        Console.WriteLine($"Текст расшифрован ключом 1 и записан в файл {Path.GetFileName(PathSettings.Task6_DecodedText)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.DECRYPT_KEY2:
                        File.WriteAllText(PathSettings.Task6_DecodedText, Decrypt(ReadCodedText(), ReadAlphabet(), ReadKey2()));
                        Console.WriteLine($"Текст расшифрован ключом 2 и записан в файл {Path.GetFileName(PathSettings.Task6_DecodedText)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.DECRYPT_SUPER_KEY:
                        File.WriteAllText(PathSettings.Task6_DecodedText, Decrypt(ReadCodedText(), ReadAlphabet(), ReadSuperKey()));
                        Console.WriteLine($"Текст расшифрован супер ключом и записан в файл {Path.GetFileName(PathSettings.Task6_DecodedText)}");
                        Console.WriteLine();
                        break;
                    case WorkMode.CLOSE_PROGRAM:
                        return;
                }
            }
        }

        private static string CalcSuperKey(string alphabet, string key1, string key2)
        {
            return new string(alphabet.Select((x, index) => key2[alphabet.ToList().IndexOf(key1[index])]).ToArray());
        }

        private static string Encrypt(string text, string alphabet, string key)
        {
            return new string(text.Select(x => key[alphabet.ToList().IndexOf(x)]).ToArray());
        }

        private static string Decrypt(string text, string alphabet, string key)
        {
            return new string(text.Select(x => alphabet[key.ToList().IndexOf(x)]).ToArray());
        }

        private static string ReadAlphabet()
        {
            return File.ReadAllText(PathSettings.Task6_Alphabet).ToLower();
        }

        private static string ReadText()
        {
            return File.ReadAllText(PathSettings.Task6_OpenText).ToLower();
        }

        private static string ReadCodedText()
        {
            return File.ReadAllText(PathSettings.Task6_CodedText).ToLower();
        }

        private static string ReadKey1()
        {
            return File.ReadAllText(PathSettings.Task6_Key1).ToLower();
        }

        private static string ReadKey2()
        {
            return File.ReadAllText(PathSettings.Task6_Key2).ToLower();
        }

        private static string ReadSuperKey()
        {
            return File.ReadAllText(PathSettings.Task6_KeyResult).ToLower();
        }

        private static WorkMode ReadWorkMode()
        {
            Console.WriteLine($"1 - Зашифровать текст в файле {Path.GetFileName(PathSettings.Task6_OpenText)} первым ключом,");
            Console.WriteLine($"2 - Зашифровать текст в файле {Path.GetFileName(PathSettings.Task6_OpenText)} вторым ключом,");
            Console.WriteLine($"3 - Посчитать ключ SuperKey такой, что шифрование этим ключом даст результат, эквивалентный шифрованию ключом 1 а затем ключом 2,");
            Console.WriteLine($"4 - Зашифровать текст в файле {Path.GetFileName(PathSettings.Task6_OpenText)} SuperKey ключом,");
            Console.WriteLine($"5 - Дешифровать текст в файле {Path.GetFileName(PathSettings.Task6_CodedText)} первым ключом,");
            Console.WriteLine($"6 - Дешифровать текст в файле {Path.GetFileName(PathSettings.Task6_CodedText)} вторым ключом,");
            Console.WriteLine($"7 - Дешифровать текст в файле {Path.GetFileName(PathSettings.Task6_CodedText)} SuperKey ключом,");
            Console.WriteLine($"0 - Завершить работу программы;");
            Console.WriteLine();

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return WorkMode.ENCRYPT_KEY1;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return WorkMode.ENCRYPT_KEY2;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return WorkMode.CALCULATE_SUPER_KEY;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        return WorkMode.ENCRYPT_SUPER_KEY;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        return WorkMode.DECRYPT_KEY1;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        return WorkMode.DECRYPT_KEY2;
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        return WorkMode.DECRYPT_SUPER_KEY;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return WorkMode.CLOSE_PROGRAM;
                    default:
                        break;
                }
            }
        }
    }
}
