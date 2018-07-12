//using StorageManagementTool.GlobalizationRessources;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;
using static StorageManagementCore.MainGUI.GlobalizationRessources.MainWindowStrings;

namespace StorageManagementCore.MainGUI {
	public partial class MainWindow : Form {
		public MainWindow() {
			InitializeComponent();
		}


		private void SetRootPath_btn_Click(object sender, EventArgs e) {
			HDDPath_fbd.ShowDialog();
			HDDSavePath_tb.Text = HDDPath_fbd.SelectedPath;
		}

		private void MainWindow_Load(object sender, EventArgs e) {
			LoadUIStrings();
			//string str = GlobalizationRessources.WrapperStrings.GetRegistryValue_Exception;
			//WrapperStrings.ResourceManager.GetString("SetRegistryValue_UnauthorizedAccess");
			HDDSavePath_tb.Text = Session.Singleton.Configuration.DefaultHDDPath;
			IEnumerable<string> rec = OperatingMethods.GetRecommendedPaths();
			foreach (string item in rec) {
				Suggestion_lb.Items.Add(item);
			}

			AdministartorStatus_tb.Text = Session.Singleton.IsAdmin
				? AdministratorPrivilegesAvailable
				: NoAdministratorPrivilegesAvailable;
			Suggestion_lb.Select();
			EnableComponents();
		}

		/// <summary>
		///  Loads UI strings from culture sepcific ressource file
		/// </summary>
		private void LoadUIStrings() {
			AdministratorSettings_gb.Text = AdministratorSettings_gb_Text;
			ApplyPresetDialog_btn.Text = ApplyPresetDialog_btn_Text;
			CustomFolderOrFileSelection_gb.Text = CustomFolderOrFileSelection_gb_Text;
			EditPagefiles_btn.Text = EditPagefiles_btn_Text;
			EditSSDMonitoring_btn.Text = EditSSDMonitoring_btn_Text;
			EditUserShellFolders_btn.Text = EditUserShellFolders_btn_Text;
			OpenWindowsSearchSettings_btn.Text = OpenWindowsSearchSettings_btn_Text;
			FileToMove_btn.Text = FileToMove_btn_Text;
			FolderToMove_btn.Text = FolderToMove_btn_Text;
			FurtherSettings_gb.Text = FurtherSettings_gb_Text;
			MoveFile_btn.Text = MoveFile_btn_Text;
			MoveFilesOrFolder_gb.Text = MoveFilesOrFolder_gb_Text;
			MoveFolder_btn.Text = MoveFolder_btn_Text;
			OpenSelectedFolder_btn.Text = OpenSelectedFolder_btn_Text;
			RestartAsAdministartor_btn.Text = RestartAsAdministartor_btn_Text;
			SetRootPathAsDefault_btn.Text = SetHDDPathAsDefault_btn_Text;
			SetRootPath_btn.Text = SetRootPath_btn_Text;
			Suggestions_gb.Text = Suggestions_gb_Text;
			Text = WindowTitle;
		}

		/// <summary>
		///  Enables/Disables/Changes components when needed to prevent illegal actions
		/// </summary>
		private void EnableComponents() {
			if (OperatingMethods.IsSendToHDDEnabled()) {
				SetSendToHDD_btn.Text = DisableSendToHDD;
			}
			else {
				SetSendToHDD_btn.Text = EnableSendToHDD;
				SetSendToHDD_btn.Enabled = Directory.Exists(Session.Singleton.Configuration.DefaultHDDPath);
			}
		}

		private void SetSendToHDD_btn_Click(object sender, EventArgs e) {
			OperatingMethods.EnableSendToHDD(SetSendToHDD_btn.Text == EnableSendToHDD);
			EnableComponents();
		}

		private void Suggestion_lb_SelectedIndexChanged(object sender, EventArgs e) {
			FolderToMove_tb.Text = Suggestion_lb.SelectedItem.ToString();
		}

		private void OpenSelectedFolder_btn_Click(object sender, EventArgs e) {
			Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath, FolderToMove_tb.Text);
		}

		private void RestartAsAdministartor_btn_Click(object sender, EventArgs e) {
			Wrapper.RestartProgram(true);
		}

		private void FileToMoveSel_btn_Click(object sender, EventArgs e) {
			FileToMove_ofd.ShowDialog();
			FileToMovePath_tb.Text = FileToMove_ofd.FileName;
		}

		private void FolderToMove_btn_Click(object sender, EventArgs e) {
			CommonOpenFileDialog dlg = new CommonOpenFileDialog("Test");
			dlg.IsFolderPicker = true;
			dlg.ShowDialog();
			FolderToMove_tb.Text = dlg.FileName;
			//FolderToMove_fbd.ShowDialog();
			//FolderToMove_tb.Text = FolderToMove_fbd.SelectedPath;
		}

