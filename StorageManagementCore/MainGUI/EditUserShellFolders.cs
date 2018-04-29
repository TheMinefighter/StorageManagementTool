using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static StorageManagementCore.MainGUI.GlobalizationRessources.EditUserShellFolderStrings;
namespace StorageManagementCore.MainGUI
{
	public partial class EditUserShellFolders : Form
	{
		private bool _edited;

		public EditUserShellFolders()
		{
			InitializeComponent();
		}

		private void EditUserShellFolders_Load(object sender, EventArgs e)
		{
			CurrentUSFPath_lbl.Text = CurrentUSFPath_lbl_Text;
			USFOpenCurrentPath_btn.Text = USFOpenCurrentPath_btn_Text;
			NewUSFPath_lbl.Text = NewUSFPath_lbl_Text;
			USFOpenNewPath_btn.Text = USFOpenNewPath_btn_Text;
			SelectNewUSFPath_btn.Text = SelectNewUSFPath_btn_Text;
			Abort_btn.Text = Abort_btn_Text;
			SetUSF_btn.Text = SetUSF_btn_Text;
			EnableComponents();
			RefreshUSF();
		}

		private void USFOpenCurrentPath_btn_Click(object sender, EventArgs e)
		{
			if (CurrentUSFPath_tb.Text == "")
			{
				MessageBox.Show(USFOpenCurrentPath_NoPathSelected, Error,
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
				MessageBox.Show(USFOpenNewPath_NoPathSelected, Error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Directory.Exists(NewUSFPath_tb.Text))
			{
				if (MessageBox.Show(string.Format(USFOpenNewPath_InvalidPath, NewUSFPath_tb.Text), Error,
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

			UserShellFolder currentUSF =
				UserShellFolder.AllEditableUserUserShellFolders[
					ExistingUSF_lb.SelectedIndex == -1 ? 0 : ExistingUSF_lb.SelectedIndex];
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
			ExistingUSF_lb.Items.AddRange(UserShellFolder.AllEditableUserUserShellFolders
				.Select(x => (object) x.ViewedName)
				.ToArray());
		}

		private void SetUSF_btn_Click(object sender, EventArgs e)
		{
			if (ExistingUSF_lb.SelectedIndex == -1)
			{
				MessageBox.Show(SetUSF_NoneSelected, Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1);
				return;
			}

			if (NewUSFPath_tb.Text == "")
			{
				MessageBox.Show(SetUSF_NoNewPath, Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1);
				return;
			}

			if (UserShellFolder.ChangeUserShellFolder(new DirectoryInfo(CurrentUSFPath_tb.Text),
				new DirectoryInfo(NewUSFPath_tb.Text),
				UserShellFolder.AllEditableUserUserShellFolders[ExistingUSF_lb.SelectedIndex]))
			{
				RefreshUSF();
				ExistingUSF_lb_SelectedIndexChanged(null, null);
				_edited = true;
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
			foreach (Button button in new[] {SetUSF_btn, USFOpenNewPath_btn})
			{
				button.Enabled = NewUSFPath_tb.Text != "";
			}

			USFOpenCurrentPath_btn.Enabled = CurrentUSFPath_tb.Text != "";
			SelectNewUSFPath_btn.Enabled = ExistingUSF_lb.SelectedIndex != -1;
		}

		private void Abort_btn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void EditUserShellFolders_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (new[] {CloseReason.UserClosing, CloseReason.MdiFormClosing, CloseReason.FormOwnerClosing, CloseReason.None}
				    .Contains(e.CloseReason) && _edited && MessageBox.Show(Closing_WantRestart_Text,
				    Closing_WantRestart_Title,
				    MessageBoxButtons.YesNo,
				    MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
			{
				Wrapper.RestartComputer();
			}
		}
	}
}