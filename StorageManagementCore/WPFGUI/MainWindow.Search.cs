using System.IO;
using System.Windows;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private bool _searchTiOpened;

		private void SearchTi_OnLoaded(object sender, RoutedEventArgs e) { }

		private void SearchTi_OnSelected(object sender, RoutedEventArgs e) {
			if (!_searchTiOpened) {
				_searchTiOpened = true;
				if (!Session.Singleton.IsAdmin) {
					//TODO Warning
				}
			}
		}

		private void OpenCurrentSearchPathBtn_OnClick(object sender, RoutedEventArgs e) {
			FileAndFolder.OpenFolder(new DirectoryInfo(CurrentSearchPathTb.Text));
		}

		private void SelectNewSearchDirectoryBtn_OnClick(object sender, RoutedEventArgs e) {
			throw new System.NotImplementedException();
		}
	}
}