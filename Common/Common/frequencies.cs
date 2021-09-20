using System.Collections.Generic;

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
    }
}
