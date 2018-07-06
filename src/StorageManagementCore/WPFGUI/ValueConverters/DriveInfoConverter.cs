using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Forms;
using StorageManagementCore.Configuration;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class DriveInfoConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			targetType == typeof(string) && value is DriveInfo drive
				? Operation.OperatingMethods.GetDriveInfoDescription(drive)
				: throw new InvalidOperationException();


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			targetType == typeof(DriveInfo) && value is string toConvert &&
			Operation.OperatingMethods.GetDriveInfoFromDescription(out DriveInfo drive, toConvert)
				? drive
				: throw new InvalidOperationException();
	}
}