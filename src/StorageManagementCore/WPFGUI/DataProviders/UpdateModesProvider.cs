using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using StorageManagementCore.Configuration;

namespace StorageManagementCore.WPFGUI.DataProviders {
	public class UpdateModesProvider : INotifyPropertyChanged {
		public ReadOnlyCollection<UpdateMode> UpdateModes { get; } =
			Enum.GetValues(typeof(UpdateMode)).Cast<UpdateMode>().ToList().AsReadOnly();

		public UpdateModesProvider() => Session.LanguageChanged += (a, b) => OnPropertyChanged(nameof(UpdateModes));
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}