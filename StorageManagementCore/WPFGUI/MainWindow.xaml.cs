using System.Windows;
using System.Windows.Controls;

namespace StorageManagementCore.WPFGUI {
	/// <summary>
	///  Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			//	BaseTc.
		}

		private void EnOrDisableMonitoringBtn_Click(object sender, RoutedEventArgs e) { }

		private void BaseTc_SelectionChanged(object sender, SelectionChangedEventArgs e) { }


      private void MoveDependentShellFoldersCb_Unchecked(object sender, RoutedEventArgs e)
      {

      }

      private void MoveExistingItemsCb_Unchecked(object sender, RoutedEventArgs e)
      {

      }

      private void ViewHiddenFoldersCb_Checked(object sender, RoutedEventArgs e)
      {

      }

      private void ApplyShellFolderLocationBtn_Click(object sender, RoutedEventArgs e)
      {

      }
   }
}