using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrcDsAutoShutdown
{
    internal static class Program
    {
        static Mutex mutex = new Mutex(true, Assembly.GetExecutingAssembly().FullName);
        public static string pipeName = Assembly.GetExecutingAssembly().FullName.Replace(" ", "") + "Pipe";
        public static TrayIconForm trayIconForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            const string appName = "FrcDsAutoShutdown";
            bool createdNew;

            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                if (args.Contains("--settings"))
                {
                    using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
                    {
                        try
                        {
                            client.Connect(1000);
                            using (var writer = new StreamWriter(client))
                            {
                                writer.WriteLine("--settings");
                                writer.Flush();
                            }
                        }
                        catch (TimeoutException) { }
                    }
                }
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrayIconForm());
            Application.ApplicationExit += (s, e) => mutex.ReleaseMutex();

            while (true)
            {
                if (trayIconForm != null)
                {
                    Task.Run(() => trayIconForm.ListenForPipeCommands());
                    if (args.Contains("--settings"))
                    {
                        trayIconForm.OpenSettingsWindow();
                    }
                    break;
                }
            }
        }

        public static string InstallToSystem()
        {
            string targetPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "FrcDsAutoShutdown",
                "FrcDsAutoShutdown.exe"
            );
            if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            }
            File.Copy(Application.ExecutablePath, targetPath, true);
            return targetPath;
        }

        public static void InstallSettingsShortcutToPublicDesktop()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            string shortcutLocation = Path.Combine(desktopPath, "FRC DS Auto Shutdown Settings.lnk");
            var shell = new IWshRuntimeLibrary.WshShell();
            var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutLocation);
            shortcut.Description = "Shortcut to FRC DS Auto Shutdown Settings";
            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.Arguments = "--settings";
            shortcut.Save();
        }

        public static bool IsInstalledToSystem()
        {
            string targetPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "FrcDsAutoShutdown",
                "FrcDsAutoShutdown.exe"
            );
            return File.Exists(targetPath);
        }
    }
}
