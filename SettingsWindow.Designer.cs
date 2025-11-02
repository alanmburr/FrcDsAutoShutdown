using System.Reflection;

namespace FrcDsAutoShutdown
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.makeDesktopLnkButton = new System.Windows.Forms.Button();
            this.installToProgFilesButton = new System.Windows.Forms.Button();
            this.unknownIpLabel = new System.Windows.Forms.Label();
            this.registerAutoStartCheckbox = new System.Windows.Forms.CheckBox();
            this.ipAddressSourceLabel = new System.Windows.Forms.Label();
            this.enableApplicationCheckbox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.autosaveLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.shutDownPcButton = new FrcDsAutoShutdown.DoubleClickButton();
            this.doubleClickButton1 = new FrcDsAutoShutdown.DoubleClickButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.killPrograms = new System.Windows.Forms.Label();
            this.killProgramsDataGrid = new System.Windows.Forms.DataGridView();
            this.ExecutableNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.makeDesktopLnkTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.killProgramsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.makeDesktopLnkButton);
            this.groupBox1.Controls.Add(this.installToProgFilesButton);
            this.groupBox1.Controls.Add(this.unknownIpLabel);
            this.groupBox1.Controls.Add(this.registerAutoStartCheckbox);
            this.groupBox1.Controls.Add(this.ipAddressSourceLabel);
            this.groupBox1.Controls.Add(this.enableApplicationCheckbox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1035, 147);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enable";
            // 
            // makeDesktopLnkButton
            // 
            this.makeDesktopLnkButton.Enabled = false;
            this.makeDesktopLnkButton.Location = new System.Drawing.Point(205, 76);
            this.makeDesktopLnkButton.Margin = new System.Windows.Forms.Padding(4);
            this.makeDesktopLnkButton.Name = "makeDesktopLnkButton";
            this.makeDesktopLnkButton.Size = new System.Drawing.Size(245, 28);
            this.makeDesktopLnkButton.TabIndex = 5;
            this.makeDesktopLnkButton.Text = "Add Settings shortcut to desktop";
            this.makeDesktopLnkTooltip.SetToolTip(this.makeDesktopLnkButton, "You must first install the program to Program Files.");
            this.makeDesktopLnkButton.UseVisualStyleBackColor = true;
            this.makeDesktopLnkButton.Click += new System.EventHandler(this.makeDesktopLnkButton_Click);
            // 
            // installToProgFilesButton
            // 
            this.installToProgFilesButton.Location = new System.Drawing.Point(16, 76);
            this.installToProgFilesButton.Margin = new System.Windows.Forms.Padding(4);
            this.installToProgFilesButton.Name = "installToProgFilesButton";
            this.installToProgFilesButton.Size = new System.Drawing.Size(181, 28);
            this.installToProgFilesButton.TabIndex = 4;
            this.installToProgFilesButton.Text = "Install to Program Files";
            this.installToProgFilesButton.UseVisualStyleBackColor = true;
            this.installToProgFilesButton.Click += new System.EventHandler(this.installToProgFilesButton_Click);
            // 
            // unknownIpLabel
            // 
            this.unknownIpLabel.AutoSize = true;
            this.unknownIpLabel.BackColor = System.Drawing.SystemColors.Control;
            this.unknownIpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unknownIpLabel.ForeColor = System.Drawing.Color.Red;
            this.unknownIpLabel.Location = new System.Drawing.Point(711, 111);
            this.unknownIpLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.unknownIpLabel.Name = "unknownIpLabel";
            this.unknownIpLabel.Size = new System.Drawing.Size(154, 17);
            this.unknownIpLabel.TabIndex = 3;
            this.unknownIpLabel.Text = "Could not determine IP!";
            this.unknownIpLabel.Visible = false;
            // 
            // registerAutoStartCheckbox
            // 
            this.registerAutoStartCheckbox.AutoSize = true;
            this.registerAutoStartCheckbox.Location = new System.Drawing.Point(16, 52);
            this.registerAutoStartCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.registerAutoStartCheckbox.Name = "registerAutoStartCheckbox";
            this.registerAutoStartCheckbox.Size = new System.Drawing.Size(520, 20);
            this.registerAutoStartCheckbox.TabIndex = 2;
            this.registerAutoStartCheckbox.Text = "Register the app to start when the system starts (enable/disable via Task Manager" +
    ")";
            this.registerAutoStartCheckbox.UseVisualStyleBackColor = true;
            this.registerAutoStartCheckbox.CheckedChanged += new System.EventHandler(this.registerAutoStartCheckbox_CheckedChanged);
            // 
            // ipAddressSourceLabel
            // 
            this.ipAddressSourceLabel.AutoSize = true;
            this.ipAddressSourceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipAddressSourceLabel.Location = new System.Drawing.Point(12, 111);
            this.ipAddressSourceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ipAddressSourceLabel.Name = "ipAddressSourceLabel";
            this.ipAddressSourceLabel.Size = new System.Drawing.Size(712, 17);
            this.ipAddressSourceLabel.TabIndex = 1;
            this.ipAddressSourceLabel.Text = "Team number\'s IP Address is automatically retrieved from DS. Please make sure you" +
    "r team number is set in DS.";
            // 
            // enableApplicationCheckbox
            // 
            this.enableApplicationCheckbox.AutoSize = true;
            this.enableApplicationCheckbox.Location = new System.Drawing.Point(16, 23);
            this.enableApplicationCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.enableApplicationCheckbox.Name = "enableApplicationCheckbox";
            this.enableApplicationCheckbox.Size = new System.Drawing.Size(520, 20);
            this.enableApplicationCheckbox.TabIndex = 0;
            this.enableApplicationCheckbox.Text = "Should the computer be shut down and specified apps be killed when time expires?";
            this.enableApplicationCheckbox.UseVisualStyleBackColor = true;
            this.enableApplicationCheckbox.CheckedChanged += new System.EventHandler(this.enableApplicationCheckbox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(16, 15, 16, 15);
            this.panel1.Size = new System.Drawing.Size(1067, 645);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.autosaveLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.copyrightLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 598);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(1035, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1035, 34);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // autosaveLabel
            // 
            this.autosaveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.autosaveLabel.AutoSize = true;
            this.autosaveLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autosaveLabel.Location = new System.Drawing.Point(349, 8);
            this.autosaveLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.autosaveLabel.Name = "autosaveLabel";
            this.autosaveLabel.Size = new System.Drawing.Size(337, 17);
            this.autosaveLabel.TabIndex = 3;
            this.autosaveLabel.Text = "Settings save automatically";
            this.autosaveLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(868, 9);
            this.copyrightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(163, 16);
            this.copyrightLabel.TabIndex = 2;
            this.copyrightLabel.Text = "Copyright © 2025 Alan Burr";
            this.copyrightLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.shutDownPcButton);
            this.panel2.Controls.Add(this.doubleClickButton1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(339, 28);
            this.panel2.TabIndex = 4;
            // 
            // shutDownPcButton
            // 
            this.shutDownPcButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.shutDownPcButton.ForeColor = System.Drawing.Color.Red;
            this.shutDownPcButton.Location = new System.Drawing.Point(202, 0);
            this.shutDownPcButton.Name = "shutDownPcButton";
            this.shutDownPcButton.Size = new System.Drawing.Size(137, 28);
            this.shutDownPcButton.TabIndex = 6;
            this.shutDownPcButton.Text = "SHUT DOWN PC";
            this.shutDownPcButton.UseVisualStyleBackColor = true;
            this.shutDownPcButton.DoubleClick += new System.EventHandler(this.shutDownPcBtn_DoubleClick);
            // 
            // doubleClickButton1
            // 
            this.doubleClickButton1.Location = new System.Drawing.Point(1, 0);
            this.doubleClickButton1.Margin = new System.Windows.Forms.Padding(4);
            this.doubleClickButton1.Name = "doubleClickButton1";
            this.doubleClickButton1.Size = new System.Drawing.Size(171, 28);
            this.doubleClickButton1.TabIndex = 5;
            this.doubleClickButton1.Text = "Double-click to Quit";
            this.doubleClickButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.killPrograms);
            this.groupBox2.Controls.Add(this.killProgramsDataGrid);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(16, 162);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(1035, 432);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Programs to kill";
            // 
            // killPrograms
            // 
            this.killPrograms.AutoSize = true;
            this.killPrograms.Dock = System.Windows.Forms.DockStyle.Top;
            this.killPrograms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.killPrograms.Location = new System.Drawing.Point(4, 19);
            this.killPrograms.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.killPrograms.Name = "killPrograms";
            this.killPrograms.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.killPrograms.Size = new System.Drawing.Size(885, 39);
            this.killPrograms.TabIndex = 1;
            this.killPrograms.Text = resources.GetString("killPrograms.Text");
            // 
            // killProgramsDataGrid
            // 
            this.killProgramsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.killProgramsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExecutableNameColumn});
            this.killProgramsDataGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.killProgramsDataGrid.Location = new System.Drawing.Point(4, 60);
            this.killProgramsDataGrid.Margin = new System.Windows.Forms.Padding(4);
            this.killProgramsDataGrid.Name = "killProgramsDataGrid";
            this.killProgramsDataGrid.RowHeadersWidth = 51;
            this.killProgramsDataGrid.Size = new System.Drawing.Size(1027, 368);
            this.killProgramsDataGrid.TabIndex = 0;
            this.killProgramsDataGrid.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.killProgramsDataGrid_UserAddedRow);
            this.killProgramsDataGrid.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.killProgramsDataGrid_UserDeletedRow);
            // 
            // ExecutableNameColumn
            // 
            this.ExecutableNameColumn.HeaderText = "Program executable name";
            this.ExecutableNameColumn.MinimumWidth = 64;
            this.ExecutableNameColumn.Name = "ExecutableNameColumn";
            this.ExecutableNameColumn.Width = 700;
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 645);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1082, 591);
            this.Name = "SettingsWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Settings - FRC Driver Station Auto Shutdown";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.killProgramsDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox enableApplicationCheckbox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.DataGridView killProgramsDataGrid;
        private System.Windows.Forms.Label autosaveLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExecutableNameColumn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label ipAddressSourceLabel;
        private System.Windows.Forms.Label killPrograms;
        private System.Windows.Forms.CheckBox registerAutoStartCheckbox;
        private System.Windows.Forms.Label unknownIpLabel;
        private System.Windows.Forms.Button installToProgFilesButton;
        private System.Windows.Forms.Button makeDesktopLnkButton;
        private System.Windows.Forms.ToolTip makeDesktopLnkTooltip;
        private System.Windows.Forms.Panel panel2;
        private DoubleClickButton shutDownPcButton;
        private DoubleClickButton doubleClickButton1;
    }
}

