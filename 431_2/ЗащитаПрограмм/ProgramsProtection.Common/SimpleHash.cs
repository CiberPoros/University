using System;
using System.Linq;

namespace ProgramsProtection.Common
{
    internal class SimpleHash
    {
        public byte[] GetHash(byte[] source)
        {
            var size = source.Length / sizeof(int);
            var ints = new int[size];
            for (var index = 0; index < size; index++)
            {
                ints[index] = BitConverter.ToInt32(source, index * sizeof(int));
            }

            var result = ints.FirstOrDefault();

            for (int i = 1; i < ints.Length; i++)
            {
                result ^= ints[i];
            }

            return BitConverter.GetBytes(result);
        }
    }
}
