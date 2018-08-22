using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtendedMessageBoxLibrary;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using StorageManagementCore.Backend;
using StorageManagementCore.GlobalizationRessources;
using StorageManagementCore.MainGUI.GlobalizationRessources;

namespace StorageManagementCore.Operation {
	public static class OperatingMethods {
		/// <summary>
		/// </summary>
		public enum QuestionAnswer {
			Yes,
			No,
			Ask
		}

		/// <summary>
		///  Key, where The Windows search data is stored
		/// </summary>
		public static readonly RegistryValue SearchDatatDirectoryRegistryValue = new RegistryValue(
			@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "DataDirectory");

		/// <summary>
		///  Creates a string representation of an DriveInfo
		/// </summary>
		/// <param name="item">The DriveInfo object to represent</param>
		/// <returns>The string representation</returns>
		public static string GetDriveInfoDescription(DriveInfo item) => item.IsReady
			? $"{item.VolumeLabel} ({item.Name} ; {DriveType2String(item.DriveType)})"
			: item.Name;

		/// <summary>
		///  Moves a Directory to another Loaction using symlinks
		/// </summary>
		/// <param name="toMove">The Directory to move</param>
		/// <param name="newLocation">The Directory to move the file to</param>
		/// <param name="adjustNewPath"></param>
		/// <returns>Whether the operation were successful</returns>
		public static bool MoveFolderPhysically(DirectoryInfo toMove, DirectoryInfo newLocation, bool adjustNewPath = false) {
			if (toMove == newLocation) {
				if (MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.MoveFolderOrFile_PathsEqual,
					    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry) {
					MoveFolderPhysically(toMove, newLocation, adjustNewPath);
				}
			}

			if (adjustNewPath) {
				newLocation = new DirectoryInfo(Path.Combine(newLocation.FullName, toMove.FullName.Remove(1, 1)));
			}

			if (newLocation.Parent != null && !newLocation.Parent.Exists) {
				newLocation.Parent.Create();
			}

			if (toMove.Exists) {
				if (!FileAndFolder.MoveDirectory(toMove, newLocation)) {
					return false;
				}
			}

			return FileAndFolder.CreateFolderSymlink(toMove, newLocation);
		}

		public static bool MoveFilePhysically(FileInfo file, DirectoryInfo newLocation) =>
			MoveFilePhysically(file,
				new FileInfo(Path.Combine(newLocation.FullName, file.FullName.Remove(1, 1))));

		/// <summary>
		///  Moves a file to another Location using symlinks
		/// </summary>
		/// <param name="toMove">The file to move</param>
		/// <param name="newLocation">The location to move the file to</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool MoveFilePhysically(FileInfo toMove, FileInfo newLocation) {
			if (toMove == newLocation) {
				if (
					MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.MoveFolderOrFile_PathsEqual,
						MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry) {
					MoveFilePhysically(toMove, newLocation);
				}
				else {
					return false;
				}
			}

			if (newLocation.Directory != null && !newLocation.Directory.Exists) {
				newLocation.Directory.Create();
			}

			if (toMove.Exists) {
				if (!FileAndFolder.MoveFile(toMove, newLocation)) {
					return false;
				}
			}

			return FileAndFolder.CreateFileSymlink(toMove, newLocation);
		}

