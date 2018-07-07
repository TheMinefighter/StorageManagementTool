using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageManagementCore.WPFGUI.DataProviders
{
    class LanguageProvider : INotifyPropertyChanged
    {
        private static readonly IReadOnlyCollection<CultureInfo> InnerValue =
	        new CultureInfo[] { null }.Concat(Program.AvailableSpecificCultures.SelectMany(x => x)).ToArray();//ToArray to prevent useless OnDemand Evaluation

        public IReadOnlyCollection<CultureInfo> AvailableLanguages => InnerValue;
		 //Nothing more than declaration, because it can't change during runtime
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
