using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrcDsAutoShutdown
{
    public partial class TrayIconForm : Form
    {
        protected static SettingsWindow settingsWindowInstance;

        public TrayIconForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Visible = false;
            this.ShowInTaskbar = false;

            this.trayIcon.ShowBalloonTip(1000, "FRC DS Auto Shutdown", "Application is running in the system tray.", ToolTipIcon.Info);
            var teamIPNetwork = ExternalProcessManager.GetTeamNetworkIPAddressFromDSTeamNumberIni();
            if (teamIPNetwork.Equals(IPAddress.None))
            {
                this.trayIcon.ShowBalloonTip(
                    1000,
                    "FRC DS Auto Shutdown",
                    "Could not determine team number/IP from DS settings file.",
                    ToolTipIcon.Warning
                );
            }

            settingsWindowInstance = new SettingsWindow();
            DhcpManager.StartListening(settingsWindowInstance.ToggleDsIpLabel);
        }

        public void OpenSettingsWindow()
        {
            try
            {
                settingsWindowInstance.ShowDialog();
            }
            catch (InvalidOperationException)
            {
                settingsWindowInstance.BringToFront();
            }
        }

        private void trayOpenSettingsMenuItem_Click(object sender, EventArgs e)
        {
            OpenSettingsWindow();
        }

        private void trayExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void TrayIconForm_Load(object sender, EventArgs e)
        {

        }

        public void ListenForPipeCommands()
        {
            while (true)
            {
                using (var server = new NamedPipeServerStream(Program.pipeName, PipeDirection.In))
                {
                    server.WaitForConnection();
                    using (var reader = new StreamReader(server))
                    {
                        var command = reader.ReadLine();
                        if (command != null && command.Equals("--settings"))
                        {
                            OpenSettingsWindow();
                        }
                    }
                }
            }
        }

        public static void ShowBalloonTip(string title, string message, ToolTipIcon? icon = null)
        {
            var trayForm = Application.OpenForms.OfType<TrayIconForm>().FirstOrDefault();
            if (trayForm != null)
            {
                trayForm.trayIcon.ShowBalloonTip(1000, title, message, icon ?? ToolTipIcon.Info);
            }
        }
    }
}
