using System;
using System.IO;
using System.Windows.Forms;
using StorageManagementTool.GlobalizationRessources;

using StorageManagementTool.MainGUI;

namespace StorageManagementTool.MainGUI
{
   public partial class MainWindow : Form
   {
      public MainWindow()
      {
         InitializeComponent();
      }


      private void SetRootPath_btn_Click(object sender, EventArgs e)
      {
         HDDPath_fbd.ShowDialog();
         HDDSavePathText.Text = HDDPath_fbd.SelectedPath;
      }

      private void MainWindow_Load(object sender, EventArgs e)
      {

         LoadUIStrings();
         //string str = GlobalizationRessources.WrapperStrings.GetRegistryValue_Exception;
         //WrapperStrings.ResourceManager.GetString("SetRegistryValue_UnauthorizedAccess");
         HDDSavePathText.Text = Session.Singleton.CurrentConfiguration.DefaultHDDPath;
         string[] rec = OperatingMethods.GetRecommendedPaths();
         foreach (string item in rec)
         {
            Suggestion_lb.Items.Add(item);
         }

         AdministartorStatus_tb.Text = Session.Singleton.IsAdmin
            ? MainWindowStrings.AdministratorPriviligesAvailable
            : MainWindowStrings.NoAdministratorPriviligesAvailable;
         Suggestion_lb.Select();
         EnableComponents();
      }
      /// <summary>
      /// Loads UI strings from culture sepcific ressource file
      /// </summary>
      private void LoadUIStrings()
      {
         AdministratorSettings_gb.Text = MainWindowStrings.AdministratorSettings_gb_Text;
         ApplyPresetDialog_btn.Text = MainWindowStrings.ApplyPresetDialog_btn_Text;
         CustomFolderOrFileSelection_gb.Text = MainWindowStrings.CustomFolderOrFileSelection_gb_Text;
         EditPagefiles_btn.Text = MainWindowStrings.EditPagefiles_btn_Text;
         EditSSDMonitoring_btn.Text = MainWindowStrings.EditSSDMonitoring_btn_Text;
         EditUserShellFolders_btn.Text = MainWindowStrings.EditUserShellFolders_btn_Text;
         FileToMove_btn.Text = MainWindowStrings.FileToMove_btn_Text;
         FolderToMove_btn.Text = MainWindowStrings.FolderToMove_btn_Text;
         FurtherSettings_gb.Text = MainWindowStrings.FurtherSettings_gb_Text;
         MoveFile_btn.Text = MainWindowStrings.MoveFile_btn_Text;
         MoveFilesOrFolder_gb.Text = MainWindowStrings.MoveFilesOrFolder_gb_Text;
         MoveFolder_btn.Text = MainWindowStrings.MoveFolder_btn_Text;
         OpenSelectedFolder_btn.Text = MainWindowStrings.OpenSelectedFolder_btn_Text;
         RestartAsAdministartor_btn.Text = MainWindowStrings.RestartAsAdministartor_btn_Text;
         SetRootPathAsDefault_btn.Text = MainWindowStrings.SetHDDPathAsDefault_btn_Text;
         SetRootPath_btn.Text = MainWindowStrings.SetRootPath_btn_Text;
         Suggestions_gb.Text = MainWindowStrings.Suggestions_gb_Text;
         this.Text = MainWindowStrings.WindowTitle;
      }

      /// <summary>
      ///    Enables/Disables/Changes components when needed to prevent illegal actions
      /// </summary>
      private void EnableComponents()
      {
         if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
            MainWindowStrings.StoreOnHDDLink + ".lnk")))
         {
            SetSendToHDD_btn.Text = MainWindowStrings.DisableSendToHDD;
         }
         else
         {
            SetSendToHDD_btn.Text = MainWindowStrings.EnableSendToHDD;
            SetSendToHDD_btn.Enabled = Directory.Exists(Session.Singleton.CurrentConfiguration.DefaultHDDPath);
         }
      }

      private void SetSendToHDD_btn_Click(object sender, EventArgs e)
      {
         OperatingMethods.EnableSendToHDD(SetSendToHDD_btn.Text == MainWindowStrings.EnableSendToHDD);
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
                   MainWindowStrings.MoveFolder_NoNewPath,
                   MainWindowStrings.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
               SetRootPath_btn_Click(null, null);
               MoveFolder_btn_Click(null, null);
            }

            return;
         }

         if (FolderToMove_tb.Text == "")
         {
            if (MessageBox.Show(MainWindowStrings.MoveFolder_FolderPathEmpty, MainWindowStrings.Error,
                   MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
               FolderToMove_btn_Click(null, null);
               MoveFolder_btn_Click(null, null);
            }

            return;
         }

         DirectoryInfo newPath = new DirectoryInfo(Path.Combine(Session.Singleton.CurrentConfiguration.DefaultHDDPath,
            FolderToMove_tb.Text.Remove(1, 1)));
         string oldPath = FolderToMove_tb.Text;
         ProgramStatusStrip.Text = OperatingMethods.MoveFolder(new DirectoryInfo(oldPath), newPath)
            ? MainWindowStrings.MoveFolderSuccessful
            : MainWindowStrings.MoveFolderError;
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
         if (Session.Singleton.CurrentConfiguration.DefaultHDDPath == "")
         {
            if (MessageBox.Show(
                   MainWindowStrings.MoveFile_NoNewPath,
                   MainWindowStrings.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
               SetRootPath_btn_Click(null, null);
               MoveFile_btn_Click(null, null);
            }

            return;
         }

         if (FileToMovePath_tb.Text == "")
         {
            if (MessageBox.Show(MainWindowStrings.MoveFile_FilePathEmpty, MainWindowStrings.Error,
                   MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
               FileToMoveSel_btn_Click(null, null);
               MoveFile_btn_Click(null, null);
            }

            return;
         }

         string newPath = Path.Combine(Session.Singleton.CurrentConfiguration.DefaultHDDPath,
            FileToMovePath_tb.Text.Remove(1, 1));
         string oldPath = FileToMovePath_tb.Text;
         ProgramStatusStrip.Text = OperatingMethods.MoveFile(new FileInfo(oldPath), new FileInfo(newPath))
            ? MainWindowStrings.MoveFileSuccessful
            : MainWindowStrings.MoveFileError;
         FileToMovePath_tb.Text = "";
      }

      private void EditSSDMonitoring_btn_Click(object sender, EventArgs e)
      {
         new MonitoringSettings().ShowDialog();
      }

      private void SetRootPathAsDefault_btn_Click(object sender, EventArgs e)
      {
         Session.Singleton.CurrentConfiguration.DefaultHDDPath = HDDPath_fbd.SelectedPath;
         Session.Singleton.SaveCfg();
      }

      private void EditPagefiles_btn_Click(object sender, EventArgs e)
      {
         new PageFileOptionDialog().ShowDialog();
      }

      private void OpenWindowsSearchsettings_btn_Click(object sender, EventArgs e)
      {
         new EditWindowsSearchSettings().ShowDialog();
      }

      private void EditUserShellFolders_btn_Click(object sender, EventArgs e)
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