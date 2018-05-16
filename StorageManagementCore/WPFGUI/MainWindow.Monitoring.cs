using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow
	{
		private Configuration.MonitoringConfiguration _newMonitoringCfg;
		private void MonitoringTi_OnLoaded(object sender, RoutedEventArgs e)
		{
			_newMonitoringCfg = Session.Singleton.CurrentConfiguration.MonitoringSettings;
			bool ssdMonitoringEnabled = Operation.SSDMonitoring.SSDMonitoringEnabled();
			EnOrDisableMonitoringCb.IsChecked = ssdMonitoringEnabled;
			MonitoringEnOrDisabled(ssdMonitoringEnabled);
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
	}
}