using System.Collections.Generic;

namespace CodingTheory.Common
{
    public class CompressionMoveToFront
    {
        public IEnumerable<int> Compress(string text, string alphabet)
        {
            var alphabetList = new LinkedList<char>(alphabet);

            foreach (var currentChar in text)
            { 
                var node = alphabetList.First;
                var index = 0;

                while (node.Value != currentChar)
                {
                    node = node.Next;
                    index++;
                }

                alphabetList.Remove(node);
                alphabetList.AddFirst(currentChar);

                yield return index;
            }
        }

        public IEnumerable<char> Decompress(string alphabet, IEnumerable<int> code)
        {
            var alphabetList = new LinkedList<char>(alphabet);

            foreach (var number in code)
            {
                var node = alphabetList.First;

                for (int i = 0; i < number; i++)
                {
                    node = node.Next;
                }

                var currentChar = node.Value;
                alphabetList.Remove(node);
                alphabetList.AddFirst(currentChar);

                yield return currentChar;
            }
        }
    }
}
