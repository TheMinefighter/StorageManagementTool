using System.Windows;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private bool _searchTiOpened;
			
		private void SearchTi_OnLoaded(object sender, RoutedEventArgs e) {
		
		}

		private void SearchTi_OnSelected(object sender, RoutedEventArgs e) {
			if (!_searchTiOpened) {
				_searchTiOpened = true;
				if (!Session.Singleton.IsAdmin) {
					//TODO Warning
				}
			}
		}
	}
}