using System.Windows.Forms;

namespace ExtendedMessageBoxLibary
{
   partial class ExtendedMessageBoxDialog
   {
      private Button[] MsgBoxDialogbtns;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtendedMessageBoxDialog));
            this.checkableBox = new System.Windows.Forms.CheckBox();
            this.MainText_tb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // checkableBox
            // 
            this.checkableBox.AutoSize = true;
            this.checkableBox.Location = new System.Drawing.Point(12, 78);
            this.checkableBox.Name = "checkableBox";
            this.checkableBox.Size = new System.Drawing.Size(80, 17);
            this.checkableBox.TabIndex = 7;
            this.checkableBox.Text = "checkBox1";
            this.checkableBox.UseVisualStyleBackColor = true;
            // 
            // MainText_tb
            // 
            this.MainText_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainText_tb.BackColor = System.Drawing.SystemColors.Control;
            this.MainText_tb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MainText_tb.Location = new System.Drawing.Point(12, 12);
            this.MainText_tb.Multiline = true;
            this.MainText_tb.Name = "MainText_tb";
            this.MainText_tb.Size = new System.Drawing.Size(316, 60);
            this.MainText_tb.TabIndex = 8;
            this.MainText_tb.Text = "If you see this something gone wrong";
            // 
            // ExtendedMessageBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(340, 160);
            this.Controls.Add(this.MainText_tb);
            this.Controls.Add(this.checkableBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtendedMessageBoxDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExtendedMessageBoxDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExtendedMessageBoxDialog_FormClosed);
            this.Load += new System.EventHandler(this.ExtendedMessageBoxDialog_Load);
            this.ResizeEnd += new System.EventHandler(this.ExtendedMessageBoxDialog_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.ExtendedMessageBoxDialog_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion
      private CheckBox checkableBox;
        private TextBox MainText_tb;
    }
}