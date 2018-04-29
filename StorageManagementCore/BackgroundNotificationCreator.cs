using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ExtendedMessageBoxLibrary;

namespace StorageManagementCore
{
	/// <summary>
	///  Class containing functionalities for Background Process
	/// </summary>
	public static class BackgroundNotificationCreator
	{
		/// <summary>
		///  The JSON configuration
		/// </summary>
		private static JSONConfig _jsonConfig;

		/// <summary>
		///  The FileSystemWatchers currently active
		/// </summary>
		private static readonly List<FileSystemWatcher> FileSystemWatchers = new List<FileSystemWatcher>();

		/// <summary>
		///  Dictionary from FileSystemWatchers to MonitoredFolders
		/// </summary>
		private static readonly Dictionary<FileSystemWatcher, JSONConfig.MonitoringSetting.MonitoredFolder>
			FileSystemWatcher2MonitoredFolders
				= new Dictionary<FileSystemWatcher, JSONConfig.MonitoringSetting.MonitoredFolder>();

		/// <summary>
		///  Initalizes the Background Process
		/// </summary>
		public static void Initalize()
		{
			_jsonConfig = Session.Singleton.CurrentConfiguration;

			foreach (JSONConfig.MonitoringSetting.MonitoredFolder monitoredFolder in _jsonConfig.MonitoringSettings
				.MonitoredFolders)
			{
				if (monitoredFolder.ForFiles != JSONConfig.MonitoringSetting.MonitoringAction.Ignore)
				{
					FileSystemWatcher tempFileSystemWatcher = new FileSystemWatcher(monitoredFolder.TargetPath);
					tempFileSystemWatcher.Created += MonitoredFolderWatcher_FileCreated;
					tempFileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
					FileSystemWatchers.Add(tempFileSystemWatcher);
					FileSystemWatcher2MonitoredFolders.Add(tempFileSystemWatcher, monitoredFolder);
					tempFileSystemWatcher.EnableRaisingEvents = true;
				}

				if (monitoredFolder.ForFolders != JSONConfig.MonitoringSetting.MonitoringAction.Ignore)
				{
					FileSystemWatcher tempFileSystemWatcher = new FileSystemWatcher(monitoredFolder.TargetPath);
					tempFileSystemWatcher.Created += MonitoredFolderWatcher_FolderCreated;
					tempFileSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName;
					FileSystemWatchers.Add(tempFileSystemWatcher);
					FileSystemWatcher2MonitoredFolders.Add(tempFileSystemWatcher, monitoredFolder);
					tempFileSystemWatcher.EnableRaisingEvents = true;
				}
			}

			while (true)
			{
				Thread.Sleep(2000000000); //Needed to keep FileSystemWatchers active
			}
		}

//Multi Lang
		private static void MonitoredFolderWatcher_FolderCreated(object sender, FileSystemEventArgs e)
		{
			JSONConfig.MonitoringSetting.MonitoredFolder tmp = FileSystemWatcher2MonitoredFolders[(FileSystemWatcher) sender];
			switch (tmp.ForFolders)
			{
				case JSONConfig.MonitoringSetting.MonitoringAction.Ignore:
					break;
				case JSONConfig.MonitoringSetting.MonitoringAction.Ask:
					if (ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
						    new[]
						    {
							    "Es wurde der neue Ordner " + new DirectoryInfo(e.FullPath).Name + "  in ",
							    tmp.TargetPath,
							    " gefunden. Soll diese verschoben oder ignoriert werden"
						    }, "Neue Datei gefunden", new[]
						    {
							    "Ignorieren",
							    "Verschieben"
						    }
					    )).NumberOfClickedButton == 1)
					{
						OperatingMethods.MoveFolder(new DirectoryInfo(e.FullPath),
							new DirectoryInfo(_jsonConfig.DefaultHDDPath), true);
					}

					break;
				case JSONConfig.MonitoringSetting.MonitoringAction.Move:
					OperatingMethods.MoveFolder(new DirectoryInfo(e.FullPath),
						new DirectoryInfo(_jsonConfig.DefaultHDDPath), true);

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static void MonitoredFolderWatcher_FileCreated(object sender, FileSystemEventArgs e)
		{
			JSONConfig.MonitoringSetting.MonitoredFolder tmp = FileSystemWatcher2MonitoredFolders[(FileSystemWatcher) sender];
			switch (tmp.ForFiles)
			{
				case JSONConfig.MonitoringSetting.MonitoringAction.Ignore:
					break;
				case JSONConfig.MonitoringSetting.MonitoringAction.Ask:
					if (ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
						    new[]
						    {
							    $"Es wurde die neue Datei {new FileInfo(e.FullPath).Name}  in ",
							    tmp.TargetPath,
							    " erzeugt. Soll diese verschoben oder ignoriert werden"
						    }, "Neue Datei gefunden", new[]
						    {
							    "Ignorieren",
							    "Verschieben"
						    }
					    )).NumberOfClickedButton == 1)
					{
						OperatingMethods.MoveFolder(new DirectoryInfo(e.FullPath),
							new DirectoryInfo(_jsonConfig.DefaultHDDPath), true);
					}

					break;
				case JSONConfig.MonitoringSetting.MonitoringAction.Move:
					OperatingMethods.MoveFile(new FileInfo(e.FullPath),
						new FileInfo(Path.Combine(_jsonConfig.DefaultHDDPath, e.FullPath.Remove(1, 1)))
					);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}