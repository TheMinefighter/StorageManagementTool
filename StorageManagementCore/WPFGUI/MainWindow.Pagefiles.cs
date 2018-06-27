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

		//From https://stackoverflow.com/a/1268648/6730162 last access 5/20/2018
		private static bool IsTextNumber(string text) => !new Regex("[^0-9]+").IsMatch(text);

		private void PageFileSizeTb_OnPasting(object sender, DataObjectPastingEventArgs e) {
			if (e.DataObject.GetDataPresent(typeof(string))) {
				string text = (string) e.DataObject.GetData(typeof(string));
				if (!IsTextNumber(text)) {
					e.CancelCommand();
				}
			}
			else {
				e.CancelCommand();
			}
		}

		private void PageFileSizeTb_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
			e.Handled = !IsTextNumber(e.Text);
		}

		private void ApplyPageFileSysBtn_OnClick(object sender, RoutedEventArgs e) { }
	}
}