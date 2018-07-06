using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.VisualBasic.FileIO;

namespace StorageManagementCore.WPFGUI.DataProviders {
	/// <summary>
	/// Just redirects <see cref="FileSystem"/>.<see cref="Drives"/> but implementing <see cref="System.ComponentModel.INotifyPropertyChanged"/>
	/// </summary>
	public class DriveInfoProvider : INotifyPropertyChanged {
		private static readonly ManagementEventWatcher EventWatcher;

		static DriveInfoProvider() {
			EventWatcher = new ManagementEventWatcher("SELECT * FROM Win32_VolumeChangeEvent");
			EventWatcher.Start();
		}
		public DriveInfoProvider() {
			EventWatcher.EventArrived += (o, args) => OnPropertyChanged(nameof(Drives));
		}
		/// <summary>
		/// The redirected <see cref="DriveInfo"/>s but with <see cref="INotifyPropertyChanged"/>
		/// </summary>
		public ReadOnlyCollection<DriveInfo> Drives => FileSystem.Drives;
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName]
			string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}