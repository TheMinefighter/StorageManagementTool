using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class ShellFolderFilterConverter : IMultiValueConverter {
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
#if DEBUG
			if (values.Length != 2) {throw new ArgumentException("values must contain exact two elements",nameof(values));}

			if (values[0] is IReadOnlyCollection<ShellFolder> shellFolders) {
				if (values[1] is bool allowAllShellFolders) {
					return shellFolders.Where(x => allowAllShellFolders || (!x.Undefined && x.ShouldBeEdited));
				}
				else {
					throw new ArgumentException("values[1] must be a bool", nameof(values));
				}
			}
			else {
				throw new ArgumentException("values[0] must be a IReadOnlyCollection<ShellFolder>", nameof(values));
			}
#else
			bool allowAllShellFolders = (bool) values[1];
			return ((IReadOnlyCollection<ShellFolder>) values[0]).Where(x => allowAllShellFolders || (!x.Undefined && x.ShouldBeEdited));
#endif
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
			throw new InvalidOperationException();
	}
}