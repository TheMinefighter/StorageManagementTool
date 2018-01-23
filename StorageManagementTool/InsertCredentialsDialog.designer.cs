using System;
using System.Windows.Forms;
namespace StorageManagementTool
{
    sealed partial class InsertCredentialsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertCredentialsDialog));
            this.Username_tb = new System.Windows.Forms.TextBox();
            this.Headline0_lbl = new System.Windows.Forms.Label();
            this.Ok_btn = new System.Windows.Forms.Button();
            this.Password_lbl = new System.Windows.Forms.Label();
            this.Password_tb = new System.Windows.Forms.TextBox();
            this.Username_lbl = new System.Windows.Forms.Label();
            this.Headline1_lbl = new System.Windows.Forms.Label();
            this.Abort_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Username_tb
            // 
            resources.ApplyResources(this.Username_tb, "Username_tb");
            this.Username_tb.Name = "Username_tb";
            // 
            // Headline0_lbl
            // 
            resources.ApplyResources(this.Headline0_lbl, "Headline0_lbl");
            this.Headline0_lbl.Name = "Headline0_lbl";
            // 
            // Ok_btn
            // 
            resources.ApplyResources(this.Ok_btn, "Ok_btn");
            this.Ok_btn.Name = "Ok_btn";
            this.Ok_btn.UseVisualStyleBackColor = true;
            this.Ok_btn.Click += new System.EventHandler(this.Ok_btn_Click);
            // 
            // Password_lbl
            // 
            resources.ApplyResources(this.Password_lbl, "Password_lbl");
            this.Password_lbl.Name = "Password_lbl";
            // 
            // Password_tb
            // 
            resources.ApplyResources(this.Password_tb, "Password_tb");
            this.Password_tb.Name = "Password_tb";
            this.Password_tb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Password_tb_KeyDown);
            // 
            // Username_lbl
            // 
            resources.ApplyResources(this.Username_lbl, "Username_lbl");
            this.Username_lbl.Name = "Username_lbl";
            // 
            // Headline1_lbl
            // 
            resources.ApplyResources(this.Headline1_lbl, "Headline1_lbl");
            this.Headline1_lbl.Name = "Headline1_lbl";
            // 
            // Abort_btn
            // 
            resources.ApplyResources(this.Abort_btn, "Abort_btn");
            this.Abort_btn.Name = "Abort_btn";
            this.Abort_btn.UseVisualStyleBackColor = true;
            this.Abort_btn.Click += new System.EventHandler(this.Abort_btn_Click);
            // 
            // InsertCredentialsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Abort_btn);
            this.Controls.Add(this.Headline1_lbl);
            this.Controls.Add(this.Username_lbl);
            this.Controls.Add(this.Password_lbl);
            this.Controls.Add(this.Password_tb);
            this.Controls.Add(this.Ok_btn);
            this.Controls.Add(this.Headline0_lbl);
            this.Controls.Add(this.Username_tb);
            this.Name = "InsertCredentialsDialog";
            this.Load += new System.EventHandler(this.InsertCredentialsDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Username_tb;
        private System.Windows.Forms.Label Headline0_lbl;
        private System.Windows.Forms.Button Ok_btn;
        private System.Windows.Forms.Label Password_lbl;
        private System.Windows.Forms.TextBox Password_tb;
        private System.Windows.Forms.Label Username_lbl;
        private System.Windows.Forms.Label Headline1_lbl;
        private Button Abort_btn;
    }
}