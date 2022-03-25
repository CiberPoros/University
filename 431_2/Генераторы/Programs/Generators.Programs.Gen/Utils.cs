using Generators.Programs.Gen.Generators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Gen
{
    internal static class Utils
    {
        // mutable
        public static IList<byte> AddLast(IList<byte> target, bool bit, int position)
        {
            var actualLength = Math.Min(target.Count(), position / 8);

            var expectedLength = (int)Math.Ceiling((position + .0) / 8) + 1;
            var additionalByteCount = expectedLength - target.Count;

            for (int i = 0; i < additionalByteCount; i++)
            {
                target.Add(default);
            }

            if (bit)
            {
                target[position / 8] |= (byte)(1 << (position % 8));
            }

            return target;
        }

        public static bool GetBitFromByteArray(IList<byte> source, int index)
        {
            return (source[index / 8] & (1 << (index % 8))) > 0;
        }

        public static IList<byte> GenerateInitialBitVector(int length)
        {
            var result = new byte[(int)Math.Ceiling((length + .0) / 8)];
            for (int i = 0; i < length; i++)
            {
                if (IGenerator.Rnd.Next(2) > 0)
                {
                    result[i / 8] |= (byte)(1 << (7 - (length % 8)));
                }
            }

            return result.ToList();
        }

        public static byte[] ConvertIntArrayToByteArray(int[] source)
        {
            byte[] result = new byte[source.Length * sizeof(int)];
            Buffer.BlockCopy(source, 0, result, 0, result.Length);

            return result;
        }
    }
}
