namespace StorageManagementTool
{
    partial class EditUserShellFolders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditUserShellFolders));
            this.USFOpenCurrentPath_btn = new System.Windows.Forms.Button();
            this.SetUSF_btn = new System.Windows.Forms.Button();
            this.CurrentUSFPath_lbl = new System.Windows.Forms.Label();
            this.NewUSFPath_lbl = new System.Windows.Forms.Label();
            this.NewUSFPath_tb = new System.Windows.Forms.TextBox();
            this.CurrentUSFPath_tb = new System.Windows.Forms.TextBox();
            this.ExistingUSF_lb = new System.Windows.Forms.ListBox();
            this.NewUSFPath_fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.USFOpenNewPath_btn = new System.Windows.Forms.Button();
            this.SetNewUSFPath_btn = new System.Windows.Forms.Button();
            this.Abort_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // USFOpenCurrentPath_btn
            // 
            this.USFOpenCurrentPath_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.USFOpenCurrentPath_btn.Location = new System.Drawing.Point(12, 205);
            this.USFOpenCurrentPath_btn.Name = "USFOpenCurrentPath_btn";
            this.USFOpenCurrentPath_btn.Size = new System.Drawing.Size(269, 23);
            this.USFOpenCurrentPath_btn.TabIndex = 18;
            this.USFOpenCurrentPath_btn.Text = "Aktuellen Pfad Öffnen";
            this.USFOpenCurrentPath_btn.UseVisualStyleBackColor = true;
            this.USFOpenCurrentPath_btn.Click += new System.EventHandler(this.USFOpenCurrentPath_btn_Click);
            // 
            // SetUSF_btn
            // 
            this.SetUSF_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetUSF_btn.Location = new System.Drawing.Point(109, 331);
            this.SetUSF_btn.Name = "SetUSF_btn";
            this.SetUSF_btn.Size = new System.Drawing.Size(172, 23);
            this.SetUSF_btn.TabIndex = 12;
            this.SetUSF_btn.Text = "Neuen Pfad übernehmen";
            this.SetUSF_btn.UseVisualStyleBackColor = true;
            this.SetUSF_btn.Click += new System.EventHandler(this.SetUSF_btn_Click);
            // 
            // CurrentUSFPath_lbl
            // 
            this.CurrentUSFPath_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentUSFPath_lbl.AutoSize = true;
            this.CurrentUSFPath_lbl.Location = new System.Drawing.Point(12, 163);
            this.CurrentUSFPath_lbl.Name = "CurrentUSFPath_lbl";
            this.CurrentUSFPath_lbl.Size = new System.Drawing.Size(73, 13);
            this.CurrentUSFPath_lbl.TabIndex = 10;
            this.CurrentUSFPath_lbl.Text = "Aktueller Pfad";
            // 
            // NewUSFPath_lbl
            // 
            this.NewUSFPath_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewUSFPath_lbl.AutoSize = true;
            this.NewUSFPath_lbl.Location = new System.Drawing.Point(12, 231);
            this.NewUSFPath_lbl.Name = "NewUSFPath_lbl";
            this.NewUSFPath_lbl.Size = new System.Drawing.Size(61, 13);
            this.NewUSFPath_lbl.TabIndex = 9;
            this.NewUSFPath_lbl.Text = "Neuer Pfad";
            // 
            // NewUSFPath_tb
            // 
            this.NewUSFPath_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewUSFPath_tb.Location = new System.Drawing.Point(12, 247);
            this.NewUSFPath_tb.Name = "NewUSFPath_tb";
            this.NewUSFPath_tb.ReadOnly = true;
            this.NewUSFPath_tb.Size = new System.Drawing.Size(269, 20);
            this.NewUSFPath_tb.TabIndex = 2;
            this.NewUSFPath_tb.TextChanged += new System.EventHandler(this.NewUSFPath_tb_TextChanged);
            // 
            // CurrentUSFPath_tb
            // 
            this.CurrentUSFPath_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentUSFPath_tb.Location = new System.Drawing.Point(12, 179);
            this.CurrentUSFPath_tb.Name = "CurrentUSFPath_tb";
            this.CurrentUSFPath_tb.ReadOnly = true;
            this.CurrentUSFPath_tb.Size = new System.Drawing.Size(269, 20);
            this.CurrentUSFPath_tb.TabIndex = 1;
            this.CurrentUSFPath_tb.TextChanged += new System.EventHandler(this.CurrentUSFPath_tb_TextChanged);
            // 
            // ExistingUSF_lb
            // 
            this.ExistingUSF_lb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExistingUSF_lb.FormattingEnabled = true;
            this.ExistingUSF_lb.Location = new System.Drawing.Point(12, 12);
            this.ExistingUSF_lb.Name = "ExistingUSF_lb";
            this.ExistingUSF_lb.Size = new System.Drawing.Size(269, 147);
            this.ExistingUSF_lb.TabIndex = 0;
            this.ExistingUSF_lb.SelectedIndexChanged += new System.EventHandler(this.ExistingUSF_lb_SelectedIndexChanged);
            // 
            // USFOpenNewPath_btn
            // 
            this.USFOpenNewPath_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.USFOpenNewPath_btn.Location = new System.Drawing.Point(12, 273);
            this.USFOpenNewPath_btn.Name = "USFOpenNewPath_btn";
            this.USFOpenNewPath_btn.Size = new System.Drawing.Size(269, 23);
            this.USFOpenNewPath_btn.TabIndex = 19;
            this.USFOpenNewPath_btn.Text = "Neuen Pfad Öffnen";
            this.USFOpenNewPath_btn.UseVisualStyleBackColor = true;
            this.USFOpenNewPath_btn.Click += new System.EventHandler(this.USFOpenNewPath_btn_Click);
            // 
            // SetNewUSFPath_btn
            // 
            this.SetNewUSFPath_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetNewUSFPath_btn.Location = new System.Drawing.Point(12, 302);
            this.SetNewUSFPath_btn.Name = "SetNewUSFPath_btn";
            this.SetNewUSFPath_btn.Size = new System.Drawing.Size(269, 23);
            this.SetNewUSFPath_btn.TabIndex = 11;
            this.SetNewUSFPath_btn.Text = "Neuen Pfad auswählen";
            this.SetNewUSFPath_btn.UseVisualStyleBackColor = true;
            this.SetNewUSFPath_btn.Click += new System.EventHandler(this.SetNewUSFPath_btn_Click);
            // 
            // Abort_btn
            // 
            this.Abort_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Abort_btn.Location = new System.Drawing.Point(12, 330);
            this.Abort_btn.Name = "Abort_btn";
            this.Abort_btn.Size = new System.Drawing.Size(91, 23);
            this.Abort_btn.TabIndex = 20;
            this.Abort_btn.Text = "Abbrechen";
            this.Abort_btn.UseVisualStyleBackColor = true;
            this.Abort_btn.Click += new System.EventHandler(this.Abort_btn_Click);
            // 
            // EditUserShellFolders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 366);
            this.Controls.Add(this.Abort_btn);
            this.Controls.Add(this.SetNewUSFPath_btn);
            this.Controls.Add(this.USFOpenNewPath_btn);
            this.Controls.Add(this.USFOpenCurrentPath_btn);
            this.Controls.Add(this.ExistingUSF_lb);
            this.Controls.Add(this.SetUSF_btn);
            this.Controls.Add(this.CurrentUSFPath_tb);
            this.Controls.Add(this.NewUSFPath_tb);
            this.Controls.Add(this.CurrentUSFPath_lbl);
            this.Controls.Add(this.NewUSFPath_lbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditUserShellFolders";
            this.Text = "UserShellFolder bearbeiten";
            this.Load += new System.EventHandler(this.EditUserShellFolders_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button USFOpenCurrentPath_btn;
        private System.Windows.Forms.Button SetUSF_btn;
        private System.Windows.Forms.Label CurrentUSFPath_lbl;
        private System.Windows.Forms.Label NewUSFPath_lbl;
        private System.Windows.Forms.TextBox NewUSFPath_tb;
        private System.Windows.Forms.TextBox CurrentUSFPath_tb;
        private System.Windows.Forms.ListBox ExistingUSF_lb;
        private System.Windows.Forms.FolderBrowserDialog NewUSFPath_fbd;
        private System.Windows.Forms.Button USFOpenNewPath_btn;
        private System.Windows.Forms.Button SetNewUSFPath_btn;
        private System.Windows.Forms.Button Abort_btn;
    }
}