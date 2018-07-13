using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	/// <inheritdoc cref="Window" />
	/// <summary>
	///  Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private MainViewModel ViewModel => (MainViewModel) Resources["ViewModel"];

		public MainWindow() {
			try {
				InitializeComponent();
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}

			//PagefilesTi.DataContext = ProposedPagefileConfiguration;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			if (!Session.Singleton.IsAdmin && Session.Singleton.Configuration.CredentialsOnStartup) {
				List<string> args = Environment.GetCommandLineArgs().ToList();
				args.RemoveAt(0);
				Wrapper.RestartProgram(true, args.ToArray());
			}

			AdvancedUserShellFolder.LoadUSF();
			if (Session.Singleton.IsAdmin) {
				Title += " (Administrator)";
			}
		}


		private void BaseTc_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

		private void SetSwapfileState_OnClick(object sender, RoutedEventArgs e) {
			//TODO add Disabled state
			switch (Swapfile.GetSwapfileState()) {
				case Swapfile.SwapfileState.Standard:
					Swapfile.ChangeSwapfileStadium(true, Swapfile.SwapfileState.Standard);
					break;
				case Swapfile.SwapfileState.Disabled:
					Swapfile.ChangeSwapfileStadium(true, Swapfile.SwapfileState.Disabled,
						(DriveInfo) NewSwapfileDriveCmb.SelectedItem);
					break;
				case Swapfile.SwapfileState.Moved:
					Swapfile.ChangeSwapfileStadium(false, Swapfile.SwapfileState.Moved);
					break;
				case Swapfile.SwapfileState.None:
					Swapfile.ChangeSwapfileStadium(true, Swapfile.SwapfileState.Standard);
					FileAndFolder.CreateShortcut(
						$"-ContinueSwapfile forward -Drive {((DriveInfo) NewSwapfileDriveCmb.SelectedItem).Name}",
						new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
							CmdRootContext.StartupProceedFileName)));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void EnOrDisableHibernateBtn_OnClick(object sender, RoutedEventArgs e) {
				OperatingMethods.SetHibernate(!File.Exists(Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%\\hibfil.sys")));
		}
	}
}