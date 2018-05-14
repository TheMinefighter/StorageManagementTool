using System.Windows;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void MonitoringTi_OnLoaded(object sender, RoutedEventArgs e) {
			bool ssdMonitoringEnabled = SSDMonitoring.SSDMonitoringEnabled();
			MonitoredFoldersLb.IsEnabled = ssdMonitoringEnabled;
			
			if (ssdMonitoringEnabled) {
				
			}
			else {
				
			}
		}
	}
}