using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Contains information about a folder, which should be monitored
	/// </summary>
	public class MonitoredFolder : ICloneable, INotifyPropertyChanged {
		private MonitoringAction _forDirectories;
		private MonitoringAction _forFiles;
		private string _targetPath;

		/// <summary>
		///  The MonitoringAction to execute for new folders
		/// </summary>
		public MonitoringAction ForDirectories {
			get => _forDirectories;
			set {
				if (_forDirectories != value) {
					_forDirectories = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		///  The MonitoringAction to execute for new files
		/// </summary>
		public MonitoringAction ForFiles {
			get => _forFiles;
			set {
				if (_forFiles != value) {
					_forFiles = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		///  The target path of this MonitoredFolder
		/// </summary>
		public string TargetPath {
			get => _targetPath;
			set {
				if (_targetPath != value) {
					_targetPath = value;
					OnPropertyChanged();
				}
			}
		}

		public MonitoredFolder() { }

		/// <summary>
		///  Creates a new MonitoredFolder object with a given target targetPath
		/// </summary>
		/// <param name="targetPath">The target targetPath</param>
		public MonitoredFolder(string targetPath) {
			TargetPath = targetPath;
			ForFiles = MonitoringAction.Ask;
			ForDirectories = MonitoringAction.Ask;
		}

		public object Clone() =>
			new MonitoredFolder {ForDirectories = ForDirectories, ForFiles = ForFiles, TargetPath = TargetPath};

		public event PropertyChangedEventHandler PropertyChanged;


		public override string ToString() => TargetPath;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName]
			string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}