using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace FrcDsAutoShutdown
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.enableApplicationCheckbox.Checked = Properties.Settings.Default.ShutdownEnabled;
            var killNames = Properties.Settings.Default.KillProgramExecutableNames.Cast<string>().ToList();
            if (killNames.FindAll(s => s.ToLower().IndexOf("DriverStation") == 0).Count <= 0)
            {
                killNames.Insert(0, "DriverStation");
            }
            foreach (var exeName in killNames.Distinct())
            {
                this.killProgramsDataGrid.Rows.Add(exeName);
            }
            FixDuplicatesInProgramsToKill();

            var teamIPNetwork = ExternalProcessManager.GetTeamNetworkIPAddressFromDSTeamNumberIni();
            if (teamIPNetwork.Equals(IPAddress.None))
            {
                this.unknownIpLabel.Text = "Could not determine IP!";
                this.unknownIpLabel.ForeColor = System.Drawing.Color.Red;
                this.unknownIpLabel.Visible = true;
            }
            else
            {
                this.unknownIpLabel.Text = $"Team IP Address: {teamIPNetwork.ToString()}";
                this.unknownIpLabel.ForeColor = System.Drawing.Color.Green;
                this.unknownIpLabel.Visible = true;
            }


            var isInstalled = Program.IsInstalledToSystem();
            if (isInstalled)
            {
                this.makeDesktopLnkButton.Enabled = isInstalled;
                this.makeDesktopLnkTooltip.SetToolTip(this.makeDesktopLnkButton, null);
            }
        }
        private void killProgramsDataGrid_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            var row = e.Row;
            var cell = row.Cells[0];
            string cellValue = cell.Value == null ? "" : cell.Value.ToString();
            var exeName = string.IsNullOrEmpty(cellValue) ? "" : cellValue.Trim();
            var exeNameSplitRemove = exeName.LastIndexOf(".exe");
            exeName = exeName.Substring(0, exeNameSplitRemove == -1 ? exeName.Length : exeNameSplitRemove);
            Properties.Settings.Default.KillProgramExecutableNames.Add(exeName);
            Properties.Settings.Default.Save();
            //e.Row.Cells[0].Value = exeName;
            //FixDuplicatesInProgramsToKill();
        }

        private void killProgramsDataGrid_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            var exeName = e.Row.Cells[0].Value.ToString().Trim();
            var exeNameSplitRemove = exeName.LastIndexOf(".exe");
            exeName = exeName.Substring(0, exeNameSplitRemove == -1 ? exeName.Length : exeNameSplitRemove);
            Properties.Settings.Default.KillProgramExecutableNames.Remove(exeName);
            Properties.Settings.Default.Save();
            //e.Row.Cells[0].Value = exeName;
            //FixDuplicatesInProgramsToKill();
        }

        private void enableApplicationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShutdownEnabled = this.enableApplicationCheckbox.Checked;
            Properties.Settings.Default.Save();
        }

        private void quitButton_DoubleClick(object sender, EventArgs e)
        {
            TrayIconForm.Exit();
        }

        private void registerAutoStartCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RegisterAutoStartup = this.registerAutoStartCheckbox.Checked;
            Properties.Settings.Default.Save();

            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (this.registerAutoStartCheckbox.Checked)
            {
                key.SetValue("FRC DS Auto Shutdown", Application.ExecutablePath.ToString());
            }
            else
            {
                key.DeleteValue("FRC DS Auto Shutdown", false);
            }
        }

        private void FixDuplicatesInProgramsToKill()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            var seen = new HashSet<string>(comparer);
            var unique = new StringCollection();

            // Guard against null grid
            if (this.killProgramsDataGrid == null || this.killProgramsDataGrid.Rows == null)
            {
                // Ensure settings collection exists
                if (Properties.Settings.Default.KillProgramExecutableNames == null)
                {
                    Properties.Settings.Default.KillProgramExecutableNames = new StringCollection();
                    Properties.Settings.Default.Save();
                }
                return;
            }

            // Iterate backwards to allow safe removal of rows
            for (int i = this.killProgramsDataGrid.Rows.Count - 1; i >= 0; i--)
            {
                var row = this.killProgramsDataGrid.Rows[i];
                if (row == null || row.IsNewRow)
                {
                    // Skip placeholder/new row
                    continue;
                }

                string exeName = string.Empty;
                if (row.Cells != null && row.Cells.Count > 0 && row.Cells[0].Value != null)
                {
                    exeName = row.Cells[0].Value.ToString().Trim();
                }

                if (string.IsNullOrWhiteSpace(exeName) || seen.Contains(exeName))
                {
                    // Remove duplicate or empty entries
                    this.killProgramsDataGrid.Rows.RemoveAt(i);
                }
                else
                {
                    seen.Add(exeName);
                    unique.Add(exeName);
                }
            }
            foreach (string exeName in Properties.Settings.Default.KillProgramExecutableNames)
            {
                if (!string.IsNullOrWhiteSpace(exeName) && seen.Add(exeName))
                {
                    unique.Add(exeName);
                }
            }
            Properties.Settings.Default.KillProgramExecutableNames = unique;
            Properties.Settings.Default.Save();
        }

        public void ToggleDsIpLabel(object sender, ElapsedEventArgs e)
        {
            var isVisible = DhcpManager.TeamIPNetwork.Equals(IPAddress.None);
            if (this.IsHandleCreated) this.Invoke(new Action(() => {
                this.unknownIpLabel.Visible = isVisible;
            }));
        }

        private void installToProgFilesButton_Click(object sender, EventArgs e)
        {
            string exePath = Program.InstallToSystem();
            MessageBox.Show(
                "Installed to system. The application needs to restart.",
                Assembly.GetExecutingAssembly().FullName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C timeout /T 3 /NOBREAK >nul && start \"\" \"{exePath}\"",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                Verb = "runas"
            };
            Process.Start(psi);
            Application.Exit();
        }

        private void makeDesktopLnkButton_Click(object sender, EventArgs e)
        {
            Program.InstallSettingsShortcutToPublicDesktop();
        }

        private void shutDownPcBtn_DoubleClick(object sender, EventArgs e)
        {
            ExternalProcessManager.ShutdownWorkstation();
        }
    }
}
