using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;
using СMIP.Common;

namespace CMIP.Programs.Task2
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
                    case WorkMode.CREATE_BIGRAMS:
                        MakeBigramsAndSave();
                        break;
                    case WorkMode.CREATE_FREQUENCIES:
                        MakeFrequencies();
                        break;
                    case WorkMode.DECRYPT:
                        Decrypt();
                        break;
                    case WorkMode.CLOSE_PROGRAM:
                        return;
                    case WorkMode.NONE:
                        break;
                }
            }
        }

        private static WorkMode ReadWorkMode()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Создать множество биграмм с соответствующими эталонными частотами;");
            Console.WriteLine("2 - Посчитать эталонные частоты символов;");
            Console.WriteLine("3 - Расшифровать криптограмму;");
            Console.WriteLine("0 - Завершить работу программы...");

            for (; ; )
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return WorkMode.CREATE_BIGRAMS;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return WorkMode.CREATE_FREQUENCIES;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return WorkMode.DECRYPT;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return WorkMode.CLOSE_PROGRAM;
                    default:
                        break;
                }
            }
        }

        private static void Decrypt()
        {
            var alphabet = File.ReadAllText(PathSettings.Task2_Alphabet);

            var frequenciesInput = File.ReadAllLines(PathSettings.Task2_Frequencies);
            var etalonFrequencies = frequenciesInput.ToDictionary(x => x[0], x => double.Parse(x[2..]));

            var bigramsInput = File.ReadAllLines(PathSettings.Task2_Bigrams);
            var etalonBigrams = bigramsInput.ToDictionary(x => x.Substring(0, 2), x => double.Parse(x[3..]));

            var text = File.ReadAllLines(PathSettings.Task2_CodedText).Select(x => x.ToLower()).ToArray();
            var frequencies = Frequencies.CalculateFrequencies(text, alphabet);

            var freq1 = etalonFrequencies.OrderByDescending(x => x.Value).Select(x => x.Key).ToArray();
            var freq2 = frequencies.OrderByDescending(x => x.Value).Select(x => x.Key).ToArray();

            var map = new List<DoubleChar>();
            for (int i = 0; i < freq1.Length; i++)
            {
                map.Add(new DoubleChar() { First = freq1[i], Second = freq2[i] });
            }

            var alphabetSet = new HashSet<char>(alphabet);

            var decodedText = DecodeText(text, alphabetSet, map);
            var bigrams = BigramsAnylize.CalcBigramsFrequenciesByText(decodedText, alphabet);
            var deviation = etalonBigrams.Select(x => (bigrams[x.Key] - x.Value) * (bigrams[x.Key] - x.Value)).Sum();

            var keys = alphabetSet.ToArray();
            bool haveChange = false;
            do
            {
                haveChange = false;

                for (int i = 0; i < keys.Length - 1; i++)
                {
                    var haveChangeInternal = false;
                    do
                    {
                        haveChangeInternal = false;

                        for (int j = i + 1; j < keys.Length; j++)
                        {
                            var temp = map[i].Second;
                            map[i].Second = map[j].Second;
                            map[j].Second = temp;

                            decodedText = DecodeText(text, alphabetSet, map);
                            var currentBigrams = BigramsAnylize.CalcBigramsFrequenciesByText(decodedText, alphabet);
                            var currentDeviation = etalonBigrams.Select(x => (currentBigrams[x.Key] - x.Value) * (currentBigrams[x.Key] - x.Value)).Sum();

                            if (currentDeviation < deviation)
                            {
                                haveChangeInternal = true;
                                haveChange = true;
                                deviation = currentDeviation;
                                bigrams = currentBigrams;
                                continue;
                            }

                            temp = map[i].Second;
                            map[i].Second = map[j].Second;
                            map[j].Second = temp;
                        }
                    }
                    while (haveChangeInternal);
                }
            }
            while (haveChange);

            decodedText = DecodeText(text, alphabetSet, map);

            File.WriteAllLines(PathSettings.Task2_DecodedText, decodedText);
            Console.WriteLine($"Шифрограмма была расшифрована и записана в файл \"{Path.GetFileName(PathSettings.Task2_DecodedText)}\"");
            Console.WriteLine();
        }

        private static string[] DecodeText(string[] text, HashSet<char> alphabet, List<DoubleChar> map)
        {   
            var mapInternal = map.ToDictionary(x => x.First, x => x.Second);

            var result = new List<string>();
            foreach (var s in text)
            {
                var sb = new StringBuilder();
                foreach (var c in s)
                {
                    sb.Append(mapInternal.ContainsKey(c) ? mapInternal[c] : c);
                }

                result.Add(sb.ToString());
            }

            return result.ToArray();
        }

        private static void MakeFrequencies()
        {
            var alphabet = File.ReadAllText(PathSettings.Task2_Alphabet);
            var frequencies = Frequencies.CalculateFrequenciesByBooks(PathSettings.BooksPath, alphabet);

            File.WriteAllLines(PathSettings.Task2_Frequencies, frequencies.OrderByDescending(x => x.Value).Select(x => $"{x.Key} {x.Value}"));

            Console.WriteLine($"Частоты подсчитаны и сохранены в файл \"{Path.GetFileName(PathSettings.Task2_Frequencies)}\"");
            Console.WriteLine();
        }

        private static void DecryptByFrequencies()
        {
            var alphabet = File.ReadAllText(PathSettings.Task2_Alphabet);
            var text = File.ReadAllLines(PathSettings.Task2_CodedText).Select(x => x.ToLower()).ToArray();
            var frequenciesInput = File.ReadAllLines(PathSettings.Task2_Frequencies);

            var frequencies = frequenciesInput.ToDictionary(x => x[0], x => double.Parse(x[2..]));
            var currentFrequencies = Frequencies.CalculateFrequencies(text, alphabet);

            var freq1 = frequencies.OrderByDescending(x => x.Value).Select(x => x.Key).ToArray();
            var freq2 = currentFrequencies.OrderByDescending(x => x.Value).Select(x => x.Key).ToArray();

            var map = new Dictionary<char, char>();

            for (int i = 0; i < freq1.Length; i++)
            {
                map.Add(freq2[i], freq1[i]);
            }

            var decodedText = text.Select(x => new string(x.Select(c => map.ContainsKey(c) ? map[c] : c).ToArray()));

            File.WriteAllLines(PathSettings.Task2_DecodedText, decodedText);

            Console.WriteLine($"Шифрограмма была расшифрована и записана в файл \"{Path.GetFileName(PathSettings.Task2_DecodedText)}\"");
            Console.WriteLine();
        }

        private static void MakeBigramsAndSave()
        {
            var alphabet = File.ReadAllText(PathSettings.Task2_Alphabet);
            var bigrams = BigramsAnylize.CalcBigramsFrequencies(PathSettings.BooksPath, alphabet);

            File.WriteAllLines(PathSettings.Task2_Bigrams, bigrams.OrderByDescending(x => x.Value).Select(x => $"{x.Key} {x.Value}"));

            Console.WriteLine($"Множество биграмм с соответствующими частотами создано и сохранено в файл \"{Path.GetFileName(PathSettings.Task2_Bigrams)}\"");
            Console.WriteLine();
        }

        private static void DecryptByBigrams()
        {
            var alphabet = File.ReadAllText(PathSettings.Task2_Alphabet);
            var text = File.ReadAllLines(PathSettings.Task2_CodedText).Select(x => x.ToLower()).ToArray();
            var bigramsInput = File.ReadAllLines(PathSettings.Task2_Bigrams);

            var bigrams = bigramsInput.ToDictionary(x => x.Substring(0, 2), x => double.Parse(x[3..]));

            var decodedText = BigramsAnylize.DecryptByBigrams(bigrams, alphabet, text);

            File.WriteAllLines(PathSettings.Task2_DecodedText, decodedText);

            Console.WriteLine($"Шифрограмма была расшифрована и записана в файл \"{Path.GetFileName(PathSettings.Task2_DecodedText)}\"");
            Console.WriteLine();
        }
    }
}
