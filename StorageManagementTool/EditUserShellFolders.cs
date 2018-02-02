using System;
using System.IO;
using System.Windows.Forms;
using static StorageManagementTool.GlobalizationRessources.EditUserShellFolderStrings;

namespace StorageManagementTool
{
    public partial class EditUserShellFolders : Form
    {
        public EditUserShellFolders()
        {
            InitializeComponent();
        }

        private void USFOpenCurrentPath_btn_Click(object sender, EventArgs e)
        {
            if (CurrentUSFPath_tb.Text == "")
            {
                MessageBox.Show("Wählen sie einen UserShellFolder aus um dessen aktuellen Pfad öffnen zu können", Error,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(CurrentUSFPath_tb.Text))
            {
                if (MessageBox.Show(string.Format(USFOpenCurrentpath_InvalidPath, CurrentUSFPath_tb.Text), Error,
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
                MessageBox.Show("Wählen sie einen neuen Pfad für den UserShellFolder um diesen öffnen zu können", Error,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(NewUSFPath_tb.Text))
            {
                if (MessageBox.Show(string.Format(USFOpenNewPath_InvalidPath, NewUSFPath_tb.Text), Error,
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) ==
                    DialogResult.Retry)
                {
                    USFOpenNewPath_btn_Click (null, null);
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

        private void SetNewUSFPath_btn_Click(object sender, EventArgs e)
        {
            NewUSFPath_fbd.ShowDialog();
            NewUSFPath_tb.Text = NewUSFPath_fbd.SelectedPath;
        }

        private void EditUserShellFolders_Load(object sender, EventArgs e)
        {
            EnableComponents();
            RefreshUSF();
        }

        private void RefreshUSF()
        {
            foreach (UserShellFolder usf in UserShellFolder.AllEditableUserUserShellFolders)
            {
                ExistingUSF_lb.Items.Add(usf.ViewedName);
            }
        }

        private void SetUSF_btn_Click(object sender, EventArgs e)
        {
            if (ExistingUSF_lb.SelectedIndex==-1)
            {
                MessageBox.Show(SetUSF_NoneSelected, Error,MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
                return;
            }

            if (NewUSFPath_tb.Text=="")
            {
                MessageBox.Show(SetUSF_NoNewPath, Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                return;
            }
            if (OperatingMethods.ChangeUserShellFolder(new DirectoryInfo(CurrentUSFPath_tb.Text), new DirectoryInfo(NewUSFPath_tb.Text),
                UserShellFolder.AllEditableUserUserShellFolders[ExistingUSF_lb.SelectedIndex]))
            {
                RefreshUSF();
                ExistingUSF_lb_SelectedIndexChanged(null,null);
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
            SetNewUSFPath_btn.Enabled = ExistingUSF_lb.SelectedIndex != -1;
        }
    }
}