using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Utils
    {    
        public static byte[] ConvertBitsSequenceToByteArray(string bits)
        {
            var result = new List<byte>();

            if (string.IsNullOrWhiteSpace(bits))
            {
                return result.ToArray();
            }

            byte currentByte = 0;
            byte mask = 1;

            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == '1')
                {
                    currentByte |= mask;
                }

                mask <<= 1;

                if (i % 8 == 7)
                {
                    result.Add(currentByte);

                    currentByte = 0;
                    mask = 1;
                }
            }

            result.Add(currentByte);

            return result.ToArray();
        }

        public static string ConvertByteArrayToBitsSequence(byte[] array)
        {
            var result = new StringBuilder();

            foreach (var b in array)
            {
                for (int mask = 1; mask <= 128; mask <<= 1)
                {
                    result.Append((b & mask) == 0 ? '0' : '1');
                }
            }

            return result.ToString();
        }
    }
}
