using System;
using System.IO;
using System.Windows;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;
using static StorageManagementCore.WPFGUI.GlobalizationRessources.PagefileSettingsStrings;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		//public PagefileSysConfiguration ProposedPagefileConfiguration { get; set; } = new PagefileSysConfiguration();

		private void PagefilesTi_OnLoaded(object sender, RoutedEventArgs e) {
			EnOrDisableHibernateBtn.Content =
				File.Exists(Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%\\hibfil.sys"))
					? DisableHibernate_btn_Text
					: Enablehibernate_btn_Text;
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
			PagefilesGb.Header = PagefileSettings_gb_Text;
			HibfilGb.Header = HiberfilSettings_gb_Text;
			SwapfileGb.Header = SwapfileSettings_gb_Text;
		}

		private void PagefilesTi_OnSelected(object sender, RoutedEventArgs e) { }

		private void ApplyPageFileSysBtn_OnClick(object sender, RoutedEventArgs e) {
			ViewModel.LoadPagefiles();
		}

		private void SystemManagedPagefilesCb_OnChecked(object sender, RoutedEventArgs e) {
			PagefilesLb.SelectedItem = null;
		}

		private void AddPagefileBtn_OnClick(object sender, RoutedEventArgs e) {
			ViewModel.ProposedPagefileConfiguration.Pagefiles.Add(
				new Pagefile(new ConfiguredDrive((DriveInfo) DriveForNewPageFileCmb.SelectedItem), 2048, 4096));
		}

		private void RemovePagefileBtn_OnClick(object sender, RoutedEventArgs e) {
			ViewModel.ProposedPagefileConfiguration.Pagefiles.RemoveAt(PagefilesLb.SelectedIndex);
		}

		private void SetSwapfileState_OnClick(object sender, RoutedEventArgs e) {
			switch (Swapfile.GetSwapfileState()) {
				case Swapfile.SwapfileState.Standard:
					Swapfile.ChangeSwapfileStadium(true, Swapfile.SwapfileState.Standard);
					break;
				case Swapfile.SwapfileState.Disabled:
					Swapfile.ChangeSwapfileStadium(true, Swapfile.SwapfileState.Disabled,
						(DriveInfo) NewSwapfileDriveCmb.SelectedItem);
					break;
				case Swapfile.SwapfileState.Moved:
					Swapfile.ChangeSwapfileStadium(false, Swapfile.SwapfileState.Moved);
					break;
				case Swapfile.SwapfileState.None:
					Swapfile.ChangeSwapfileStadium(true, Swapfile.SwapfileState.Standard);
					FileAndFolder.CreateShortcut(
						$"-ContinueSwapfile forward -Drive {((DriveInfo) NewSwapfileDriveCmb.SelectedItem).Name}",
						new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
							CmdRootContext.StartupProceedFileName)));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void EnOrDisableHibernateBtn_OnClick(object sender, RoutedEventArgs e) {
			OperatingMethods.SetHibernate(!File.Exists(Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%\\hibfil.sys")));
		}
	}
}