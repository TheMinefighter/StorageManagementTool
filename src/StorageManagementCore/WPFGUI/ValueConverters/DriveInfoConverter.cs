using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class DriveInfoConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (targetType != typeof(string)) {
				throw new ArgumentException("targetType must be typeof(string)", nameof(targetType));
			}
			if (value is DriveInfo drive) {
				return OperatingMethods.GetDriveInfoDescription(drive);
			}
			else {
				return "Some Error just occurred if u see that";
			}
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			if (targetType != typeof(DriveInfo)) {
				throw new ArgumentException("targetType must be typeof(DriveInfo)", nameof(targetType));
			}
			if (value is string toConvert) {
				if (OperatingMethods.GetDriveInfoFromDescription(out DriveInfo drive, toConvert)) {
					return drive;
				}
				else {
					throw new ArgumentOutOfRangeException(nameof(value), "the value supplied could not be converted to a DriveInfo");
				}
			}
			else {
				throw new ArgumentException("value must be  a string", nameof(value));
			}
		}
	}
}