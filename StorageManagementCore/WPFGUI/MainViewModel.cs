using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using StorageManagementCore.Configuration;

namespace StorageManagementCore.WPFGUI {
	public class MainViewModel :INotifyPropertyChanged {
		private Pagefile _selectedPagefile;

		public Pagefile SelectedPagefile {
			get => _selectedPagefile;
			set {
				if (!ReferenceEquals(value, _selectedPagefile)) {
					_selectedPagefile = value; 
					OnPropertyChanged(nameof(SelectedPagefile));
				}

				}
		}

		public PagefileSysConfiguration PagefileConfiguration { get; set; }

		public MainViewModel() {
			PagefileConfiguration= Operation.PagefileManagement.GetCurrentPagefileConfiguration(out PagefileSysConfiguration tmp)?tmp: new PagefileSysConfiguration();
			SelectedPagefile = null;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName]
			string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}