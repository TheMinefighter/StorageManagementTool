using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;
using static StorageManagementCore.WPFGUI.GlobalizationRessources.PagefileSettingsStrings;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		public PagefileSysConfiguration ProposedPagefileConfiguration { get; set; } = new PagefileSysConfiguration();

		private void PagefilesTi_OnLoaded(object sender, RoutedEventArgs e) {
			PagefilesTi.Header = PagefilesTiText;
			NewSwapfileDriveCmb.ItemsSource =
				Wrapper.GetDrives().Select(OperatingMethods.GetDriveInfoDescription);
			DriveForNewPageFileCmb.ItemsSource = Wrapper.GetDrives().Select(OperatingMethods.GetDriveInfoDescription);
		}

		private void PagefilesTi_OnSelected(object sender, RoutedEventArgs e) { }

		private void ApplyPageFileSysBtn_OnClick(object sender, RoutedEventArgs e) { }
	}
}