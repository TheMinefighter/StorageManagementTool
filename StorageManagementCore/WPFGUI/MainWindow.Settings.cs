using System.Windows;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void SettingsTi_OnLoaded(object sender, RoutedEventArgs e) {
			//TODO IsAdministratorTb
		}

		private void RestartAsAdministratorBtn_OnClick(object sender, RoutedEventArgs e) {
			Backend.Wrapper.RestartAsAdministrator();
			}
	}
}