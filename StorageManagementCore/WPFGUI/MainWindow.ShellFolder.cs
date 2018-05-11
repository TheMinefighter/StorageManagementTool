using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConfirmationDialogs;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void ShellFolderTi_OnLoaded(object sender, RoutedEventArgs e) {
			RefreshShellFolders();
		}

		private void RefreshShellFolders() {
			ShellFoldersLb.ItemsSource = ViewHiddenFoldersCb.IsChecked == true
				? Backend.AdvancedUserShellFolder.AllUSF.Select(x => x.LocalizedName)
				: Backend.AdvancedUserShellFolder.AllUSF.Where(x => !x.Undefined).Select(x => x.LocalizedName);
		}

		private void ChangeDependentShellFoldersCb_Unchecked(object sender, RoutedEventArgs e) {
			ChangeDependentShellFoldersCb.IsChecked = !Confirmation.Confirm();
		}

		private void MoveExistingItemsCb_Unchecked(object sender, RoutedEventArgs e) {
			MoveExistingItemsCb.IsChecked= !Confirmation.Confirm();
		}

		private void ViewHiddenFoldersCb_Checked(object sender, RoutedEventArgs e) {
			if (Confirmation.Confirm()) {
				RefreshShellFolders();
			}
			else {
				ViewHiddenFoldersCb.IsChecked = false;
			}
		}


		private void ApplyShellFolderLocationBtn_Click(object sender, RoutedEventArgs e) {
//TODO do
		}

		private void OpenCurrentShellFolderPathBtn_OnClick(object sender, RoutedEventArgs e) {
			Backend.FileAndFolder.OpenFolder(new DirectoryInfo(CurrentShellFolderPathTb.Text));
		}

		private void ViewHiddenFoldersCb_Unchecked(object sender, RoutedEventArgs e) {
			RefreshShellFolders();
		}

		private void ShellFoldersLb_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			CurrentShellFolderPathTb.Text =
				Backend.SpecialFolders.GetSpecialFolderPath(Backend.AdvancedUserShellFolder.GetUSF((string) ShellFoldersLb.SelectedValue)
					.WindowsIdentifier);
		}
	}
}