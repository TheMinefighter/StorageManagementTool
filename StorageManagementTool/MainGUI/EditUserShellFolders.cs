using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using StorageManagementTool.GlobalizationRessources;

namespace StorageManagementTool.MainGUI
{
   public partial class EditUserShellFolders : Form
   {
      public EditUserShellFolders()
      {
         InitializeComponent();
      }

      private void EditUserShellFolders_Load(object sender, EventArgs e)
      {
         CurrentUSFPath_lbl.Text = EditUserShellFolderStrings.CurrentUSFPath_lbl_Text;
         USFOpenCurrentPath_btn.Text = EditUserShellFolderStrings.USFOpenCurrentPath_btn_Text;
         NewUSFPath_lbl.Text = EditUserShellFolderStrings.NewUSFPath_lbl_Text;
         USFOpenNewPath_btn.Text = EditUserShellFolderStrings.USFOpenNewPath_btn_Text;
         SelectNewUSFPath_btn.Text = EditUserShellFolderStrings.SelectNewUSFPath_btn_Text;
         Abort_btn.Text = EditUserShellFolderStrings.Abort_btn_Text;
         SetUSF_btn.Text = EditUserShellFolderStrings.SetUSF_btn_Text;
         EnableComponents();
         RefreshUSF();
      }

      private void USFOpenCurrentPath_btn_Click(object sender, EventArgs e)
      {
         if (CurrentUSFPath_tb.Text == "")
         {
            MessageBox.Show(EditUserShellFolderStrings.USFOpenCurrentPath_NoPathSelected, EditUserShellFolderStrings.Error,
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         if (!Directory.Exists(CurrentUSFPath_tb.Text))
         {
            if (MessageBox.Show(string.Format(EditUserShellFolderStrings.USFOpenCurrentpath_InvalidPath, CurrentUSFPath_tb.Text), EditUserShellFolderStrings.Error,
                   MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) ==
                DialogResult.Retry)
            {
               USFOpenCurrentPath_btn_Click(null, null);
            }

            return;
         }

         Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath, CurrentUSFPath_tb.Text);
      }

      private void USFOpenNewPath_btn_Click(object sender, EventArgs e)
      {
         if (NewUSFPath_tb.Text == "")
         {
            MessageBox.Show(EditUserShellFolderStrings.USFOpenNewPath_NoPathSelected, EditUserShellFolderStrings.Error,
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         if (!Directory.Exists(NewUSFPath_tb.Text))
         {
            if (MessageBox.Show(string.Format(EditUserShellFolderStrings.USFOpenNewPath_InvalidPath, NewUSFPath_tb.Text), EditUserShellFolderStrings.Error,
                   MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) ==
                DialogResult.Retry)
            {
               USFOpenNewPath_btn_Click(null, null);
            }

            return;
         }

         Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath, NewUSFPath_tb.Text);
      }

      private void ExistingUSF_lb_SelectedIndexChanged(object sender, EventArgs e)
      {
         EnableComponents();
         UserShellFolder currentUSF = UserShellFolder.AllEditableUserUserShellFolders[ExistingUSF_lb.SelectedIndex];
         CurrentUSFPath_tb.Text = UserShellFolder.GetPath(currentUSF).FullName;
      }

      private void SelectNewUSFPath_btn_Click(object sender, EventArgs e)
      {
         NewUSFPath_fbd.ShowDialog();
         NewUSFPath_tb.Text = NewUSFPath_fbd.SelectedPath;
      }


      private void RefreshUSF()
      {
         ExistingUSF_lb.Items.Clear();
         ExistingUSF_lb.Items.AddRange(UserShellFolder.AllEditableUserUserShellFolders.Select(x => x.ViewedName)
            .ToArray());
      }

      private void SetUSF_btn_Click(object sender, EventArgs e)
      {
         if (ExistingUSF_lb.SelectedIndex == -1)
         {
            MessageBox.Show(EditUserShellFolderStrings.SetUSF_NoneSelected, EditUserShellFolderStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
               MessageBoxDefaultButton.Button1);
            return;
         }

         if (NewUSFPath_tb.Text == "")
         {
            MessageBox.Show(EditUserShellFolderStrings.SetUSF_NoNewPath, EditUserShellFolderStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
               MessageBoxDefaultButton.Button1);
            return;
         }

         if (OperatingMethods.ChangeUserShellFolder(new DirectoryInfo(CurrentUSFPath_tb.Text),
            new DirectoryInfo(NewUSFPath_tb.Text),
            UserShellFolder.AllEditableUserUserShellFolders[ExistingUSF_lb.SelectedIndex]))
         {
            RefreshUSF();
            ExistingUSF_lb_SelectedIndexChanged(null, null);
         }
      }


      private void NewUSFPath_tb_TextChanged(object sender, EventArgs e)
      {
         EnableComponents();
      }

      private void CurrentUSFPath_tb_TextChanged(object sender, EventArgs e)
      {
         EnableComponents();
      }

      private void EnableComponents()
      {
         new[] {SetUSF_btn, USFOpenNewPath_btn}.ForEach(x => x.Enabled = NewUSFPath_tb.Text != "");
         USFOpenCurrentPath_btn.Enabled = CurrentUSFPath_tb.Text != "";
         SelectNewUSFPath_btn.Enabled = ExistingUSF_lb.SelectedIndex != -1;
      }

      private void Abort_btn_Click(object sender, EventArgs e)
      {
         Close();
      }
   }
}