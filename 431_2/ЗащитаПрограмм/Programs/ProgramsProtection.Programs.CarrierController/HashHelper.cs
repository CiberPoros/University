using System.Security.Cryptography;

namespace ProgramsProtection.Programs.CarrierController
{
    internal class HashHelper
    {
        // -1 если от всего файла хэш нужен
        public static byte[] GetHash(byte[] data, int overlaySize = -1)
        {
            MD5 md5Hash = MD5.Create();

            if (overlaySize == -1)
                return md5Hash.ComputeHash(data);

            byte[] temp = new byte[data.Length - overlaySize];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = data[i];

            return md5Hash.ComputeHash(temp);
        }
    }
}
