using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	public class MainViewModel : INotifyPropertyChanged {
		private Pagefile _selectedPagefile;
		private MonitoredFolder _selectedMonitoredFolder;
		private PagefileSysConfiguration _proposedPagefileConfiguration;

		public Pagefile SelectedPagefile {
			get => _selectedPagefile;
			set {
				if (!ReferenceEquals(value, _selectedPagefile)) {
					_selectedPagefile = value;
					OnPropertyChanged();
				}
			}
		}

		public MonitoredFolder SelectedMonitoredFolder {
			get => _selectedMonitoredFolder;
			set {
				if (_selectedMonitoredFolder != value) {
					_selectedMonitoredFolder = value;
					OnPropertyChanged();
				}
			}
		}

		public PagefileSysConfiguration ProposedPagefileConfiguration {
			get => _proposedPagefileConfiguration;
			set {
				if (_proposedPagefileConfiguration != value) {
					_proposedPagefileConfiguration = value;
					OnPropertyChanged();
				}
			}
		}

		public MainViewModel() {
			ProposedPagefileConfiguration = PagefileManagement.GetCurrentPagefileConfiguration(out PagefileSysConfiguration tmp)
				? tmp
				: new PagefileSysConfiguration();
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