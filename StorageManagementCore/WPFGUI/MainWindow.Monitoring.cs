﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;
using StorageManagementCore.WPFGUI.GlobalizationRessources;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private readonly Dictionary<MonitoringAction, RadioButton> _forDirectoriesDictionary =
			new Dictionary<MonitoringAction, RadioButton>();

		private readonly Dictionary<MonitoringAction, RadioButton> _forFilesDictionary =
			new Dictionary<MonitoringAction, RadioButton>();

		private bool _isMonitoringActive;
		private MonitoringConfiguration _newMonitoringCfg;

		private void MonitoringTi_OnLoaded(object sender, RoutedEventArgs e) {
			_newMonitoringCfg = Session.Singleton.CurrentConfiguration.MonitoringSettings;
			_isMonitoringActive = SSDMonitoring.SSDMonitoringEnabled();
			EnOrDisableMonitoringCb.IsChecked = _isMonitoringActive;
			MonitoringEnOrDisabled(_isMonitoringActive);
			LoadRadioButtonDictionaries();
			LocalizeMonitoring();
			DisableDueSelection(false);
		}

		private void LocalizeMonitoring() {
			ForFilesAskRb.Content = MonitoringSettingsStrings.AskForAction_Text;
			ForDirectoriesAsk.Content = MonitoringSettingsStrings.AskForAction_Text;
			ForFilesIgnoreRb.Content = MonitoringSettingsStrings.Ignore_Text;
			ForDirectoriesIgnore.Content = MonitoringSettingsStrings.Ignore_Text;
			ForFilesMoveRb.Content = MonitoringSettingsStrings.AutomaticMove_Text;
			ForDirectoriesMove.Content = MonitoringSettingsStrings.AutomaticMove_Text;
			AddMonitoredFolderBtn.Content = MonitoringSettingsStrings.AddFolder_btn_Text;
			ChangeMonitoredFolderPathBtn.Content = MonitoringSettingsStrings.ChangeFolder_btn_Text;
			RemoveMonitoredFolderBtn.Content = MonitoringSettingsStrings.RemoveSelectedFolder_btn_Text;
			ApplyMonitoringBtn.Content = MonitoringSettingsStrings.SaveSettings_btn_Text;
			EnOrDisableMonitoringCb.Content = MonitoringSettingsStrings.EnableNotifications_cb_Text;
		}

		private void LoadRadioButtonDictionaries() {
			_forDirectoriesDictionary.Add(MonitoringAction.Ask, ForDirectoriesAsk);
			_forDirectoriesDictionary.Add(MonitoringAction.Ignore, ForDirectoriesIgnore);
			_forDirectoriesDictionary.Add(MonitoringAction.Move, ForDirectoriesMove);
			_forFilesDictionary.Add(MonitoringAction.Ask, ForFilesAskRb);
			_forFilesDictionary.Add(MonitoringAction.Ignore, ForFilesIgnoreRb);
			_forFilesDictionary.Add(MonitoringAction.Move, ForFilesMoveRb);
		}

		private void MonitoringEnOrDisabled(bool ssdMonitoringEnabled) {
			AddMonitoredFolderBtn.IsEnabled = ssdMonitoringEnabled;
			MonitoredFoldersLb.IsEnabled = ssdMonitoringEnabled;
			MonitoredFoldersLb.ItemsSource = ssdMonitoringEnabled ? _newMonitoringCfg.MonitoredFolders : Enumerable.Empty<MonitoredFolder>();
		}

		private void MonitoredFoldersLb_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			bool somethingSelected = MonitoredFoldersLb.SelectedIndex != -1;
			if (somethingSelected) {
				_forFilesDictionary[_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].ForFiles]
					.IsChecked = true;

				_forDirectoriesDictionary[
					_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].ForDirectories].IsChecked = true;
			}

			DisableDueSelection(somethingSelected);
		}

		private void DisableDueSelection(bool somethingSelected) {
			RemoveMonitoredFolderBtn.IsEnabled = somethingSelected;
			ChangeMonitoredFolderPathBtn.IsEnabled = somethingSelected;
			OpenMonitoredFolderBtn.IsEnabled = somethingSelected;
		}

		//public class DisplayableMonitoredFolder
		//{
		//	public string Message { get; set; }
		//	public Brush MessageColor { get; set; }
		//}
		private void EnOrDisableMonitoringCb_OnChecked(object sender, RoutedEventArgs e) {
			MonitoringEnOrDisabled(EnOrDisableMonitoringCb.IsChecked.Value);
		}

		private void AddMonitoredFolderBtn_OnClick(object sender, RoutedEventArgs e) {
			//TODO Add desc
			_newMonitoringCfg.MonitoredFolders.Add(new MonitoredFolder(FileAndFolder.SelectDirectory("").FullName));
			MonitoredFoldersLb.Items.Refresh();
			MonitoredFoldersLb.SelectedIndex = _newMonitoringCfg.MonitoredFolders.Count - 1;
			MonitoredFoldersLb.Focus();
		}

		private void ChangeMonitoredFolderPathBtn_OnClick(object sender, RoutedEventArgs e) {
			//TODO Add desc
			_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].TargetPath =
				FileAndFolder.SelectDirectory("").FullName;
			MonitoredFoldersLb.Items.Refresh();
			MonitoredFoldersLb.Focus();
		}

		private void RemoveMonitoredFolderBtn_OnClick(object sender, RoutedEventArgs e) {
			_newMonitoringCfg.MonitoredFolders.RemoveAt(MonitoredFoldersLb.SelectedIndex);
			MonitoredFoldersLb.Items.Refresh();
			MonitoredFoldersLb.Focus();
		}

		private void ApplyMonitoringBtn_OnClick(object sender, RoutedEventArgs e) {
			Session.Singleton.CurrentConfiguration.MonitoringSettings = _newMonitoringCfg.Clone() as MonitoringConfiguration;
			if (_isMonitoringActive != EnOrDisableMonitoringCb.IsChecked) {
				SSDMonitoring.SetSSDMonitoring(EnOrDisableMonitoringCb.IsChecked.Value);
			}
		}

		private void MonitoringForFiles_Changed(object sender, RoutedEventArgs e) {
			_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].ForFiles =
				_forFilesDictionary.First(x => x.Value.IsChecked == true).Key;
		}

		private void MonitoringForDirectories_Changed(object sender, RoutedEventArgs e) {
			_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].ForDirectories =
				_forDirectoriesDictionary.First(x => x.Value.IsChecked == true).Key;
		}

		private void OpenMonitoredFolderBtn_OnClick(object sender, RoutedEventArgs e) {
			FileAndFolder.OpenFolder(new DirectoryInfo(MonitoredFoldersLb.SelectedItem as string));
		}
	}
}