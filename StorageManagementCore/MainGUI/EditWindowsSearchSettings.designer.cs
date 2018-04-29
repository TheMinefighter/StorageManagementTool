namespace StorageManagementCore.MainGUI
{
    partial class EditWindowsSearchSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditWindowsSearchSettings));
            this.OpenCurrentPath_btn = new System.Windows.Forms.Button();
            this.SelectNewPath_btn = new System.Windows.Forms.Button();
            this.SaveSettings_btn = new System.Windows.Forms.Button();
            this.Abort_btn = new System.Windows.Forms.Button();
            this.CurrentPath_tb = new System.Windows.Forms.TextBox();
            this.CurrentLocation_lbl = new System.Windows.Forms.Label();
            this.ShowFurtherSettings_btn = new System.Windows.Forms.Button();
            this.NewPath_tb = new System.Windows.Forms.TextBox();
            this.NewPath_lbl = new System.Windows.Forms.Label();
            this.OpenNewPath_btn = new System.Windows.Forms.Button();
            this.SelectNewPath_fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.RefreshCurrentPath_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OpenCurrentPath_btn
            // 
            this.OpenCurrentPath_btn.Location = new System.Drawing.Point(12, 55);
            this.OpenCurrentPath_btn.Name = "OpenCurrentPath_btn";
            this.OpenCurrentPath_btn.Size = new System.Drawing.Size(179, 23);
            this.OpenCurrentPath_btn.TabIndex = 2;
            this.OpenCurrentPath_btn.Text = " Aktuellen Speicherort öffnen";
            this.OpenCurrentPath_btn.UseVisualStyleBackColor = true;
            this.OpenCurrentPath_btn.Click += new System.EventHandler(this.OpenCurrentPath_btn_Click);
            // 
            // SelectNewPath_btn
            // 
            this.SelectNewPath_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectNewPath_btn.Location = new System.Drawing.Point(197, 123);
            this.SelectNewPath_btn.Name = "SelectNewPath_btn";
            this.SelectNewPath_btn.Size = new System.Drawing.Size(178, 23);
            this.SelectNewPath_btn.TabIndex = 0;
            this.SelectNewPath_btn.Text = "Neuen Speicherort auswählen";
            this.SelectNewPath_btn.UseVisualStyleBackColor = true;
            this.SelectNewPath_btn.Click += new System.EventHandler(this.SelectNewPath_btn_Click);
            // 
            // SaveSettings_btn
            // 
            this.SaveSettings_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSettings_btn.Location = new System.Drawing.Point(197, 181);
            this.SaveSettings_btn.Name = "SaveSettings_btn";
            this.SaveSettings_btn.Size = new System.Drawing.Size(178, 23);
            this.SaveSettings_btn.TabIndex = 1;
            this.SaveSettings_btn.Text = "Änderungen Speichern";
            this.SaveSettings_btn.UseVisualStyleBackColor = true;
            this.SaveSettings_btn.Click += new System.EventHandler(this.SaveSettings_btn_Click);
            // 
            // Abort_btn
            // 
            this.Abort_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Abort_btn.Location = new System.Drawing.Point(13, 181);
            this.Abort_btn.Name = "Abort_btn";
            this.Abort_btn.Size = new System.Drawing.Size(178, 23);
            this.Abort_btn.TabIndex = 6;
            this.Abort_btn.Text = "Abbrechen";
            this.Abort_btn.UseVisualStyleBackColor = true;
            this.Abort_btn.Click += new System.EventHandler(this.Abort_btn_Click);
            // 
            // CurrentPath_tb
            // 
            this.CurrentPath_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentPath_tb.Location = new System.Drawing.Point(12, 29);
            this.CurrentPath_tb.Name = "CurrentPath_tb";
            this.CurrentPath_tb.ReadOnly = true;
            this.CurrentPath_tb.Size = new System.Drawing.Size(363, 20);
            this.CurrentPath_tb.TabIndex = 7;
            this.CurrentPath_tb.TextChanged += new System.EventHandler(this.CurrentPath_tb_TextChanged);
            // 
            // CurrentLocation_lbl
            // 
            this.CurrentLocation_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentLocation_lbl.AutoSize = true;
            this.CurrentLocation_lbl.Location = new System.Drawing.Point(9, 13);
            this.CurrentLocation_lbl.Name = "CurrentLocation_lbl";
            this.CurrentLocation_lbl.Size = new System.Drawing.Size(105, 13);
            this.CurrentLocation_lbl.TabIndex = 5;
            this.CurrentLocation_lbl.Text = "Aktueller Speicherort";
            // 
            // ShowFurtherSettings_btn
            // 
            this.ShowFurtherSettings_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowFurtherSettings_btn.Location = new System.Drawing.Point(13, 152);
            this.ShowFurtherSettings_btn.Name = "ShowFurtherSettings_btn";
            this.ShowFurtherSettings_btn.Size = new System.Drawing.Size(362, 23);
            this.ShowFurtherSettings_btn.TabIndex = 5;
            this.ShowFurtherSettings_btn.Text = "Weitere Einstellungen zeigen";
            this.ShowFurtherSettings_btn.UseVisualStyleBackColor = true;
            this.ShowFurtherSettings_btn.Click += new System.EventHandler(this.ShowFurtherSettings_btn_Click);
            // 
            // NewPath_tb
            // 
            this.NewPath_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPath_tb.Location = new System.Drawing.Point(13, 97);
            this.NewPath_tb.Name = "NewPath_tb";
            this.NewPath_tb.ReadOnly = true;
            this.NewPath_tb.Size = new System.Drawing.Size(362, 20);
            this.NewPath_tb.TabIndex = 8;
            this.NewPath_tb.TextChanged += new System.EventHandler(this.NewPath_tb_TextChanged);
            // 
            // NewPath_lbl
            // 
            this.NewPath_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPath_lbl.AutoSize = true;
            this.NewPath_lbl.Location = new System.Drawing.Point(9, 81);
            this.NewPath_lbl.Name = "NewPath_lbl";
            this.NewPath_lbl.Size = new System.Drawing.Size(93, 13);
            this.NewPath_lbl.TabIndex = 8;
            this.NewPath_lbl.Text = "Neuer Speicherort";
            // 
            // OpenNewPath_btn
            // 
            this.OpenNewPath_btn.Location = new System.Drawing.Point(12, 123);
            this.OpenNewPath_btn.Name = "OpenNewPath_btn";
            this.OpenNewPath_btn.Size = new System.Drawing.Size(179, 23);
            this.OpenNewPath_btn.TabIndex = 4;
            this.OpenNewPath_btn.Text = "Neuen Speicherort öffnen";
            this.OpenNewPath_btn.UseVisualStyleBackColor = true;
            this.OpenNewPath_btn.Click += new System.EventHandler(this.OpenNewPath_btn_Click);
            // 
            // SelectNewPath_fbd
            // 
            this.SelectNewPath_fbd.Description = "Wöhlen sie einen neuen Ordner aus in dem die Windows Suchinformationen gespeicher" +
    "t werden sollen";
            // 
            // RefreshCurrentPath_btn
            // 
            this.RefreshCurrentPath_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshCurrentPath_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshCurrentPath_btn.Location = new System.Drawing.Point(197, 55);
            this.RefreshCurrentPath_btn.Name = "RefreshCurrentPath_btn";
            this.RefreshCurrentPath_btn.Size = new System.Drawing.Size(178, 23);
            this.RefreshCurrentPath_btn.TabIndex = 3;
            this.RefreshCurrentPath_btn.Text = "Aktuellen Speicherort aktualisieren";
            this.RefreshCurrentPath_btn.UseVisualStyleBackColor = true;
            this.RefreshCurrentPath_btn.Click += new System.EventHandler(this.RefreshCurrentPath_btn_Click);
            // 
            // EditWindowsSearchSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 216);
            this.Controls.Add(this.RefreshCurrentPath_btn);
            this.Controls.Add(this.OpenNewPath_btn);
            this.Controls.Add(this.NewPath_lbl);
            this.Controls.Add(this.NewPath_tb);
            this.Controls.Add(this.ShowFurtherSettings_btn);
            this.Controls.Add(this.CurrentLocation_lbl);
            this.Controls.Add(this.CurrentPath_tb);
            this.Controls.Add(this.Abort_btn);
            this.Controls.Add(this.SaveSettings_btn);
            this.Controls.Add(this.SelectNewPath_btn);
            this.Controls.Add(this.OpenCurrentPath_btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditWindowsSearchSettings";
            this.Text = "Suchindizierung anpassen";
            this.Load += new System.EventHandler(this.EditWindowsSearchSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenCurrentPath_btn;
        private System.Windows.Forms.Button SelectNewPath_btn;
        private System.Windows.Forms.Button SaveSettings_btn;
        private System.Windows.Forms.Button Abort_btn;
        private System.Windows.Forms.TextBox CurrentPath_tb;
        private System.Windows.Forms.Label CurrentLocation_lbl;
        private System.Windows.Forms.Button ShowFurtherSettings_btn;
        private System.Windows.Forms.TextBox NewPath_tb;
        private System.Windows.Forms.Label NewPath_lbl;
        private System.Windows.Forms.Button OpenNewPath_btn;
        private System.Windows.Forms.FolderBrowserDialog SelectNewPath_fbd;
        private System.Windows.Forms.Button RefreshCurrentPath_btn;
    }
}