		/// <summary>
		///  Recommends Paths to move to another drive
		/// </summary>
		/// <returns>The recommended Paths</returns>
		public static IEnumerable<string> GetRecommendedPaths() {
			List<string> ret = new List<string>();

			IEnumerable<string> blacklist = new string[] {
				Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData"),
				Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local\Microsoft"),
				Environment.ExpandEnvironmentVariables(@"%temp%"),
				Environment.ExpandEnvironmentVariables(@"%tmp%")
			};
			string[] currentsubfolders =
				Directory.GetDirectories(Environment.ExpandEnvironmentVariables(@"%userprofile%"));
			for (int i = 0; i < currentsubfolders.GetLength(0); i++) {
				if (!FileAndFolder.IsPathSymbolic(currentsubfolders[i]) &&
				    !blacklist.Contains(currentsubfolders[i])) {
					ret.Add(currentsubfolders[i]);
				}
			}

			currentsubfolders =
				Directory.GetDirectories(Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local"));
			for (int i = 0; i < currentsubfolders.GetLength(0); i++) {
				if (!FileAndFolder.IsPathSymbolic(currentsubfolders[i]) &&
				    !blacklist.Contains(currentsubfolders[i])) {
					ret.Add(currentsubfolders[i]);
				}
			}

			return ret.ToArray();
		}

		/// <summary>
		///  Gets names of DriveTypes
		/// </summary>
		/// <param name="toName">The DriveType Object, which name should be returned</param>
		/// <returns>The  name of the DriveType Object</returns>
		public static string DriveType2String(DriveType toName) {
			switch (toName) {
				case DriveType.CDRom: return OperatingMethodsStrings.DriveType2String_CDRom;
				case DriveType.Fixed: return OperatingMethodsStrings.DriveType2String_Fixed;
				case DriveType.Network: return OperatingMethodsStrings.DriveType2String_Network;
				case DriveType.Ram: return OperatingMethodsStrings.DriveType2String_RAM;
				case DriveType.Removable: return OperatingMethodsStrings.DriveType2String_Removable;
				case DriveType.NoRootDirectory: return OperatingMethodsStrings.DriveType2String_NoRootDirectory;
				default: return OperatingMethodsStrings.DriveType2String_Unknown;
			}
		}

		/// <summary>
		///  Enables Send to HDD
		/// </summary>
		/// <param name="enable">Whether to enable or disable Send to HDD</param>
		public static void EnableSendToHDD(bool enable = true) {
			if (enable) {
				#region Based upon https://stackoverflow.com/a/4909475/6730162 access on 5.11.2017 

				FileAndFolder.CreateShortcut(" -move -auto-detect -SrcPath", new FileInfo(Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
						OperatingMethodsStrings.StoreOnHDDLinkName + ".lnk")),
					"Lagert den Speicherort der gegebenen Datei aus");

				#endregion
			}
			else {
				File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
					OperatingMethodsStrings.StoreOnHDDLinkName + ".lnk"));
			}
		}

		/// <summary>
		///  Sets the Search data path
		/// </summary>
		/// <param name="newPath">The new path for the search data</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool SetSearchDataPath(DirectoryInfo newPath) {
			if (newPath.Exists) {
				if (RegistryMethods.SetRegistryValue(SearchDatatDirectoryRegistryValue,
					newPath.CreateSubdirectory("Search").CreateSubdirectory("Data").FullName,
					RegistryValueKind.String,
					true)) {
					if (!Session.Singleton.IsAdmin) {
						if (MessageBox.Show(
							    EditWindowsSearchSettingsStrings.SetSearchDataPath_RestartNoAdmin,
							    OperatingMethodsStrings.SetSearchDataPath_RestartNow_Title, MessageBoxButtons.YesNo,
							    MessageBoxIcon.Question) ==
						    DialogResult.Yes) {
							Wrapper.RestartComputer();
						}
					}

					return true;
				}

				return false;
			}

			MessageBox.Show(OperatingMethodsStrings.SetSearchDataPath_InvalidPath, OperatingMethodsStrings.Error,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
			return false;
		}

		/// <summary>
		///  Enables/Disables availability of hibernate
		/// </summary>
		/// <param name="enable">Whether hibernate should be enabled (true) or disabled (false) </param>
		public static void SetHibernate(bool enable) {
			Wrapper.ExecuteExecuteable(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "powercfg.exe"),
				$"/h {(enable ? "on" : "off")}",
				true, true,
				true);
		}

		/// <summary>
		///  gets the DrivEInfo onject from its description genarated using GetDriveInfoDescription()
		/// </summary>
		/// <param name="driveInfo">The DriveInfo described</param>
		/// <param name="description">The description of the DriveInfo</param>
		/// <returns>Whether the described DriveInfo were found</returns>
		public static bool GetDriveInfoFromDescription(out DriveInfo driveInfo, string description) {
			driveInfo = FileSystem.Drives.FirstOrDefault(x => GetDriveInfoDescription(x) == description);
			return driveInfo != null;
		}

		/// <summary>
		///  Checks if the send to feature is enabled
		/// </summary>
		/// <returns>Whether the send to feature is enabled</returns>
		public static bool IsSendToHDDEnabled() => File.Exists(Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
			OperatingMethodsStrings.StoreOnHDDLinkName + ".lnk"));

		/// <summary>
		///  Reads the path of the windows search data
		/// </summary>
		/// <param name="directory"> The directory containing the Windows search data</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool GetSearchDataPath(out DirectoryInfo directory) {
			directory = null;

			if (!RegistryMethods.GetRegistryValue(SearchDatatDirectoryRegistryValue, out object text,
				true)) {
				return false;
			}

			//Registry value also contains the \Search\Data which should probably not be removed due to the fact that the Windows Editor isn´t allowing that too
			DirectoryInfo dir = new DirectoryInfo((string) text);
			if (dir.Parent?.Parent == null) {
				return false;
			}

			directory = dir.Parent.Parent;
			return true;
		}

		//
		//      public static void CheckForSysinternals() {
		//         if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "PsTools", "PSEXEC.exe"))) {
		//            string zipName = Path.Combine(Directory.GetCurrentDirectory(), "PsTools.zip");
		//            using (WebClient tmpClient = new WebClient()) {
		//               try {
		//                  tmpClient.DownloadFile("https://download.sysinternals.com/files/PSTools.zip", zipName);
		//               }
		//               catch (Exception) {
		//                  Console.WriteLine();
		//               }
		//            }
		//
		//            ZipFile.ExtractToDirectory(zipName, Path.Combine(Directory.GetCurrentDirectory(), "PsTools"));
		//            FileAndFolder.DeleteFile(new FileInfo(zipName));
		//         }
		//      }
		/// <summary>
		///  Refreshes the current Stadium of the Swapfile Movement
		/// </summary>
		/// <summary>
		///  Fills an given Listbox with information about the available Drives
		/// </summary>
		/// <param name="toFill"></param>
		public static void FillWithDriveInfo(ListBox toFill) {
			toFill.Items.Clear();
			foreach (DriveInfo item in (IEnumerable<DriveInfo>) FileSystem.Drives) {
				toFill.Items.Add(GetDriveInfoDescription(item));
			}
		}

		/// <summary>
		///  Moves an User ShellFolder to a new Location
		/// </summary>
		/// <param name="oldDir">The old Directory of</param>
		/// <param name="newDir">The new Directory of the new </param>
		/// <param name="toChange">The UserShellFolder to edit</param>
		/// <param name="copyContents"></param>
		/// <param name="deleteOldContents"></param>
		/// <returns>Whether the Operation were successful</returns>
		public static bool ChangeShellFolderChecked(DirectoryInfo oldDir, DirectoryInfo newDir, ShellFolder toChange,
			QuestionAnswer copyContents = QuestionAnswer.Ask,
			QuestionAnswer deleteOldContents = QuestionAnswer.Ask) {
			if (!newDir.Exists) {
				newDir.Create();
			}

			DirectoryInfo currentPath = toChange.GetPath();
			Dictionary<ShellFolder, DirectoryInfo> childs = ShellFolder.AllShellFolders
				.Select(x => new KeyValuePair<ShellFolder, DirectoryInfo>(x, x.GetPath()))
				.Where(x => Wrapper.IsSubfolder(x.Value, currentPath)).ToDictionary();
			bool moveAll = false;

			foreach (KeyValuePair<ShellFolder, DirectoryInfo> child in childs) {
				//Add strings
				bool move = false;
				if (!moveAll) {
					//No;Yes;YesAll
					ExtendedMessageBoxResult result = ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
						string.Format(OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_Text,
							child.Key.LocalizedName),
						OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_Title,
						childs.Count == 1
							? new[] {
								OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_Yes,
								OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_No
							}
							: new[] {
								OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_YesAll,
								OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_Yes,
								OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_No
							}, 0));
					if (result.NumberOfClickedButton == 2) {
						moveAll = true;
					}
					else {
						move = result.NumberOfClickedButton == 1;
					}
				}

				if (moveAll || move) {
					string newPathOfChild = Path.Combine(newDir.FullName,
						child.Value.FullName.Substring(currentPath.FullName.Length));
					bool retry;
					bool skip = false;
					do {
						retry = false;
						if (child.Key.SetPath(new DirectoryInfo(newPathOfChild))) {
							switch (ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
								string.Format(OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Text,
									child.Key.LocalizedName, toChange.LocalizedName, newPathOfChild), OperatingMethodsStrings.Error,
								new[] {
									OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Retry,
									string.Format(OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Skip,
										child.Key.LocalizedName),
									OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Ignore,
									OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Abort
								}, 0)).NumberOfClickedButton) {
								case 0:
									retry = true;
									break;
								case 1:
									skip = true;
									break;
								case 2: break;
								case 3: return false;
							}
						}
					} while (retry);

					if (skip) {
						break;
					}
				}
			}

			if (toChange.SetPath(newDir)) {
				if (newDir.Exists && oldDir.Exists &&
				    (copyContents == QuestionAnswer.Yes ||
				     copyContents == QuestionAnswer.Ask &&
				     MessageBox.Show(
					     OperatingMethodsStrings.ChangeUserShellFolder_MoveContent_Text,
					     OperatingMethodsStrings.ChangeUserShellFolder_MoveContent_Title, MessageBoxButtons.YesNo,
					     MessageBoxIcon.Asterisk,
					     MessageBoxDefaultButton.Button1) ==
				     DialogResult.Yes)) {
					if (!oldDir.Exists || oldDir.GetFileSystemInfos().Length == 0 ||
					    FileAndFolder.MoveDirectory(oldDir, newDir)) {
						string defaultDirectory = toChange.DefaultValue;
						if (defaultDirectory == null) {
							MessageBox.Show(UserShellFolderStrings.Error);
							return false;
						}

						DirectoryInfo defaultDirectoryInfo =
							new DirectoryInfo(Environment.ExpandEnvironmentVariables(defaultDirectory));
						if (defaultDirectoryInfo.FullName != oldDir.FullName) {
							if (defaultDirectoryInfo.Exists) {
								FileAndFolder.DeleteDirectory(defaultDirectoryInfo, true, false);
							}

							Wrapper.ExecuteCommand($"mklink /D \"{defaultDirectoryInfo.FullName}\\\" \"{newDir.FullName}\"",
								true, true);
						}

						if (oldDir.Exists) {
							FileAndFolder.DeleteDirectory(oldDir, true, false);
						}

						Wrapper.ExecuteCommand($"mklink /D \"{oldDir.FullName}\\\" \"{newDir.FullName}\"", true, true);
					}
				}

				return true;
			}

			return false;
		}
	}
}