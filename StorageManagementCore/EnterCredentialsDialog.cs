using System;
using System.Security;
using System.Windows.Forms;
using static StorageManagementCore.GlobalizationRessources.EnterCredentialsStrings;

namespace StorageManagementCore {
	public sealed partial class EnterCredentialsDialog : Form {
		public EnterCredentialsDialog() {
			InitializeComponent();
		}

		private void Ok_btn_Click(object sender, EventArgs e) {
			SecureString password = new SecureString();

			foreach (char c in Password_tb.Text) {
				password.AppendChar(c);
			}

			Password_tb.Text = "";
			if (!Wrapper.IsUser(Username_tb.Text)) {
				MessageBox.Show(EnterAUsername, Error, MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				Password_tb.Text = "";
				Username_tb.Text = "";
				Username_tb.Focus();
				return;
			}

			if (Wrapper.IsAdmin(Username_tb.Text)) {
				((EnterCredentials.DialogReturnData) Tag).IsAdmin = true;
			}
			else {
				if (((EnterCredentials.DialogReturnData) Tag).AdminRequired) {
					MessageBox.Show(NotAdministratorButRequired, Error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					Password_tb.Text = "";
					Username_tb.Text = "";
					Username_tb.Focus();
					return;
				}

				((EnterCredentials.DialogReturnData) Tag).IsAdmin = false;
			}

			string username = Username_tb.Text;
			EnterCredentials.Credentials givenCredentials = new EnterCredentials.Credentials {
				Password = password,
				Username = username
			};
			if (!Wrapper.TestCredentials(givenCredentials)) {
				MessageBox.Show(EnteredCredentialsAreInvalid, Error, MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				Password_tb.Focus();
				return;
			}

			((EnterCredentials.DialogReturnData) Tag).GivenCredentials = givenCredentials;
			((EnterCredentials.DialogReturnData) Tag).IsAborted = false;
			Close();
		}


		private void Abort_btn_Click(object sender, EventArgs e) {
			((EnterCredentials.DialogReturnData) Tag).IsAborted = true;
			Close();
		}

		private void InsertCredentialsDialog_Load(object sender, EventArgs e) {
			Text = Window_Title;
			if (((EnterCredentials.DialogReturnData) Tag).AdminRequired) {
				Headline0_lbl.Text = string.Format(AdministratorInstructions, Environment.NewLine);
				if (Wrapper.IsAdmin(Environment.UserName)) {
					Username_tb.Text = Environment.UserName;
				}
			}
			else {
				Headline0_lbl.Text = string.Format(NormalInstructions, Environment.NewLine);
				Username_tb.Text = Environment.UserName;
			}

			Ok_btn.Text = Ok_btn_Text;
			Abort_btn.Text = Abort_btn_Text;
			Password_lbl.Text = Password_lbl_Text;
			Username_lbl.Text = Username_lbl_Text;
		}

		private void Password_tb_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				Ok_btn_Click(null, null);
			}
		}
	}
}