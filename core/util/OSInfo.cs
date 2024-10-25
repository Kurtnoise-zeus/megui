// ****************************************************************************
// 
// Copyright (C) 2005-2024 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Threading;

namespace MeGUI
{
    /// <summary>
    /// OSInfo Class based from http://www.codeproject.com/csharp/osversion_producttype.asp
    /// </summary>
    public class OSInfo
    {
        #region OSInfo 
        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        [DllImport("kernel32.dll")]
        private static extern bool GetProductInfo(int dwOSMajorVersion, int dwOSMinorVersion, int dwSpMajorVersion, int dwSpMinorVersion, out uint pdwReturnedProductType);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        private const int VER_NT_WORKSTATION = 1;
        private const int VER_NT_DOMAIN_CONTROLLER = 2;
        private const int VER_NT_SERVER = 3;
        private const int VER_SUITE_SMALLBUSINESS = 1;
        private const int VER_SUITE_ENTERPRISE = 2;
        private const int VER_SUITE_TERMINAL = 16;
        private const int VER_SUITE_DATACENTER = 128;
        private const int VER_SUITE_SINGLEUSERTS = 256;
        private const int VER_SUITE_PERSONAL = 512;
        private const int VER_SUITE_BLADE = 1024;
        #endregion

        /// <summary>
        /// Returns the service pack information of the operating system running on this computer.
        /// </summary>
        /// <returns>A string containing the operating system service pack information.</returns>
        private static string GetOSServicePack()
        {
            OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX()
            {
                dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX))
            };

