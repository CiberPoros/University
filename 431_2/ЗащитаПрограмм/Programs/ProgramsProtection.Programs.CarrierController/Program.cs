using System;
using System.Text;
using static System.Console;
using System.IO;
using static ProgramsProtection.Programs.CarrierController.HashHelper;
using static ProgramsProtection.Programs.CarrierController.DriveInformationHelper;
using System.Collections.Generic;

namespace ProgramsProtection.Programs.CarrierController
{
    internal class Program
    {
        static byte[] overlayName;
        static byte[] overlayHash;
        static byte[] overlaySNDD;
        static byte[] overlaySize;
        static readonly string _constNameForRename = "abracadabra.txt";

        static void Main()
        {
            if (IsFirstUse())
            {
                CreateAndWriteParams();
                SUICIDE();
            }
            else
            {
                try
                {
                    CheckCorrectParams();
                }
                catch (Exception e)
                {
                    WriteLine(e.Message);
                    ReadKey();
                    return;
                }

                for (; ; )
                {
                    WriteLine("Я работаю...");
                    System.Threading.Thread.Sleep(2000);
                }
            }
        }

        private static void SUICIDE()
        {
            File.WriteAllLines("batnik.bat", new string[] { "CHCP 1251", "sleep 1", "del \"" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + "\"", "rename " + _constNameForRename + " \"" + "Программа.exe" + "\"", "del batnik.bat" }, Encoding.Default);
            System.Diagnostics.Process.Start("batnik.bat");
        }

        private static bool IsFirstUse()
        {
            byte[] data = File.ReadAllBytes(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            for (int i = data.Length - 1, k = 0; k < 5; i--, k++)
                if (data[i] != 0xFE)
                    return true;
            return false;
        }

        private static void CheckCorrectParams()
        {
            ParseOverlay();

            if (!CheckCorrectParameter(overlaySNDD, Encoding.Default.GetBytes(GetInformationOnAllDrives()[System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\')[0]])))
                throw new Exception("Неверный носитель. Программа может работать только на носителе, на который была установлена.");
        }

        private static void ParseOverlay()
        {
            byte[] data = File.ReadAllBytes(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            int i = data.Length - 6;
            int size = 0;

            ParseOneOverlayParameter(ref i, ref size, ref data, ref overlaySize);
            ParseOneOverlayParameter(ref i, ref size, ref data, ref overlaySNDD);
            ParseOneOverlayParameter(ref i, ref size, ref data, ref overlayHash);
            ParseOneOverlayParameter(ref i, ref size, ref data, ref overlayName);
        }

        private static void ParseOneOverlayParameter(ref int i, ref int size, ref byte[] data, ref byte[] overlayParameter)
        {
            for (; ; i--)
            {
                if (data[i] == 0xFE && data[i - 1] == 0xFE && data[i - 2] == 0xFE && data[i - 3] == 0xFE && data[i - 4] == 0xFE)
                    break;
                size++;
            }

            overlayParameter = new byte[size - 5];
            for (int j = 0, k = i + 1; j < size - 5; j++, k++)
                overlayParameter[j] = data[k + 5];

            i -= 5;
            size = 0;
        }

        private static bool CheckCorrectParameter(byte[] parameter, byte[] etalon)
        {
            if (parameter.Length != etalon.Length)
                return false;

            for (int i = 0; i < parameter.Length; i++)
                if (parameter[i] != etalon[i])
                    return false;

            return true;
        }

        // формат: |||||Name:name|||||Hash:hash|||||SerialNumberDiskDrive:sndd|||||Size:size|||||, где разделитель | есть 0xFE
        private static void CreateAndWriteParams()
        {
            byte[] data = File.ReadAllBytes(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            byte[] res_data = ConcatDataAnsParams(data, GetNameField(), GetHashField(), GetSNDDField(), GetSizeField());

            File.WriteAllBytes(_constNameForRename, res_data);
        }

        // просто законкатенирует все массивы, которые переданы были
        private static byte[] ConcatDataAnsParams(byte[] data, byte[] nameField, byte[] hashField, byte[] snddField, byte[] sizeField)
        {
            byte[] res = new byte[data.Length + nameField.Length + hashField.Length + snddField.Length + sizeField.Length];
            int i = 0;

            for (; i < data.Length; i++)
                res[i] = data[i];

            for (int j = 0; j < nameField.Length; j++, i++)
                res[i] = nameField[j];

            for (int j = 0; j < hashField.Length; j++, i++)
                res[i] = hashField[j];

            for (int j = 0; j < snddField.Length; j++, i++)
                res[i] = snddField[j];

            for (int j = 0; j < sizeField.Length; j++, i++)
                res[i] = sizeField[j];

            return res;
        }

        // вернет |||||Name:name||||| размер 10 + 13 + 5 = 28
        private static byte[] GetNameField()
        {
            string name = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            byte[] nameField = new byte[10 + name.Length + 5];
            for (int i = 0; i < 5; i++)
            {
                nameField[i] = 0xFE;
                nameField[nameField.Length - 1 - i] = 0xFE;
            }

            string temp = "Name:";
            for (int i = 5, j = 0; i < 10; i++, j++)
                nameField[i] = Encoding.Default.GetBytes(temp[j].ToString())[0];

            for (int i = 10, j = 0; j < name.Length; j++, i++)
                nameField[i] = Encoding.Default.GetBytes(name[j].ToString())[0];

            return nameField;
        }

        // вернет Hash:hash||||| размер 5 + hash.length + 5
        private static byte[] GetHashField()
        {
            byte[] data = File.ReadAllBytes(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            byte[] hash = GetHash(data);
            byte[] hashField = new byte[5 + hash.Length + 5];

            for (int i = 0; i < 5; i++)
                hashField[hashField.Length - 1 - i] = 0xFE;

            string temp = "Hash:";
            for (int i = 0; i < 5; i++)
                hashField[i] = Encoding.Default.GetBytes(temp[i].ToString())[0];

            for (int i = 5, j = 0; j < hash.Length; j++, i++)
                hashField[i] = hash[j];

            return hashField;
        }

        // вернет Size:size||||| размер 5 + size.length + 5
        private static byte[] GetSizeField()
        {
            byte[] data = File.ReadAllBytes(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            byte[] size = BitConverter.GetBytes(data.Length);
            byte[] sizeField = new byte[5 + size.Length + 5];

            for (int i = 0; i < 5; i++)
                sizeField[sizeField.Length - 1 - i] = 0xFE;

            string temp = "Size:";
            for (int i = 0; i < 5; i++)
                sizeField[i] = Encoding.Default.GetBytes(temp[i].ToString())[0];

            for (int i = 5, j = 0; j < size.Length; j++, i++)
                sizeField[i] = size[j];

            return sizeField;
        }
    }
}
