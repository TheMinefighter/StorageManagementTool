using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI.DataProviders {
	public class SuggestionProvider : INotifyPropertyChanged {
		private static readonly FileSystemWatcher localappdata =
			new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) {
				EnableRaisingEvents = true
			};

		private static readonly FileSystemWatcher user =
			new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) {
				EnableRaisingEvents = true
			};

		public IEnumerable<string> Suggestions => OperatingMethods.GetRecommendedPaths();

		public SuggestionProvider() {
			localappdata.Created += (sender, args) => OnPropertyChanged(nameof(Suggestions));
			localappdata.Deleted += (sender, args) => OnPropertyChanged(nameof(Suggestions));
			localappdata.Renamed += (sender, args) => OnPropertyChanged(nameof(Suggestions));
			user.Created += (sender, args) => OnPropertyChanged(nameof(Suggestions));
			user.Deleted += (sender, args) => OnPropertyChanged(nameof(Suggestions));
			user.Renamed += (sender, args) => OnPropertyChanged(nameof(Suggestions));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}