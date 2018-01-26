using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace StorageManagementTool
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void HDDSavePath_Click(object sender, EventArgs e)
        {
            HDDPath_fbd.ShowDialog();
            HDDSavePathText.Text = HDDPath_fbd.SelectedPath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            System.Threading.Thread.CurrentThread.CurrentUICulture =
                System.Globalization.CultureInfo.GetCultureInfo("de-DE");
            //string str = GlobalizationRessources.WrapperStrings.GetRegistryValue_Exception;
            //WrapperStrings.ResourceManager.GetString("SetRegistryValue_UnauthorizedAccess");
            HDDSavePathText.Text = Session.Singleton.CfgJson.DefaultHDDPath;
            string[] rec = OperatingMethods.GetRecommendedPaths();
            foreach (string item in rec)
            {
                Suggestion_lb.Items.Add(item);
            }

            AdministartorStatus_tb.Text = Session.Singleton.Isadmin
                ? "Das Programm wird als Adminstrator ausgeführt"
                : "Das Programm wird NICHT als Adminstrator ausgeführt";
            Suggestion_lb.Select();
            EnableComponents();
        }

        /// <summary>
        /// Enables/Disables/Changes components when needed to prevent illegal actions
        /// </summary>
        private void EnableComponents()
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
                "Auf HDD Speichern.lnk")))
            {
                SetSendToHDD_btn.Text = "Senden an HDD deaktivieren";
            }
            else
            {
                SetSendToHDD_btn.Text = "Senden an HDD aktivieren";
                SetSendToHDD_btn.Enabled = Directory.Exists(Session.Singleton.CfgJson.DefaultHDDPath);
            }
        }

        private void SetSendToHDD_btn_Click(object sender, EventArgs e)
        {
            if (SetSendToHDD_btn.Text == "Senden an HDD aktivieren")
            {
                #region From https://stackoverflow.com/a/4909475/6730162 access on 5.11.2017 

                WshShell shell = new WshShell();
                string shortcutAddress = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
                    "Auf HDD Speichern.lnk");
                IWshShortcut shortcut = (IWshShortcut) shell.CreateShortcut(shortcutAddress);
                shortcut.Description = "Lagert den Speicherort der gegebenen Datei aus";
                shortcut.TargetPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                shortcut.Arguments = " -move -auto-detect -SrcPath";
                shortcut.Save();

                #endregion
            }
            else
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
                    "Auf HDD Speichern.lnk"));
            }

            EnableComponents();
        }

        private void Suggestion_lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            FolderToMove_tb.Text = Suggestion_lb.SelectedItem.ToString();
        }

        private void OpenSelectedFolder_btn_Click(object sender, EventArgs e)
        {
            Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath, FolderToMove_tb.Text, false, false, false);
        }

        public void RestartAsAdministartor_btn_Click(object sender, EventArgs e)
        {
            Wrapper.RestartAsAdministrator();
        }

        private void FileToMoveSel_btn_Click(object sender, EventArgs e)
        {
            FileToMove_ofd.ShowDialog();
            FileToMovePath_tb.Text = FileToMove_ofd.FileName;
        }

        private void FolderToMove_btn_Click(object sender, EventArgs e)
        {
            FolderToMove_fbd.ShowDialog();
            FolderToMove_tb.Text = FolderToMove_fbd.SelectedPath;
        }

        private void MoveFolder_btn_Click(object sender, EventArgs e)
        {
            if (Session.Singleton.CfgJson.DefaultHDDPath == "")
            {
                if (MessageBox.Show(
                        "Der Pfad für den neuen Speicherort neue ist leer, möchten sie jetzt einen Speicherort auswählen?",
                        "Fehler", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    HDDSavePath_Click(null, null);
                    MoveFolder_btn_Click(null, null);
                }

                return;
            }

            if (FolderToMove_tb.Text == "")
            {
                if (MessageBox.Show("Der Dateipfad ist leer, möchten sie jetzt eine Datei auswählen?", "Fehler",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    FolderToMove_btn_Click(null, null);
                    MoveFolder_btn_Click(null, null);
                }

                return;
            }

            List<char> hddList = FolderToMove_tb.Text.ToList();
            hddList.RemoveAt(1);
            string newPath = Session.Singleton.CfgJson.DefaultHDDPath + "\\" + new string(hddList.ToArray());
            string oldPath = FolderToMove_tb.Text;
            ProgramStatusStrip.Text = OperatingMethods.MoveFolder(new DirectoryInfo(oldPath),new DirectoryInfo(newPath) )
                ? "Ordner erfolgreich verschoben"
                : "Ordner aufgrund eines Fehlers nicht verschoben";
            FolderToMove_tb.Text = "";
            Suggestion_lb.Items.Clear();
            string[] rec = OperatingMethods.GetRecommendedPaths();
            foreach (string item in rec)
            {
                Suggestion_lb.Items.Add(item);
            }
        }

        private void MoveFile_btn_Click(object sender, EventArgs e)
        {
            #region Tritt nur bei unvollständig ausgefülltem Formular aus

            if (Session.Singleton.CfgJson.DefaultHDDPath == "")
            {
                if (MessageBox.Show(
                        "Der Pfad für den neuen Speicherort neue ist leer, möchten sie jetzt einen Speicherort auswählen?",
                        "Fehler", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    HDDSavePath_Click(null, null);
                    MoveFile_btn_Click(null, null);
                }

                return;
            }

            if (FileToMovePath_tb.Text == "")
            {
                if (MessageBox.Show("Der Dateipfad ist leer, möchten sie jetzt eine Datei auswählen?", "Fehler",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    FileToMoveSel_btn_Click(null, null);
                    MoveFile_btn_Click(null, null);
                }

                ;
                return;
            }

            #endregion

            string newPath = Path.Combine(Session.Singleton.CfgJson.DefaultHDDPath, FileToMovePath_tb.Text.Remove(1, 1));
            string oldPath = FileToMovePath_tb.Text;
            ProgramStatusStrip.Text = OperatingMethods.MoveFile(new FileInfo(oldPath), new FileInfo(newPath))
                ? "Datei-Speicherort erfolgreich verschoben"
                : "Datei-Speicherort aufgrund eines Fehlers nicht verschoben";
            FileToMovePath_tb.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new MonitoringSettings().ShowDialog();
        }

        private void SetHDDPathAsDefault_btn_Click(object sender, EventArgs e)
        {
            Session.Singleton.CfgJson.DefaultHDDPath = HDDPath_fbd.SelectedPath;
            Session.Singleton.SaveCfg();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            new PageFileOptionDialog().ShowDialog();
        }

        private void OpenWindowsSearchsettings_btn_Click(object sender, EventArgs e)
        {
            new EditWindowsSearchSettings().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new EditUserShellFolders().ShowDialog();
        }
    }
}