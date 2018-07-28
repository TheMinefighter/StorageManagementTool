using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class ShellFolderLocalizedNameConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
#if DEBUG
			if (targetType != typeof(string)) {
				throw new ArgumentException("targetType must be typeof(string)", nameof(targetType));
			}

			if (!(value is ShellFolder)) {
				throw new ArgumentException("value must be a ShellFolder", nameof(value));
			}
#endif
			return ((ShellFolder) value).LocalizedName;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
#if DEBUG
			if (targetType != typeof(ShellFolder)) {
				throw new ArgumentException("targetType must be typeof(ShellFolder)", nameof(targetType));
			}

			if (!(value is string)) {
				throw new ArgumentException("value must be  a string", nameof(value));
			}
#endif
			string toConvert = (string) value;
			return ShellFolder.AllShellFolders.First(x => x.LocalizedName == toConvert);
		}
	}
}