            if (!GetVersionEx(ref osVersionInfo))
            {
                return "";
            }
            else
            {
                if (osVersionInfo.szCSDVersion != "")
                {
                    return " SP" + osVersionInfo.szCSDVersion.Substring(13, 1);
                }
                else
                {
                    return "";
                }
            }
        }

        private static string GetOSProduct(OSVERSIONINFOEX osVersionInfo)
        {
            Dictionary<int, string> OSProduct = new Dictionary<int, string>
            {
                { 0x00000000, "PRODUCT_UNDEFINED" },
                { 0x00000001, "PRODUCT_ULTIMATE" },
                { 0x00000002, "PRODUCT_HOME_BASIC" },
                { 0x00000003, "PRODUCT_HOME_PREMIUM" },
                { 0x00000004, "PRODUCT_ENTERPRISE" },
                { 0x00000005, "PRODUCT_HOME_BASIC_N" },
                { 0x00000006, "PRODUCT_BUSINESS" },
                { 0x00000007, "PRODUCT_STANDARD_SERVER" },
                { 0x00000008, "PRODUCT_DATACENTER_SERVER" },
                { 0x00000009, "PRODUCT_SMALLBUSINESS_SERVER" },
                { 0x0000000A, "PRODUCT_ENTERPRISE_SERVER" },
                { 0x0000000B, "PRODUCT_STARTER" },
                { 0x0000000C, "PRODUCT_DATACENTER_SERVER_CORE" },
                { 0x0000000D, "PRODUCT_STANDARD_SERVER_CORE " },
                { 0x0000000E, "PRODUCT_ENTERPRISE_SERVER_CORE" },
                { 0x0000000F, "PRODUCT_ENTERPRISE_SERVER_IA64" },
                { 0x00000010, "PRODUCT_BUSINESS_N" },
                { 0x00000011, "PRODUCT_WEB_SERVER" },
                { 0x00000012, "PRODUCT_CLUSTER_SERVER" },
                { 0x00000013, "PRODUCT_HOME_SERVER" },
                { 0x00000014, "PRODUCT_STORAGE_EXPRESS_SERVER" },
                { 0x00000015, "PRODUCT_STORAGE_STANDARD_SERVER" },
                { 0x00000016, "PRODUCT_STORAGE_WORKGROUP_SERVER" },
                { 0x00000017, "PRODUCT_STORAGE_ENTERPRISE_SERVER" },
                { 0x00000018, "PRODUCT_SERVER_FOR_SMALLBUSINESS" },
                { 0x00000019, "PRODUCT_SMALLBUSINESS_SERVER_PREMIUM" },
                { 0x0000001A, "PRODUCT_HOME_PREMIUM_N" },
                { 0x0000001B, "PRODUCT_ENTERPRISE_N" },
                { 0x0000001C, "PRODUCT_ULTIMATE_N" },
                { 0x0000001D, "PRODUCT_WEB_SERVER_CORE" },
                { 0x0000001E, "PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT" },
                { 0x0000001F, "PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY" },
                { 0x00000020, "PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING" },
                { 0x00000021, "PRODUCT_SERVER_FOUNDATION" },
                { 0x00000022, "PRODUCT_HOME_PREMIUM_SERVER" },
                { 0x00000023, "PRODUCT_SERVER_FOR_SMALLBUSINESS_V" },
                { 0x00000024, "PRODUCT_STANDARD_SERVER_V" },
                { 0x00000025, "PRODUCT_DATACENTER_SERVER_V" },
                { 0x00000026, "PRODUCT_ENTERPRISE_SERVER_V" },
                { 0x00000027, "PRODUCT_DATACENTER_SERVER_CORE_V" },
                { 0x00000028, "PRODUCT_STANDARD_SERVER_CORE_V" },
                { 0x00000029, "PRODUCT_ENTERPRISE_SERVER_CORE_V" },
                { 0x0000002A, "PRODUCT_HYPERV" },
                { 0x0000002B, "PRODUCT_STORAGE_EXPRESS_SERVER_CORE" },
                { 0x0000002C, "PRODUCT_STORAGE_STANDARD_SERVER_CORE" },
                { 0x0000002D, "PRODUCT_STORAGE_WORKGROUP_SERVER_CORE" },
                { 0x0000002E, "PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE" },
                { 0x0000002F, "PRODUCT_STARTER_N" },
                { 0x00000030, "PRODUCT_PROFESSIONAL" },
                { 0x00000031, "PRODUCT_PROFESSIONAL_N" },
                { 0x00000032, "PRODUCT_SB_SOLUTION_SERVER" },
                { 0x00000033, "PRODUCT_SERVER_FOR_SB_SOLUTIONS" },
                { 0x00000034, "PRODUCT_STANDARD_SERVER_SOLUTIONS" },
                { 0x00000035, "PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE" },
                { 0x00000036, "PRODUCT_SB_SOLUTION_SERVER_EM" },
                { 0x00000037, "PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM" },
                { 0x00000038, "PRODUCT_SOLUTION_EMBEDDEDSERVER" },
                { 0x0000003B, "PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT" },
                { 0x0000003C, "PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL" },
                { 0x0000003D, "PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC" },
                { 0x0000003E, "PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC" },
                { 0x0000003F, "PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE" },
                { 0x00000040, "PRODUCT_CLUSTER_SERVER_V" },
                { 0x00000042, "PRODUCT_STARTER_E" },
                { 0x00000043, "PRODUCT_HOME_BASIC_E" },
                { 0x00000044, "PRODUCT_HOME_PREMIUM_E" },
                { 0x00000045, "PRODUCT_PROFESSIONAL_E" },
                { 0x00000046, "PRODUCT_ENTERPRISE_E" },
                { 0x00000047, "PRODUCT_ULTIMATE_E" },
                { 0x00000048, "PRODUCT_ENTERPRISE_EVALUATION" },
                { 0x0000004C, "PRODUCT_MULTIPOINT_STANDARD_SERVER" },
                { 0x0000004D, "PRODUCT_MULTIPOINT_PREMIUM_SERVER" },
                { 0x0000004F, "PRODUCT_STANDARD_EVALUATION_SERVER" },
                { 0x00000050, "PRODUCT_DATACENTER_EVALUATION_SERVER" },
                { 0x00000054, "PRODUCT_ENTERPRISE_N_EVALUATION" },
                { 0x0000005F, "PRODUCT_STORAGE_WORKGROUP_EVALUATION_SERVER" },
                { 0x00000060, "PRODUCT_STORAGE_STANDARD_EVALUATION_SERVER" },
                { 0x00000062, "PRODUCT_CORE_N" },
                { 0x00000063, "PRODUCT_CORE_COUNTRYSPECIFIC" },
                { 0x00000064, "PRODUCT_CORE_SINGLELANGUAGE" },
                { 0x00000065, "PRODUCT_CORE" },
                { 0x00000067, "PRODUCT_PROFESSIONAL_WMC" },
                { 0x00000068, "PRODUCT_MOBILE_CORE" },
                { 0x00000079, "PRODUCT_EDUCATION" },
                { 0x0000007A, "PRODUCT_EDUCATION_N" },
                { 0x0000007B, "PRODUCT_IOTUAP" },
                { 0x0000007D, "PRODUCT_ENTERPRISE_S" },
                { 0x0000007E, "PRODUCT_ENTERPRISE_S_N" },
                { 0x00000081, "PRODUCT_ENTERPRISE_S_EVALUATION" },
                { 0x00000082, "PRODUCT_ENTERPRISE_S_N_EVALUATION" },
                { 0x00000083, "PRODUCT_IOTUAPCOMMERCIAL" },
                { 0x00000085, "PRODUCT_MOBILE_ENTERPRISE" },
                { 0x000000A1, "PRODUCT_PRO_WORKSTATION" },
                { 0x000000A2, "PRODUCT_PRO_WORKSTATION_N" },
                { 0x000000B2, "PRODUCT_CLOUD" },
                { 0x000000B3, "PRODUCT_CLOUDN" }
            };

            GetProductInfo(osVersionInfo.dwMajorVersion,
                    osVersionInfo.dwMinorVersion,
                    osVersionInfo.wServicePackMajor,
                    osVersionInfo.wServicePackMinor,
                    out uint product);

            OSProduct.TryGetValue((int)product, out string strOSProduct);
            if (String.IsNullOrEmpty(strOSProduct))
                strOSProduct = "PRODUCT_UNKNOWN";
            return strOSProduct;
        }

        #region Public Methods
        /// <summary>
        /// Determines whether the specified process is running under WOW64. 
        /// </summary>
        /// <returns>a boolean</returns>
        public static bool IsWow64()
        {
            if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 0)
                return false;   // windows 2000

            Process p = Process.GetCurrentProcess();
            IntPtr handle = p.Handle;
            bool success = IsWow64Process(handle, out bool isWow64);
            if ((!success) && (IntPtr.Size != 8))
                throw new Exception();
            else
                return isWow64;
        }

        /// <summary>
        /// Returns the name of the operating system running on this computer.
        /// </summary>
        /// <returns>A string containing the the operating system name.</returns>
        public static string GetOSName()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX()
            {
                dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX))
            };

            string osName = "UNKNOWN";
            bool x64Detection = true;

            if (!GetVersionEx(ref osVersionInfo))
                return "";

            switch (osInfo.Platform)
            {
                case PlatformID.Win32Windows:
                    {
                        x64Detection = false;
                        switch (osInfo.Version.Minor)
                        {
                            case 0: osName = "Windows 95"; break;
                            case 10:
                                {
                                    if (osInfo.Version.Revision.ToString() == "2222A")
                                        osName = "Windows 98 Second Edition";
                                    else
                                        osName = "Windows 98";
                                }
                                break;
                            case 90: osName = "Windows Me"; break;
                        }
                        break;
                    }
                case PlatformID.Win32NT:
                    {
                        switch (osInfo.Version.Major)
                        {
                            case 3:
                                x64Detection = false;
                                osName = "Windows NT 3.51";
                                break;
                            case 4:
                                {
                                    x64Detection = false;
                                    switch (osVersionInfo.wProductType)
                                    {
                                        case 1: osName = "Windows NT 4.0 Workstation"; break;
                                        case 3: osName = "Windows NT 4.0 Server"; break;
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    switch (osInfo.Version.Minor)
                                    {
                                        case 0: // win2K
                                            {
                                                x64Detection = false;
                                                if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                                                    osName = "Windows 2000 Datacenter Server";
                                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                                                    osName = "Windows 2000 Advanced Server";
                                                else
                                                    osName = "Windows 2000";
                                                break;
                                            }
                                        case 1: // winXP
                                            {
                                                if ((osVersionInfo.wSuiteMask & VER_SUITE_PERSONAL) == VER_SUITE_PERSONAL)
                                                    osName = "Windows XP Home Edition";
                                                else
                                                    osName = "Windows XP Professional";
                                                break;
                                            }
                                        case 2: // winserver 2003
                                            {
                                                if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                                                    osName = "Windows Server 2003 DataCenter";
                                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                                                    osName = "Windows Server 2003 Enterprise";
                                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_BLADE) == VER_SUITE_BLADE)
                                                    osName = "Windows Server 2003 Web";
                                                else
                                                    osName = "Windows Server 2003 Standard";
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case 6:
                                {
                                    string strOSProduct = GetOSProduct(osVersionInfo);

                                    switch (osInfo.Version.Minor)
                                    {
                                        case 0: // Vista
                                            {
                                                if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                                                    osName = "Windows Vista ";
                                                else
                                                    osName = "Windows Server 2008 ";
                                                switch (strOSProduct)
                                                {
                                                    case "PRODUCT_ULTIMATE": osName += "Ultimate"; break;
                                                    case "PRODUCT_HOME_BASIC":
                                                    case "PRODUCT_HOME_BASIC_N": osName += "Home Basic"; break;
                                                    case "PRODUCT_HOME_PREMIUM": osName += "Premium"; break;
                                                    case "PRODUCT_ENTERPRISE": osName += "Enterprise"; break;
                                                    case "PRODUCT_BUSINESS":
                                                    case "PRODUCT_BUSINESS_N": osName += "Business"; break;
                                                    case "PRODUCT_STARTER": osName += "Starter"; break;
                                                    case "PRODUCT_ENTERPRISE_SERVER": osName += "Enterprise"; break;
                                                    case "PRODUCT_STANDARD_SERVER": osName += "Standard"; break;
                                                    default: osName += "(" + strOSProduct.Replace("PRODUCT_", "") + ")"; break;
                                                }
                                                break;
                                            }
                                        case 1: // Windows 7
                                            {
                                                if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                                                    osName = "Windows 7 ";
                                                else
                                                    osName = "Windows Server 2008 R2 ";
                                                switch (strOSProduct)
                                                {
                                                    case "PRODUCT_ULTIMATE": osName += "Ultimate"; break;
                                                    case "PRODUCT_HOME_BASIC":
                                                    case "PRODUCT_HOME_BASIC_N": osName += "Home Basic"; break;
                                                    case "PRODUCT_HOME_PREMIUM": osName += "Premium"; break;
                                                    case "PRODUCT_ENTERPRISE": osName += "Enterprise"; break;
                                                    case "PRODUCT_PROFESSIONAL":
                                                    case "PRODUCT_PROFESSIONAL_N":
                                                    case "PRODUCT_BUSINESS":
                                                    case "PRODUCT_BUSINESS_N": osName += "Professional"; break;
                                                    case "PRODUCT_STARTER": osName += "Starter"; break;
                                                    case "PRODUCT_ENTERPRISE_SERVER": osName += "Enterprise"; break;
                                                    case "PRODUCT_STANDARD_SERVER": osName += "Standard"; break;
                                                    default: osName += "(" + strOSProduct.Replace("PRODUCT_", "") + ")"; break;
                                                }
                                                break;
                                            }
                                        case 2: // Windows 8
                                        case 3: // Windows 8.1
                                            {
                                                if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                                                    osName = "Windows 8" + (osInfo.Version.Minor == 3 ? ".1 " : " ");
                                                else
                                                    osName = "Windows Server 2012" + (osInfo.Version.Minor == 3 ? " R2 " : " ");
                                                switch (strOSProduct)
                                                {
                                                    case "PRODUCT_CORE":
                                                    case "PRODUCT_CORE_COUNTRYSPECIFIC":
                                                    case "PRODUCT_CORE_N": osName += "Standard"; break;
                                                    case "PRODUCT_ENTERPRISE":
                                                    case "PRODUCT_ENTERPRISE_N": osName += "Enterprise"; break;
                                                    case "PRODUCT_PROFESSIONAL":
                                                    case "PRODUCT_PROFESSIONAL_N": osName += "Professional"; break;
                                                    case "PRODUCT_PROFESSIONAL_WMC": osName += "Professional with Media Center"; break;
                                                    case "PRODUCT_ENTERPRISE_SERVER": osName += "Enterprise"; break;
                                                    case "PRODUCT_STANDARD_SERVER": osName += "Standard"; break;
                                                    default: osName += "(" + strOSProduct.Replace("PRODUCT_", "") + ")"; break;
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case 10:
                                {
                                    switch (osInfo.Version.Minor)
                                    {
                                        case 0: // Windows 10
                                            {
                                                if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                                                {
                                                    osName = (osVersionInfo.dwBuildNumber >= 22000 ? osName = "Windows 11 " : "Windows 10 ");
                                                }
                                                else
                                                    osName = "Windows Server 2016 ";

                                                if (osVersionInfo.dwBuildNumber >= 22000)
                                                {
                                                    using (var objOS = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                                                    {
                                                        foreach (ManagementObject objMgmt in objOS.Get())
                                                        {
                                                            osName = objMgmt.Properties["Caption"].Value.ToString();
                                                        }
                                                    }

                                                    string release = string.Empty;
                                                    switch (osVersionInfo.dwBuildNumber)
                                                    {
                                                        case 22000: osName += " 21H2"; break;
                                                        case 22621: osName += " 22H2"; break;
                                                        case 22631: osName += " 23H2"; break;
                                                        case 26100: osName += " 24H2"; break;
                                                        default: osName += string.IsNullOrEmpty(release) ? string.Empty : " " + release; break;
                                                    }
                                                }

                                                else
                                                {

                                                    string strOSProduct = GetOSProduct(osVersionInfo);
                                                    switch (strOSProduct)
                                                    {
                                                        case "PRODUCT_CLOUD":
                                                        case "PRODUCT_CLOUDN": osName += "S"; break;
                                                        case "PRODUCT_CORE":
                                                        case "PRODUCT_CORE_COUNTRYSPECIFIC":
                                                        case "PRODUCT_CORE_N": osName += "Home"; break;
                                                        case "PRODUCT_ENTERPRISE":
                                                        case "PRODUCT_ENTERPRISE_N": osName += "Enterprise"; break;
                                                        case "PRODUCT_ENTERPRISE_SERVER": osName += "Enterprise"; break;
                                                        case "PRODUCT_PROFESSIONAL":
                                                        case "PRODUCT_PROFESSIONAL_N": osName += "Pro"; break;
                                                        case "PRODUCT_EDUCATION":
                                                        case "PRODUCT_EDUCATION_N": osName += "Education"; break;
                                                        case "PRODUCT_MOBILE_CORE":
                                                        case "PRODUCT_MOBILE_ENTERPRISE": osName += "Mobile"; break;
                                                        case "PRODUCT_PRO_WORKSTATION":
                                                        case "PRODUCT_PRO_WORKSTATION_N": osName += "Pro for Workstations"; break;
                                                        case "PRODUCT_STANDARD_SERVER": osName += "Standard"; break;
                                                        default: osName += "(" + strOSProduct.Replace("PRODUCT_", "") + ")"; break;
                                                    }


                                                    // get release id if available
                                                    string release = string.Empty;
                                                    try
                                                    {
                                                        release = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", string.Empty);
                                                        if (release.Equals("2009"))
                                                            release = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", string.Empty);
                                                    }
                                                    catch { }

                                                    switch (osVersionInfo.dwBuildNumber)
                                                    {
                                                        case 10240: osName += " 1507"; break;
                                                        case 10586: osName += " 1511"; break;
                                                        case 14393: osName += " 1607"; break;
                                                        case 15063: osName += " 1703"; break;
                                                        default: osName += string.IsNullOrEmpty(release) ? string.Empty : " " + release; break;
                                                    }
                                                }
                                                    break;
                                                

                                            }
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }

            if (!MainForm.Instance.Settings.IsMeGUIx64)
            {
                if (x64Detection)
                {
                    if (!IsWow64())
                        osName += " x86";
                    else
                        osName += " x64";
                }
            }
            else
                osName += " x64";

            // get update revision if available
            int ubr = 0;
            try
            {
                ubr = (int)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ubr", 0);
            }
            catch { }

            if (ubr > 0)
                return string.Format("{0}{1} ({2}.{3}.{4}.{5})", osName, OSInfo.GetOSServicePack(), osInfo.Version.Major, osInfo.Version.Minor, osInfo.Version.Build, ubr);
            else
                return string.Format("{0}{1} ({2}.{3}.{4}.{5})", osName, OSInfo.GetOSServicePack(), osInfo.Version.Major, osInfo.Version.Minor, osInfo.Version.Revision, osInfo.Version.Build);
        }

        /// <summary>
        /// Returns the build number of the OS
        /// </summary>
        /// <returns>A decimal conatining the version e.g. 10.0</returns>
        public static decimal GetOSBuild()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            if (osInfo.Platform != PlatformID.Win32NT)
                return 0;
            return osInfo.Version.Major + osInfo.Version.Minor / 10;
        }

        /// <summary>
        /// Returns the name of the highest .NET Framework running on this computer.
        /// </summary>
        /// <returns>A string containing the Name of the Framework Version.</returns>
        /// 
        public static string GetDotNetVersion()
        {
            return GetDotNetVersion("");
        }

        /// <summary>
        /// Returns the version of the .NET framework running on this computer.
        /// </summary>
        /// <param name="getSpecificVersion">if not empty only the specified version and if empty the highest version will be returned</param>
        /// <returns>A string containing the version of the framework version.</returns>
        public static string GetDotNetVersion(string getSpecificVersion)
        {
            string fv = "unknown";
            string componentsKeyName = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\";
            using (Microsoft.Win32.RegistryKey componentsKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(componentsKeyName))
            {
                try
                {
                    string[] instComps = componentsKey.GetSubKeyNames();
                    ArrayList versions = new ArrayList();

                    foreach (string instComp in instComps)
                    {
                        if (!instComp.StartsWith("v"))
                            continue;

                        bool bFound = false;
                        Microsoft.Win32.RegistryKey key = componentsKey.OpenSubKey(instComp);
                        string version = (string)key.GetValue("Version", "");

                        if (!String.IsNullOrEmpty(version))
                        {
                            versions.Add(version);
                            continue;
                        }
                        else
                        {
                            foreach (string strRegKey in key.GetSubKeyNames())
                            {
                                Microsoft.Win32.RegistryKey strKey = key.OpenSubKey(strRegKey);
                                version = (string)strKey.GetValue("Version", "");
                                if (!String.IsNullOrEmpty(version))
                                {
                                    bFound = true;
                                    versions.Add(version);
                                }
                            }
                        }
                        if (!bFound)
                        {
                            string install = key.GetValue("Install", "").ToString();
                            version = instComp.Substring(1);
                            if (!version.Equals("4") && install.Equals("1"))
                                versions.Add(version);
                        }
                    }
                    versions.Sort();

                    foreach (string version in versions)
                    {
                        if (!String.IsNullOrEmpty(getSpecificVersion) && (version.StartsWith(getSpecificVersion) || DotNetVersionFormated(version).StartsWith(getSpecificVersion)))
                            return DotNetVersionFormated(version);
                        fv = version;
                    }

                    if (!String.IsNullOrEmpty(getSpecificVersion))
                        return null;
                }
                catch
                {
                    return null;
                }
            }
            return DotNetVersionFormated(fv);
        }

        /// <summary>
        /// Returns the name of the dotNet Framework formated
        /// </summary>
        /// <returns>A string containing the dotNet Framework</returns>
        /// 
        public static string DotNetVersionFormated(string dotNetVersion)
        {
            string dnvf = "unknown";
            string major = string.Empty;
            string minor = string.Empty;
            string build = string.Empty;
            string revision = string.Empty;

            try
            {
                if (dotNetVersion != "unknown")
                {
                    string[] versions = dotNetVersion.Split('.');

                    if (versions.Length >= 1)
                        major = versions[0].ToString();
                    if (versions.Length > 1)
                        minor = versions[1].ToString();
                    if (versions.Length > 2)
                        build = versions[2].ToString();
                    if (versions.Length > 3)
                        revision = versions[3].ToString();

                    switch (major)
                    {
                        case "1":
                            {
                                switch (minor)
                                {
                                    case "0":
                                        {
                                            switch (revision)
                                            {
                                                case "209": dnvf = "1.0 SP1"; break;
                                                case "288": dnvf = "1.0 SP2"; break;
                                                case "6018": dnvf = "1.0 SP3"; break;
                                                default: dnvf = "1.0"; break;
                                            }
                                        }
                                        break;
                                    case "1":
                                        {
                                            switch (revision)
                                            {
                                                case "2032":
                                                case "2300": dnvf = "1.1 SP1"; break;
                                                default: dnvf = "1.1"; break;
                                            }
                                        }
                                        break;
                                    default: dnvf = "1.x"; break;
                                }
                                break;
                            }
                        case "2":
                            {
                                switch (revision)
                                {
                                    case "1433":
                                    case "1434": dnvf = "2.0 SP1"; break;
                                    case "2407":
                                    case "3053":
                                    case "3074":
                                    case "4016":
                                    case "4927": dnvf = "2.0 SP2"; break;
                                    default: dnvf = "2.0"; break;
                                }
                            }
                            break;
                        case "3":
                            {
                                switch (minor)
                                {
                                    case "0":
                                        {
                                            switch (revision)
                                            {
                                                case "648": dnvf = "3.0 SP1"; break;
                                                case "1453":
                                                case "2123":
                                                case "4000":
                                                case "4037":
                                                case "4902": // Se7en
                                                case "4926": // Se7en
                                                    dnvf = "3.0 SP2"; break;
                                                default: dnvf = "3.0"; break;
                                            }
                                        }
                                        break;
                                    case "5":
                                        {
                                            switch (revision)
                                            {
                                                case "4926": // Se7en
                                                case "1": dnvf = "3.5 SP1"; break;
                                                default: dnvf = "3.5"; break;
                                            }
                                        }
                                        break;
                                    default: dnvf = "3.x"; break;
                                }
                            }
                            break;
                        case "4":
                            {
                                switch (minor)
                                {
                                    case "0": dnvf = "4.0"; break;
                                    case "5":
                                        {
                                            switch (build)
                                            {
                                                case "50709": dnvf = "4.5"; break;
                                                case "51641": dnvf = "4.5.1"; break;
                                                case "51650": dnvf = "4.5.2"; break;
                                                default: dnvf = "4.5.x"; break;
                                            }
                                        }
                                        break;
                                    case "6":
                                        {
                                            switch (build)
                                            {
                                                case "00079": dnvf = "4.6"; break;
                                                default: dnvf = "4.6.x"; break;
                                            }
                                        }
                                        break;
                                    default: dnvf = "4." + minor; break;
                                }
                            }
                            break;
                        default: dnvf = major + ".x"; break;
                    }

                    if (string.IsNullOrEmpty(revision))
                        dnvf += " (" + major + "." + minor + "." + build + ")";
                    else
                        dnvf += " (" + major + "." + minor + "." + build + "." + revision + ")";
                }
            }
            catch
            {
                dnvf = "unknown: " + dotNetVersion;
            }
            return dnvf;
        }
        #endregion

        /// <value>
        /// Returns true on Windows XP or newer operating systems; otherwise, false.
        /// </value>
        public static bool IsWindowsXPOrNewer
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT
                    && ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
                        || Environment.OSVersion.Version.Major > 5);
            }
        }

        /// <value>
        /// Returns true on Windows Vista or newer operating systems; otherwise, false.
        /// </value>
        public static bool IsWindowsVistaOrNewer
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6;
            }
        }

        /// <value>
        /// Returns true on Windows 7 or newer operating systems; otherwise, false.
        /// </value>
        public static bool IsWindows7OrNewer
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT
                    && ((Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1)
                        || Environment.OSVersion.Version.Major > 6);
            }
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        #region process/thread handling

        /// <summary>
        /// Gets the desired thread priority based on the process priority
        /// </summary>
        /// <param name="priority">the process priority</param>
        /// <returns>th desired thread priority</returns>
        public static ThreadPriority GetThreadPriority(WorkerPriorityType priority)
        {
            ThreadPriority threadPriority;
            switch (priority)
            {
                case WorkerPriorityType.BELOW_NORMAL:
                    threadPriority = ThreadPriority.BelowNormal;
                    break;
                case WorkerPriorityType.NORMAL:
                    threadPriority = ThreadPriority.Normal;
                    break;
                case WorkerPriorityType.ABOVE_NORMAL:
                    threadPriority = ThreadPriority.AboveNormal;
                    break;
                default:
                    threadPriority = ThreadPriority.Lowest;
                    break;
            }
            return threadPriority;
        }


        [DllImport("ntdll", CharSet = CharSet.Unicode)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetPriorityClass(IntPtr handle, uint priorityClass);

        [DllImport("Kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetCurrentThread();

        [DllImport("Kernel32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetThreadPriority(IntPtr hThread, int nPriority);
        private const int PROCESS_INFORMATION_MEMORY_PRIORITY = 0x27;
        private const int PROCESS_INFORMATION_IO_PRIORITY = 0x21;
        private const int PRIORITY_MEMORY_NORMAL = 5;
        private const int PRIORITY_MEMORY_LOW = 3;
        private const int PRIORITY_MEMORY_VERY_LOW = 1;
        private const int PRIORITY_IO_NORMAL = 2;
        private const int PRIORITY_IO_LOW = 1;
        private const int PRIORITY_IO_VERY_LOW = 0;
        private const uint PROCESS_MODE_BACKGROUND_BEGIN = 0x00100000;
        private const uint PROCESS_MODE_BACKGROUND_END = 0x00200000;
        private const int THREAD_MODE_BACKGROUND_BEGIN = 0x00010000;
        private const int THREAD_MODE_BACKGROUND_END = 0x00020000;

        public static void KillProcess(Process oProc)
        {
            try
            {
                oProc.Kill();
            }
            catch { }
        }

        /// <value>
        /// Sets the process, memory and I/O priority on Windows Vista or newer operating systems
        /// </value>
        public static bool SetProcessPriority(Process oMainProcess, WorkerPriorityType processPriority, bool lowIOPriority, int iMinimumChildProcessCount)
        {
            try
            {
                if (oMainProcess == null || oMainProcess.HasExited)
                    return false;

                ProcessPriorityClass priority = ProcessPriorityClass.Idle;
                switch (processPriority)
                {
                    case WorkerPriorityType.BELOW_NORMAL:
                        priority = ProcessPriorityClass.BelowNormal;
                        break;
                    case WorkerPriorityType.NORMAL:
                        priority = ProcessPriorityClass.Normal;
                        break;
                    case WorkerPriorityType.ABOVE_NORMAL:
                        priority = ProcessPriorityClass.AboveNormal;
                        break;
                    default:
                        priority = ProcessPriorityClass.Idle;
                        break;
                }

                // get the list of child processes
                // make sure that the required child processes have been started already
                DateTime oCheckStarted = DateTime.Now;
                List<Process> arrProc = new List<Process>();
                do
                {
                    arrProc = GetChildProcesses(oMainProcess);
                    int iCount = 0;
                    foreach (Process oProc in arrProc)
                        if (oProc.ProcessName != "conhost")
                            iCount++;
                    if (iCount >= iMinimumChildProcessCount)
                        break;
                    MeGUI.core.util.Util.Wait(500);
                } while (!oMainProcess.HasExited && oCheckStarted.AddSeconds(60) > DateTime.Now); // wait max. 60 seconds to find all child processes
                arrProc.Insert(0, oMainProcess);

                foreach (Process oProc in arrProc)
                {
                    if (oProc == null || oProc.HasExited)
                        continue;

                    // set process priority
                    oProc.PriorityClass = priority;

                    if (!OSInfo.IsWindowsVistaOrNewer)
                        continue;

                    int prioIO = PRIORITY_IO_NORMAL;
                    int prioMemory = PRIORITY_MEMORY_NORMAL;
                    if (lowIOPriority)
                    {
                        prioIO = PRIORITY_IO_VERY_LOW;
                        prioMemory = PRIORITY_MEMORY_VERY_LOW;
                        SetPriorityClass(oProc.Handle, PROCESS_MODE_BACKGROUND_BEGIN);
                    }
                    else
                        SetPriorityClass(oProc.Handle, PROCESS_MODE_BACKGROUND_END);
                    NtSetInformationProcess(oProc.Handle, PROCESS_INFORMATION_IO_PRIORITY, ref prioIO, Marshal.SizeOf(prioIO));
                    NtSetInformationProcess(oProc.Handle, PROCESS_INFORMATION_MEMORY_PRIORITY, ref prioMemory, Marshal.SizeOf(prioMemory));
                }
            }
            catch
            {
                return false;
            }
            return true;
        }


        public static List<Process> GetChildProcesses(Process process)
        {
            List<Process> children = new List<Process>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher(String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));

            foreach (ManagementObject mo in mos.Get())
            {
                Process oProc = Process.GetProcessById(Convert.ToInt32(mo["ProcessID"]));
                children.Add(oProc);
                children.AddRange(GetChildProcesses(oProc));
            }

            return children;
        }

        /// <value>
        /// Sets the thread background priority on Windows Vista or newer operating systems
        /// </value>
        public static void SetThreadPriority(Thread _thread, bool lowIOPriority)
        {
            if (!OSInfo.IsWindowsVistaOrNewer)
                return;

            if (lowIOPriority)
                SetThreadPriority((IntPtr)GetNativeThreadId(_thread), THREAD_MODE_BACKGROUND_BEGIN);
            else
                SetThreadPriority((IntPtr)GetNativeThreadId(_thread), THREAD_MODE_BACKGROUND_END);
        }

        public static int GetNativeThreadId(Thread thread)
        {
            var f = typeof(Thread).GetField("DONT_USE_InternalThread",
                System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var pInternalThread = (IntPtr)f.GetValue(thread);
            var nativeId = Marshal.ReadInt32(pInternalThread, (IntPtr.Size == 8) ? 0x022C : 0x0160); // found by analyzing the memory
            return nativeId;
        }

        #endregion

        #region start/stop process
        [Flags()]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        public static bool SuspendProcess(Process oMainProcess)
        {
            bool bResult = false;

            try
            {
                List<Process> arrProc = GetChildProcesses(oMainProcess);
                arrProc.Insert(0, oMainProcess);

                foreach (Process oProc in arrProc)
                {
                    if (oProc.HasExited || oProc.ProcessName == string.Empty)
                        continue;

                    foreach (ProcessThread pT in oProc.Threads)
                    {
                        IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                        if (pOpenThread == IntPtr.Zero)
                            continue;

                        SuspendThread(pOpenThread);
                        CloseHandle(pOpenThread);
                        bResult = true;
                    }
                }
            }
            catch (Exception) { }

            return bResult;
        }

        public static bool ResumeProcess(Process oMainProcess)
        {
            bool bResult = false;
            try
            {
                List<Process> arrProc = GetChildProcesses(oMainProcess);
                arrProc.Insert(0, oMainProcess);

                foreach (Process oProc in arrProc)
                {
                    if (oProc.HasExited || oProc.ProcessName == string.Empty)
                        continue;

                    foreach (ProcessThread pT in oProc.Threads)
                    {
                        IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                        if (pOpenThread == IntPtr.Zero)
                            continue;

                        var suspendCount = 0;
                        do
                        {
                            suspendCount = ResumeThread(pOpenThread);
                        } while (suspendCount > 0);

                        CloseHandle(pOpenThread);
                        bResult = true;
                    }
                }
            }
            catch (Exception) { }

            return bResult;
        }

        #endregion
    }
}