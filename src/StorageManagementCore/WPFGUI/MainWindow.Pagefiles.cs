using System.IO;
using System.Linq;
using System.Windows;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;
using static StorageManagementCore.WPFGUI.GlobalizationRessources.PagefileSettingsStrings;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		//public PagefileSysConfiguration ProposedPagefileConfiguration { get; set; } = new PagefileSysConfiguration();

		private void PagefilesTi_OnLoaded(object sender, RoutedEventArgs e) {
			PagefilesTi.Header = PagefilesTiText;
			//DriveForNewPageFileCmb.ItemsSource = Wrapper.GetDrives().Select(OperatingMethods.GetDriveInfoDescription);
			LocalizePagefiles();
			//TODO Cleanup resources
		}

		private void LocalizePagefiles() {
			MaximumPagefileSizeLbl.Text = MaximumPagefileSize_lbl_Text;
			MinimumPagefileSizeLbl.Text = MinimumPagefileSize_lbl_Text;
			DriveForNewPageFileLbl.Text = PagefileDrive_lbl_Text;
			AddPagefileBtn.Content = AddPagefileBtnText;
			RemovePagefileBtn.Content = RemovePagefileBtnText;
			ApplyPagefileSysBtn.Content = ApplyPagefileChanges_btn_Text;
		}

		private void PagefilesTi_OnSelected(object sender, RoutedEventArgs e) { }

		private void ApplyPageFileSysBtn_OnClick(object sender, RoutedEventArgs e) { }

		private void SystemManagedPagefilesCb_OnChecked(object sender, RoutedEventArgs e) {
			PagefilesLb.SelectedItem = null;
		}

		private void AddPagefileBtn_OnClick(object sender, RoutedEventArgs e) {
			ViewModel.ProposedPagefileConfiguration.Pagefiles.Add(
				new Pagefile(new ConfiguredDrive((DriveInfo) DriveForNewPageFileCmb.SelectedItem),2048, 4096));
		}

		private void RemovePagefileBtn_OnClick(object sender, RoutedEventArgs e) {
			ViewModel.ProposedPagefileConfiguration.Pagefiles.RemoveAt(PagefilesLb.SelectedIndex);
		}
	}
}