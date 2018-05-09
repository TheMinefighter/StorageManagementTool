using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfirmationDialog
{
   /// <summary>
   /// Interaction logic for ConfirmationWindow.xaml
   /// </summary>
   public partial class ConfirmationWindow : Window
   {
      public ConfirmationWindow(string text)
      {
	      InitializeComponent();
	      DescriptionTb.Text = text;
      }


	   private void ConfirmBtn_OnClick(object sender, RoutedEventArgs e) {
		   Tag=true,
	   }
   }
}
