using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Security;
using static StorageManagementTool.InsertCredentials;

namespace StorageManagementTool
{
    public sealed partial class InsertCredentialsDialog : Form
    {
        public InsertCredentialsDialog()
        {
            InitializeComponent();
        }

        private void Ok_btn_Click(object sender, EventArgs e)
        {
            if (!Wrapper.IsUserInLocalGroup(Username_tb.Text, "Benutzer"))
            {
                MessageBox.Show("Bitte geben sie den Namen eines Benutzers an!", "Fehler", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Password_tb.Text = "";
                Username_tb.Text = "";
                Username_tb.Focus();
                return;
            }
            if (Wrapper.IsUserInLocalGroup(Username_tb.Text, "Administratoren"))
            {
                ((DialogReturnData)this.Tag).IsAdmin = true;
            }
            else
            {
                if (((DialogReturnData)this.Tag).AdminRequired)
                {
                    MessageBox.Show("Der gennante Benutzer verfügt nicht über Administratoren-privilegien.", "Fehler",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Password_tb.Text = "";
                    Username_tb.Text = "";
                    Username_tb.Focus();
                    return;
                }
                else
                {
                    ((DialogReturnData)this.Tag).IsAdmin = false;
                }
            }
            Process pProcess = new Process
            {
                StartInfo = new ProcessStartInfo(Environment.ExpandEnvironmentVariables(@"%windir%\system32\cmd.exe"), " /C exit")
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
                MessageBox.Show("Die angegebenen Anmeldedaten sind ungültig", "Fehler", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Password_tb.Text = "";
                Password_tb.Focus();
                return;
            }
            ((DialogReturnData)this.Tag).GivenCredentials.Username = Username_tb.Text;
            Password_tb.Text.ToCharArray().ToList()
                .ForEach(x => ((DialogReturnData)this.Tag).GivenCredentials.Password.AppendChar(x));
            ((DialogReturnData)this.Tag).IsAborted = false;
            this.Close();
        }


        private void Abort_btn_Click(object sender, EventArgs e)
        {
            ((DialogReturnData)this.Tag).IsAborted = true;
            this.Close();
        }

        private void InsertCredentialsDialog_Load(object sender, EventArgs e)
        {

            if (((DialogReturnData)this.Tag).AdminRequired)
            {
                Headline1_lbl.Text = "für einen Administratorenbenutzer ein ";
                if (Wrapper.IsUserInLocalGroup(Environment.UserName, "Administratoren"))
                {
                    Username_tb.Text = Environment.UserName;
                }
            }
            else
            {
                Username_tb.Text = Environment.UserName;
            }
        }

        private void Password_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { Ok_btn_Click(null, null); }
        }
        private void InsertCredentialsDialog_closing(object sender, EventArgs e)
        {
        }
    }
}
