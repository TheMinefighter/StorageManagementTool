using System;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using static StorageManagementTool.MainGUI.GlobalizationRessources.EditWindowsSearchSettingsStrings;

namespace StorageManagementTool.MainGUI {
   public partial class EditWindowsSearchSettings : Form {
      public EditWindowsSearchSettings() {
         InitializeComponent();
      }

      private void OpenCurrentPath_btn_Click(object sender, EventArgs e) {
         if (Directory.Exists(CurrentPath_tb.Text)) {
            Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath,
               CurrentPath_tb.Text);
         }
      }

      private void OpenNewPath_btn_Click(object sender, EventArgs e) {
         if (Directory.Exists(NewPath_tb.Text)) {
            Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath,
               NewPath_tb.Text);
         }
      }

      private void SelectNewPath_btn_Click(object sender, EventArgs e) {
         SelectNewPath_fbd.ShowDialog();
         NewPath_tb.Text = SelectNewPath_fbd.SelectedPath;
      }

      private void ShowFurtherSettings_btn_Click(object sender, EventArgs e) {
         {
            Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path, @"rundll32.exe"),
               " shell32.dll,Control_RunDLL srchadmin.dll");
         }
      }

      private void SaveSettings_btn_Click(object sender, EventArgs e) {
         OperatingMethods.SetSearchDataPath(new DirectoryInfo(NewPath_tb.Text));
         ServiceController wSearch = new ServiceController("WSearch");
         //if (!Wrapper.RecursiveServiceRestart(wSearch))
         //{
         //   if (MessageBox.Show(String.Format(
         //             SetSearchDataPath_RestartErrorService, wSearch.DisplayName),
         //          SetSearchDataPath_RestartNow_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
         //       DialogResult.Yes)
         //   {
         //      Wrapper.RestartComputer();
         //   }
         //}
      }

      private void Abort_btn_Click(object sender, EventArgs e) {
         Close();
      }

      private void CurrentPath_tb_TextChanged(object sender, EventArgs e) {
         EnableComponents();
      }

      private void NewPath_tb_TextChanged(object sender, EventArgs e) {
         EnableComponents();
      }

      private void EnableComponents() {
         OpenNewPath_btn.Enabled = Directory.Exists(NewPath_tb.Text);
         OpenCurrentPath_btn.Enabled = Directory.Exists(CurrentPath_tb.Text);
         SaveSettings_btn.Enabled = Directory.Exists(NewPath_tb.Text);
      }

      private void EditWindowsSearchSettings_Load(object sender, EventArgs e) {
         LoadUIStrings();
         EnableComponents();
         RefreshCurrentPath();
      }

      private void LoadUIStrings() {
         Abort_btn.Text = Abort_btn_Text;
         CurrentLocation_lbl.Text = CurrentLocation_lbl_Text;
         NewPath_lbl.Text = NewPath_lbl_Text;
         OpenCurrentPath_btn.Text = OpenCurrentPath_btn_Text;
         OpenNewPath_btn.Text = OpenNewPath_btn_Text;
         RefreshCurrentPath_btn.Text = RefreshCurrentPath_btn_Text;
         SaveSettings_btn.Text = SaveSettings_btn_Text;
         SelectNewPath_btn.Text = SelectNewPath_btn_Text;
         ShowFurtherSettings_btn.Text = ShowFurtherSettings_btn_Text;
         Text = WindowTitle;
      }

      private void RefreshCurrentPath() {
         string displayedSearchDataPath = Error;
         if (OperatingMethods.GetSearchDataPath(out DirectoryInfo dir)) {
            displayedSearchDataPath = dir.FullName;
         }

         CurrentPath_tb.Text = displayedSearchDataPath;
      }

      private void RefreshCurrentPath_btn_Click(object sender, EventArgs e) {
         RefreshCurrentPath();
      }
   }
}