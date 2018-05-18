﻿using System.IO;
using System.Windows;
using StorageManagementCore.WPFGUI.GlobalizationRessources;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private bool _searchTiOpened;

		private void SearchTi_OnLoaded(object sender, RoutedEventArgs e) {
			SelectNewSearchDirectoryBtn.Content = SearchSettingsStrings.SelectNewPath_btn_Text;
			NewSearchPathLbl.Text = SearchSettingsStrings.NewPath_lbl_Text;
			OpenCurrentSearchDirectoryBtn.Content = SearchSettingsStrings.OpenCurrentPath_btn_Text;
			CurrentSearchDirectoryLbl.Text = SearchSettingsStrings.CurrentLocation_lbl_Text;
			ApplySearchPathBtn.Content = SearchSettingsStrings.SaveSettings_btn_Text;
		}

		private void SearchTi_OnSelected(object sender, RoutedEventArgs e) {
			if (!_searchTiOpened) {
				_searchTiOpened = true;
				if (!Session.Singleton.IsAdmin) {
					//TODO Warning
					ApplySearchPathBtn.IsEnabled = false;
				}
				else {
					if (Operation.OperatingMethods.GetSearchDataPath(out DirectoryInfo path)) {
						CurrentSearchPathTb.Text = path.FullName;
					}
				}
			}
		}

		private void OpenCurrentSearchDirectoryBtn_OnClick(object sender, RoutedEventArgs e) {
			Backend.FileAndFolder.OpenFolder(new DirectoryInfo(CurrentSearchPathTb.Text));
		}

		private void SelectNewSearchDirectoryBtn_OnClick(object sender, RoutedEventArgs e) {
			NewSearchPathTb.Text = Backend.FileAndFolder.SelectDirectory().FullName;
		}
	}
}