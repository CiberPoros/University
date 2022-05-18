using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace ProgramsProtection.Programs.CarrierController
{
    internal class DriveInformationHelper
    {
        public static Dictionary<string, string> GetInformationOnAllDrives()
        {
            var driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            Dictionary<string, string> res = new Dictionary<string, string>();

            foreach (ManagementObject d in driveQuery.Get())
            {
                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText); // соответствие фищического диска и его разбиениям

                foreach (ManagementObject p in partitionQuery.Get())
                {
                    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath);
                    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText); // соответствие логического диска и его разбиениям

                    foreach (ManagementObject ld in logicalDriveQuery.Get())
                    {
                        var selfPhisicalName = Convert.ToString(d.Properties["Name"].Value); // физическое имя текущего (именно имя!)
                        var selfDriveName = Convert.ToString(ld.Properties["Name"].Value); // название логического с двоеточием!

                        var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                        foreach (ManagementObject wmi_HD in searcher.Get())
                        {
                            if (wmi_HD.Properties["Name"].Value.ToString() == selfPhisicalName) // если совпадает, добавляем в словарь по названию логического серийник физического
                                res.Add(selfDriveName, wmi_HD["SerialNumber"].ToString());
                        }
                    }
                }
            }

            return res;
        }

        // вернет SNDD:sndd||||| размер 10 + sndd.length + 5
        public static byte[] GetSNDDField()
        {
            Dictionary<string, string> d = GetInformationOnAllDrives();
            byte[] SNDD = Encoding.Default.GetBytes(d[System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\')[0]]);
            byte[] SNDDField = new byte[5 + SNDD.Length + 5];

            for (int i = 0; i < 5; i++)
                SNDDField[SNDDField.Length - 1 - i] = 0xFE;

            string temp = "SNDD:";
            for (int i = 0; i < 5; i++)
                SNDDField[i] = Encoding.Default.GetBytes(temp[i].ToString())[0];

            for (int i = 5, j = 0; j < SNDD.Length; j++, i++)
                SNDDField[i] = SNDD[j];

            return SNDDField;
        }
    }
}
