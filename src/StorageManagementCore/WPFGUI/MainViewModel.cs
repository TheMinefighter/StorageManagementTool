using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	/// <summary>
	///  The Main ViewModelModel containing all further GUI Data
	/// </summary>
	public class MainViewModel : INotifyPropertyChanged {
		private ShellFolder _selectedShellfolder;
		private PagefileSysConfiguration _proposedPagefileConfiguration;
		private MonitoredFolder _selectedMonitoredFolder;
		private Pagefile _selectedPagefile;
		private string _proposedShellfolderPath = "";

		public string ProgramVersion => Program.VersionTag;
		public bool UpdateAvailable => Directory.Exists(Path.Combine(
			Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
			UpdateInstaller.Update.UpdateDataDirectory));

		public UpdateMode UpdateModeSelected {
			get => Session.Singleton.Configuration.UpdateSettings.Mode;
			set {
				Session.Singleton.Configuration.UpdateSettings.Mode = value;
				Session.Singleton.SaveCfg();
			}
		}

		public ShellFolder SelectedShellfolder {
			get => _selectedShellfolder;
			set {
				_selectedShellfolder = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CurrentShellfolderPath));
				OnPropertyChanged(nameof(CanPathBeApplied));
			}
		}


		public string CurrentShellfolderPath {
			get {
				if (SelectedShellfolder is null) {
					return "";
				}

				DirectoryInfo path = SelectedShellfolder.GetPath();
				return path != null ? path.FullName : "Seems as if your PC hasn´t configured that path for whatever reason";
			}
		}

		public string ProposedShellfolderPath {
			get => _proposedShellfolderPath;
			set {
				_proposedShellfolderPath = value;
				OnPropertyChanged(nameof(CanPathBeApplied));
            OnPropertyChanged();
			}
		}

		public bool CanPathBeApplied => SelectedShellfolder != null && Directory.Exists(ProposedShellfolderPath);

		public bool UsePrereleases {
			get => Session.Singleton.Configuration.UpdateSettings.UsePrereleases;
			set {
				Session.Singleton.Configuration.UpdateSettings.UsePrereleases = value;
				Session.Singleton.SaveCfg();
			}
		}

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