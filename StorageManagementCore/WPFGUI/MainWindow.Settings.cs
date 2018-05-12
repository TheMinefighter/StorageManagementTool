using System.Windows;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void SettingsTi_OnLoaded(object sender, RoutedEventArgs e) {
			//TODO IsAdministratorTb
		}

		private void RestartAsAdministratorBtn_OnClick(object sender, RoutedEventArgs e) {
			Wrapper.RestartAsAdministrator();
		}
	}
}