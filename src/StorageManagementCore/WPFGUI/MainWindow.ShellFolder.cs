﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using ConfirmationDialogs;
using StorageManagementCore.Backend;
using static StorageManagementCore.WPFGUI.GlobalizationResources.EditUserShellFolderStrings;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		
		private bool _shellFolderTiOpened;

		private void ShellFolderTi_OnLoaded(object sender, RoutedEventArgs e) {
			RefreshShellFolders();
			LocalizeShellFolders();
		}

		private void ShellFolderTi_OnSelected(object sender, RoutedEventArgs e) {
			if (!_shellFolderTiOpened) {
				_shellFolderTiOpened = true;
				if (!Session.Singleton.IsAdmin) {
					//TODO Warning	
				}
			}
		}

		private void LocalizeShellFolders() {
			ShellFolderTi.Header = ShellFolderTiText;
			CurrentShellFolderPathLbl.Text = CurrentUSFPath_lbl_Text;
			OpenCurrentShellFolderPathBtn.Content = USFOpenCurrentPath_btn_Text;
			SelectNewShellFolderPathBtn.Content = SelectNewUSFPath_btn_Text;
			NewShellFolderPathLbl.Text = NewUSFPath_lbl_Text;
			MoveExistingShellFolderItemsCb.Content = MoveExistingItemsCbText;
			ChangeDependentShellFoldersCb.Content = ChangeDependentShellFoldersCbText;
			ViewHiddenFoldersCb.Content = ViewHiddenFoldersCbText;
			ApplyShellFolderLocationBtn.Content = SetUSF_btn_Text;
		}

		private void RefreshShellFolders() {
//			ShellFoldersLb.ItemsSource = ViewHiddenFoldersCb.IsChecked == true
//				? ShellFolder.AllShellFolders.Select(x => x.LocalizedName)
//				: ShellFolder.AllShellFolders.Where(x => !x.Undefined).Select(x => x.LocalizedName);
		}

		private void ChangeDependentShellFoldersCb_Unchecked(object sender, RoutedEventArgs e) {
			ChangeDependentShellFoldersCb.IsChecked = !Confirmation.Confirm();
		}

		private void MoveExistingItemsCb_Unchecked(object sender, RoutedEventArgs e) {
			MoveExistingShellFolderItemsCb.IsChecked = !Confirmation.Confirm();
		}

		private void ViewHiddenFoldersCb_Checked(object sender, RoutedEventArgs e) {
			ViewHiddenFoldersCb.IsChecked = Confirmation.Confirm();
		}


		private void ApplyShellFolderLocationBtn_Click(object sender, RoutedEventArgs e) {
//			SpecialFolders.SetSpecialFolderPath(
//				ShellFolder.GetUSF(ShellFoldersLb.SelectedItem as string), NewShellFolderPathTb.Text);
//			ShellFoldersLb_OnSelectionChanged(null, null);
		}

		private void OpenCurrentShellFolderPathBtn_OnClick(object sender, RoutedEventArgs e) {
			FileAndFolder.OpenFolder(new DirectoryInfo(CurrentShellFolderPathTb.Text));
		}

		private void ViewHiddenFoldersCb_Unchecked(object sender, RoutedEventArgs e) {
			RefreshShellFolders();
		}

		private void ShellFoldersLb_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			DirectoryInfo path = ((ShellFolder) ShellFoldersLb.SelectedItem).GetPath();
//			CurrentShellFolderPathTb.Text = path != null ? path.FullName : "Seems as if your PC hasn´t configured that path for whatever reason";

		//	ApplyShellFolderLocationBtn.IsEnabled =
//				ShellFoldersLb.SelectedIndex != -1 && Directory.Exists(MoveObjectsRootPathTb?.Text ?? "LOL");

			if (AutomaticShellFolderPathCb.IsChecked == true) {
				//NewShellFolderPathTb.Text=Session.Singleton.CurrentConfiguration.DefaultHDDPath
				//TODO Implement this
			}
		}


		private void CurrentShellFolderPathTb_OnTextChanged(object sender, TextChangedEventArgs e) {
			if (MainWindowObject.IsLoaded) {
				bool exists = Directory.Exists(CurrentShellFolderPathTb.Text);
				OpenCurrentShellFolderPathBtn.IsEnabled = exists;
				CurrentShellFolderPathTb.Background = exists ? Brushes.White : Brushes.DarkOrange;
			}
		}

		private void AutomaticShellFolderPathCb_Checked(object sender, RoutedEventArgs e) {
			NewShellFolderPathTb.IsEnabled = !AutomaticShellFolderPathCb.IsChecked.Value;
			SelectNewShellFolderPathBtn.IsEnabled = !AutomaticShellFolderPathCb.IsChecked.Value;
		}

		private void SelectNewShellFolderPathBtn_OnClick(object sender, RoutedEventArgs e) {
			NewShellFolderPathTb.Text = FileAndFolder.SelectDirectory().FullName;
		}

		private class ApplyShellFolderLocation : ICommand {
			public bool CanExecute(object parameter) => throw new NotImplementedException();

			public void Execute(object parameter) {
				throw new NotImplementedException();
			}

			public event EventHandler CanExecuteChanged;
		}

		private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e) {
			System.Diagnostics.Process.Start(e.Uri.ToString());
		}
	}
}