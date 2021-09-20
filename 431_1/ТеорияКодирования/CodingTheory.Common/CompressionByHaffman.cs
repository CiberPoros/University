using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodingTheory.Common
{
    public class CompressionByHaffman
    {
        private readonly Dictionary<char, double> _frequencies;

        private Dictionary<char, string> _codes;
        private Dictionary<string, char> _codesInverted;

        public CompressionByHaffman(Dictionary<char, double> frequencies)
        {
            _frequencies = frequencies;

            MakeCodes();
        }

        public string Compress(string text)
        {
            var sb = new StringBuilder();

            foreach (var c in text)
            {
                sb.Append(_codes[c]);
            }

            return sb.ToString();
        }

        public string DeCompress(string compressedText)
        {
            var sb = new StringBuilder();
            var currentString = string.Empty;

            for (int i = 0; i < compressedText.Length; i++)
            {
                currentString += compressedText[i];

                if (_codesInverted.TryGetValue(currentString, out var value))
                {
                    sb.Append(value);
                    currentString = string.Empty;
                }
            }

            return sb.ToString();
        }

        private void MakeCodes()
        {
            _codes = new Dictionary<char, string>();
            _codesInverted = new Dictionary<string, char>();

            if (_frequencies.Count == 1)
            {
                _codes.Add(_frequencies.First().Key, "0");
                _codesInverted.Add("0", _frequencies.First().Key);

                return;
            }

            var tree = new SortedSet<Node<(double freq, string value)>>();
            foreach (var kvp in _frequencies)
            {
                tree.Add(new Node<(double freq, string value)>((kvp.Value, kvp.Key.ToString())));
            }

            while (tree.Count > 1)
            {
                var first = tree.Min;
                tree.Remove(first);

                var second = tree.Min;
                tree.Remove(second);

                var newNode = new Node<(double freq, string value)>((first.Value.freq + second.Value.freq, first.Value.value + second.Value.value))
                {
                    Left = first,
                    Right = second
                };

                tree.Add(newNode);
            }

            _codes = Dfs(tree.Min);
            _codesInverted = _codes.ToDictionary(x => x.Value, x => x.Key);
        }

        private static Dictionary<char, string> Dfs(Node<(double freq, string value)> root)
        {
            var result = new Dictionary<char, string>();

            DfsInternal(root, new StringBuilder(), result);

            return result;

            static void DfsInternal(Node<(double freq, string value)> currentNode, StringBuilder currentCode, Dictionary<char, string> codes)
            {
                if (currentNode.Left is null && currentNode.Right is null)
                {
                    codes.Add(currentNode.Value.value.First(), currentCode.ToString());
                    return;
                }

                currentCode.Append('0');
                DfsInternal(currentNode.Left, currentCode, codes);
                currentCode.Remove(currentCode.Length - 1, 1);

                currentCode.Append('1');
                DfsInternal(currentNode.Right, currentCode, codes);
                currentCode.Remove(currentCode.Length - 1, 1);
            }
        }

        private class Node<T> : IComparable<Node<T>> where T : IComparable<T>
        {
            public Node(T value)
            {
                Value = value;
            }

            public Node<T> Left { get; set; }
            public Node<T> Right { get; set; }

            public T Value { get; set; }

            public int CompareTo(Node<T> other) => Value.CompareTo(other.Value);
        }
    }
}
