using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace СMIP.Common
{
    public static class BigramsAnylize
    {
        public static Dictionary<string, double> CalcBigramsFrequencies(string booksPath, string alphabet)
        {
            var bigrams = new Dictionary<string, long>();
            foreach (var c1 in alphabet)
            {
                foreach (var c2 in alphabet)
                {
                    bigrams.TryAdd(new StringBuilder().Append(c1).Append(c2).ToString(), 0);
                }
            }

            var alphabetSet = new HashSet<char>(alphabet);
            var cnt = 0L;
            foreach (var filename in Directory.EnumerateFiles(booksPath, "*.txt"))
            {
                string[] text = File.ReadAllLines(filename);
                foreach (string s in text)
                {
                    for (int i = 0; i < s.Length - 1; i++)
                    {
                        var str = s.Substring(i, 2).ToLower();

                        if (!alphabetSet.Contains(str[0]) || !alphabetSet.Contains(str[1]))
                        {
                            continue;
                        }

                        bigrams[str]++;
                        cnt++;
                    }
                }
            }

            return bigrams.ToDictionary(x => x.Key, x => (x.Value + .0) / cnt);
        }

        public static Dictionary<string, double> CalcBigramsFrequenciesByText(IEnumerable<string> text, string alphabet)
        {
            var bigrams = new Dictionary<string, long>();

            foreach (var c1 in alphabet)
            {
                foreach (var c2 in alphabet)
                {
                    bigrams.TryAdd(new StringBuilder().Append(c1).Append(c2).ToString(), 0);
                }
            }

            var alphabetSet = new HashSet<char>(alphabet);
            var cnt = 0L;
            foreach (var s in text)
            {
                for (int i = 0; i < s.Length - 1; i++)
                {
                    var str = s.Substring(i, 2).ToLower();

                    if (!alphabetSet.Contains(str[0]) || !alphabetSet.Contains(str[1]))
                    {
                        continue;
                    }

                    bigrams[str]++;
                    cnt++;
                }
            }

            return bigrams.ToDictionary(x => x.Key, x => (x.Value + .0) / cnt);
        }

        public static string[] DecryptByBigrams(Dictionary<string, double> bigrams, string alphabet, string[] text)
        {
            var currentBigramsCounts = new Dictionary<string, long>();
            foreach (var c1 in alphabet)
            {
                foreach (var c2 in alphabet)
                {
                    currentBigramsCounts.TryAdd(new StringBuilder().Append(c1).Append(c2).ToString(), 0);
                }
            }

            var alphabetSet = new HashSet<char>(alphabet);
            var cnt = 0L;
            foreach (string s in text)
            {
                for (int i = 0; i < s.Length - 1; i++)
                {
                    var str = s.Substring(i, 2).ToLower();

                    if (!alphabetSet.Contains(str[0]) || !alphabetSet.Contains(str[1]))
                    {
                        continue;
                    }

                    currentBigramsCounts[str]++;
                    cnt++;
                }
            }

            var currentBigrams = currentBigramsCounts.ToDictionary(x => x.Key, x => (x.Value + .0) / cnt);
            var map = new Dictionary<char, char>();

            foreach (var c in alphabet)
            {
                map.Add(c, default);
            }

            var bigrams1 = bigrams.Select(x => x.Key).ToList();
            var bigrams2 = currentBigrams.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

            HashSet<char> used = new HashSet<char>();
            for (int i = 0; i < bigrams1.Count; i++)
            {
                if (map.ContainsKey(bigrams2[i][0])
                    && map[bigrams2[i][0]] == default
                    && map.ContainsKey(bigrams2[i][1])
                    && map[bigrams2[i][1]] == default
                    && !used.Contains(bigrams1[i][0])
                    && !used.Contains(bigrams1[i][1]))
                {
                    map[bigrams2[i][0]] = bigrams1[i][0];
                    map[bigrams2[i][1]] = bigrams1[i][1];

                    used.Add(bigrams1[i][0]);
                    used.Add(bigrams1[i][1]);
                }

                if (!map.Any(x => x.Value == default))
                {
                    break;
                }
            }

            return text.Select(x => new string(x.Select(c => map.ContainsKey(c) ? map[c] : c).ToArray())).ToArray();
        }
    }
}
