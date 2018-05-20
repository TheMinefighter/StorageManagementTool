using System.Linq;
using System.Windows;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;
using static StorageManagementCore.WPFGUI.GlobalizationRessources.PagefileSettingsStrings;
namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void PagefilesTi_OnLoaded(object sender, RoutedEventArgs e) {
			NewSwapfileDriveCmb.ItemsSource =
				Wrapper.getDrives().Select(OperatingMethods.GetDriveInfoDescription);
			DriveForNewPageFileCmb.ItemsSource = Wrapper.getDrives().Select(OperatingMethods.GetDriveInfoDescription);
		}

		private void PagefilesTi_OnSelected(object sender, RoutedEventArgs e) { }
	}
}