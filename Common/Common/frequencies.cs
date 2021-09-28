using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common
{
    public static class Frequencies
    {
        public static Dictionary<char, double> CalculateFrequencies(string text)
        {
            var result = new Dictionary<char, double>();

            var counter = new Dictionary<char, long>();

            foreach (var c in text)
            {
                if (!counter.ContainsKey(c))
                {
                    counter.Add(c, 1);

                    continue;
                }

                counter[c]++;
            }

            foreach (var kvp in counter)
            {
                result.Add(kvp.Key, (kvp.Value + .0) / text.Length);
            }

            return result;
        }

        public static Dictionary<char, double> CalculateFrequencies(string[] text, string alpfabet)
        {
            var counter = new Dictionary<char, long>();

            foreach (var c in alpfabet)
            {
                counter.Add(c, 0);
            }

            var cnt = 0L;
            foreach (var s in text)
            {
                foreach (var c in s)
                {
                    if (!counter.ContainsKey(c))
                    {
                        continue;
                    }

                    counter[c]++;
                    cnt++;
                }
            }

            return counter.ToDictionary(x => x.Key, x => (x.Value + .0) / cnt);
        }

        public static Dictionary<char, double> CalculateFrequenciesByBooks(string booksPath, string alpfabet)
        {
            var counter = new Dictionary<char, long>();

            foreach (var c in alpfabet)
            {
                counter.Add(c, 0);
            }

            var cnt = 0L;
            foreach (var filename in Directory.EnumerateFiles(booksPath, "*.txt"))
            {
                string[] text = File.ReadAllLines(filename);
                foreach (string s in text)
                {
                    foreach (var c in s)
                    {
                        if (!counter.ContainsKey(c))
                        {
                            continue;
                        }

                        counter[c]++;
                        cnt++;
                    }
                }
            }

            return counter.ToDictionary(x => x.Key, x => (x.Value + .0) / cnt);
        }
    }
}
