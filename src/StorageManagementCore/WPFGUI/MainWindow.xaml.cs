using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI {
	/// <inheritdoc cref="Window" />
	/// <summary>
	///  Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private MainViewModel ViewModel => (MainViewModel) Resources["ViewModel"];

		public MainWindow() {
#if DEBUG
			try {
				InitializeComponent();
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
#else
			InitializeComponent();
#endif

			//PagefilesTi.DataContext = ProposedPagefileConfiguration;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			if (!Session.Singleton.IsAdmin && Session.Singleton.Configuration.CredentialsOnStartup) {
				List<string> args = Environment.GetCommandLineArgs().ToList();
				args.RemoveAt(0);
				Wrapper.RestartProgram(true, args.ToArray());
			}

			if (Session.Singleton.IsAdmin) {
				Title += " (Administrator)";
			}
		}


		private void BaseTc_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
	}
}