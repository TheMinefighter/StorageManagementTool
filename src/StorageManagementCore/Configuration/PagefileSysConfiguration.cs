using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	public class PagefileSysConfiguration : INotifyPropertyChanged {
		private bool _systemManaged;

		public bool SystemManaged {
			get => _systemManaged;
			set {
				if (value != _systemManaged) {
					_systemManaged = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(Manual));
				}
			}
		}


		public bool Manual => !SystemManaged;

		[NotNull]
		[ItemNotNull]
		public ObservableCollection<Pagefile> Pagefiles { get; }

		public PagefileSysConfiguration() => Pagefiles = new ObservableCollection<Pagefile>();
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName]
			string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}