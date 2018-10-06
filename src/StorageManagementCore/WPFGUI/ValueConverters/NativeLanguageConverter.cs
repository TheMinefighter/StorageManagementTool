using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using static StorageManagementCore.WPFGUI.GlobalizationResources.SettingsStrings;

namespace StorageManagementCore.WPFGUI.ValueConverters {
	public class NativeLanguageConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
#if DEBUG
			if (targetType != typeof(string)) {
				throw new ArgumentException("targetType must be typeof(string)", nameof(targetType));
			}
#endif
			switch (value) {
				case CultureInfo cultureInfo:
					return cultureInfo.NativeName;
				case DBNull _:
					return SystemLanguageText;
				default:
					throw new ArgumentException("value must be  a DBNull or a CultureInfo", nameof(value));
			}
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
#if DEBUG
			if (targetType != typeof(object)) {
				throw new ArgumentException("targetType must be typeof(object)", nameof(targetType));
			}

			if (!(value is string)) {
				throw new ArgumentException("value must be  a string", nameof(value));
			}
#endif
			string toConvert = (string) value;

			if (toConvert == SystemLanguageText) {
				return DBNull.Value;
			}
			else {
				return CultureInfo.GetCultures(CultureTypes.AllCultures).First(x => x.NativeName == toConvert);
			}
		}
	}
}