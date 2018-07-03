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
			NewSwapfileDriveCmb.ItemsSource =
				Wrapper.GetDrives().Select(OperatingMethods.GetDriveInfoDescription);
			DriveForNewPageFileCmb.ItemsSource = Wrapper.GetDrives().Select(OperatingMethods.GetDriveInfoDescription);
			LocalizePagefiles();
			//TODO Cleanup resources

		}

		private void LocalizePagefiles()
		{
			MaximumPagefileSizeLbl.Text = MaximumPagefileSize_lbl_Text;
			MinimumPagefileSizeLbl.Text = MinimumPagefileSize_lbl_Text;
			DriveForNewPageFileLbl.Text = PagefileDrive_lbl_Text;
		}

		private void PagefilesTi_OnSelected(object sender, RoutedEventArgs e) { }

		private void ApplyPageFileSysBtn_OnClick(object sender, RoutedEventArgs e) { }

		private void SystemManagedPagefilesCb_OnUnchecked(object sender, RoutedEventArgs e)
		{
			PagefilesLb.SelectedItem = null;
		}

		private void AddPagefileBtn_OnClick(object sender, RoutedEventArgs e)
		{
			ViewModel.PagefileConfiguration.Pagefiles.Add(new Pagefile(OperatingMethods.));
		}
	}
}