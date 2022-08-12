// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
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
using System.Runtime.InteropServices;

namespace MeGUI
{
    public abstract class Shutdown
    {
        private static Shutdown instance;

        static Shutdown()
        {
            PlatformID id = Environment.OSVersion.Platform;
            if (id == PlatformID.Win32NT || id == PlatformID.Win32S || id == PlatformID.Win32Windows || id == PlatformID.WinCE)
                instance = new WindowsShutdown();
            else
                instance = new NullShutdown();
        }

        public abstract bool DoShutdown();

        public static bool shutdown()
        {
            return instance.DoShutdown();
        }

        private class NullShutdown : Shutdown
        {
            public override bool DoShutdown()
            {
                // FIXME: Do nothing would be best...
                return false;
            }
        }

        /// <summary>
        /// Summary description for Shutdown.
        /// </summary>
        private class WindowsShutdown : Shutdown
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            internal struct TokPriv1Luid
            {
                public int Count;
                public long Luid;
                public int Attr;
            }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int flg, uint rea);

        private const int SE_PRIVILEGE_ENABLED = 0x00000002;
        private const int TOKEN_QUERY = 0x00000008;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        private const int EWX_SHUTDOWN = 0x00000001;
        private const int EWX_REBOOT = 0x00000002;
        private const int EWX_FORCE = 0x00000004;
        private const int EWX_POWEROFF = 0x00000008;
        private const int EWX_FORCEIFHUNG = 0x00000010;

        private const uint SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000;
        private const uint SHTDN_REASON_MAJOR_APPLICATION = 0x00040000;
        private const uint SHTDN_REASON_MAJOR_SYSTEM = 0x00050000;

        private const uint SHTDN_REASON_MINOR_MAINTENANCE = 0x00000001;
        private const uint SHTDN_REASON_MINOR_NONE = 0x000000ff;
        private const uint SHTDN_REASON_MINOR_UPGRADE = 0x00000003;

        private const uint SHTDN_REASON_FLAG_PLANNED = 0x80000000;

        public WindowsShutdown()
        {
        }

        public override bool DoShutdown()
        {
            bool success;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            success = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            success = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            success = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            return ExitWindowsEx(EWX_SHUTDOWN + EWX_FORCE, SHTDN_REASON_MAJOR_APPLICATION | SHTDN_REASON_MINOR_NONE | SHTDN_REASON_FLAG_PLANNED);
        }
     }
  }
}