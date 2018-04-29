using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using StorageManagementCore.GlobalizationRessources;
using StorageManagementCore.MainGUI.GlobalizationRessources;
using File = System.IO.File;

namespace StorageManagementCore
{
	public static partial class OperatingMethods
	{
		/// <summary>
		/// </summary>
		public enum QuestionAnswer
		{
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
		public static string GetDriveInfoDescription(DriveInfo item)
		{
			return item.IsReady
				? $"{item.VolumeLabel} ({item.Name} ; {DriveType2String(item.DriveType)})"
				: item.Name;
		}

		/// <summary>
		///  Moves a Directory to another Loaction using symlinks
		/// </summary>
		/// <param name="dir">The Directory to move</param>
		/// <param name="newLocation">The Directory to move the file to</param>
		/// <param name="adjustNewPath"></param>
		/// <returns>Whether the operation were successful</returns>
		public static bool MoveFolder(DirectoryInfo dir, DirectoryInfo newLocation, bool adjustNewPath = false)
		{
			if (dir == newLocation)
			{
				if (MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.MoveFolderOrFile_PathsEqual,
					    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
				{
					MoveFolder(dir, newLocation, adjustNewPath);
				}
			}

			if (adjustNewPath)
			{
				newLocation = new DirectoryInfo(Path.Combine(newLocation.FullName, dir.FullName.Remove(1, 1)));
			}

			if (newLocation.Parent != null && !newLocation.Parent.Exists)
			{
				newLocation.Parent.Create();
			}

			if (dir.Exists)
			{
				if (!Wrapper.FileAndFolder.MoveDirectory(dir, newLocation))
				{
					return false;
				}
			}

			return Wrapper.FileAndFolder.CreateFolderSymlink(dir, newLocation);
		}

		/// <summary>
		///  Moves a file to anothrrer Loaction using symlinks
		/// </summary>
		/// <param name="file">The file to move</param>
		/// <param name="newLocation">The location to move the file to</param>
		/// <param name="adjustNewPath"></param>
		/// <returns>Whether the operation were successful</returns>
		public static bool MoveFile(FileInfo file, FileInfo newLocation, bool adjustNewPath = false)
		{
			if (file == newLocation)
			{
				if (
					MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.MoveFolderOrFile_PathsEqual,
						MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
				{
					MoveFile(file, newLocation);
				}
				else
				{
					return false;
				}
			}

			if (adjustNewPath)
			{
				newLocation = new FileInfo(Path.Combine(newLocation.FullName, file.FullName.Remove(1, 1)));
			}

			if (!newLocation.Directory.Exists)
			{
				newLocation.Directory.Create();
			}

			if (file.Exists)
			{
				if (!Wrapper.FileAndFolder.MoveFile(file, newLocation))
				{
					return false;
				}
			}

			return Wrapper.FileAndFolder.CreateFileSymlink(file, newLocation);
			//throw new NotImplementedException();
		}

		/// <summary>
		///  Recommends Paths to move to another drive
		/// </summary>
		/// <returns>The recommended Paths</returns>
		public static IEnumerable<string> GetRecommendedPaths()
		{
			List<string> ret = new List<string>();
			if (
				!Wrapper.FileAndFolder.IsPathSymbolic(Environment.ExpandEnvironmentVariables(@"%AppData%")))
			{
				ret.Add(Environment.ExpandEnvironmentVariables(@"%AppData%"));
			}

			IEnumerable<string> blacklist = new string[]
			{
				Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData"),
				Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local\Microsoft"),
				Environment.ExpandEnvironmentVariables(@"%temp%"),
				Environment.ExpandEnvironmentVariables(@"%tmp%")
			};
			string[] currentsubfolders =
				Directory.GetDirectories(Environment.ExpandEnvironmentVariables(@"%userprofile%"));
			for (int i = 0; i < currentsubfolders.GetLength(0); i++)
			{
				if (!Wrapper.FileAndFolder.IsPathSymbolic(currentsubfolders[i]) &&
				    !blacklist.Contains(currentsubfolders[i]))
				{
					ret.Add(currentsubfolders[i]);
				}
			}

			currentsubfolders =
				Directory.GetDirectories(Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local"));
			for (int i = 0; i < currentsubfolders.GetLength(0); i++)
			{
				if (!Wrapper.FileAndFolder.IsPathSymbolic(currentsubfolders[i]) &&
				    !blacklist.Contains(currentsubfolders[i]))
				{
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
		public static string DriveType2String(DriveType toName)
		{
			switch (toName)
			{
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
		///  Changes the systems pagefile settings
		/// </summary>
		/// <param name="currentSelection">The selected partition entry</param>
		/// <param name="maxSize">The maximum Size of the Pagefile in MB</param>
		/// <param name="minSize">The minimum Size of the Pagefile in MB</param>
		/// <returns>Whether the Operation were successfull</returns>
		public static bool ChangePagefileSettings(string currentSelection, int maxSize, int minSize)
		{
			List<string> tempDriveInfoList = FileSystem.Drives.Select(GetDriveInfoDescription).ToList();
			int selectedPartitionIndex;
			if (tempDriveInfoList.Contains(currentSelection)) //Tests whether the selected partition is available
			{
				selectedPartitionIndex = tempDriveInfoList.IndexOf(currentSelection);
			}
			else
			{
				MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_SelectedPartitionMissing,
					OperatingMethodsStrings.Error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			DriveInfo toUse = FileSystem.Drives[selectedPartitionIndex];
			return ChangePagefileSettings(toUse, maxSize, minSize);
		}

		/// <summary>
		///  Changes the Pagefile Settings
		/// </summary>
		/// <param name="toUse">The drive to move to</param>
		/// <param name="maxSize">The max size of pagefile in MB</param>
		/// <param name="minSize">The min size of the pagefile in MB</param>
		/// <returns></returns>
		public static bool ChangePagefileSettings(DriveInfo toUse, int maxSize, int minSize)
		{
			string wmicPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "wbem\\wmic.exe");
			if (maxSize < minSize) //Tests whether the maxSize is smaller than the minSize
			{
				MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_MinGreaterMax,
					OperatingMethodsStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (toUse.AvailableFreeSpace < minSize * 1048576L) //Tests whether enough space is available
			{
				MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_NotEnoughSpace,
					OperatingMethodsStrings.Error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (Wrapper.ExecuteExecuteable(
				wmicPath, "computersystem get AutomaticManagedPagefile /Value"
				, out string[] tmp, out int _, out int _, true, true, true, true, false)) //Tests
			{
				if (bool.Parse(tmp[2].Split('=')[1]))
				{
					Wrapper.ExecuteCommand(
						wmicPath
						+ Environment.ExpandEnvironmentVariables(
							" computersystem where \"name='%computername%' \" set AutomaticManagedPagefile=False")
						, true, true, out _); //Disables automatic Pagefile  management
					Wrapper.ExecuteExecuteable(
						wmicPath
						, "computersystem get AutomaticManagedPagefile /Value"
						, out tmp, out int _, out _, waitforexit: true, hidden: true, admin: true);
					if (!bool.Parse(tmp[2].Split('=')[1]))
					{
						MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_CouldntDisableManagement,
							OperatingMethodsStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
				}
			}

			Wrapper.ExecuteExecuteable(wmicPath,
				"pagefileset delete /NOINTERACTIVE", out _, out int _, out _, waitforexit: true, hidden: true,
				admin: true); //Deletes all Pagefiles

			Wrapper.ExecuteExecuteable(wmicPath,
				$"pagefileset create name=\"{Path.Combine(toUse.Name, "Pagefile.sys")}\"", out _,
				out int _, out _, waitforexit: true, hidden: true, admin: true); //Creates new Pagefile

			Wrapper.ExecuteExecuteable(wmicPath,
				$"pagefileset where name=\"{Path.Combine(toUse.Name, "Pagefile.sys")}\" set InitialSize={minSize},MaximumSize={maxSize}",
				out _, out int _, out _, waitforexit: true, hidden: true, admin: true);
			// Sets Pagefile Size

			Wrapper.ExecuteExecuteable(wmicPath,
				" get", out tmp, out int _, out int _, true, true,
				true, true); //Checks wether there is exactly 1 pagefile existing
			if (tmp.Length != 2)
			{
				switch (MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_Not1Pagefile,
					OperatingMethodsStrings.Error,
					MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
				{
					case DialogResult.Cancel: return false;
					case DialogResult.Retry: return ChangePagefileSettings(toUse, maxSize, minSize);
				}
			}

			return true;
		}

		/// <summary>
		///  Enables Send to HDD
		/// </summary>
		/// <param name="enable">Whether to enable or disable Send to HDD</param>
		public static void EnableSendToHDD(bool enable = true)
		{
			if (enable)
			{
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
			else
			{
				File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
					OperatingMethodsStrings.StoreOnHDDLinkName + ".lnk"));
			}
		}

		/// <summary>
		///  Sets the Search data path
		/// </summary>
		/// <param name="newPath">The new path for the search data</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool SetSearchDataPath(DirectoryInfo newPath)
		{
			if (newPath.Exists)
			{
				if (Wrapper.RegistryMethods.SetRegistryValue(SearchDatatDirectoryRegistryValue,
					newPath.CreateSubdirectory("Search").CreateSubdirectory("Data").FullName,
					RegistryValueKind.String,
					true))
				{
					if (!Session.Singleton.IsAdmin)
					{
						if (MessageBox.Show(
							    EditWindowsSearchSettingsStrings.SetSearchDataPath_RestartNoAdmin,
							    OperatingMethodsStrings.SetSearchDataPath_RestartNow_Title, MessageBoxButtons.YesNo,
							    MessageBoxIcon.Question) ==
						    DialogResult.Yes)
						{
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
		public static void SetHibernate(bool enable)
		{
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
		public static bool GetDriveInfoFromDescription(out DriveInfo driveInfo, string description)
		{
			driveInfo = Wrapper.getDrives().FirstOrDefault(x => GetDriveInfoDescription(x) == description);
			return driveInfo != null;
		}

		/// <summary>
		///  Checks if the send to feature is enabled
		/// </summary>
		/// <returns>Whether the send to feature is enabled</returns>
		public static bool IsSendToHDDEnabled()
		{
			return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
				OperatingMethodsStrings.StoreOnHDDLinkName + ".lnk"));
		}

		/// <summary>
		///  Reads the path of the windows search data
		/// </summary>
		/// <param name="directory"> The directory containing the Windows search data</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool GetSearchDataPath(out DirectoryInfo directory)
		{
			directory = null;

			if (!Wrapper.RegistryMethods.GetRegistryValue(SearchDatatDirectoryRegistryValue, out object text,
				true))
			{
				return false;
			}

			//Registry value also contains the \Search\Data which should probably not be removed due to the fact that the Windows Editor isn´t allowing that too
			DirectoryInfo dir = new DirectoryInfo((string) text);
			if (dir.Parent?.Parent == null)
			{
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
		//            Wrapper.FileAndFolder.DeleteFile(new FileInfo(zipName));
		//         }
		//      }
	}
}