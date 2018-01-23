using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExtendedMessageBoxLibary
{
   public partial class ExtendedMessageBoxDialog : Form
   
   {
      private ExtendedMessageBox _msgBoxOwner;
      private bool _checkboxexisting;
      public ExtendedMessageBoxDialog()
      {
         InitializeComponent();
      }

      public void ExtendedMessageBoxDialog_ResizeEnd(object sender, EventArgs e)
      {
         
      }

      private void ExtendedMessageBoxDialog_SizeChanged(object sender, EventArgs e)
      {
         checkableBox.Location = new Point(checkableBox.Location.X, 9+Textlbl.Size.Height+3+Textlbl.Location.Y);
         int btnY = this.checkableBox.Location.Y;
         if (_checkboxexisting)
         {
            btnY = btnY+checkableBox.Size.Height + 6;
         } 
         double btnsize =(double) (this.Size.Width - 12-16-6)/((ExtendedMessageBox )this.Tag).ExMsgBoxcfg.NumberOfBbuttons-6;
         double currentleftallign = 12;

         foreach (Button i in this.MsgBoxDialogbtns)
         {
            i.Size = new Size((int) Math.Round(btnsize), this.Height-51-btnY);
            i.Location = new Point((int)Math.Round(currentleftallign), btnY);
            currentleftallign = 6 + btnsize+currentleftallign;
         }


      }
      private void ExtendedMessageBoxDialog_Load(object sender, EventArgs e)
      {
         this._msgBoxOwner = ((ExtendedMessageBox)this.Tag);
         this._checkboxexisting = (_msgBoxOwner.ExMsgBoxcfg.ShowCheckBox);
         this.Text = _msgBoxOwner.ExMsgBoxcfg.Title;
         this.Textlbl.Text = ExtendedMessageBox.ConnectStringArrayWithConnector(_msgBoxOwner.ExMsgBoxcfg.Text,Environment.NewLine);
         this.MsgBoxDialogbtns = new Button[_msgBoxOwner.ExMsgBoxcfg.NumberOfBbuttons];
         for (int i = 0; i < this.MsgBoxDialogbtns.Length; i++)
         { 
             this.MsgBoxDialogbtns[i] = new Button
             {
                 Location = new Point(12, 48),
                 Name = "button" + i.ToString(),
                 Size = new Size(75, 23),
                 TabIndex = 2,
                 Text = _msgBoxOwner.ExMsgBoxcfg.Buttons[i],
                 UseVisualStyleBackColor = true,
                 Tag = i
             };
             this.MsgBoxDialogbtns[i].Click += new EventHandler(ExtendedMessageBoxDialogbtn_click);
            this.MsgBoxDialogbtns[i].Anchor = (AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)| AnchorStyles.Top);
            this.Controls.Add(MsgBoxDialogbtns[i]);
         }
         this.ExtendedMessageBoxDialog_SizeChanged(null, null);
         
         //   this.Size = new Size(this.Size.Width,MsgBoxDialogbtns[0].Location.Y+MsgBoxDialogbtns[0].Size.Height+51);
         if (_msgBoxOwner.ExMsgBoxcfg.ShowCheckBox)
         {
            checkableBox.Text ="Auswahl merken";
         }
         else
         {
            this.Controls.Remove(this.checkableBox);
            this.checkableBox.Visible = false;
         }
      }

      private void ExtendedMessageBoxDialogbtn_click(object sender, EventArgs e)
      {
         _msgBoxOwner.ToBeReturned = new ExtendedMessageBoxResult(_msgBoxOwner.ExMsgBoxcfg, ((int)((Button)sender).Tag));
         
         this.Close();
      }

      private void ExtendedMessageBoxDialog_FormClosed(object sender, FormClosedEventArgs e)
      {
         
      }
   }
}
