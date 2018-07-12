using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class DriveInfoConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (targetType == typeof(string)) {
				if (value is DriveInfo drive) {
					return OperatingMethods.GetDriveInfoDescription(drive);
				}
				else {
					return "Some Error just occurred if u see that";
				}
			}
			else {
				throw new InvalidOperationException();
			}
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			targetType == typeof(DriveInfo) && value is string toConvert &&
			OperatingMethods.GetDriveInfoFromDescription(out DriveInfo drive, toConvert)
				? drive
				: throw new InvalidOperationException();
	}
}