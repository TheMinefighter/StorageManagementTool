using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using static StorageManagementTool.EnterCredentials;
using static StorageManagementTool.GlobalizationRessources.EnterCredentials;

namespace StorageManagementTool
{
    public sealed partial class EnterCredentialsDialog : Form
    {
        public EnterCredentialsDialog()
        {
            InitializeComponent();
        }

        private void Ok_btn_Click(object sender, EventArgs e)
        {
            if (!Wrapper.IsUserInLocalGroup(Username_tb.Text, "Benutzer"))
            {
                MessageBox.Show(EnterAUsername, Error, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Password_tb.Text = "";
                Username_tb.Text = "";
                Username_tb.Focus();
                return;
            }

            if (
           //     Wrapper.IsUserInLocalGroup(Username_tb.Text, "Administratoren")
                Wrapper.IsAdmin(Username_tb.Text)
                )
            {
                ((DialogReturnData) Tag).IsAdmin = true;
            }
            else
            {
                if (((DialogReturnData) Tag).AdminRequired)
                {
                    MessageBox.Show(NotAdministratorButRequired, Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Password_tb.Text = "";
                    Username_tb.Text = "";
                    Username_tb.Focus();
                    return;
                }

                ((DialogReturnData) Tag).IsAdmin = false;
            }

            Process pProcess = new Process
            {
                StartInfo = new ProcessStartInfo(Path.Combine(Wrapper.System32Path,"cmd.exe"), " /C exit")
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    Password = new SecureString(),
                    UserName = Username_tb.Text
                }
            };

            foreach (char c in Password_tb.Text)
            {
                pProcess.StartInfo.Password.AppendChar(c);
            }

            try
            {
                pProcess.Start();
            }
            catch (Exception)
            {
                MessageBox.Show(EnteredCredentialsAreInvalid, Error, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Password_tb.Text = "";
                Password_tb.Focus();
                return;
            }

            ((DialogReturnData) Tag).GivenCredentials.Username = Username_tb.Text;
            Password_tb.Text.ToCharArray().ToList()
                .ForEach(x => ((DialogReturnData) Tag).GivenCredentials.Password.AppendChar(x));
            ((DialogReturnData) Tag).IsAborted = false;
            Close();
        }


        private void Abort_btn_Click(object sender, EventArgs e)
        {
            ((DialogReturnData) Tag).IsAborted = true;
            Close();
        }

        private void InsertCredentialsDialog_Load(object sender, EventArgs e)
        {
            this.Text = Window_Title;

            if (((DialogReturnData) Tag).AdminRequired)
            {
                Headline0_lbl.Text = string.Format(AdministratorInstructions,Environment.NewLine);
                if (Wrapper.IsUserInLocalGroup(Environment.UserName, "Administratoren"))
                {
                    Username_tb.Text = Environment.UserName;
                }
            }
            else
            {
                Headline0_lbl.Text = string.Format(NormalInstructions, Environment.NewLine);
                Username_tb.Text = Environment.UserName;
            }

            Ok_btn.Text = Ok_btn_Text;
            Abort_btn.Text = Abort_btn_Text;
            Password_lbl.Text = Password_lbl_Text;
            Username_lbl.Text = Username_lbl_Text;
        }

        private void Password_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Ok_btn_Click(null, null);
            }
        }

        private void InsertCredentialsDialog_closing(object sender, EventArgs e)
        {
        }
    }
}