using System.Linq;
using System.Windows;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void MonitoringTi_OnLoaded(object sender, RoutedEventArgs e) {
			bool ssdMonitoringEnabled = SSDMonitoring.SSDMonitoringEnabled();
			MonitoredFoldersLb.IsEnabled = ssdMonitoringEnabled;
			MonitoredFoldersLb.Items.Clear();
			if (ssdMonitoringEnabled) {
				MonitoredFoldersLb.ItemsSource = Session.Singleton.CurrentConfiguration.MonitoringSettings.MonitoredFolders.Select(x => x.TargetPath);
			}
			else {
				
			}
		}
	}
}