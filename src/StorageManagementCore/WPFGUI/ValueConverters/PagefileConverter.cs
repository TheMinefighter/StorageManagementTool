using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class PagefileConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (targetType != typeof(string)) {
				throw new ArgumentOutOfRangeException(nameof(targetType), "targetType must be typeof(string)");
			}

			if (value is Pagefile drive) {
				if (drive.Drive.LocalDrive.IsReady) {
					return string.Format("{0} ({1} ; {2}; {3}MB - {4}MB)", drive.Drive.LocalDrive.VolumeLabel,
						drive.Drive.LocalDrive.Name,
						OperatingMethods.DriveType2String(drive.Drive.LocalDrive.DriveType), drive.MinSize, drive.MaxSize);
				}
				else {
					return "";
				}
			}
			else {
				throw new ArgumentOutOfRangeException(nameof(value), "value must be a Pagefile");
			}
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			//TODO Add converter tests
			if (targetType != typeof(Pagefile)) {
				throw new ArgumentOutOfRangeException(nameof(targetType), "targetType must be typeof(Pagefile)");
			}
			else {
				if (value is string toConvert) {
					string[] partsA = toConvert.Split('(')[0].Split(';');
					string[] partsB = partsA[2].Split('-').Select(x => x.Split('M')[0]).ToArray();
					return new Pagefile(new ConfiguredDrive(new DriveInfo(partsA[0])), int.Parse(partsB[1]), int.Parse(partsB[0]));
				}
				else {
					throw new ArgumentOutOfRangeException(nameof(value), "value must be a string");
				}
			}
		}
	}
}