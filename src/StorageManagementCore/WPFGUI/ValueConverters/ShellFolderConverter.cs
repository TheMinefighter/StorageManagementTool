using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class ShellFolderLocalizedNameConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (targetType != typeof(string)) {
				throw new ArgumentException("targetType must be typeof(string)", nameof(targetType));
			}
			if (value is ShellFolder folder) {
				return folder.LocalizedName;
			}
			else {
				throw new ArgumentException("value must be a ShellFolder", nameof(value));
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			if (targetType != typeof(ShellFolder)) {
				throw new ArgumentException("targetType must be typeof(ShellFolder)", nameof(targetType));
			}
			else {
				if (value is string toConvert) {
					return  ShellFolder.AllShellFolders.First(x => x.LocalizedName == toConvert);
				}
				else {
					throw new ArgumentException("value must be a string", nameof(value));
				}
			}
		}
	}
}