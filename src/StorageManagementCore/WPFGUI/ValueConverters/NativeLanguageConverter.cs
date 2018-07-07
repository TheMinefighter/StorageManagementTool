using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using static StorageManagementCore.WPFGUI.GlobalizationRessources.SettingsStrings;
namespace StorageManagementCore.WPFGUI.ValueConverters
{
    public class NativeLanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
	        if (targetType == typeof(string))
	        {
		        switch (value)
		        {
			        case CultureInfo cultureInfo:
				        return cultureInfo.NativeName;
			        case null:
				        return SystemLanguageText;
			        default:
				        throw new InvalidOperationException();
		        }
	        }
	        else
	        {
		        throw new InvalidOperationException();
	        }
        }


	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            targetType == typeof(CultureInfo) && value is string toConvert ?(toConvert==SystemLanguageText?null: CultureInfo.GetCultures(CultureTypes.AllCultures).First(x => x.NativeName == toConvert)) :
                throw new InvalidOperationException();
    }
}