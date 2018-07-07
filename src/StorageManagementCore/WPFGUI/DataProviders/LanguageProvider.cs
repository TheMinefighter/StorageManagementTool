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
        private static readonly IReadOnlyCollection<object> InnerValue =
	        new Object[] { DBNull.Value }.Concat(Program.AvailableSpecificCultures.SelectMany(x => x)).ToArray();//ToArray to prevent useless OnDemand Evaluation

        public IReadOnlyCollection<object> AvailableLanguages => InnerValue;
		 //Nothing more than declaration, because it can't change during runtime
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
