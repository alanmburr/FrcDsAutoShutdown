using IniParser;
using IniParser.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace FrcDsAutoShutdown
{
    internal class ExternalProcessManager
    {
        public static void KillProcessesAndShutDownIfAppEnabled()
        {
            if (!Properties.Settings.Default.ShutdownEnabled)
            {
                return;
            }
            if (Process.GetProcessesByName("DriverStation").Length <= 0)
            {
                // We should abort if Driver Station is not running
                return;
            }
            KillProcesses();
            ShutdownWorkstation();
        }

        private static void KillProcesses()
        {
            if (!Properties.Settings.Default.ShutdownEnabled)
            {
                return;
            }
            foreach (var exeName in Properties.Settings.Default.KillProgramExecutableNames)
            {
                var processes = Process.GetProcessesByName(exeName);
                foreach (var process in processes)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception) { }
                }
            }
        }

        private static void ShutdownWorkstation()
        {
            if (!Properties.Settings.Default.ShutdownEnabled)
            {
                return;
            }
            ShutdownHelper.ForceShutdown();
        }

        public static IPAddress GetTeamNetworkIPAddressFromDSTeamNumberIni()
        {
            string iniFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\FRC\FRC DS Data Storage.ini";
            if (!System.IO.File.Exists(iniFilePath))
            {
                return IPAddress.None;
            }
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniFilePath);
            string teamNumberStr = data["Setup"]["Team Number"];
            if (int.TryParse(teamNumberStr, out int teamNumber))
            {
                return TeamNumberToIPAddress(teamNumber);
            }
            return IPAddress.None;
        }

        private static IPAddress TeamNumberToIPAddress(int teamNumber)
        {
            string teamStr = teamNumber.ToString().PadLeft(5, '0');

            if (teamStr.Length == 4)
            {
                // Original format: 10.TE.AM.2
                string te = teamStr.Substring(0, 2);
                string am = teamStr.Substring(2, 2);
                return IPAddress.Parse($"10.{te}.{am}.2");
            }
            else if (teamStr.Length == 5)
            {
                // New format: 10.TEA.Mf.0
                string tea = teamStr.Substring(0, 3);
                string mf = teamStr.Substring(3, 2);
                return IPAddress.Parse($"10.{tea}.{mf}.0");
            }
            else
            {
                throw new ArgumentException("Team number must be 4 or 5 digits.");
            }
        }

    }

    internal class ShutdownHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out long lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public long Luid;
            public int Attributes;
        }

        const uint SE_PRIVILEGE_ENABLED = 0x00000002;
        const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        const uint TOKEN_QUERY = 0x0008;
        const uint EWX_POWEROFF = 0x00000008;
        const uint EWX_FORCE = 0x00000004;
        const uint EWX_SHUTDOWN = 0x00000001;

        public static void ForceShutdown()
        {
            // Enable shutdown privilege
            OpenProcessToken(System.Diagnostics.Process.GetCurrentProcess().Handle,
                TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out IntPtr tokenHandle);

            LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out long luid);

            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES
            {
                PrivilegeCount = 1,
                Luid = luid,
                Attributes = (int)SE_PRIVILEGE_ENABLED
            };

            AdjustTokenPrivileges(tokenHandle, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);

            // Force shutdown
            ExitWindowsEx(EWX_SHUTDOWN | EWX_FORCE, 0);
        }
    }
}
