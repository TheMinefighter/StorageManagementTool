using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI {
	/// <summary>
	///  Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			if (!Session.Singleton.IsAdmin&&Session.Singleton.Configuration.CredentialsOnStartup) {
				List<string> args = Environment.GetCommandLineArgs().ToList();
				args.RemoveAt(0);
				Wrapper.RestartAsAdministrator(args.ToArray());
			}
			AdvancedUserShellFolder.LoadUSF();
			if (Session.Singleton.IsAdmin) {
				Title += " (Administrator)";
			}
		}


		private void BaseTc_SelectionChanged(object sender, SelectionChangedEventArgs e) { }


   }
}