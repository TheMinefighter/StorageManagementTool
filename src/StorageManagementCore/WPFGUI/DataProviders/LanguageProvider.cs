using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace StorageManagementCore.WPFGUI.DataProviders {
	internal class LanguageProvider : INotifyPropertyChanged {
		private static readonly IReadOnlyCollection<object> InnerValue =
			new object[] {DBNull.Value}.Concat(Program.AvailableSpecificCultures.SelectMany(x => x))
				.ToArray(); //ToArray to prevent useless OnDemand Evaluation

		public IReadOnlyCollection<object> AvailableLanguages => InnerValue;

		//Nothing more than declaration, because it can't change during runtime
		public event PropertyChangedEventHandler PropertyChanged;
	}
}