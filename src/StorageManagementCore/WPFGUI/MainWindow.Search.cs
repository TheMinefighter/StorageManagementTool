using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;
using static StorageManagementCore.WPFGUI.GlobalizationRessources.SearchSettingsStrings;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private bool _searchTiOpened;

		private void SearchTi_OnLoaded(object sender, RoutedEventArgs e) {
			LocalizeSearch();
		}

		private void LocalizeSearch() {
			SearchTi.Header = SearchTiText;
			SelectNewSearchDirectoryBtn.Content = SelectNewPath_btn_Text;
			NewSearchPathLbl.Text = NewPath_lbl_Text;
			OpenCurrentSearchDirectoryBtn.Content = OpenCurrentPath_btn_Text;
			CurrentSearchDirectoryLbl.Text = CurrentLocation_lbl_Text;
			ApplySearchPathBtn.Content = SaveSettings_btn_Text;
		}

		private void SearchTi_OnSelected(object sender, RoutedEventArgs e) {
			if (!_searchTiOpened) {
				_searchTiOpened = true;
				if (!Session.Singleton.IsAdmin) {
					//TODO Warning
					ApplySearchPathBtn.IsEnabled = false;
				}
				else {
					if (OperatingMethods.GetSearchDataPath(out DirectoryInfo path)) {
						CurrentSearchPathTb.Text = path.FullName;
					}
				}
			}
		}

		private void OpenCurrentSearchDirectoryBtn_OnClick(object sender, RoutedEventArgs e) {
			FileAndFolder.OpenFolder(new DirectoryInfo(CurrentSearchPathTb.Text));
		}

		private void ApplySearchPathBtn_OnClick(object sender, RoutedEventArgs e) { }

		private void SelectNewSearchDirectoryBtn_OnClick(object sender, RoutedEventArgs e) {
			NewSearchPathTb.Text = FileAndFolder.SelectDirectory().FullName;
		}

		private void NewSearchPathTb_OnTextChanged(object sender, TextChangedEventArgs e) {
			NewSearchPathTb.Background = Directory.Exists(NewSearchPathTb.Text) ? Brushes.White : Brushes.DarkOrange;
		}
	}
}