using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow
	{
		private readonly Dictionary<Configuration.MonitoringAction, RadioButton> _forFilesDictionary =
			new Dictionary<Configuration.MonitoringAction, RadioButton>();


		private readonly Dictionary<Configuration.MonitoringAction, RadioButton> _forDirectoriesDictionary =
			new Dictionary<Configuration.MonitoringAction, RadioButton>();
      private Configuration.MonitoringConfiguration _newMonitoringCfg;
		private void MonitoringTi_OnLoaded(object sender, RoutedEventArgs e)
		{
			_newMonitoringCfg = Session.Singleton.CurrentConfiguration.MonitoringSettings;
			bool ssdMonitoringEnabled = Operation.SSDMonitoring.SSDMonitoringEnabled();
			EnOrDisableMonitoringCb.IsChecked = ssdMonitoringEnabled;
			MonitoringEnOrDisabled(ssdMonitoringEnabled);
			_forDirectoriesDictionary.Add(Configuration.MonitoringAction.Ask,ForDirectoriesAsk);
			_forDirectoriesDictionary.Add(Configuration.MonitoringAction.Ignore,ForDirectoriesIgnore);
			_forDirectoriesDictionary.Add(Configuration.MonitoringAction.Move, ForDirectoriesMove);
			_forFilesDictionary.Add(Configuration.MonitoringAction.Ask, ForFilesAskRb);
			_forFilesDictionary.Add(Configuration.MonitoringAction.Ignore, ForFilesIgnoreRb);
			_forFilesDictionary.Add(Configuration.MonitoringAction.Move, ForFilesMoveRb);
      }

		private void MonitoringEnOrDisabled(bool ssdMonitoringEnabled)
		{
			AddMonitoredFolderBtn.IsEnabled = ssdMonitoringEnabled;
			MonitoredFoldersLb.IsEnabled = ssdMonitoringEnabled;
			MonitoredFoldersLb.ItemsSource = ssdMonitoringEnabled ? _newMonitoringCfg.MonitoredFolders : Enumerable.Empty<Configuration.MonitoredFolder>();
		}

		private void MonitoredFoldersLb_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (MonitoredFoldersLb.SelectedIndex==-1) {
				
			}
			else
			{
				_forFilesDictionary[_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].ForFiles].IsChecked = true;

				_forDirectoriesDictionary[_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].ForDirectories].IsChecked = true;
         }
		}
		//public class DisplayableMonitoredFolder
		//{
		//	public string Message { get; set; }
		//	public Brush MessageColor { get; set; }
		//}
		private void EnOrDisableMonitoringCb_OnChecked(object sender, RoutedEventArgs e)
		{
			MonitoringEnOrDisabled(EnOrDisableMonitoringCb.IsChecked.Value);
		}

		private void AddMonitoredFolderBtn_OnClick(object sender, RoutedEventArgs e)
		{
			//TODO Add desc
			_newMonitoringCfg.MonitoredFolders.Add(new Configuration.MonitoredFolder(Backend.FileAndFolder.SelectDirectory("").FullName));
			MonitoredFoldersLb.Items.Refresh();
			MonitoredFoldersLb.SelectedIndex = _newMonitoringCfg.MonitoredFolders.Count - 1;
			MonitoredFoldersLb.Focus();
		}

		private void ChangeMonitoredFolderPathBtn_OnClick(object sender, RoutedEventArgs e)
		{
			//TODO Add desc
			_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].TargetPath =
				Backend.FileAndFolder.SelectDirectory("").FullName;
			MonitoredFoldersLb.Items.Refresh();
			MonitoredFoldersLb.Focus();
      }

		private void RemoveMonitoredFolderBtn_OnClick(object sender, RoutedEventArgs e)
		{
			_newMonitoringCfg.MonitoredFolders.RemoveAt(MonitoredFoldersLb.SelectedIndex);
			MonitoredFoldersLb.Items.Refresh();
			MonitoredFoldersLb.Focus();
      }

		private void ApplyMonitoringBtn_OnClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void MonitoringForFiles_Changed(object sender, RoutedEventArgs e)
		{
			_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].ForFiles =
				_forFilesDictionary.First(x => x.Value.IsChecked == true).Key;
		}

		private void MonitoringForDirectories_Changed(object sender, RoutedEventArgs e)
		{
			_newMonitoringCfg.MonitoredFolders[MonitoredFoldersLb.SelectedIndex].ForDirectories =
				_forDirectoriesDictionary.First(x => x.Value.IsChecked == true).Key;
      }
	}
}