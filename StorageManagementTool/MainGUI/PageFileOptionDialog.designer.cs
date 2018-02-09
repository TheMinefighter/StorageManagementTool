namespace StorageManagementTool.GUI
{
    partial class PageFileOptionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageFileOptionDialog));
            this.HiberfilSettings_gb = new System.Windows.Forms.GroupBox();
            this.DisableHibernate_btn = new System.Windows.Forms.Button();
            this.Enablehibernate_btn = new System.Windows.Forms.Button();
            this.PagefileSettings_gb = new System.Windows.Forms.GroupBox();
            this.MinimumPagefileSize_nud = new System.Windows.Forms.NumericUpDown();
            this.MaximumPagefilesize_nud = new System.Windows.Forms.NumericUpDown();
            this.ExtendedPagefileOptions_btn = new System.Windows.Forms.Button();
            this.ApplyPagefileChanges_btn = new System.Windows.Forms.Button();
            this.MaximumPagefileSize_lbl = new System.Windows.Forms.Label();
            this.WhereToSaveThePagefile_lbl = new System.Windows.Forms.Label();
            this.MinimumPagefileSize_lbl = new System.Windows.Forms.Label();
            this.Pagefilepartition_lb = new System.Windows.Forms.ListBox();
            this.SwapfileSettings_gb = new System.Windows.Forms.GroupBox();
            this.Swapfilepartition_lb = new System.Windows.Forms.ListBox();
            this.Swapfileinfo_tb = new System.Windows.Forms.TextBox();
            this.SwapfileStepBack_btn = new System.Windows.Forms.Button();
            this.SwapfileStepForward_btn = new System.Windows.Forms.Button();
            this.RefreshAvailableParitions_btn = new System.Windows.Forms.Button();
            this.ProgramStatusStrip = new System.Windows.Forms.StatusStrip();
            this.HiberfilSettings_gb.SuspendLayout();
            this.PagefileSettings_gb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumPagefileSize_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximumPagefilesize_nud)).BeginInit();
            this.SwapfileSettings_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // HiberfilSettings_gb
            // 
            this.HiberfilSettings_gb.Controls.Add(this.DisableHibernate_btn);
            this.HiberfilSettings_gb.Controls.Add(this.Enablehibernate_btn);
            this.HiberfilSettings_gb.Location = new System.Drawing.Point(12, 12);
            this.HiberfilSettings_gb.Name = "HiberfilSettings_gb";
            this.HiberfilSettings_gb.Size = new System.Drawing.Size(356, 78);
            this.HiberfilSettings_gb.TabIndex = 0;
            this.HiberfilSettings_gb.TabStop = false;
            this.HiberfilSettings_gb.Text = "hiberfil.sys";
            // 
            // DisableHibernate_btn
            // 
            this.DisableHibernate_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisableHibernate_btn.Location = new System.Drawing.Point(6, 19);
            this.DisableHibernate_btn.Name = "DisableHibernate_btn";
            this.DisableHibernate_btn.Size = new System.Drawing.Size(344, 23);
            this.DisableHibernate_btn.TabIndex = 10;
            this.DisableHibernate_btn.Text = "Ruhezustand und Hybrides Herunterfahren deaktivieren";
            this.DisableHibernate_btn.UseVisualStyleBackColor = true;
            this.DisableHibernate_btn.Click += new System.EventHandler(this.DisableHibernate_btn_Click);
            // 
            // Enablehibernate_btn
            // 
            this.Enablehibernate_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Enablehibernate_btn.Location = new System.Drawing.Point(6, 48);
            this.Enablehibernate_btn.Name = "Enablehibernate_btn";
            this.Enablehibernate_btn.Size = new System.Drawing.Size(344, 23);
            this.Enablehibernate_btn.TabIndex = 11;
            this.Enablehibernate_btn.Text = "Ruhezustand  und Hybrides Herunterfahren aktivieren";
            this.Enablehibernate_btn.UseVisualStyleBackColor = true;
            this.Enablehibernate_btn.Click += new System.EventHandler(this.Enablehibernate_btn_Click);
            // 
            // PagefileSettings_gb
            // 
            this.PagefileSettings_gb.Controls.Add(this.MinimumPagefileSize_nud);
            this.PagefileSettings_gb.Controls.Add(this.MaximumPagefilesize_nud);
            this.PagefileSettings_gb.Controls.Add(this.ExtendedPagefileOptions_btn);
            this.PagefileSettings_gb.Controls.Add(this.ApplyPagefileChanges_btn);
            this.PagefileSettings_gb.Controls.Add(this.MaximumPagefileSize_lbl);
            this.PagefileSettings_gb.Controls.Add(this.WhereToSaveThePagefile_lbl);
            this.PagefileSettings_gb.Controls.Add(this.MinimumPagefileSize_lbl);
            this.PagefileSettings_gb.Controls.Add(this.Pagefilepartition_lb);
            this.PagefileSettings_gb.Location = new System.Drawing.Point(12, 97);
            this.PagefileSettings_gb.Name = "PagefileSettings_gb";
            this.PagefileSettings_gb.Size = new System.Drawing.Size(356, 156);
            this.PagefileSettings_gb.TabIndex = 1;
            this.PagefileSettings_gb.TabStop = false;
            this.PagefileSettings_gb.Text = "pagefile.sys";
            // 
            // MinimumPagefileSize_nud
            // 
            this.MinimumPagefileSize_nud.Increment = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.MinimumPagefileSize_nud.Location = new System.Drawing.Point(216, 31);
            this.MinimumPagefileSize_nud.Maximum = new decimal(new int[] {
            262144,
            0,
            0,
            0});
            this.MinimumPagefileSize_nud.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.MinimumPagefileSize_nud.Name = "MinimumPagefileSize_nud";
            this.MinimumPagefileSize_nud.Size = new System.Drawing.Size(134, 20);
            this.MinimumPagefileSize_nud.TabIndex = 10;
            this.MinimumPagefileSize_nud.ThousandsSeparator = true;
            this.MinimumPagefileSize_nud.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            // 
            // MaximumPagefilesize_nud
            // 
            this.MaximumPagefilesize_nud.Increment = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.MaximumPagefilesize_nud.Location = new System.Drawing.Point(216, 72);
            this.MaximumPagefilesize_nud.Maximum = new decimal(new int[] {
            262144,
            0,
            0,
            0});
            this.MaximumPagefilesize_nud.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.MaximumPagefilesize_nud.Name = "MaximumPagefilesize_nud";
            this.MaximumPagefilesize_nud.Size = new System.Drawing.Size(134, 20);
            this.MaximumPagefilesize_nud.TabIndex = 9;
            this.MaximumPagefilesize_nud.ThousandsSeparator = true;
            this.MaximumPagefilesize_nud.Value = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            // 
            // ExtendedPagefileOptions_btn
            // 
            this.ExtendedPagefileOptions_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtendedPagefileOptions_btn.Location = new System.Drawing.Point(213, 97);
            this.ExtendedPagefileOptions_btn.Name = "ExtendedPagefileOptions_btn";
            this.ExtendedPagefileOptions_btn.Size = new System.Drawing.Size(137, 23);
            this.ExtendedPagefileOptions_btn.TabIndex = 8;
            this.ExtendedPagefileOptions_btn.Text = "Weitere Optionen";
            this.ExtendedPagefileOptions_btn.UseVisualStyleBackColor = true;
            this.ExtendedPagefileOptions_btn.Click += new System.EventHandler(this.ExtendedPagefileOptions_btn_Click);
            // 
            // ApplyPagefileChanges_btn
            // 
            this.ApplyPagefileChanges_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyPagefileChanges_btn.Location = new System.Drawing.Point(213, 126);
            this.ApplyPagefileChanges_btn.Name = "ApplyPagefileChanges_btn";
            this.ApplyPagefileChanges_btn.Size = new System.Drawing.Size(137, 23);
            this.ApplyPagefileChanges_btn.TabIndex = 7;
            this.ApplyPagefileChanges_btn.Text = "OK";
            this.ApplyPagefileChanges_btn.UseVisualStyleBackColor = true;
            this.ApplyPagefileChanges_btn.Click += new System.EventHandler(this.ApplyPagefileChanges_btn_Click);
            // 
            // MaximumPagefileSize_lbl
            // 
            this.MaximumPagefileSize_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximumPagefileSize_lbl.AutoSize = true;
            this.MaximumPagefileSize_lbl.Location = new System.Drawing.Point(213, 55);
            this.MaximumPagefileSize_lbl.Name = "MaximumPagefileSize_lbl";
            this.MaximumPagefileSize_lbl.Size = new System.Drawing.Size(108, 13);
            this.MaximumPagefileSize_lbl.TabIndex = 6;
            this.MaximumPagefileSize_lbl.Text = "Maximale Größe (MB)";
            // 
            // WhereToSaveThePagefile_lbl
            // 
            this.WhereToSaveThePagefile_lbl.AutoSize = true;
            this.WhereToSaveThePagefile_lbl.Location = new System.Drawing.Point(6, 16);
            this.WhereToSaveThePagefile_lbl.Name = "WhereToSaveThePagefile_lbl";
            this.WhereToSaveThePagefile_lbl.Size = new System.Drawing.Size(137, 13);
            this.WhereToSaveThePagefile_lbl.TabIndex = 5;
            this.WhereToSaveThePagefile_lbl.Text = "Speicherort der pagefile.sys";
            // 
            // MinimumPagefileSize_lbl
            // 
            this.MinimumPagefileSize_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimumPagefileSize_lbl.AutoSize = true;
            this.MinimumPagefileSize_lbl.Location = new System.Drawing.Point(210, 16);
            this.MinimumPagefileSize_lbl.Name = "MinimumPagefileSize_lbl";
            this.MinimumPagefileSize_lbl.Size = new System.Drawing.Size(105, 13);
            this.MinimumPagefileSize_lbl.TabIndex = 4;
            this.MinimumPagefileSize_lbl.Text = "Minimale Größe (MB)";
            // 
            // Pagefilepartition_lb
            // 
            this.Pagefilepartition_lb.FormattingEnabled = true;
            this.Pagefilepartition_lb.Location = new System.Drawing.Point(8, 31);
            this.Pagefilepartition_lb.Name = "Pagefilepartition_lb";
            this.Pagefilepartition_lb.Size = new System.Drawing.Size(199, 108);
            this.Pagefilepartition_lb.TabIndex = 0;
            this.Pagefilepartition_lb.SelectedIndexChanged += new System.EventHandler(this.Pagefilepartition_lb_SelectedIndexChanged);
            // 
            // SwapfileSettings_gb
            // 
            this.SwapfileSettings_gb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SwapfileSettings_gb.Controls.Add(this.Swapfilepartition_lb);
            this.SwapfileSettings_gb.Controls.Add(this.Swapfileinfo_tb);
            this.SwapfileSettings_gb.Controls.Add(this.SwapfileStepBack_btn);
            this.SwapfileSettings_gb.Controls.Add(this.SwapfileStepForward_btn);
            this.SwapfileSettings_gb.Location = new System.Drawing.Point(12, 259);
            this.SwapfileSettings_gb.Name = "SwapfileSettings_gb";
            this.SwapfileSettings_gb.Size = new System.Drawing.Size(356, 163);
            this.SwapfileSettings_gb.TabIndex = 2;
            this.SwapfileSettings_gb.TabStop = false;
            this.SwapfileSettings_gb.Text = "swapfile.sys (nur >= Windows 8)";
            // 
            // Swapfilepartition_lb
            // 
            this.Swapfilepartition_lb.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Swapfilepartition_lb.FormattingEnabled = true;
            this.Swapfilepartition_lb.Location = new System.Drawing.Point(6, 88);
            this.Swapfilepartition_lb.Name = "Swapfilepartition_lb";
            this.Swapfilepartition_lb.Size = new System.Drawing.Size(201, 69);
            this.Swapfilepartition_lb.TabIndex = 3;
            // 
            // Swapfileinfo_tb
            // 
            this.Swapfileinfo_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Swapfileinfo_tb.BackColor = System.Drawing.SystemColors.Control;
            this.Swapfileinfo_tb.Location = new System.Drawing.Point(6, 19);
            this.Swapfileinfo_tb.Multiline = true;
            this.Swapfileinfo_tb.Name = "Swapfileinfo_tb";
            this.Swapfileinfo_tb.ReadOnly = true;
            this.Swapfileinfo_tb.Size = new System.Drawing.Size(344, 63);
            this.Swapfileinfo_tb.TabIndex = 2;
            this.Swapfileinfo_tb.Text = "Fehler:\r\nEin Unbekannter Fehler ist aufgetreten.";
            // 
            // SwapfileStepBack_btn
            // 
            this.SwapfileStepBack_btn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SwapfileStepBack_btn.Location = new System.Drawing.Point(213, 134);
            this.SwapfileStepBack_btn.Name = "SwapfileStepBack_btn";
            this.SwapfileStepBack_btn.Size = new System.Drawing.Size(137, 23);
            this.SwapfileStepBack_btn.TabIndex = 1;
            this.SwapfileStepBack_btn.Text = "Schritt zurück";
            this.SwapfileStepBack_btn.UseVisualStyleBackColor = true;
            this.SwapfileStepBack_btn.Click += new System.EventHandler(this.SwapfileStepBack_btn_Click);
            // 
            // SwapfileStepForward_btn
            // 
            this.SwapfileStepForward_btn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SwapfileStepForward_btn.Location = new System.Drawing.Point(213, 88);
            this.SwapfileStepForward_btn.Name = "SwapfileStepForward_btn";
            this.SwapfileStepForward_btn.Size = new System.Drawing.Size(137, 40);
            this.SwapfileStepForward_btn.TabIndex = 0;
            this.SwapfileStepForward_btn.Text = "Nächster Schritt";
            this.SwapfileStepForward_btn.UseVisualStyleBackColor = true;
            this.SwapfileStepForward_btn.Click += new System.EventHandler(this.SwapfileStepForward_btn_Click);
            // 
            // RefreshAvailableParitions_btn
            // 
            this.RefreshAvailableParitions_btn.Location = new System.Drawing.Point(12, 428);
            this.RefreshAvailableParitions_btn.Name = "RefreshAvailableParitions_btn";
            this.RefreshAvailableParitions_btn.Size = new System.Drawing.Size(356, 21);
            this.RefreshAvailableParitions_btn.TabIndex = 3;
            this.RefreshAvailableParitions_btn.Text = "Verfügbare Partitionen aktualisieren";
            this.RefreshAvailableParitions_btn.UseVisualStyleBackColor = true;
            this.RefreshAvailableParitions_btn.Click += new System.EventHandler(this.RefreshAvailablePartitions_btn_Click);
            // 
            // ProgramStatusStrip
            // 
            this.ProgramStatusStrip.Location = new System.Drawing.Point(0, 464);
            this.ProgramStatusStrip.Name = "ProgramStatusStrip";
            this.ProgramStatusStrip.Size = new System.Drawing.Size(380, 22);
            this.ProgramStatusStrip.TabIndex = 4;
            this.ProgramStatusStrip.Text = "statusStrip1";
            // 
            // PageFileOptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 486);
            this.Controls.Add(this.ProgramStatusStrip);
            this.Controls.Add(this.RefreshAvailableParitions_btn);
            this.Controls.Add(this.SwapfileSettings_gb);
            this.Controls.Add(this.HiberfilSettings_gb);
            this.Controls.Add(this.PagefileSettings_gb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PageFileOptionDialog";
            this.Text = "Arbeitsspeicher-Auslagerungsdateien Verwalten";
            this.Load += new System.EventHandler(this.PageFileOptionDialog_Load);
            this.HiberfilSettings_gb.ResumeLayout(false);
            this.PagefileSettings_gb.ResumeLayout(false);
            this.PagefileSettings_gb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumPagefileSize_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximumPagefilesize_nud)).EndInit();
            this.SwapfileSettings_gb.ResumeLayout(false);
            this.SwapfileSettings_gb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox HiberfilSettings_gb;
        private System.Windows.Forms.Button DisableHibernate_btn;
        private System.Windows.Forms.Button Enablehibernate_btn;
        private System.Windows.Forms.GroupBox PagefileSettings_gb;
        private System.Windows.Forms.Button ExtendedPagefileOptions_btn;
        private System.Windows.Forms.Button ApplyPagefileChanges_btn;
        private System.Windows.Forms.Label MaximumPagefileSize_lbl;
        private System.Windows.Forms.Label WhereToSaveThePagefile_lbl;
        private System.Windows.Forms.Label MinimumPagefileSize_lbl;
        private System.Windows.Forms.ListBox Pagefilepartition_lb;
        private System.Windows.Forms.GroupBox SwapfileSettings_gb;
        private System.Windows.Forms.ListBox Swapfilepartition_lb;
        private System.Windows.Forms.TextBox Swapfileinfo_tb;
        private System.Windows.Forms.Button SwapfileStepBack_btn;
        private System.Windows.Forms.Button SwapfileStepForward_btn;
        private System.Windows.Forms.Button RefreshAvailableParitions_btn;
        private System.Windows.Forms.StatusStrip ProgramStatusStrip;
        private System.Windows.Forms.NumericUpDown MinimumPagefileSize_nud;
        private System.Windows.Forms.NumericUpDown MaximumPagefilesize_nud;
    }
}