using System;
using System.IO;
using System.Windows.Forms;

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
                MessageBox.Show("Wählen sie einen UserShellFolder aus um dessen aktuellen Pfad öffnen zu können", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath, CurrentUSFPath_tb.Text);
            }
        }

        private void USFOpenNewPath_btn_Click(object sender, EventArgs e)
        {
            if (NewUSFPath_tb.Text == "")
            {
                MessageBox.Show("Wählen sie einen neuen Ort für den UserShellFolder um diesen öffnen zu können", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath, NewUSFPath_tb.Text);
            }
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
            if (OperatingMethods.ChangeUserShellFolder(new DirectoryInfo(CurrentUSFPath_tb.Text), new DirectoryInfo(NewUSFPath_tb.Text),
                UserShellFolder.AllEditableUserUserShellFolders[ExistingUSF_lb.SelectedIndex]))
            {
                CurrentUSFPath_tb.Text = NewUSFPath_tb.Text;
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