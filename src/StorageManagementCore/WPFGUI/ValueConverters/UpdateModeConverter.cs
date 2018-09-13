using System;
using System.Globalization;
using System.Windows.Data;
using StorageManagementCore.Configuration;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class UpdateModeConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
#if DEBUG
			if (targetType != typeof(string)) {
				throw new ArgumentException("targetType must be typeof(string)", nameof(targetType));
			}

			if (!(value is UpdateMode)) {
				throw new ArgumentException("value must be a UpdateMode", nameof(value));
			}
#endif
			return UpdateConfiguration.UpdateModeByLocalizedName[(UpdateMode) value];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
#if DEBUG
			if (targetType != typeof(UpdateMode)) {
				throw new ArgumentException("targetType must be typeof(UpdateMode)", nameof(targetType));
			}

			if (!(value is string)) {
				throw new ArgumentException("value must be  a string", nameof(value));
			}
#endif
			return UpdateConfiguration.UpdateModeByLocalizedName[(string) value];
		}
	}
}