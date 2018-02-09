using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static StorageManagementTool.GlobalizationRessources.MainWindowStrings;

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
         //string str = GlobalizationRessources.WrapperStrings.GetRegistryValue_Exception;
         //WrapperStrings.ResourceManager.GetString("SetRegistryValue_UnauthorizedAccess");
         HDDSavePathText.Text = Session.Singleton.CurrentConfiguration.DefaultHDDPath;
         string[] rec = OperatingMethods.GetRecommendedPaths();
         foreach (string item in rec)
         {
            Suggestion_lb.Items.Add(item);
         }

         AdministartorStatus_tb.Text = Session.Singleton.IsAdmin
            ? AdministratorPriviligesAvailable
            : NoAdministratorPriviligesAvailable;
         Suggestion_lb.Select();
         EnableComponents();
      }

      /// <summary>
      ///    Enables/Disables/Changes components when needed to prevent illegal actions
      /// </summary>
      private void EnableComponents()
      {
         if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
            StoreOnHDDLink+".lnk")))
         {
            SetSendToHDD_btn.Text = DisableSendToHDD;
         }
         else
         {
            SetSendToHDD_btn.Text = EnableSendToHDD;
            SetSendToHDD_btn.Enabled = Directory.Exists(Session.Singleton.CurrentConfiguration.DefaultHDDPath);
         }
      }

      private void SetSendToHDD_btn_Click(object sender, EventArgs e)
      {
         if (SetSendToHDD_btn.Text == EnableSendToHDD)
         {
            OperatingMethods.EnableSendToHDD(true);
         }
         else
         {
            OperatingMethods.EnableSendToHDD(false);
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
         if (Session.Singleton.CurrentConfiguration.DefaultHDDPath == "")
         {
            if (MessageBox.Show(
                  MoveFolder_NoNewPath,
                   Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
               HDDSavePath_Click(null, null);
               MoveFolder_btn_Click(null, null);
            }

            return;
         }

         if (FolderToMove_tb.Text == "")
         {
            if (MessageBox.Show(MoveFolder_FolderPathEmpty, Error,
                   MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
               FolderToMove_btn_Click(null, null);
               MoveFolder_btn_Click(null, null);
            }

            return;
         }

         List<char> hddList = FolderToMove_tb.Text.ToList();
         hddList.RemoveAt(1);
         string newPath = Session.Singleton.CurrentConfiguration.DefaultHDDPath + "\\" + new string(hddList.ToArray());
         string oldPath = FolderToMove_tb.Text;
         ProgramStatusStrip.Text = OperatingMethods.MoveFolder(new DirectoryInfo(oldPath), new DirectoryInfo(newPath))
            ? MoveFolderSuccessful
            : MoveFolderError;
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

         if (Session.Singleton.CurrentConfiguration.DefaultHDDPath == "")
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

         string newPath = Path.Combine(Session.Singleton.CurrentConfiguration.DefaultHDDPath,
            FileToMovePath_tb.Text.Remove(1, 1));
         string oldPath = FileToMovePath_tb.Text;
         ProgramStatusStrip.Text = OperatingMethods.MoveFile(new FileInfo(oldPath), new FileInfo(newPath))
            ? MoveFileSuccessful
            : MoveFileError;
         FileToMovePath_tb.Text = "";
      }

      private void button1_Click_1(object sender, EventArgs e)
      {
         new MonitoringSettings().ShowDialog();
      }

      private void SetHDDPathAsDefault_btn_Click(object sender, EventArgs e)
      {
         Session.Singleton.CurrentConfiguration.DefaultHDDPath = HDDPath_fbd.SelectedPath;
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

      private void ApplyPresetDialog_btn_Click(object sender, EventArgs e)
      {
         new ApplyPreset().ShowDialog();
      }

      private void button3_Click(object sender, EventArgs e)
      {
         //     ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration())
      }
   }
}