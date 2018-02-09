namespace StorageManagementTool.GUI
{
    partial class ApplyPreset
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
            this.SelectScenario_lb = new System.Windows.Forms.ListBox();
            this.SelectScenario_lbl = new System.Windows.Forms.Label();
            this.SelectSSD_lb = new System.Windows.Forms.ListBox();
            this.SelectHDD_lb = new System.Windows.Forms.ListBox();
            this.SelectSSD_lbl = new System.Windows.Forms.Label();
            this.SelectHDD_lbl = new System.Windows.Forms.Label();
            this.ApplyPreset_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectScenario_lb
            // 
            this.SelectScenario_lb.FormattingEnabled = true;
            this.SelectScenario_lb.Location = new System.Drawing.Point(9, 139);
            this.SelectScenario_lb.Name = "SelectScenario_lb";
            this.SelectScenario_lb.Size = new System.Drawing.Size(158, 43);
            this.SelectScenario_lb.TabIndex = 0;
            // 
            // SelectScenario_lbl
            // 
            this.SelectScenario_lbl.AutoSize = true;
            this.SelectScenario_lbl.Location = new System.Drawing.Point(9, 123);
            this.SelectScenario_lbl.Name = "SelectScenario_lbl";
            this.SelectScenario_lbl.Size = new System.Drawing.Size(35, 13);
            this.SelectScenario_lbl.TabIndex = 1;
            this.SelectScenario_lbl.Text = "label1";
            // 
            // SelectSSD_lb
            // 
            this.SelectSSD_lb.FormattingEnabled = true;
            this.SelectSSD_lb.Location = new System.Drawing.Point(12, 25);
            this.SelectSSD_lb.Name = "SelectSSD_lb";
            this.SelectSSD_lb.Size = new System.Drawing.Size(155, 95);
            this.SelectSSD_lb.TabIndex = 2;
            // 
            // SelectHDD_lb
            // 
            this.SelectHDD_lb.FormattingEnabled = true;
            this.SelectHDD_lb.Location = new System.Drawing.Point(173, 25);
            this.SelectHDD_lb.Name = "SelectHDD_lb";
            this.SelectHDD_lb.Size = new System.Drawing.Size(153, 95);
            this.SelectHDD_lb.TabIndex = 3;
            // 
            // SelectSSD_lbl
            // 
            this.SelectSSD_lbl.AutoSize = true;
            this.SelectSSD_lbl.Location = new System.Drawing.Point(12, 9);
            this.SelectSSD_lbl.Name = "SelectSSD_lbl";
            this.SelectSSD_lbl.Size = new System.Drawing.Size(35, 13);
            this.SelectSSD_lbl.TabIndex = 4;
            this.SelectSSD_lbl.Text = "label2";
            // 
            // SelectHDD_lbl
            // 
            this.SelectHDD_lbl.AutoSize = true;
            this.SelectHDD_lbl.Location = new System.Drawing.Point(170, 9);
            this.SelectHDD_lbl.Name = "SelectHDD_lbl";
            this.SelectHDD_lbl.Size = new System.Drawing.Size(35, 13);
            this.SelectHDD_lbl.TabIndex = 5;
            this.SelectHDD_lbl.Text = "label3";
            // 
            // ApplyPreset_btn
            // 
            this.ApplyPreset_btn.Location = new System.Drawing.Point(173, 159);
            this.ApplyPreset_btn.Name = "ApplyPreset_btn";
            this.ApplyPreset_btn.Size = new System.Drawing.Size(153, 23);
            this.ApplyPreset_btn.TabIndex = 6;
            this.ApplyPreset_btn.Text = "button1";
            this.ApplyPreset_btn.UseVisualStyleBackColor = true;
            this.ApplyPreset_btn.Click += new System.EventHandler(this.ApplyPreset_btn_Click);
            // 
            // ApplyPreset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 198);
            this.Controls.Add(this.ApplyPreset_btn);
            this.Controls.Add(this.SelectHDD_lbl);
            this.Controls.Add(this.SelectSSD_lbl);
            this.Controls.Add(this.SelectHDD_lb);
            this.Controls.Add(this.SelectSSD_lb);
            this.Controls.Add(this.SelectScenario_lbl);
            this.Controls.Add(this.SelectScenario_lb);
            this.Name = "ApplyPreset";
            this.Text = "ApplyPreset";
            this.Load += new System.EventHandler(this.ApplyPreset_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox SelectScenario_lb;
        private System.Windows.Forms.Label SelectScenario_lbl;
        private System.Windows.Forms.ListBox SelectSSD_lb;
        private System.Windows.Forms.ListBox SelectHDD_lb;
        private System.Windows.Forms.Label SelectSSD_lbl;
        private System.Windows.Forms.Label SelectHDD_lbl;
        private System.Windows.Forms.Button ApplyPreset_btn;
    }
}