namespace StorageManagementTool.MainGUI
{
    partial class MonitoringSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitoringSettings));
            this.AllFolders_lb = new System.Windows.Forms.ListBox();
            this.EnableNotifications_cb = new System.Windows.Forms.CheckBox();
            this.OpenSelectedfolder_btn = new System.Windows.Forms.Button();
            this.RemoveSelectedFolder_btn = new System.Windows.Forms.Button();
            this.Savesettings_btn = new System.Windows.Forms.Button();
            this.Addfolder_btn = new System.Windows.Forms.Button();
            this.Abort_btn = new System.Windows.Forms.Button();
            this.AutomaticMoveForFiles_rb = new System.Windows.Forms.RadioButton();
            this.AskActionForFiles_rb = new System.Windows.Forms.RadioButton();
            this.AskActionForFolders_rb = new System.Windows.Forms.RadioButton();
            this.AutomaticMoveForFolders_rb = new System.Windows.Forms.RadioButton();
            this.ActionForFolders_gb = new System.Windows.Forms.GroupBox();
            this.IgnoreForFolders_rb = new System.Windows.Forms.RadioButton();
            this.ActionForFiles_gb = new System.Windows.Forms.GroupBox();
            this.IgnoreForFiles_rb = new System.Windows.Forms.RadioButton();
            this.ChangeFolder_btn = new System.Windows.Forms.Button();
            this.InitalizeSSDMonitoring_btn = new System.Windows.Forms.Button();
            this.ActionForFolders_gb.SuspendLayout();
            this.ActionForFiles_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // AllFolders_lb
            // 
            this.AllFolders_lb.FormattingEnabled = true;
            this.AllFolders_lb.Location = new System.Drawing.Point(13, 41);
            this.AllFolders_lb.Name = "AllFolders_lb";
            this.AllFolders_lb.Size = new System.Drawing.Size(402, 95);
            this.AllFolders_lb.TabIndex = 0;
            this.AllFolders_lb.SelectedIndexChanged += new System.EventHandler(this.AllFolders_lb_SelectedIndexChanged);
            // 
            // EnableNotifications_cb
            // 
            this.EnableNotifications_cb.AutoSize = true;
            this.EnableNotifications_cb.Location = new System.Drawing.Point(13, 13);
            this.EnableNotifications_cb.Name = "EnableNotifications_cb";
            this.EnableNotifications_cb.Size = new System.Drawing.Size(167, 17);
            this.EnableNotifications_cb.TabIndex = 1;
            this.EnableNotifications_cb.Text = "SSD Überwachung aktivieren";
            this.EnableNotifications_cb.UseVisualStyleBackColor = true;
            this.EnableNotifications_cb.CheckedChanged += new System.EventHandler(this.EnableNotifications_cb_CheckedChanged);
            // 
            // OpenSelectedfolder_btn
            // 
            this.OpenSelectedfolder_btn.Location = new System.Drawing.Point(13, 142);
            this.OpenSelectedfolder_btn.Name = "OpenSelectedfolder_btn";
            this.OpenSelectedfolder_btn.Size = new System.Drawing.Size(198, 23);
            this.OpenSelectedfolder_btn.TabIndex = 4;
            this.OpenSelectedfolder_btn.Text = "Ordner öffnen";
            this.OpenSelectedfolder_btn.UseVisualStyleBackColor = true;
            this.OpenSelectedfolder_btn.Click += new System.EventHandler(this.OpenSelectedfolder_btn_Click);
            // 
            // RemoveSelectedFolder_btn
            // 
            this.RemoveSelectedFolder_btn.Location = new System.Drawing.Point(217, 171);
            this.RemoveSelectedFolder_btn.Name = "RemoveSelectedFolder_btn";
            this.RemoveSelectedFolder_btn.Size = new System.Drawing.Size(198, 23);
            this.RemoveSelectedFolder_btn.TabIndex = 5;
            this.RemoveSelectedFolder_btn.Text = "Ordner aus Liste entfernen";
            this.RemoveSelectedFolder_btn.UseVisualStyleBackColor = true;
            this.RemoveSelectedFolder_btn.Click += new System.EventHandler(this.RemoveselectedFolder_btn_Click);
            // 
            // Savesettings_btn
            // 
            this.Savesettings_btn.Location = new System.Drawing.Point(216, 294);
            this.Savesettings_btn.Name = "Savesettings_btn";
            this.Savesettings_btn.Size = new System.Drawing.Size(198, 23);
            this.Savesettings_btn.TabIndex = 7;
            this.Savesettings_btn.Text = "Speichern";
            this.Savesettings_btn.UseVisualStyleBackColor = true;
            this.Savesettings_btn.Click += new System.EventHandler(this.Savesettings_btn_Click);
            // 
            // Addfolder_btn
            // 
            this.Addfolder_btn.Location = new System.Drawing.Point(13, 171);
            this.Addfolder_btn.Name = "Addfolder_btn";
            this.Addfolder_btn.Size = new System.Drawing.Size(198, 23);
            this.Addfolder_btn.TabIndex = 8;
            this.Addfolder_btn.Text = "Ordner hinzufügern";
            this.Addfolder_btn.UseVisualStyleBackColor = true;
            this.Addfolder_btn.Click += new System.EventHandler(this.Addfolder_btn_Click);
            // 
            // Abort_btn
            // 
            this.Abort_btn.Location = new System.Drawing.Point(14, 294);
            this.Abort_btn.Name = "Abort_btn";
            this.Abort_btn.Size = new System.Drawing.Size(197, 23);
            this.Abort_btn.TabIndex = 9;
            this.Abort_btn.Text = "Abbrechen";
            this.Abort_btn.UseVisualStyleBackColor = true;
            this.Abort_btn.Click += new System.EventHandler(this.Abort_btn_Click);
            // 
            // AutomaticMoveForFiles_rb
            // 
            this.AutomaticMoveForFiles_rb.AutoSize = true;
            this.AutomaticMoveForFiles_rb.Location = new System.Drawing.Point(6, 42);
            this.AutomaticMoveForFiles_rb.Name = "AutomaticMoveForFiles_rb";
            this.AutomaticMoveForFiles_rb.Size = new System.Drawing.Size(114, 17);
            this.AutomaticMoveForFiles_rb.TabIndex = 10;
            this.AutomaticMoveForFiles_rb.TabStop = true;
            this.AutomaticMoveForFiles_rb.Tag = "Move";
            this.AutomaticMoveForFiles_rb.Text = "Sofort verschieben";
            this.AutomaticMoveForFiles_rb.UseVisualStyleBackColor = true;
            this.AutomaticMoveForFiles_rb.CheckedChanged += new System.EventHandler(this.ForFilesChanged);
            // 
            // AskActionForFiles_rb
            // 
            this.AskActionForFiles_rb.AutoSize = true;
            this.AskActionForFiles_rb.Location = new System.Drawing.Point(6, 19);
            this.AskActionForFiles_rb.Name = "AskActionForFiles_rb";
            this.AskActionForFiles_rb.Size = new System.Drawing.Size(97, 17);
            this.AskActionForFiles_rb.TabIndex = 11;
            this.AskActionForFiles_rb.TabStop = true;
            this.AskActionForFiles_rb.Tag = "Ask";
            this.AskActionForFiles_rb.Text = "Aktion erfragen";
            this.AskActionForFiles_rb.UseVisualStyleBackColor = true;
            this.AskActionForFiles_rb.CheckedChanged += new System.EventHandler(this.ForFilesChanged);
            // 
            // AskActionForFolders_rb
            // 
            this.AskActionForFolders_rb.AutoSize = true;
            this.AskActionForFolders_rb.Location = new System.Drawing.Point(6, 19);
            this.AskActionForFolders_rb.Name = "AskActionForFolders_rb";
            this.AskActionForFolders_rb.Size = new System.Drawing.Size(97, 17);
            this.AskActionForFolders_rb.TabIndex = 16;
            this.AskActionForFolders_rb.TabStop = true;
            this.AskActionForFolders_rb.Tag = "Ask";
            this.AskActionForFolders_rb.Text = "Aktion erfragen";
            this.AskActionForFolders_rb.UseVisualStyleBackColor = true;
            this.AskActionForFolders_rb.CheckedChanged += new System.EventHandler(this.ForFoldersChanged);
            // 
            // AutomaticMoveForFolders_rb
            // 
            this.AutomaticMoveForFolders_rb.AutoSize = true;
            this.AutomaticMoveForFolders_rb.Location = new System.Drawing.Point(6, 42);
            this.AutomaticMoveForFolders_rb.Name = "AutomaticMoveForFolders_rb";
            this.AutomaticMoveForFolders_rb.Size = new System.Drawing.Size(114, 17);
            this.AutomaticMoveForFolders_rb.TabIndex = 15;
            this.AutomaticMoveForFolders_rb.TabStop = true;
            this.AutomaticMoveForFolders_rb.Tag = "Move";
            this.AutomaticMoveForFolders_rb.Text = "Sofort verschieben";
            this.AutomaticMoveForFolders_rb.UseVisualStyleBackColor = true;
            this.AutomaticMoveForFolders_rb.CheckedChanged += new System.EventHandler(this.ForFoldersChanged);
            // 
            // ActionForFolders_gb
            // 
            this.ActionForFolders_gb.Controls.Add(this.IgnoreForFolders_rb);
            this.ActionForFolders_gb.Controls.Add(this.AskActionForFolders_rb);
            this.ActionForFolders_gb.Controls.Add(this.AutomaticMoveForFolders_rb);
            this.ActionForFolders_gb.Location = new System.Drawing.Point(217, 200);
            this.ActionForFolders_gb.Name = "ActionForFolders_gb";
            this.ActionForFolders_gb.Size = new System.Drawing.Size(198, 88);
            this.ActionForFolders_gb.TabIndex = 17;
            this.ActionForFolders_gb.TabStop = false;
            this.ActionForFolders_gb.Text = "bei neuen Ordnern";
            // 
            // IgnoreForFolders_rb
            // 
            this.IgnoreForFolders_rb.AutoSize = true;
            this.IgnoreForFolders_rb.Location = new System.Drawing.Point(6, 65);
            this.IgnoreForFolders_rb.Name = "IgnoreForFolders_rb";
            this.IgnoreForFolders_rb.Size = new System.Drawing.Size(72, 17);
            this.IgnoreForFolders_rb.TabIndex = 17;
            this.IgnoreForFolders_rb.TabStop = true;
            this.IgnoreForFolders_rb.Tag = "Ignore";
            this.IgnoreForFolders_rb.Text = "Ignorieren";
            this.IgnoreForFolders_rb.UseVisualStyleBackColor = true;
            this.IgnoreForFolders_rb.CheckedChanged += new System.EventHandler(this.ForFoldersChanged);
            // 
            // ActionForFiles_gb
            // 
            this.ActionForFiles_gb.Controls.Add(this.IgnoreForFiles_rb);
            this.ActionForFiles_gb.Controls.Add(this.AskActionForFiles_rb);
            this.ActionForFiles_gb.Controls.Add(this.AutomaticMoveForFiles_rb);
            this.ActionForFiles_gb.Location = new System.Drawing.Point(16, 200);
            this.ActionForFiles_gb.Name = "ActionForFiles_gb";
            this.ActionForFiles_gb.Size = new System.Drawing.Size(195, 88);
            this.ActionForFiles_gb.TabIndex = 18;
            this.ActionForFiles_gb.TabStop = false;
            this.ActionForFiles_gb.Text = "bei neuen Dateien";
            // 
            // IgnoreForFiles_rb
            // 
            this.IgnoreForFiles_rb.AutoSize = true;
            this.IgnoreForFiles_rb.Location = new System.Drawing.Point(6, 65);
            this.IgnoreForFiles_rb.Name = "IgnoreForFiles_rb";
            this.IgnoreForFiles_rb.Size = new System.Drawing.Size(72, 17);
            this.IgnoreForFiles_rb.TabIndex = 18;
            this.IgnoreForFiles_rb.TabStop = true;
            this.IgnoreForFiles_rb.Tag = "Ignore";
            this.IgnoreForFiles_rb.Text = "Ignorieren";
            this.IgnoreForFiles_rb.UseVisualStyleBackColor = true;
            this.IgnoreForFiles_rb.CheckedChanged += new System.EventHandler(this.ForFilesChanged);
            // 
            // ChangeFolder_btn
            // 
            this.ChangeFolder_btn.Location = new System.Drawing.Point(217, 141);
            this.ChangeFolder_btn.Name = "ChangeFolder_btn";
            this.ChangeFolder_btn.Size = new System.Drawing.Size(198, 23);
            this.ChangeFolder_btn.TabIndex = 19;
            this.ChangeFolder_btn.Text = "Ordner ändern";
            this.ChangeFolder_btn.UseVisualStyleBackColor = true;
            this.ChangeFolder_btn.Click += new System.EventHandler(this.ChangeFolder_btn_Click);
            // 
            // InitalizeSSDMonitoring_btn
            // 
            this.InitalizeSSDMonitoring_btn.Location = new System.Drawing.Point(216, 12);
            this.InitalizeSSDMonitoring_btn.Name = "InitalizeSSDMonitoring_btn";
            this.InitalizeSSDMonitoring_btn.Size = new System.Drawing.Size(198, 23);
            this.InitalizeSSDMonitoring_btn.TabIndex = 20;
            this.InitalizeSSDMonitoring_btn.Text = "SSD Überwachung initialisieren";
            this.InitalizeSSDMonitoring_btn.UseVisualStyleBackColor = true;
            this.InitalizeSSDMonitoring_btn.Click += new System.EventHandler(this.InitalizeSSDMonitoring_btn_Click);
            // 
            // MonitoringSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 329);
            this.Controls.Add(this.InitalizeSSDMonitoring_btn);
            this.Controls.Add(this.ChangeFolder_btn);
            this.Controls.Add(this.ActionForFiles_gb);
            this.Controls.Add(this.ActionForFolders_gb);
            this.Controls.Add(this.Abort_btn);
            this.Controls.Add(this.Addfolder_btn);
            this.Controls.Add(this.Savesettings_btn);
            this.Controls.Add(this.RemoveSelectedFolder_btn);
            this.Controls.Add(this.OpenSelectedfolder_btn);
            this.Controls.Add(this.EnableNotifications_cb);
            this.Controls.Add(this.AllFolders_lb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MonitoringSettings";
            this.Text = "SSD Überwachung";
            this.Load += new System.EventHandler(this.NotificationSettings_Load);
            this.ActionForFolders_gb.ResumeLayout(false);
            this.ActionForFolders_gb.PerformLayout();
            this.ActionForFiles_gb.ResumeLayout(false);
            this.ActionForFiles_gb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox AllFolders_lb;
        private System.Windows.Forms.CheckBox EnableNotifications_cb;
        private System.Windows.Forms.Button OpenSelectedfolder_btn;
        private System.Windows.Forms.Button RemoveSelectedFolder_btn;
        private System.Windows.Forms.Button Savesettings_btn;
        private System.Windows.Forms.Button Addfolder_btn;
        private System.Windows.Forms.Button Abort_btn;
        private System.Windows.Forms.RadioButton AutomaticMoveForFiles_rb;
        private System.Windows.Forms.RadioButton AskActionForFiles_rb;
        private System.Windows.Forms.RadioButton AskActionForFolders_rb;
        private System.Windows.Forms.RadioButton AutomaticMoveForFolders_rb;
        private System.Windows.Forms.GroupBox ActionForFolders_gb;
        private System.Windows.Forms.RadioButton IgnoreForFolders_rb;
        private System.Windows.Forms.GroupBox ActionForFiles_gb;
        private System.Windows.Forms.RadioButton IgnoreForFiles_rb;
        private System.Windows.Forms.Button ChangeFolder_btn;
        private System.Windows.Forms.Button InitalizeSSDMonitoring_btn;
    }
}