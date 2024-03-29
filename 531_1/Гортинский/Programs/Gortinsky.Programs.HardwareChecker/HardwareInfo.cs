﻿using System.Text;

namespace HardwareScanner
{
    internal class HardwareInfo
    {
        public Win32_Processor? Win32_Processor { get; set; }
        public Win32_BIOS? Win32_BIOS { get; set; }
        public Win32_PCMCIAController? Win32_PCMCIAController { get; set; }
        public Win32_NetworkAdapter? Win32_NetworkAdapter { get; set; }

        public static string GetHardwareInfoString()
        {
            var result = new StringBuilder();

            var type = typeof(HardwareInfo);
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                var propName = prop.Name;
                var internalProps = propType.GetProperties();

                foreach (var propInternal in internalProps)
                {
                    var internalPropName = propInternal.Name;

                    var sysObjects = new System.Management.ManagementClass(propName).GetInstances();
                    foreach (System.Management.ManagementObject sysObject in sysObjects)
                    {
                        try
                        {
                            result.Append(sysObject[internalPropName]?.ToString() ?? string.Empty);
                            break;
                        }
                        catch { }
                    }
                }
            }

            return result.ToString();
        }
    }

    public class Win32_Processor
    {
        public string? UniqueId { get; set; }
        public string? ProcessorId { get; set; }
        public string? Name { get; set; }
        public string? MaxClockSpeed { get; set; }
    }

    public class Win32_BIOS
    {
        public string? Manufacturer { get; set; }
        public string? SMBIOSBIOSVersion { get; set; }
        public string? IdentificationCode { get; set; }
        public string? SerialNumber { get; set; }
        public string? ReleaseDate { get; set; }
        public string? Version { get; set; }
    }

    public class Win32_PCMCIAController
    {
        public string? DeviceID { get; set; }
        public string? Manufacturer { get; set; }
        public string? Name { get; set; }
        public string? PNPDeviceID { get; set; }
    }

    public class Win32_NetworkAdapter
    {
        public string? GUID { get; set; }
        public string? DeviceID { get; set; }
        public string? Name { get; set; }
        public string? MACAddress { get; set; }
    }
}
