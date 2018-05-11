using System;
using System.Linq;
using System.Windows;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void ShellFolderTi_OnLoaded(object sender, RoutedEventArgs e) {
			RefreshShellFolders();
		}

		private void RefreshShellFolders() {
			ShellFoldersLb.ItemsSource = ViewHiddenFoldersCb.IsChecked == true
				? AdvancedUserShellFolder.AllUSF.Select(x => x.LocalizedName)
				: AdvancedUserShellFolder.AllUSF.Where(x => !x.Undefined).Select(x => x.LocalizedName);
		}

		private void MoveDependentShellFoldersCb_Unchecked(object sender, RoutedEventArgs e) {
			//TODO Warning
		}

		private void MoveExistingItemsCb_Unchecked(object sender, RoutedEventArgs e) {
			//TODO Warning
		}

		private void ViewHiddenFoldersCb_Checked(object sender, RoutedEventArgs e) {
			RefreshShellFolders();
		}


		private void ApplyShellFolderLocationBtn_Click(object sender, RoutedEventArgs e) {
			//TODO Do
		}

		private void OpenCurrentShellFolderPathBtn_OnClick(object sender, RoutedEventArgs e) {
			throw new NotImplementedException();
		}

		private void ViewHiddenFoldersCb_Unchecked(object sender, RoutedEventArgs e) {
			RefreshShellFolders();
		}
	}
}