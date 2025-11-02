using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrcDsAutoShutdown
{
    internal class DSEventsWatchdog
    {
        public static bool Configured { get; private set; } = false;
        private static FileSystemWatcher Watcher;

        public static bool Configure()
        {
            if (Configured) return Configured;
            try
            {
                Watcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\FRC\Log Files\DSLogs")
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                    Filter = "*.dsevents"
                };
            }
            catch (ArgumentException)
            {
                return false;
            }
            Configured = true;
            return Configured;
        }

        public static void StartWatching()
        {
            if (!Configured) return;
            Watcher.Created += LogFileChanged;
            Watcher.Changed += LogFileChanged;
            Watcher.EnableRaisingEvents = true;
        }

        public static void StopWatching()
        {
            if (!Configured) return;
            Watcher.Created -= LogFileChanged;
            Watcher.Changed -= LogFileChanged;
            Watcher.EnableRaisingEvents = false;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void Dispose()
        {
            Watcher?.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static async void LogFileChanged(object sender, FileSystemEventArgs e)
        {
            var fileName = e.FullPath;
            if (fileName == null) return;
            var dsFileReader = new DSEventReader(fileName);
            dsFileReader.Read();
            bool fmsDisconnected = dsFileReader.Entries
                .Where(dse => dse.Data.Contains("FMS Disconnect"))
                .Count() > 0;
            dsFileReader = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (fmsDisconnected)
            {
                MessageSender.SendMessageToAll($"FRC DS Auto Shutdown - {DateTime.Now.ToString()}", "This would shut down the computer now (DS file read)");
                await Task.Delay(2500);
                ExternalProcessManager.KillProcessesAndShutDownIfAppEnabled();
            }
        }
    }
}
