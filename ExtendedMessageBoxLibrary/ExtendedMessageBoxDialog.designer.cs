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
            this.Textlbl = new System.Windows.Forms.Label();
            this.checkableBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Textlbl
            // 
            this.Textlbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Textlbl.AutoSize = true;
            this.Textlbl.Location = new System.Drawing.Point(12, 9);
            this.Textlbl.Name = "Textlbl";
            this.Textlbl.Size = new System.Drawing.Size(13, 13);
            this.Textlbl.TabIndex = 6;
            this.Textlbl.Text = "g";
            // 
            // checkableBox
            // 
            this.checkableBox.AutoSize = true;
            this.checkableBox.Location = new System.Drawing.Point(12, 25);
            this.checkableBox.Name = "checkableBox";
            this.checkableBox.Size = new System.Drawing.Size(80, 17);
            this.checkableBox.TabIndex = 7;
            this.checkableBox.Text = "checkBox1";
            this.checkableBox.UseVisualStyleBackColor = true;
            // 
            // ExtendedMessageBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(340, 160);
            this.Controls.Add(this.checkableBox);
            this.Controls.Add(this.Textlbl);
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
      private System.Windows.Forms.Label Textlbl;
      private CheckBox checkableBox;
   }
}