using System.Windows;
using System.Windows.Controls;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI {
	/// <summary>
	///  Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow()
		{

			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			AdvancedUserShellFolder.LoadUSF();
		}


		private void BaseTc_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

		private void OpenCurrentSearchPathBtn_OnClick(object sender, RoutedEventArgs e)
		{
			throw new System.NotImplementedException();
		}
	}
}