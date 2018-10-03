using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ExtendedMessageBoxLibrary;
using StorageManagementCore.Configuration;
using static StorageManagementCore.GlobalizationRessources.BackgroundNotificationStrings;

namespace StorageManagementCore.Operation {
	/// <summary>
	///  Class containing functionalities for Background Process
	/// </summary>
	public static class BackgroundNotificationCreator {
		/// <summary>
		///  Dictionary from FileSystemWatchers to SelectedMonitoredFolder
		/// </summary>
		private static readonly Dictionary<FileSystemWatcher, MonitoredFolder>
			FileSystemWatcher2MonitoredFolders
				= new Dictionary<FileSystemWatcher, MonitoredFolder>();

		/// <summary>
		///  Initalizes the Background Process
		/// </summary>
		public static void Initalize() {
			foreach (MonitoredFolder monitoredFolder in Session.Singleton.Configuration.MonitoringSettings
				.MonitoredFolders) {
				if (monitoredFolder.ForFiles != MonitoringAction.Ignore) {
					FileSystemWatcher tempFileSystemWatcher = new FileSystemWatcher(monitoredFolder.TargetPath);
					tempFileSystemWatcher.Created += MonitoredFolderWatcher_FileCreated;
					tempFileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
					FileSystemWatcher2MonitoredFolders.Add(tempFileSystemWatcher, monitoredFolder);
					tempFileSystemWatcher.EnableRaisingEvents = true;
				}

				if (monitoredFolder.ForDirectories != MonitoringAction.Ignore) {
					FileSystemWatcher tempFileSystemWatcher = new FileSystemWatcher(monitoredFolder.TargetPath);
					tempFileSystemWatcher.Created += MonitoredFolderWatcher_FolderCreated;
					tempFileSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName;
					FileSystemWatcher2MonitoredFolders.Add(tempFileSystemWatcher, monitoredFolder);
					tempFileSystemWatcher.EnableRaisingEvents = true;
				}
			}

			Thread.Sleep(Timeout.Infinite);
		}


		private static void MonitoredFolderWatcher_FolderCreated(object sender, FileSystemEventArgs e) {
			MonitoredFolder tmp = FileSystemWatcher2MonitoredFolders[(FileSystemWatcher) sender];
			switch (tmp.ForDirectories) {
				case MonitoringAction.Ignore:
					break;
				case MonitoringAction.Ask:
					if (ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
						    string.Format(NewFolderFoundText,
							    new DirectoryInfo(e.FullPath).Name, tmp.TargetPath)
						    , NewFolderFoundTitle, new[] {
							    IgnoreText,
							    MoveText
						    }
					    )).NumberOfClickedButton == 1) {
						OperatingMethods.MoveFolderPhysically(new DirectoryInfo(e.FullPath),
							new DirectoryInfo(Session.Singleton.Configuration.DefaultHDDPath), true);
					}

					break;
				case MonitoringAction.Move:
					OperatingMethods.MoveFolderPhysically(new DirectoryInfo(e.FullPath),
						new DirectoryInfo(Session.Singleton.Configuration.DefaultHDDPath), true);

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
						    string.Format(NewFileFoundText,
							    new FileInfo(e.FullPath).Name, tmp.TargetPath)
						    , NewFileFoundTitle, new[] {
							    IgnoreText,
							    MoveText
						    }
					    )).NumberOfClickedButton == 1) {
						OperatingMethods.MoveFolderPhysically(new DirectoryInfo(e.FullPath),
							new DirectoryInfo(Session.Singleton.Configuration.DefaultHDDPath), true);
					}

					break;
				case MonitoringAction.Move:
					OperatingMethods.MoveFilePhysically(new FileInfo(e.FullPath),
						new FileInfo(Path.Combine(Session.Singleton.Configuration.DefaultHDDPath, e.FullPath.Remove(1, 1)))
					);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}