using System;
using System.Collections.Generic;

namespace HardwareScanner
{
    internal class HardwareInfo
    {
        public string? ProcessorUniqueId { get; set; }
        public string? ProcessorName { get; set; }
        public string? ProcessorId { get; set; }
        public string? ProcessorMaxClockSpeed { get; set; }

        public string? BiosSMBIOSBIOSVersion { get; set; }
        public string? BiosIdentificationCode { get; set; }
        public string? BiosSerialNumber { get; set; }
        public string? BiosReleaseDate { get; set; }
        public string? BiosVersion { get; set; }

        public string? NetworkAdapterGUID { get; set; }
        public string? NetworkAdapterDeviceID { get; set; }
        public string? NetworkAdapterName { get; set; }
        public string? NetworkAdapterMACAddress { get; set; }

        public static HardwareInfo Create()
        {
            return new HardwareInfo()
            {
                ProcessorUniqueId = Identifier("Win32_Processor", "UniqueId"),
                ProcessorId = Identifier("Win32_Processor", "ProcessorId"),
                ProcessorName = Identifier("Win32_Processor", "Name"),
                ProcessorMaxClockSpeed = Identifier("Win32_Processor", "MaxClockSpeed"),

                BiosSMBIOSBIOSVersion = Identifier("Win32_BIOS", "SMBIOSBIOSVersion"),
                BiosIdentificationCode = Identifier("Win32_BIOS", "IdentificationCode"),
                BiosSerialNumber = Identifier("Win32_BIOS", "SerialNumber"),
                BiosReleaseDate = Identifier("Win32_BIOS", "ReleaseDate"),
                BiosVersion = Identifier("Win32_BIOS", "Version"),

                NetworkAdapterGUID = Identifier("Win32_NetworkAdapter", "GUID"),
                NetworkAdapterDeviceID = Identifier("Win32_NetworkAdapter", "DeviceID"),
                NetworkAdapterName = Identifier("Win32_NetworkAdapter", "Name"),
                NetworkAdapterMACAddress = Identifier("Win32_NetworkAdapter", "MACAddress")
            };
        }

        private static string Identifier(string wmiClass, string wmiProperty)
        {
            var sysObjects = new System.Management.ManagementClass(wmiClass).GetInstances();
            foreach (System.Management.ManagementObject sysObject in sysObjects)
            {
                try
                {
                    return sysObject[wmiProperty].ToString();
                }
                catch { }
            }

            return string.Empty;
        }

        public Dictionary<string, List<string>> CompareInfo(HardwareInfo hardwareInfo)
        {
            var result = new Dictionary<string, List<string>>();

            var processorDescription = "Несовпадение по параметрам процессора:";
            if (ProcessorUniqueId != hardwareInfo.ProcessorUniqueId || ProcessorId != hardwareInfo.ProcessorId)
            {
                if (!result.ContainsKey(processorDescription))
                {
                    result.Add(processorDescription, new List<string>());
                }

                result[processorDescription].Add("- разлицие по уникальному идентификатору процессора");
            }

            if (ProcessorName != hardwareInfo.ProcessorName)
            {
                if (!result.ContainsKey(processorDescription))
                {
                    result.Add(processorDescription, new List<string>());
                }

                result[processorDescription].Add("- различие по названию процессора");
            }

            if (ProcessorMaxClockSpeed != hardwareInfo.ProcessorMaxClockSpeed)
            {
                if (!result.ContainsKey(processorDescription))
                {
                    result.Add(processorDescription, new List<string>());
                }

                result[processorDescription].Add("- различие по максимальной тактовой частоте процессора");
            }

            var biosDescription = "Несовпадение по параметрам BIOS:";
            if (BiosIdentificationCode != hardwareInfo.BiosIdentificationCode)
            {
                if (!result.ContainsKey(biosDescription))
                {
                    result.Add(biosDescription, new List<string>());
                }

                result[biosDescription].Add("- разлицие по уникальному идентификатору BIOS");
            }

            if (BiosReleaseDate != hardwareInfo.BiosReleaseDate)
            {
                if (!result.ContainsKey(biosDescription))
                {
                    result.Add(biosDescription, new List<string>());
                }

                result[biosDescription].Add("- разлицие по дате выпуска текущей версии BIOS");
            }

            if (BiosSerialNumber != hardwareInfo.BiosSerialNumber)
            {
                if (!result.ContainsKey(biosDescription))
                {
                    result.Add(biosDescription, new List<string>());
                }

                result[biosDescription].Add("- разлицие по серийному номеру BIOS");
            }

            if (BiosSMBIOSBIOSVersion != hardwareInfo.BiosSMBIOSBIOSVersion || BiosVersion != hardwareInfo.BiosVersion)
            {
                if (!result.ContainsKey(biosDescription))
                {
                    result.Add(biosDescription, new List<string>());
                }

                result[biosDescription].Add("- разлицие по версии BIOS");
            }

            var networkAdapterDescription = "Несовпадение по параметрам сетевой карты:";
            if (NetworkAdapterDeviceID != hardwareInfo.NetworkAdapterDeviceID || NetworkAdapterGUID != hardwareInfo.NetworkAdapterGUID)
            {
                if (!result.ContainsKey(networkAdapterDescription))
                {
                    result.Add(networkAdapterDescription, new List<string>());
                }

                result[networkAdapterDescription].Add("- различие по уникальному идентификатору сетевой карты");
            }

            if (NetworkAdapterMACAddress != hardwareInfo.NetworkAdapterMACAddress)
            {
                if (!result.ContainsKey(networkAdapterDescription))
                {
                    result.Add(networkAdapterDescription, new List<string>());
                }

                result[networkAdapterDescription].Add("- различие по MAC адресу сетевой карты");
            }

            if (NetworkAdapterName != hardwareInfo.NetworkAdapterName)
            {
                if (!result.ContainsKey(networkAdapterDescription))
                {
                    result.Add(networkAdapterDescription, new List<string>());
                }

                result[networkAdapterDescription].Add("- различие по названию сетевой карты");
            }

            return result;
        }
    }
}
