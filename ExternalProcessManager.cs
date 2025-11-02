using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FrcDsAutoShutdown
{
    internal class ExternalProcessManager
    {
        public static void KillProcessesAndShutDownIfAppEnabled()
        {
            if (!Properties.Settings.Default.ShutdownEnabled)
            {
                TrayIconForm.ShowBalloonTip("FRC DS Auto Shutdown", "THe app would have shut down now.");
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

        public static void KillProcesses()
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

        public static void ShutdownWorkstation()
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
            Console.WriteLine($"DS Data Storage ini Path: {iniFilePath}");
            if (!System.IO.File.Exists(iniFilePath))
            {
                return IPAddress.None;
            }
            var parser = new IniDataParser();
            IniData data = parser.Parse(File.ReadAllText(iniFilePath));
            var setupSection = data.Sections.Where(s => s.SectionName.Equals("Setup")).First();
            if (setupSection == null) return IPAddress.None;
            var teamNumberItem = setupSection.Keys.Where(k => k.KeyName.Equals("TeamNumber") || k.KeyName.Equals("Team Number")).First();
            if (teamNumberItem == null) return IPAddress.None;
            string teamNumberStr = teamNumberItem.Value.ToString().Trim().Trim('"');
            Console.WriteLine($"Team Number: {teamNumberStr}");
            if (int.TryParse(teamNumberStr, out int teamNumber))
            {
                return TeamNumberToIPAddress(teamNumber);
            }
            return IPAddress.None;
        }

        private static IPAddress TeamNumberToIPAddress(int teamNumber)
        {
            string teamStr = teamNumber.ToString();

            if (teamStr.Length == 4)
            {
                // Original format: 10.TE.AM.2
                string te = teamStr.Substring(0, 2).TrimStart('0');
                string am = teamStr.Substring(2, 2).TrimStart('0');
                return IPAddress.Parse($"10.{te}.{am}.2");
            }
            else if (teamStr.Length == 5)
            {
                // New format: 10.TEA.Mf.0
                string tea = teamStr.Substring(0, 3).TrimStart('0');
                string mf = teamStr.Substring(3, 2).TrimStart('0');
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

    internal class MessageSender
    {
        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
            IntPtr hServer,
            int SessionId,
            string pTitle,
            int TitleLength,
            string pMessage,
            int MessageLength,
            int Style,
            int Timeout,
            out int pResponse,
            bool bWait);

        [DllImport("wtsapi32.dll")]
        static extern IntPtr WTSOpenServer(string pServerName);

        [DllImport("wtsapi32.dll")]
        static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSQuerySessionInformation(
            IntPtr hServer,
            int sessionId,
            WTSInfoClass wtsInfoClass,
            out IntPtr ppBuffer,
            out int pBytesReturned);

        [DllImport("wtsapi32.dll")]
        static extern bool WTSEnumerateSessions(
            IntPtr hServer,
            int Reserved,
            int Version,
            out IntPtr ppSessionInfo,
            out int pCount);

        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pointer);

        enum WTSInfoClass
        {
            WTSUserName = 5,
            WTSDomainName = 7
        }

        [StructLayout(LayoutKind.Sequential)]
        struct WTS_SESSION_INFO
        {
            public int SessionID;
            public string pWinStationName;
            public int State;
        }

        public static void SendMessageToAll(string title, string message)
        {
            IntPtr serverHandle = WTSOpenServer(Environment.MachineName);
            IntPtr ppSessionInfo = IntPtr.Zero;
            int sessionCount = 0;

            if (WTSEnumerateSessions(serverHandle, 0, 1, out ppSessionInfo, out sessionCount))
            {
                int dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                for (int i = 0; i < sessionCount; i++)
                {
                    IntPtr currentSession = new IntPtr(ppSessionInfo.ToInt64() + i * dataSize);
                    WTS_SESSION_INFO sessionInfo = (WTS_SESSION_INFO)Marshal.PtrToStructure(currentSession, typeof(WTS_SESSION_INFO));

                    int response;
                    WTSSendMessage(serverHandle, sessionInfo.SessionID,
                        title, title.Length,
                        message, message.Length,
                        0, 10, out response, false);
                }
                WTSFreeMemory(ppSessionInfo);
            }

            WTSCloseServer(serverHandle);
        }
    }
}
