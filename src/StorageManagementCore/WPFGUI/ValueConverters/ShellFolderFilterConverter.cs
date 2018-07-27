using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Windows.Data;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class ShellFolderFilterConverter : IMultiValueConverter {
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
#if DEBUG
			if (targetType != typeof(IEnumerable)) {
				throw new ArgumentException("targetType must be typeof(IEnumerable)", nameof(targetType));
			}
			if (values.Length != 2) {throw new ArgumentException("values must contain exactly two elements",nameof(values));}

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
			return ((IReadOnlyCollection<ShellFolder>) values[0]).Where(x => allowAllShellFolders || (x.Defined && x.ShouldBeEdited));
#endif
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
#if DEBUG
			if (targetTypes.Length!=2) {
				throw new ArgumentException("targetTypes must contain exactly two elements", nameof(targetTypes));
			}

			if (targetTypes[0]!=typeof(IReadOnlyCollection<ShellFolder>)) {
				throw  new ArgumentException("The first targeted type must be an IReadOnlyCollection<ShellFolder>", nameof(targetTypes));
			}

			if (targetTypes[1]!=typeof(bool)) {
				throw  new ArgumentException("The second targeted type must be a bool", nameof(targetTypes));
			}

			if (!(value is IEnumerable<ShellFolder>)) {
				throw  new ArgumentException("The value must be an IEnumerable<ShellFolder>", nameof(value));
			}
#endif
			return new object[]
				{ShellFolder.AllShellFolders, ShellFolder.AllShellFolders.Count == ((IEnumerable<ShellFolder>) value).Count()};
		}
	}
}