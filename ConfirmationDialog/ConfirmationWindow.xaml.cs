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


namespace ConfirmationDialog {
	/// <summary>
	/// Interaction logic for ConfirmationWindow.xaml
	/// </summary>
	public partial class ConfirmationWindow : Window {
		internal ConfirmationWindow(ConfirmationTag tag) {
			Tag = tag;
			DescriptionTb.Text = tag.Text;
		}


		private void ConfirmBtn_OnClick(object sender, RoutedEventArgs e) {
			if (ConfirmationBoxTb.Text == ((ConfirmationTag) Tag).Text) {
				((ConfirmationTag) Tag).Confirmed = true;
				Close();
			}
		}

		private void ConfirmationBoxTb_OnTextChanged(object sender, TextChangedEventArgs e) {
			if (ConfirmationBoxTb.Text == ((ConfirmationTag) Tag).Text) {
				ConfirmBtn.IsEnabled = true;
			}
		}
	}
}