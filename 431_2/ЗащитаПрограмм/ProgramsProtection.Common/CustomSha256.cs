using System;

namespace ProgramsProtection.Common
{
    internal class CustomSha256
    {
        // names of most variables as in the algorithm
        public byte[] GetHash(byte[] source)
        {
            var extended = new byte[(source.Length + 1) + 448 - (source.Length + 1) % 512];
            source.CopyTo(extended, 0);

            ulong length = (ulong)source.LongLength;

            extended[^8] |= (byte)(length >> 56);
            extended[^7] |= (byte)(length >> 48);
            extended[^6] |= (byte)(length >> 40);
            extended[^5] |= (byte)(length >> 32);
            extended[^4] |= (byte)(length >> 24);
            extended[^3] |= (byte)(length >> 16);
            extended[^2] |= (byte)(length >> 8);
            extended[^1] |= (byte)(length);

            // just constants
            var hArr = new uint[]
            {
                0x6a09e667,
                0xbb67ae85,
                0x3c6ef372,
                0xa54ff53a,
                0x510e527f,
                0x9b05688c,
                0x1f83d9ab,
                0x5be0cd19,
            };

            // just constants
            var k = new uint[]
            {
                0x428A2F98, 0x71374491, 0xB5C0FBCF, 0xE9B5DBA5, 
                0x3956C25B, 0x59F111F1, 0x923F82A4, 0xAB1C5ED5, 
                0xD807AA98, 0x12835B01, 0x243185BE, 0x550C7DC3, 
                0x72BE5D74, 0x80DEB1FE, 0x9BDC06A7, 0xC19BF174, 
                0xE49B69C1, 0xEFBE4786, 0x0FC19DC6, 0x240CA1CC, 
                0x2DE92C6F, 0x4A7484AA, 0x5CB0A9DC, 0x76F988DA, 
                0x983E5152, 0xA831C66D, 0xB00327C8, 0xBF597FC7, 
                0xC6E00BF3, 0xD5A79147, 0x06CA6351, 0x14292967, 
                0x27B70A85, 0x2E1B2138, 0x4D2C6DFC, 0x53380D13, 
                0x650A7354, 0x766A0ABB, 0x81C2C92E, 0x92722C85, 
                0xA2BFE8A1, 0xA81A664B, 0xC24B8B70, 0xC76C51A3, 
                0xD192E819, 0xD6990624, 0xF40E3585, 0x106AA070, 
                0x19A4C116, 0x1E376C08, 0x2748774C, 0x34B0BCB5, 
                0x391C0CB3, 0x4ED8AA4A, 0x5B9CCA4F, 0x682E6FF3,
                0x748F82EE, 0x78A5636F, 0x84C87814, 0x8CC70208, 
                0x90BEFFFA, 0xA4506CEB, 0xBEF9A3F7, 0xC67178F2
            };

            for (int i = 0; i < extended.Length; i += 64)
            {
                uint[] w = new uint[64];
                Buffer.BlockCopy(extended, i, w, 0, 16);

                for (int j = 16; j < w.Length; j++)
                {
                    var s0 = Rotr(w[j - 15], 7) ^ Rotr(w[j - 15], 18) ^ (w[j - 15] >> 3);
                    var s1 = Rotr(w[j - 2], 17) ^ Rotr(w[j - 2], 19) ^ (w[j - 2] >> 10);
                    w[j] = w[j - 16] + s0 + w[j - 7] + s1;
                }

                var a = hArr[0];
                var b = hArr[1];
                var c = hArr[2];
                var d = hArr[3];
                var e = hArr[4];
                var f = hArr[5];
                var g = hArr[6];
                var h = hArr[7];

                for (int j = 0; j < 64; j++)
                {
                    var sigma0 = Rotr(a, 2) ^ Rotr(a, 13) ^ Rotr(a, 22);
                    var ma = (a & b) ^ (a & c) ^ (b & c);
                    var t2 = sigma0 + ma;
                    var sigma1 = Rotr(e, 6) ^ Rotr(e, 11) ^ Rotr(e, 25);
                    var ch = (e & f) ^ ((~e) & g);
                    var t1 = h + sigma1 + ch + k[j] + w[j];

                    h = g;
                    g = f;
                    f = e;
                    e = d + t1;
                    d = c;
                    c = b;
                    b = a;
                    a = t1 + t2;
                }

                hArr[0] += a;
                hArr[1] += b;
                hArr[2] += c;
                hArr[3] += d;
                hArr[4] += e;
                hArr[5] += f;
                hArr[6] += g;
                hArr[7] += h;
            }

            byte[] result = new byte[hArr.Length * sizeof(uint)];
            Buffer.BlockCopy(hArr, 0, result, 0, result.Length);

            return result;
        }

        private static uint Rotr(uint value, int offset)
        {
            return (value >> offset) | ((value << (32 - offset)) & 0xFFFFFFFF);
        }
    }
}
