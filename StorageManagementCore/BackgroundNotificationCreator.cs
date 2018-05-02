using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ExtendedMessageBoxLibrary;
using StorageManagementCore.Configuration;

namespace StorageManagementCore {
	/// <summary>
	///  Class containing functionalities for Background Process
	/// </summary>
	public static class BackgroundNotificationCreator {
		/// <summary>
		///  The JSON configuration
		/// </summary>
		private static MainConfiguration _mainConfiguration;

		/// <summary>
		///  The FileSystemWatchers currently active
		/// </summary>
		private static readonly List<FileSystemWatcher> FileSystemWatchers = new List<FileSystemWatcher>();

		/// <summary>
		///  Dictionary from FileSystemWatchers to MonitoredFolders
		/// </summary>
		private static readonly Dictionary<FileSystemWatcher, MonitoredFolder>
			FileSystemWatcher2MonitoredFolders
				= new Dictionary<FileSystemWatcher, MonitoredFolder>();

		/// <summary>
		///  Initalizes the Background Process
		/// </summary>
		public static void Initalize() {
			_mainConfiguration = Session.Singleton.CurrentConfiguration;

			foreach (MonitoredFolder monitoredFolder in _mainConfiguration.MonitoringSettings
				.MonitoredFolders) {
				if (monitoredFolder.ForFiles != MonitoringAction.Ignore) {
					FileSystemWatcher tempFileSystemWatcher = new FileSystemWatcher(monitoredFolder.TargetPath);
					tempFileSystemWatcher.Created += MonitoredFolderWatcher_FileCreated;
					tempFileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
					FileSystemWatchers.Add(tempFileSystemWatcher);
					FileSystemWatcher2MonitoredFolders.Add(tempFileSystemWatcher, monitoredFolder);
					tempFileSystemWatcher.EnableRaisingEvents = true;
				}

				if (monitoredFolder.ForFolders != MonitoringAction.Ignore) {
					FileSystemWatcher tempFileSystemWatcher = new FileSystemWatcher(monitoredFolder.TargetPath);
					tempFileSystemWatcher.Created += MonitoredFolderWatcher_FolderCreated;
					tempFileSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName;
					FileSystemWatchers.Add(tempFileSystemWatcher);
					FileSystemWatcher2MonitoredFolders.Add(tempFileSystemWatcher, monitoredFolder);
					tempFileSystemWatcher.EnableRaisingEvents = true;
				}
			}

			while (true) {
				Thread.Sleep(2000000000); //Needed to keep FileSystemWatchers active; I know thats dirty
			}
		}

//Multi Lang
		private static void MonitoredFolderWatcher_FolderCreated(object sender, FileSystemEventArgs e) {
			MonitoredFolder tmp = FileSystemWatcher2MonitoredFolders[(FileSystemWatcher) sender];
			switch (tmp.ForFolders) {
				case MonitoringAction.Ignore:
					break;
				case MonitoringAction.Ask:
					if (ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
						    new[] {
							    "Es wurde der neue Ordner " + new DirectoryInfo(e.FullPath).Name + "  in ",
							    tmp.TargetPath,
							    " gefunden. Soll diese verschoben oder ignoriert werden"
						    }, "Neue Datei gefunden", new[] {
							    "Ignorieren",
							    "Verschieben"
						    }
					    )).NumberOfClickedButton == 1) {
						OperatingMethods.MoveFolder(new DirectoryInfo(e.FullPath),
							new DirectoryInfo(_mainConfiguration.DefaultHDDPath), true);
					}

					break;
				case MonitoringAction.Move:
					OperatingMethods.MoveFolder(new DirectoryInfo(e.FullPath),
						new DirectoryInfo(_mainConfiguration.DefaultHDDPath), true);

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static void MonitoredFolderWatcher_FileCreated(object sender, FileSystemEventArgs e) {
			MonitoredFolder tmp = FileSystemWatcher2MonitoredFolders[(FileSystemWatcher) sender];
			switch (tmp.ForFiles) {
				case MonitoringAction.Ignore:
					break;
				case MonitoringAction.Ask:
					if (ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
						    new[] {
							    $"Es wurde die neue Datei {new FileInfo(e.FullPath).Name}  in ",
							    tmp.TargetPath,
							    " erzeugt. Soll diese verschoben oder ignoriert werden"
						    }, "Neue Datei gefunden", new[] {
							    "Ignorieren",
							    "Verschieben"
						    }
					    )).NumberOfClickedButton == 1) {
						OperatingMethods.MoveFolder(new DirectoryInfo(e.FullPath),
							new DirectoryInfo(_mainConfiguration.DefaultHDDPath), true);
					}

					break;
				case MonitoringAction.Move:
					OperatingMethods.MoveFile(new FileInfo(e.FullPath),
						new FileInfo(Path.Combine(_mainConfiguration.DefaultHDDPath, e.FullPath.Remove(1, 1)))
					);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}