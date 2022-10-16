namespace HardwareScanner
{
    internal class HardwareInfo
    {
        public string? ProcessorUniqueId { get; set; }
        public string? ProcessorName { get; set; }
        public string? ProcessorId { get; set; }
        public string? ProcessorMaxClockSpeed { get; set; }

        public string? BiosManufacturer { get; set; }
        public string? BiosSMBIOSBIOSVersion { get; set; }
        public string? BiosIdentificationCode { get; set; }
        public string? BiosSerialNumber { get; set; }
        public string? BiosReleaseDate { get; set; }
        public string? BiosVersion { get; set; }

        public string? ControllerDeviceID { get; set; }
        public string? ControllerManufacturer { get; set; }
        public string? ControllerName { get; set; }
        public string? ControllerPNPDeviceID { get; set; }

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

                BiosManufacturer = Identifier("Win32_BIOS", "Manufacturer"),
                BiosSMBIOSBIOSVersion = Identifier("Win32_BIOS", "SMBIOSBIOSVersion"),
                BiosIdentificationCode = Identifier("Win32_BIOS", "IdentificationCode"),
                BiosSerialNumber = Identifier("Win32_BIOS", "SerialNumber"),
                BiosReleaseDate = Identifier("Win32_BIOS", "ReleaseDate"),
                BiosVersion = Identifier("Win32_BIOS", "Version"),

                ControllerDeviceID = Identifier("Win32_PCMCIAController", "DeviceID"),
                ControllerManufacturer = Identifier("Win32_PCMCIAController", "Manufacturer"),
                ControllerName = Identifier("Win32_PCMCIAController", "Name"),
                ControllerPNPDeviceID = Identifier("Win32_PCMCIAController", "PNPDeviceID"),

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

        public override bool Equals(object? obj)
        {
            return obj is HardwareInfo info &&
                   ProcessorUniqueId == info.ProcessorUniqueId &&
                   ProcessorName == info.ProcessorName &&
                   ProcessorId == info.ProcessorId &&
                   ProcessorMaxClockSpeed == info.ProcessorMaxClockSpeed &&
                   BiosManufacturer == info.BiosManufacturer &&
                   BiosSMBIOSBIOSVersion == info.BiosSMBIOSBIOSVersion &&
                   BiosIdentificationCode == info.BiosIdentificationCode &&
                   BiosSerialNumber == info.BiosSerialNumber &&
                   BiosReleaseDate == info.BiosReleaseDate &&
                   BiosVersion == info.BiosVersion &&
                   ControllerDeviceID == info.ControllerDeviceID &&
                   ControllerManufacturer == info.ControllerManufacturer &&
                   ControllerName == info.ControllerName &&
                   ControllerPNPDeviceID == info.ControllerPNPDeviceID &&
                   NetworkAdapterGUID == info.NetworkAdapterGUID &&
                   NetworkAdapterDeviceID == info.NetworkAdapterDeviceID &&
                   NetworkAdapterName == info.NetworkAdapterName &&
                   NetworkAdapterMACAddress == info.NetworkAdapterMACAddress;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(ProcessorUniqueId);
            hash.Add(ProcessorName);
            hash.Add(ProcessorId);
            hash.Add(ProcessorMaxClockSpeed);
            hash.Add(BiosManufacturer);
            hash.Add(BiosSMBIOSBIOSVersion);
            hash.Add(BiosIdentificationCode);
            hash.Add(BiosSerialNumber);
            hash.Add(BiosReleaseDate);
            hash.Add(BiosVersion);
            hash.Add(ControllerDeviceID);
            hash.Add(ControllerManufacturer);
            hash.Add(ControllerName);
            hash.Add(ControllerPNPDeviceID);
            hash.Add(NetworkAdapterGUID);
            hash.Add(NetworkAdapterDeviceID);
            hash.Add(NetworkAdapterName);
            hash.Add(NetworkAdapterMACAddress);
            return hash.ToHashCode();
        }
    }
}
