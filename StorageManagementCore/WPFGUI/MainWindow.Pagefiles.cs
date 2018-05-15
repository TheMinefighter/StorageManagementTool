using System.Linq;
using System.Windows;

namespace StorageManagementCore.WPFGUI
{
   public partial class MainWindow
   {
      private void PagefilesTi_OnLoaded(object sender, RoutedEventArgs e)
      {
         NewSwapfileDriveCmb.ItemsSource =
            Backend.Wrapper.getDrives().Select(Operation.OperatingMethods.GetDriveInfoDescription);
         DriveForNewPageFileCmb.ItemsSource = Backend.Wrapper.getDrives().Select(Operation.OperatingMethods.GetDriveInfoDescription);
      }

	   private void PagefilesTi_OnSelected(object sender, RoutedEventArgs e)
	   {
		   
	   }
   }
}