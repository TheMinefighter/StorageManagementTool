using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using StorageManagementCore.Backend;

namespace StorageManagementCore.WPFGUI.DataProviders {
	public class ShellFolderProvider :INotifyPropertyChanged {
		public ReadOnlyCollection<ShellFolder> KnownShellFolders => ShellFolder.AllShellFolders;
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}