		private void MoveFolder_btn_Click(object sender, EventArgs e) {
			if (Session.Singleton.Configuration.DefaultHDDPath == "") {
				if (MessageBox.Show(
					    MoveFolder_NoNewPath,
					    Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
					SetRootPath_btn_Click(null, null);
					MoveFolder_btn_Click(null, null);
				}

				return;
			}

			if (FolderToMove_tb.Text == "") {
				if (MessageBox.Show(MoveFolder_FolderPathEmpty, Error,
					    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
					FolderToMove_btn_Click(null, null);
					MoveFolder_btn_Click(null, null);
				}

				return;
			}

			ProgramStatusStrip.Text =
				OperatingMethods.MoveFolderPhysically(new DirectoryInfo(FolderToMove_tb.Text), new DirectoryInfo(HDDSavePath_tb.Text),
					true)
					? MoveFolderSuccessful
					: MoveFolderError;
			FolderToMove_tb.Text = "";
			Suggestion_lb.Items.Clear();
			IEnumerable<string> rec = OperatingMethods.GetRecommendedPaths();
			foreach (string item in rec) {
				Suggestion_lb.Items.Add(item);
			}
		}

		private void MoveFile_btn_Click(object sender, EventArgs e) {
			if (Session.Singleton.Configuration.DefaultHDDPath == "") {
				if (MessageBox.Show(
					    MoveFile_NoNewPath,
					    Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
					SetRootPath_btn_Click(null, null);
					MoveFile_btn_Click(null, null);
				}

				return;
			}

			if (FileToMovePath_tb.Text == "") {
				if (MessageBox.Show(MoveFile_FilePathEmpty, Error,
					    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
					FileToMoveSel_btn_Click(null, null);
					MoveFile_btn_Click(null, null);
				}

				return;
			}

			string newPath = Path.Combine(Session.Singleton.Configuration.DefaultHDDPath,
				FileToMovePath_tb.Text.Remove(1, 1));
			string oldPath = FileToMovePath_tb.Text;
			ProgramStatusStrip.Text = OperatingMethods.MoveFilePhysically(new FileInfo(oldPath), new FileInfo(newPath))
				? MoveFileSuccessful
				: MoveFileError;
			FileToMovePath_tb.Text = "";
		}

		private void EditSSDMonitoring_btn_Click(object sender, EventArgs e) {
			new MonitoringSettings().ShowDialog();
		}

		private void SetRootPathAsDefault_btn_Click(object sender, EventArgs e) {
			Session.Singleton.Configuration.DefaultHDDPath = HDDPath_fbd.SelectedPath;
			Session.Singleton.SaveCfg();
		}

		private void EditPagefiles_btn_Click(object sender, EventArgs e) {
			new PagefileSettings().ShowDialog();
		}

		private void OpenWindowsSearchSettings_btn_Click(object sender, EventArgs e) {
			new EditWindowsSearchSettings().ShowDialog();
		}

		private void EditUserShellFolders_btn_Click(object sender, EventArgs e) {
			new EditUserShellFolders().ShowDialog();
		}

		private void ApplyPresetDialog_btn_Click(object sender, EventArgs e) {
			new ApplyPreset().ShowDialog();
		}

		private void button3_Click(object sender, EventArgs e) {
			AdvancedUserShellFolder.LoadUSF();
			SpecialFolders.DebugShowAllFolders();
//         OperatingMethods.MoveFolderPhysically(new DirectoryInfo(@"c:\Program Files\Microsoft Office"),
//            new DirectoryInfo("F:\\SSDAllo\\C\\Program Files\\Microsoft Office"));
			//         Wrapper.RegistryMethods.ApplyRegfile();
			//         Wrapper.RegistryMethods.SetProtectedRegistryValue(new RegistryValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders","OEM LINKS"),"F:\\TobiasAcc\\Desktop\\JufoTesting\\OEM2",RegistryValueKind.String );
			//   Wrapper.RegistryMethods.SetProtectedRegistryValue();

			//OperatingMethods.CheckForSysinternals();
			//string parameters =
			//     $"  /K runas /profile /user:TobiasAcc  \"{Wrapper.AddBackslahes($"{Path.Combine(Wrapper.System32Path, "cmd.exe")} /k {Path.Combine(Directory.GetCurrentDirectory(), "PsTools", "PSEXEC.exe")} -u {Environment.UserName} -h -i \"{Path.Combine(Wrapper.System32Path, "reg.exe")}\" {Wrapper.AddBackslahes("ADD \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders\" /v \"OEM LINKS\" /d \"F:\\TobiasAcc\" /f")}\"")} ";

			//$" /K {Path.Combine(Directory.GetCurrentDirectory(), "PsTools", "PSEXEC.exe")} -i -s " +
			//$"\"{Path.Combine(Wrapper.System32Path, "reg.exe")}\" " +
			//$"\"{Wrapper.AddBackslahes($" ADD \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders\" /v \"OEM LINKS\" /d \"F:\\TobiasAcc\" /f")}\"";
			// Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path,"cmd.exe"), parameters,
			//out string[] _,out int _, false,true,false,true,false);
			//         Wrapper.RegistryMethods.SetRegistryValue(
			//            new RegistryValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "OEM Links"), 
			//            "F:\\TobiasAcc", RegistryValueKind.ExpandString);
			//         Wrapper.ExecuteCommand(
			//            "reg ADD \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders\" /v \"OEM LINKS\" /d \"F:\\TobiasAcc\" /f",
			//            true, false, debug: true);
			//         FileAndFolder.DeleteFile(new FileInfo("F:\\Prg\\Energy\\Eray.exe"));
			//Wrapper.RegistryMethods.GetRegistryValue(
			//   new RegistryValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders", "Cache"),
			//   out object tmp,true);
		}
	}
}