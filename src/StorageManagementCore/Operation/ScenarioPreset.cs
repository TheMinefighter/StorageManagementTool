using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.Devices;
using StorageManagementCore.Backend;
using StorageManagementCore.GlobalizationResources;

namespace StorageManagementCore.Operation {
	/// <summary>
	///  Stores the presets for the scenarios
	/// </summary>
	public struct ScenarioPreset {
		public static ScenarioPreset[] AvailablePresets;

		public override string ToString() => Id;

		/// <summary>
		///  Whether a HDD is required for this preset
		/// </summary>
		public bool HDDRequired;

		public string Id;

		/// <summary>
		///  The name of the preset
		/// </summary>
		public string ViewedName;

		/// <summary>
		///  Whether a SSD is required for this preset
		/// </summary>
		public bool SSDRequired;

		/// <summary>
		///  the action to run
		/// </summary>
		public Action<DriveInfo, DriveInfo> ToRun;

		private static void LocalSSDAndNAS(DriveInfo ssd, DriveInfo hdd) {
			Dictionary<ShellFolder, string> usfToMove = new Dictionary<ShellFolder, string> {
				{ShellFolder.KnownShellFolders.Documents, "Documents"},
				{ShellFolder.KnownShellFolders.Music, "Music"},
				{ShellFolder.KnownShellFolders.Pictures, "Pictures"},
				{ShellFolder.KnownShellFolders.RoamingAppData, "AppData\\Roaming"},
				{ShellFolder.KnownShellFolders.Downloads, "Downloads"},
				{ShellFolder.KnownShellFolders.Desktop, "Desktop"}
			};
			bool empty = false;
			int i = 0;
			do {
				if (Directory.Exists(Path.Combine(hdd.RootDirectory.FullName, $"SSD{i}"))) {
					empty = true;
				}

				i++;
			} while (!empty);

			DirectoryInfo baseDir = hdd.RootDirectory.CreateSubdirectory($"SSD{i}");

			Session.Singleton.Configuration.DefaultHDDPath = baseDir.FullName;
			Session.Singleton.SaveCfg();
			DirectoryInfo userDir = baseDir.CreateSubdirectory(Environment.UserName);
			foreach (KeyValuePair<ShellFolder, string> currentPair in usfToMove) {
				ShellFolder moving = currentPair.Key;
				OperatingMethods.ChangeShellFolderChecked(moving.GetPath(), userDir.CreateSubdirectory(currentPair.Value),
					moving,
					OperatingMethods.QuestionAnswer.Yes, OperatingMethods.QuestionAnswer.Yes);
			}

			Dictionary<ShellFolder, string> csfToMove = new Dictionary<ShellFolder, string> {
				{ShellFolder.KnownShellFolders.ProgramFilesX86, "Program Files (x86)"},
				{ShellFolder.KnownShellFolders.PublicDesktop, "Common Desktop"}
			};
			DirectoryInfo commonDir = baseDir.CreateSubdirectory("Common Data");
			foreach (KeyValuePair<ShellFolder, string> currentPair in csfToMove) {
				ShellFolder moving = currentPair.Key;
				OperatingMethods.ChangeShellFolderChecked(moving.GetPath(), commonDir.CreateSubdirectory(currentPair.Value),
					moving,
					OperatingMethods.QuestionAnswer.Yes, OperatingMethods.QuestionAnswer.Yes);
			}

			ShellFolder.KnownShellFolders.ProgramFiles.SetPath(commonDir
				.GetDirectories("Program Files (x86", SearchOption.TopDirectoryOnly).First());
			int memory = (int) (new ComputerInfo().TotalPhysicalMemory / 1048576L);
			PagefileManagement.ChangePagefileSettings(hdd, memory, memory * 2);
			OperatingMethods.EnableSendToHDD();
			OperatingMethods.SetHibernate(false);
			OperatingMethods.SetSearchDataPath(baseDir.CreateSubdirectory("WindowsSearchData"));
		}

		private static void LocalSSDAndHDD(DriveInfo ssd, DriveInfo hdd) {
			Dictionary<ShellFolder, string> usfToMove = new Dictionary<ShellFolder, string> {
				{ShellFolder.KnownShellFolders.Documents, "Documents"},
				{ShellFolder.KnownShellFolders.Music, "Music"},
				{ShellFolder.KnownShellFolders.Pictures, "Pictures"},
				{ShellFolder.KnownShellFolders.RoamingAppData, "AppData\\Roaming"},
				{ShellFolder.KnownShellFolders.Downloads, "Downloads"},
				{ShellFolder.KnownShellFolders.Desktop, "Desktop"}
			};
			bool empty = false;
			int i = 0;
			do {
				if (Directory.Exists(Path.Combine(hdd.RootDirectory.FullName, $"SSD{i}"))) {
					empty = true;
				}

				i++;
			} while (!empty);

			DirectoryInfo baseDir = hdd.RootDirectory.CreateSubdirectory($"SSD{i}");
			Session.Singleton.Configuration.DefaultHDDPath = baseDir.FullName;
			Session.Singleton.SaveCfg();
			DirectoryInfo userDir = baseDir.CreateSubdirectory(Environment.UserName);
			foreach (KeyValuePair<ShellFolder, string> currentPair in usfToMove) {
				ShellFolder moving = currentPair.Key;
				OperatingMethods.ChangeShellFolderChecked(moving.GetPath(), userDir.CreateSubdirectory(currentPair.Value),
					moving,
					OperatingMethods.QuestionAnswer.Yes, OperatingMethods.QuestionAnswer.Yes);
			}

			int memory = (int) (new ComputerInfo().TotalPhysicalMemory / 1048576L);
			PagefileManagement.ChangePagefileSettings(hdd, memory, memory * 2);
			OperatingMethods.SetHibernate(false);
			OperatingMethods.EnableSendToHDD();
			OperatingMethods.SetSearchDataPath(baseDir.CreateSubdirectory("WindowsSearchData"));
		}

		/// <summary>
		///  Loads all configured presets
		/// </summary>
		public static void LoadPresets() {
			AvailablePresets = new[] {
				new ScenarioPreset {
					HDDRequired = true,
					ViewedName = ScenarioPresetStrings.Presets_LocalHDDAndSSD,
					ToRun = LocalSSDAndHDD,
					Id = "LocalSSDAndHDD"
				},
				new ScenarioPreset {
					HDDRequired = true,
					SSDRequired = true,
					ViewedName = ScenarioPresetStrings.Presets_LocalSSDAndNAS,
					ToRun = LocalSSDAndNAS,
					Id = "LocalSSDAndNAS"
				}
			};
		}
	}
}