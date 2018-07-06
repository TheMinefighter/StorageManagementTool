using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using Microsoft.VisualBasic.FileIO;
using StorageManagementCore.Configuration;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class PagefileConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			targetType == typeof(string) && value is Pagefile drive
				? (drive.Drive.LocalDrive.IsReady
					? string.Format("{0} ({1} ; {2}; {3}MB - {4}MB)", drive.Drive.LocalDrive.VolumeLabel, drive.Drive.LocalDrive.Name,
						Operation.OperatingMethods.DriveType2String(drive.Drive.LocalDrive.DriveType), drive.MinSize, drive.MaxSize)
					: null
				)
				: throw new InvalidOperationException();


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			//TODO Add converter tests
			if (targetType == typeof(Pagefile) && value is string toConvert) {
				string[] partsA = toConvert.Split('(')[0].Split(';');
				string[] partsB = partsA[2].Split('-').Select(x=>x.Split('M')[0]).ToArray();
				return new Pagefile(new ConfiguredDrive(new DriveInfo(partsA[0])),int.Parse(partsB[0]),int.Parse(partsB[1]) );
			}
			else {
				throw new InvalidOperationException();
			}
		}
	}
}