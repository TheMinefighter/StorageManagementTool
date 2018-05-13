using System.IO;
using System.Windows;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void SettingsTi_OnLoaded(object sender, RoutedEventArgs e) {
			DefaultHDDPathChanged();
			//TODO IsAdministratorTb
		}

		private void DefaultHDDPathChanged() {
			EnOrDisableSendToHDDCb.IsEnabled = HDDPathValid();
		}

		private void RestartAsAdministratorBtn_OnClick(object sender, RoutedEventArgs e) {
			Wrapper.RestartAsAdministrator();
		}

		private void EnOrDisableSendToHDDCb_OnChecked(object sender, RoutedEventArgs e) {
			if (HDDPathValid()) {
				OperatingMethods.EnableSendToHDD(true);
			}
			else {
				//TODO throw message
			}
		}

		private static bool HDDPathValid() => Session.Singleton.CurrentConfiguration.DefaultHDDPath != null &&
		                                      Directory.Exists(Session.Singleton.CurrentConfiguration.DefaultHDDPath);

		private void EnOrDisableSendToHDDCb_OnUnchecked(object sender, RoutedEventArgs e) {
			OperatingMethods.EnableSendToHDD(false);
		}
	}
}