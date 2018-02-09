namespace StorageManagementTool
{
   partial class MainWindow
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
         this.FolderToMove_fbd = new System.Windows.Forms.FolderBrowserDialog();
         this.FolderToMove_btn = new System.Windows.Forms.Button();
         this.MoveFolder_btn = new System.Windows.Forms.Button();
         this.HDDPath_fbd = new System.Windows.Forms.FolderBrowserDialog();
         this.SetRootPath_btn = new System.Windows.Forms.Button();
         this.FolderToMove_tb = new System.Windows.Forms.TextBox();
         this.HDDSavePathText = new System.Windows.Forms.TextBox();
         this.Suggestion_lb = new System.Windows.Forms.ListBox();
         this.CustomFolderOrFileSelection_gb = new System.Windows.Forms.GroupBox();
         this.SetRootPathAsDefault_btn = new System.Windows.Forms.Button();
         this.FileToMovePath_tb = new System.Windows.Forms.TextBox();
         this.FileToMove_btn = new System.Windows.Forms.Button();
         this.Suggestions_gb = new System.Windows.Forms.GroupBox();
         this.OpenSelectedFolder_btn = new System.Windows.Forms.Button();
         this.MoveFilesOrFolder_gb = new System.Windows.Forms.GroupBox();
         this.MoveFile_btn = new System.Windows.Forms.Button();
         this.OpenWindowsSearchsettings_btn = new System.Windows.Forms.Button();
         this.EditSSDMonitoring_btn = new System.Windows.Forms.Button();
         this.AdministartorStatus_tb = new System.Windows.Forms.TextBox();
         this.RestartAsAdministartor_btn = new System.Windows.Forms.Button();
         this.AdministratorSettings_gb = new System.Windows.Forms.GroupBox();
         this.FileToMove_ofd = new System.Windows.Forms.OpenFileDialog();
         this.ProgramStatusStrip = new System.Windows.Forms.StatusStrip();
         this.EditPagefiles_btn = new System.Windows.Forms.Button();
         this.EditUserShellFolders_btn = new System.Windows.Forms.Button();
         this.SetSendToHDD_btn = new System.Windows.Forms.Button();
         this.FurtherSettings_gb = new System.Windows.Forms.GroupBox();
         this.ApplyPresetDialog_btn = new System.Windows.Forms.Button();
         this.button3 = new System.Windows.Forms.Button();
         this.CustomFolderOrFileSelection_gb.SuspendLayout();
         this.Suggestions_gb.SuspendLayout();
         this.MoveFilesOrFolder_gb.SuspendLayout();
         this.AdministratorSettings_gb.SuspendLayout();
         this.FurtherSettings_gb.SuspendLayout();
         this.SuspendLayout();
         // 
         // FolderToMove_btn
         // 
         this.FolderToMove_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.FolderToMove_btn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.FolderToMove_btn.Location = new System.Drawing.Point(6, 19);
         this.FolderToMove_btn.Name = "FolderToMove_btn";
         this.FolderToMove_btn.Size = new System.Drawing.Size(173, 23);
         this.FolderToMove_btn.TabIndex = 0;
         this.FolderToMove_btn.Text = "Ordner zum Verschieben wählen";
         this.FolderToMove_btn.UseVisualStyleBackColor = true;
         this.FolderToMove_btn.Click += new System.EventHandler(this.FolderToMove_btn_Click);
         // 
         // MoveFolder_btn
         // 
         this.MoveFolder_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.MoveFolder_btn.Location = new System.Drawing.Point(6, 449);
         this.MoveFolder_btn.Name = "MoveFolder_btn";
         this.MoveFolder_btn.Size = new System.Drawing.Size(177, 23);
         this.MoveFolder_btn.TabIndex = 1;
         this.MoveFolder_btn.Text = "Ordner Speicherort verschieben";
         this.MoveFolder_btn.UseVisualStyleBackColor = true;
         this.MoveFolder_btn.Click += new System.EventHandler(this.MoveFolder_btn_Click);
         // 
         // SetRootPath_btn
         // 
         this.SetRootPath_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.SetRootPath_btn.Location = new System.Drawing.Point(6, 73);
         this.SetRootPath_btn.Name = "SetRootPath_btn";
         this.SetRootPath_btn.Size = new System.Drawing.Size(391, 23);
         this.SetRootPath_btn.TabIndex = 2;
         this.SetRootPath_btn.Text = "HDD Speicherort setzen";
         this.SetRootPath_btn.UseVisualStyleBackColor = true;
         this.SetRootPath_btn.Click += new System.EventHandler(this.SetRootPath_btn_Click);
         // 
         // FolderToMove_tb
         // 
         this.FolderToMove_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.FolderToMove_tb.Location = new System.Drawing.Point(6, 48);
         this.FolderToMove_tb.Name = "FolderToMove_tb";
         this.FolderToMove_tb.ReadOnly = true;
         this.FolderToMove_tb.Size = new System.Drawing.Size(173, 20);
         this.FolderToMove_tb.TabIndex = 3;
         // 
         // HDDSavePathText
         // 
         this.HDDSavePathText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.HDDSavePathText.Location = new System.Drawing.Point(6, 102);
         this.HDDSavePathText.Name = "HDDSavePathText";
         this.HDDSavePathText.ReadOnly = true;
         this.HDDSavePathText.Size = new System.Drawing.Size(391, 20);
         this.HDDSavePathText.TabIndex = 4;
         // 
         // Suggestion_lb
         // 
         this.Suggestion_lb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Suggestion_lb.FormattingEnabled = true;
         this.Suggestion_lb.Location = new System.Drawing.Point(6, 16);
         this.Suggestion_lb.Name = "Suggestion_lb";
         this.Suggestion_lb.Size = new System.Drawing.Size(391, 199);
         this.Suggestion_lb.TabIndex = 6;
         this.Suggestion_lb.SelectedIndexChanged += new System.EventHandler(this.Suggestion_lb_SelectedIndexChanged);
         // 
         // CustomFolderOrFileSelection_gb
         // 
         this.CustomFolderOrFileSelection_gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.CustomFolderOrFileSelection_gb.Controls.Add(this.SetRootPathAsDefault_btn);
         this.CustomFolderOrFileSelection_gb.Controls.Add(this.FileToMovePath_tb);
         this.CustomFolderOrFileSelection_gb.Controls.Add(this.FileToMove_btn);
         this.CustomFolderOrFileSelection_gb.Controls.Add(this.FolderToMove_btn);
         this.CustomFolderOrFileSelection_gb.Controls.Add(this.SetRootPath_btn);
         this.CustomFolderOrFileSelection_gb.Controls.Add(this.FolderToMove_tb);
         this.CustomFolderOrFileSelection_gb.Controls.Add(this.HDDSavePathText);
         this.CustomFolderOrFileSelection_gb.Location = new System.Drawing.Point(6, 18);
         this.CustomFolderOrFileSelection_gb.Name = "CustomFolderOrFileSelection_gb";
         this.CustomFolderOrFileSelection_gb.Size = new System.Drawing.Size(401, 157);
         this.CustomFolderOrFileSelection_gb.TabIndex = 7;
         this.CustomFolderOrFileSelection_gb.TabStop = false;
         this.CustomFolderOrFileSelection_gb.Text = "Manuelle  Auswahl";
         // 
         // SetRootPathAsDefault_btn
         // 
         this.SetRootPathAsDefault_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.SetRootPathAsDefault_btn.Location = new System.Drawing.Point(6, 128);
         this.SetRootPathAsDefault_btn.Name = "SetRootPathAsDefault_btn";
         this.SetRootPathAsDefault_btn.Size = new System.Drawing.Size(391, 23);
         this.SetRootPathAsDefault_btn.TabIndex = 7;
         this.SetRootPathAsDefault_btn.Text = "HDD Speicherort als Standard speichern";
         this.SetRootPathAsDefault_btn.UseVisualStyleBackColor = true;
         this.SetRootPathAsDefault_btn.Click += new System.EventHandler(this.SetRootPathAsDefault_btn_Click);
         // 
         // FileToMovePath_tb
         // 
         this.FileToMovePath_tb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.FileToMovePath_tb.Location = new System.Drawing.Point(185, 48);
         this.FileToMovePath_tb.Name = "FileToMovePath_tb";
         this.FileToMovePath_tb.ReadOnly = true;
         this.FileToMovePath_tb.Size = new System.Drawing.Size(212, 20);
         this.FileToMovePath_tb.TabIndex = 6;
         // 
         // FileToMove_btn
         // 
         this.FileToMove_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.FileToMove_btn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.FileToMove_btn.Location = new System.Drawing.Point(185, 19);
         this.FileToMove_btn.Name = "FileToMove_btn";
         this.FileToMove_btn.Size = new System.Drawing.Size(212, 23);
         this.FileToMove_btn.TabIndex = 5;
         this.FileToMove_btn.Text = "Datei zum Verschieben wählen";
         this.FileToMove_btn.UseVisualStyleBackColor = true;
         this.FileToMove_btn.Click += new System.EventHandler(this.FileToMoveSel_btn_Click);
         // 
         // Suggestions_gb
         // 
         this.Suggestions_gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Suggestions_gb.Controls.Add(this.OpenSelectedFolder_btn);
         this.Suggestions_gb.Controls.Add(this.Suggestion_lb);
         this.Suggestions_gb.Location = new System.Drawing.Point(6, 181);
         this.Suggestions_gb.Name = "Suggestions_gb";
         this.Suggestions_gb.Size = new System.Drawing.Size(401, 262);
         this.Suggestions_gb.TabIndex = 8;
         this.Suggestions_gb.TabStop = false;
         this.Suggestions_gb.Text = "Vorschläge";
         // 
         // OpenSelectedFolder_btn
         // 
         this.OpenSelectedFolder_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.OpenSelectedFolder_btn.Location = new System.Drawing.Point(6, 233);
         this.OpenSelectedFolder_btn.Name = "OpenSelectedFolder_btn";
         this.OpenSelectedFolder_btn.Size = new System.Drawing.Size(391, 23);
         this.OpenSelectedFolder_btn.TabIndex = 7;
         this.OpenSelectedFolder_btn.Text = "Ausgewählten Ordner öffnen";
         this.OpenSelectedFolder_btn.UseVisualStyleBackColor = true;
         this.OpenSelectedFolder_btn.Click += new System.EventHandler(this.OpenSelectedFolder_btn_Click);
         // 
         // MoveFilesOrFolder_gb
         // 
         this.MoveFilesOrFolder_gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.MoveFilesOrFolder_gb.Controls.Add(this.MoveFile_btn);
         this.MoveFilesOrFolder_gb.Controls.Add(this.MoveFolder_btn);
         this.MoveFilesOrFolder_gb.Controls.Add(this.CustomFolderOrFileSelection_gb);
         this.MoveFilesOrFolder_gb.Controls.Add(this.Suggestions_gb);
         this.MoveFilesOrFolder_gb.Location = new System.Drawing.Point(12, 12);
         this.MoveFilesOrFolder_gb.Name = "MoveFilesOrFolder_gb";
         this.MoveFilesOrFolder_gb.Size = new System.Drawing.Size(411, 480);
         this.MoveFilesOrFolder_gb.TabIndex = 12;
         this.MoveFilesOrFolder_gb.TabStop = false;
         this.MoveFilesOrFolder_gb.Text = "Ordner Speicherort ändern";
         // 
         // MoveFile_btn
         // 
         this.MoveFile_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.MoveFile_btn.Location = new System.Drawing.Point(189, 449);
         this.MoveFile_btn.Name = "MoveFile_btn";
         this.MoveFile_btn.Size = new System.Drawing.Size(218, 23);
         this.MoveFile_btn.TabIndex = 9;
         this.MoveFile_btn.Text = "Datei Speicherort verschieben";
         this.MoveFile_btn.UseVisualStyleBackColor = true;
         this.MoveFile_btn.Click += new System.EventHandler(this.MoveFile_btn_Click);
         // 
         // OpenWindowsSearchsettings_btn
         // 
         this.OpenWindowsSearchsettings_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.OpenWindowsSearchsettings_btn.Location = new System.Drawing.Point(6, 76);
         this.OpenWindowsSearchsettings_btn.Name = "OpenWindowsSearchsettings_btn";
         this.OpenWindowsSearchsettings_btn.Size = new System.Drawing.Size(213, 23);
         this.OpenWindowsSearchsettings_btn.TabIndex = 10;
         this.OpenWindowsSearchsettings_btn.Text = "Suchindizierung anpassen";
         this.OpenWindowsSearchsettings_btn.UseVisualStyleBackColor = true;
         this.OpenWindowsSearchsettings_btn.Click += new System.EventHandler(this.OpenWindowsSearchsettings_btn_Click);
         // 
         // EditSSDMonitoring_btn
         // 
         this.EditSSDMonitoring_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.EditSSDMonitoring_btn.Location = new System.Drawing.Point(6, 134);
         this.EditSSDMonitoring_btn.Name = "EditSSDMonitoring_btn";
         this.EditSSDMonitoring_btn.Size = new System.Drawing.Size(213, 23);
         this.EditSSDMonitoring_btn.TabIndex = 20;
         this.EditSSDMonitoring_btn.Text = "SSD Überwachung anpassen";
         this.EditSSDMonitoring_btn.UseVisualStyleBackColor = true;
         this.EditSSDMonitoring_btn.Click += new System.EventHandler(this.EditSSDMonitoring_btn_Click);
         // 
         // AdministartorStatus_tb
         // 
         this.AdministartorStatus_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.AdministartorStatus_tb.Location = new System.Drawing.Point(6, 48);
         this.AdministartorStatus_tb.Multiline = true;
         this.AdministartorStatus_tb.Name = "AdministartorStatus_tb";
         this.AdministartorStatus_tb.ReadOnly = true;
         this.AdministartorStatus_tb.Size = new System.Drawing.Size(213, 38);
         this.AdministartorStatus_tb.TabIndex = 13;
         // 
         // RestartAsAdministartor_btn
         // 
         this.RestartAsAdministartor_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.RestartAsAdministartor_btn.Location = new System.Drawing.Point(6, 19);
         this.RestartAsAdministartor_btn.Name = "RestartAsAdministartor_btn";
         this.RestartAsAdministartor_btn.Size = new System.Drawing.Size(213, 23);
         this.RestartAsAdministartor_btn.TabIndex = 14;
         this.RestartAsAdministartor_btn.Text = "Neustarten mit Adminstatorrechten";
         this.RestartAsAdministartor_btn.UseVisualStyleBackColor = true;
         this.RestartAsAdministartor_btn.Click += new System.EventHandler(this.RestartAsAdministartor_btn_Click);
         // 
         // AdministratorSettings_gb
         // 
         this.AdministratorSettings_gb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.AdministratorSettings_gb.Controls.Add(this.RestartAsAdministartor_btn);
         this.AdministratorSettings_gb.Controls.Add(this.AdministartorStatus_tb);
         this.AdministratorSettings_gb.Location = new System.Drawing.Point(429, 210);
         this.AdministratorSettings_gb.Name = "AdministratorSettings_gb";
         this.AdministratorSettings_gb.Size = new System.Drawing.Size(225, 94);
         this.AdministratorSettings_gb.TabIndex = 16;
         this.AdministratorSettings_gb.TabStop = false;
         this.AdministratorSettings_gb.Text = "Administratorenprivilegien";
         // 
         // FileToMove_ofd
         // 
         this.FileToMove_ofd.Filter = "Alle Dateien|*.*";
         // 
         // ProgramStatusStrip
         // 
         this.ProgramStatusStrip.Location = new System.Drawing.Point(0, 496);
         this.ProgramStatusStrip.Name = "ProgramStatusStrip";
         this.ProgramStatusStrip.Size = new System.Drawing.Size(666, 22);
         this.ProgramStatusStrip.TabIndex = 18;
         this.ProgramStatusStrip.Text = "statusStrip1";
         // 
         // EditPagefiles_btn
         // 
         this.EditPagefiles_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.EditPagefiles_btn.Location = new System.Drawing.Point(6, 105);
         this.EditPagefiles_btn.Name = "EditPagefiles_btn";
         this.EditPagefiles_btn.Size = new System.Drawing.Size(213, 23);
         this.EditPagefiles_btn.TabIndex = 19;
         this.EditPagefiles_btn.Text = "Arbeitsspeicherauslagerungs Dateien";
         this.EditPagefiles_btn.UseVisualStyleBackColor = true;
         this.EditPagefiles_btn.Click += new System.EventHandler(this.EditPagefiles_btn_Click);
         // 
         // EditUserShellFolders_btn
         // 
         this.EditUserShellFolders_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.EditUserShellFolders_btn.Location = new System.Drawing.Point(6, 47);
         this.EditUserShellFolders_btn.Name = "EditUserShellFolders_btn";
         this.EditUserShellFolders_btn.Size = new System.Drawing.Size(213, 23);
         this.EditUserShellFolders_btn.TabIndex = 21;
         this.EditUserShellFolders_btn.Text = "UserShellFolder bearbeiten";
         this.EditUserShellFolders_btn.UseVisualStyleBackColor = true;
         this.EditUserShellFolders_btn.Click += new System.EventHandler(this.EditUserShellFolders_btn_Click);
         // 
         // SetSendToHDD_btn
         // 
         this.SetSendToHDD_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.SetSendToHDD_btn.Location = new System.Drawing.Point(6, 18);
         this.SetSendToHDD_btn.Name = "SetSendToHDD_btn";
         this.SetSendToHDD_btn.Size = new System.Drawing.Size(213, 23);
         this.SetSendToHDD_btn.TabIndex = 22;
         this.SetSendToHDD_btn.Text = "Senden an HDD aktivieren";
         this.SetSendToHDD_btn.UseVisualStyleBackColor = true;
         this.SetSendToHDD_btn.Click += new System.EventHandler(this.SetSendToHDD_btn_Click);
         // 
         // FurtherSettings_gb
         // 
         this.FurtherSettings_gb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.FurtherSettings_gb.Controls.Add(this.ApplyPresetDialog_btn);
         this.FurtherSettings_gb.Controls.Add(this.EditSSDMonitoring_btn);
         this.FurtherSettings_gb.Controls.Add(this.OpenWindowsSearchsettings_btn);
         this.FurtherSettings_gb.Controls.Add(this.EditPagefiles_btn);
         this.FurtherSettings_gb.Controls.Add(this.EditUserShellFolders_btn);
         this.FurtherSettings_gb.Controls.Add(this.SetSendToHDD_btn);
         this.FurtherSettings_gb.Location = new System.Drawing.Point(429, 12);
         this.FurtherSettings_gb.Name = "FurtherSettings_gb";
         this.FurtherSettings_gb.Size = new System.Drawing.Size(225, 192);
         this.FurtherSettings_gb.TabIndex = 24;
         this.FurtherSettings_gb.TabStop = false;
         this.FurtherSettings_gb.Text = "Weitere Einstellungen";
         // 
         // ApplyPresetDialog_btn
         // 
         this.ApplyPresetDialog_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ApplyPresetDialog_btn.Location = new System.Drawing.Point(6, 163);
         this.ApplyPresetDialog_btn.Name = "ApplyPresetDialog_btn";
         this.ApplyPresetDialog_btn.Size = new System.Drawing.Size(213, 23);
         this.ApplyPresetDialog_btn.TabIndex = 23;
         this.ApplyPresetDialog_btn.Text = "Automatische Einrichtung";
         this.ApplyPresetDialog_btn.UseVisualStyleBackColor = true;
         this.ApplyPresetDialog_btn.Click += new System.EventHandler(this.ApplyPresetDialog_btn_Click);
         // 
         // button3
         // 
         this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.button3.Location = new System.Drawing.Point(430, 470);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(224, 23);
         this.button3.TabIndex = 25;
         this.button3.Text = "The magic debug button";
         this.button3.UseVisualStyleBackColor = true;
         this.button3.Visible = false;
         this.button3.Click += new System.EventHandler(this.button3_Click);
         // 
         // MainWindow
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(666, 518);
         this.Controls.Add(this.button3);
         this.Controls.Add(this.FurtherSettings_gb);
         this.Controls.Add(this.ProgramStatusStrip);
         this.Controls.Add(this.AdministratorSettings_gb);
         this.Controls.Add(this.MoveFilesOrFolder_gb);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "MainWindow";
         this.Text = "Speichermanagement Tool";
         this.Load += new System.EventHandler(this.MainWindow_Load);
         this.CustomFolderOrFileSelection_gb.ResumeLayout(false);
         this.CustomFolderOrFileSelection_gb.PerformLayout();
         this.Suggestions_gb.ResumeLayout(false);
         this.MoveFilesOrFolder_gb.ResumeLayout(false);
         this.AdministratorSettings_gb.ResumeLayout(false);
         this.AdministratorSettings_gb.PerformLayout();
         this.FurtherSettings_gb.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

      #endregion
      
      private System.Windows.Forms.FolderBrowserDialog FolderToMove_fbd;
      private System.Windows.Forms.Button FolderToMove_btn;
      private System.Windows.Forms.Button MoveFolder_btn;
      private System.Windows.Forms.FolderBrowserDialog HDDPath_fbd;
      private System.Windows.Forms.Button SetRootPath_btn;
      private System.Windows.Forms.TextBox FolderToMove_tb;
      public System.Windows.Forms.TextBox HDDSavePathText;
      private System.Windows.Forms.GroupBox CustomFolderOrFileSelection_gb;
      private System.Windows.Forms.GroupBox Suggestions_gb;
      private System.Windows.Forms.GroupBox MoveFilesOrFolder_gb;
      private System.Windows.Forms.TextBox AdministartorStatus_tb;
      private System.Windows.Forms.Button RestartAsAdministartor_btn;
      private System.Windows.Forms.GroupBox AdministratorSettings_gb;
      private System.Windows.Forms.TextBox FileToMovePath_tb;
      private System.Windows.Forms.Button FileToMove_btn;
      private System.Windows.Forms.Button MoveFile_btn;
      public System.Windows.Forms.OpenFileDialog FileToMove_ofd;
      private System.Windows.Forms.Button OpenSelectedFolder_btn;
      public System.Windows.Forms.ListBox Suggestion_lb;
      private System.Windows.Forms.StatusStrip ProgramStatusStrip;
      private System.Windows.Forms.Button EditSSDMonitoring_btn;
      private System.Windows.Forms.Button SetRootPathAsDefault_btn;
        private System.Windows.Forms.Button EditPagefiles_btn;
        private System.Windows.Forms.Button OpenWindowsSearchsettings_btn;
        private System.Windows.Forms.Button EditUserShellFolders_btn;
        private System.Windows.Forms.Button SetSendToHDD_btn;
        private System.Windows.Forms.GroupBox FurtherSettings_gb;
        private System.Windows.Forms.Button ApplyPresetDialog_btn;
        private System.Windows.Forms.Button button3;
    }
}

