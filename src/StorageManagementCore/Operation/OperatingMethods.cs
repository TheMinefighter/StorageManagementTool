using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using StorageManagementCore.Backend;
using StorageManagementCore.GlobalizationRessources;
using StorageManagementCore.MainGUI.GlobalizationRessources;
using File = System.IO.File;

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
		public static bool MoveFolder(DirectoryInfo toMove, DirectoryInfo newLocation, bool adjustNewPath = false) {
			if (toMove == newLocation) {
				if (MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.MoveFolderOrFile_PathsEqual,
					    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry) {
					MoveFolder(toMove, newLocation, adjustNewPath);
				}
			}

			if (adjustNewPath) {
				newLocation = new DirectoryInfo(Path.Combine(newLocation.FullName, toMove.FullName.Remove(1, 1)));
			}

			DirectorySecurity currentControl = toMove.GetAccessControl();
			if (newLocation.Parent != null && !newLocation.Parent.Exists) {
				newLocation.Parent.Create();
			}

			if (toMove.Exists) {
				if (!FileAndFolder.MoveDirectory(toMove, newLocation)) {
					return false;
				}
			}

			CalculateACLInheritance(currentControl);

			newLocation.SetAccessControl(currentControl);
			return FileAndFolder.CreateFolderSymlink(toMove, newLocation);
		}

		private static void CalculateACLInheritance(FileSystemSecurity currentControl) {
			IEnumerable<FileSystemAccessRule> accessRules = currentControl.GetAccessRules(true, true, typeof(SecurityIdentifier))
				.OfType<FileSystemAccessRule>();
			foreach (FileSystemAccessRule currentRule in accessRules) {
				if (currentRule.IsInherited) {
					currentControl.RemoveAccessRule(currentRule);
					FileSystemRights newRights;

					switch (currentRule.FileSystemRights) {
						//Needed for weird (probably legacy), undocumented aliases
						case (FileSystemRights) 0x10000000:
							newRights = FileSystemRights.FullControl;
							break;
						case (FileSystemRights) (-0x1FFF0000):
							newRights = FileSystemRights.Modify;
							break;
						case (FileSystemRights) (-1610612736):
							newRights =
								(FileSystemRights) 0x000201bf; //( FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.ListDirectory)

							break;
						default:
							newRights = currentRule.FileSystemRights;
							break;
					}

					currentControl.AddAccessRule(new FileSystemAccessRule(currentRule.IdentityReference,
						newRights, currentRule.InheritanceFlags, currentRule.PropagationFlags,
						currentRule.AccessControlType));
				}
			}
		}

		public static bool MoveFile(FileInfo file, DirectoryInfo newLocation) =>
			MoveFile(file,
				new FileInfo(Path.Combine(newLocation.FullName, file.FullName.Remove(1, 1))));

		/// <summary>
		///  Moves a file to another Location using symlinks
		/// </summary>
		/// <param name="toMove">The file to move</param>
		/// <param name="newLocation">The location to move the file to</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool MoveFile(FileInfo toMove, FileInfo newLocation) {
			if (toMove == newLocation) {
				if (
					MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.MoveFolderOrFile_PathsEqual,
						MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry) {
					MoveFile(toMove, newLocation);
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
			if (
				!FileAndFolder.IsPathSymbolic(Environment.ExpandEnvironmentVariables(@"%AppData%"))) {
				ret.Add(Environment.ExpandEnvironmentVariables(@"%AppData%"));
			}

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

				WshShell shell = new WshShell();
				string shortcutAddress = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
					OperatingMethodsStrings.StoreOnHDDLinkName + ".lnk");
				IWshShortcut shortcut = (IWshShortcut) shell.CreateShortcut(shortcutAddress);
				shortcut.Description = "Lagert den Speicherort der gegebenen Datei aus";
				shortcut.TargetPath = Process.GetCurrentProcess().MainModule.FileName;
				shortcut.Arguments = " -move -auto-detect -SrcPath";
				shortcut.Save();

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
	}
}