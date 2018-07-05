using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Forms;

namespace StorageManagementCore.WPFGUI.ValueConverters
{
	public class DriveInfoConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			MessageBox.Show(targetType.ToString());
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}