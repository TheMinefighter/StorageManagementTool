using System.Windows;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		public bool SearchTiOpened;
			
		private void SearchTi_OnLoaded(object sender, RoutedEventArgs e) {
		
		}

		private void SearchTi_OnSelected(object sender, RoutedEventArgs e) {
			if (!SearchTiOpened) {
				SearchTiOpened = true;
				
			}
		}
	}
}