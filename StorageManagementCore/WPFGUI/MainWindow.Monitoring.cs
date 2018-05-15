﻿using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void MonitoringTi_OnLoaded(object sender, RoutedEventArgs e) {
			bool ssdMonitoringEnabled = Operation.SSDMonitoring.SSDMonitoringEnabled();
			EnOrDisableMonitoringCb.IsChecked = ssdMonitoringEnabled;
			MonitoredFoldersLb.IsEnabled = ssdMonitoringEnabled;
			MonitoredFoldersLb.Items.Clear();
			if (ssdMonitoringEnabled) {
				MonitoredFoldersLb.ItemsSource = Session.Singleton.CurrentConfiguration.MonitoringSettings.MonitoredFolders;
			}

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
	}
}