using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	/// <summary>
	///  The Main ViewModelModel containing all further GUI Data
	/// </summary>
	public class MainViewModel : INotifyPropertyChanged {
		private PagefileSysConfiguration _proposedPagefileConfiguration;
		private MonitoredFolder _selectedMonitoredFolder;
		private Pagefile _selectedPagefile;

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
			LoadPagefiles();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void LoadPagefiles() {
			ProposedPagefileConfiguration = PagefileManagement.GetCurrentPagefileConfiguration(out PagefileSysConfiguration tmp)
				? tmp
				: new PagefileSysConfiguration();
			SelectedPagefile = null;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}