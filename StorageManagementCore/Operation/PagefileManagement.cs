using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Csv;
using JetBrains.Annotations;
using Microsoft.VisualBasic.FileIO;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.GlobalizationRessources;

namespace StorageManagementCore.Operation {
	/// <summary>
	/// A class providing methods for managing pagefiles
	/// </summary>
	public static class PagefileManagement {
		/// <summary>
		/// The number of bytes in a Megabyte
		/// </summary>
		private const long BytesInMegabyte = 1048576L;

		/// <summary>
		/// The path of the wmic.exe
		/// </summary>
		private static readonly string WmicPath =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "wbem", "wmic.exe");

		//Note: longs will only work until 16.000PB of free storage, but in that time Windows will hopefully have all functionalities of this program
// Will Use DriveInfos here in the future but currently it is impossible for dictionaries: https://github.com/dotnet/corefx/issues/30627
		/// <summary>
		/// The free space the drives will have when the pagefiles have been removed
		/// </summary>
		/// <param name="current"> The <see cref="PagefileSysConfiguration"/> containing the pagefiles which should be calculated</param>
		/// <returns><see langword="null"/> if check failed, otherwise each drive with its free space after the removal of the listed pagefiles </returns>
		[CanBeNull]
		public static Dictionary<char, long> GetFutureFreeSpace(PagefileSysConfiguration current) {
			Dictionary<char, long> ret = FileSystem.Drives.ToDictionary(x => x.GetDriveLetter(), x => x.TotalFreeSpace);
			if (current.SystemManaged) {
				string rootPath = Path.GetPathRoot(Environment.SystemDirectory);

				FileInfo pagefile = new FileInfo(Path.Combine(rootPath, "pagefile.sys"));
				long length;
				try {
					length = pagefile.Length;
				}
				catch (Exception e) {
					return null;
				}

				ret[rootPath.First()] += length;
				return ret;
			}
			else {
				foreach (Pagefile currentPagefile in current.Pagefiles) {
					ret[currentPagefile.Drive.LocalDrive.GetDriveLetter()] += currentPagefile.MinSize * BytesInMegabyte;
				}

				return ret;
			}
		}

		/// <summary>
		/// Checks if PagefileConfiguration will fit on the drives
		/// </summary>
		/// <param name="current">The current <see cref="PagefileSysConfiguration"/>,
		/// which can be obtained by <see cref="GetCurrentPagefileConfiguration"/></param>
		/// <param name="proposed">The proposed <see cref="PagefileSysConfiguration"/></param>
		/// <returns><see langword="null"/> if check failed, an empty <see cref="List{T}"/> if pagefiles will fit on each drive,
		/// otherwise a <see cref="List{T}"/> with the drives where the future configuration would exceed the limits </returns>
		[CanBeNull]
		public static List<DriveInfo> DoesPagefileCfgFit(PagefileSysConfiguration current, PagefileSysConfiguration proposed) {
			if (proposed.SystemManaged) {
				return new List<DriveInfo>();
			}
			else {
				List<DriveInfo> ret = new List<DriveInfo>();
				Dictionary<char, long> futureFreeSpace = GetFutureFreeSpace(current);
				if (futureFreeSpace == null) {
					return null;
				}

				foreach (Pagefile proposedPagefile in proposed.Pagefiles) {
					char drive = proposedPagefile.Drive.LocalDrive.GetDriveLetter();
					if (futureFreeSpace[drive] - (proposedPagefile.MinSize * BytesInMegabyte) < 0) {
						ret.Add(new DriveInfo(drive.ToString()));
					}
				}

				return ret;
			}
		}

		/// <summary>
		/// Applies a given <see cref="PagefileSysConfiguration"/>
		/// </summary>
		/// <param name="proposed">The <see cref="PagefileSysConfiguration"/> to apply</param>
		/// <returns>Whether the Operation were successful</returns>
		public static bool ApplyConfiguration(PagefileSysConfiguration proposed) {
			if (!GetCurrentPagefileConfiguration(out PagefileSysConfiguration current)) {
				//TODO Error message, recursive
			}

			List<DriveInfo> errors = DoesPagefileCfgFit(current, proposed);
			if (errors==null) {
				//TODO Error message
			}

			if (errors.Count>0) {
				//TODO Error message
			}

			if (proposed.SystemManaged) {
				return DeleteAllPagefiles() && SetSystemManaged(true);
			}

			if (proposed.Pagefiles.Count==0) {
				return DeleteAllPagefiles();
			}

			if (current.SystemManaged&&!SetSystemManaged(false)) {
				return false;
			}

			List<Pagefile> changingPagefiles = current.Pagefiles.ToList();
			foreach (Pagefile currentPagefile in current.Pagefiles) {
				if (!proposed.Pagefiles.Select(x=>x.Drive).Contains(currentPagefile.Drive)) {
					if (DeletePagefile(currentPagefile.Drive.LocalDrive)) {
						changingPagefiles.Remove(currentPagefile);
					}
					else {
						return false;
					}
				}
			}
			
			foreach (Pagefile proposedPagefile in proposed.Pagefiles) {
				if (!current.Pagefiles.Select(x=>x.Drive).Contains(proposedPagefile.Drive)) {
					if (AddPagefile(proposedPagefile)) {
						changingPagefiles.Add(proposedPagefile);
					}
					else {
						return false;
					}
				}
			}

			foreach (Pagefile proposedPagefile in proposed.Pagefiles) {
				if (!changingPagefiles.Contains(proposedPagefile)) {
					if (!ChangePagefile(proposedPagefile)) {
						return false;
					}
				}
			}

			if (!GetCurrentPagefileConfiguration(out PagefileSysConfiguration newOne)) {
				return false;
			}
			if (newOne.Pagefiles.UnorderedEqual(proposed.Pagefiles)) {
				//TODO Pagefiles inequal error
				return false;
			}

			if (newOne.SystemManaged!=proposed.SystemManaged) {
				//TODO SystemManaged inequal error
				return false;
			}
			
			return true;
		}

		/// <summary>
		///  Reads the current <exception cref="PagefileSysConfiguration"></exception>
		/// </summary>
		/// <param name="cfg">The current <see cref="PagefileSysConfiguration" /></param>
		/// <returns>Whether the operation were successful</returns>
		public static bool GetCurrentPagefileConfiguration(out PagefileSysConfiguration cfg) {
			cfg = new PagefileSysConfiguration();
			if (!Wrapper.ExecuteExecuteable(
				WmicPath, "pagefileset list /FORMAT:CSV"
				, out string[] tmp, out int _, out int _, true, true, true, true)) {
				return false;
			}

			foreach (ICsvLine line in CsvReader.Read(new StringReader(string.Join(Environment.NewLine, tmp)))) {
				cfg.Pagefiles.Add(new Pagefile(new ConfiguredDrive(new DriveInfo(line["Name"])), int.Parse(line["MaximumSize"]),
					int.Parse(line["InitialSize"])));
			}

			if (!GetSystemManaged(out bool val)) {
				return false;
			}

			cfg.SystemManaged = val;
			return true;
		}

		/// <summary>
		///  Deletes all pagefiles
		/// </summary>
		/// <returns>Whether the operation were successful</returns>
		private static bool DeleteAllPagefiles() => Wrapper.ExecuteExecuteable(WmicPath,
			"pagefileset delete /NOINTERACTIVE", out _, out int _, out _, waitforexit: true, hidden: true,
			admin: true);

		/// <summary>
		/// Sets wether pagefiles are system managed
		/// </summary>
		/// <param name="systemManaged">whether pagefiles should be system managed in the future</param>
		/// <returns>Whether the operation were successful</returns>
		private static bool SetSystemManaged(bool systemManaged) =>
			Wrapper.ExecuteExecuteable(WmicPath, $" computersystem set AutomaticManagedPagefile={systemManaged}", true, true);

		/// <summary>
		/// Adds a pagefile to a specified drive
		/// </summary>
		/// <param name="drive">The <see cref="DriveInfo"/> to add the pagefile to</param>
		/// <returns>Whether the Operation were successful</returns>
		public static bool AddPagefile(DriveInfo drive) =>
			Wrapper.ExecuteExecuteable(WmicPath,
				$"pagefileset create name=\"{Path.Combine(drive.Name, "Pagefile.sys")}\"", out _,
				out int _, out _, waitforexit: true, hidden: true, admin: true); //Creates new Pagefile;

		/// <summary>
		/// Adds a pagefile with specified settings to a specified drive
		/// </summary>
		/// <param name="cfg">The <see cref="Pagefile"/> to add</param>
		/// <returns>Whether the Operation were successful</returns>
		public static bool AddPagefile(Pagefile cfg) => AddPagefile(cfg.Drive.LocalDrive) && ChangePagefile(cfg);

		/// <summary>
		/// Changed a specified pagefile, which allready exists
		/// </summary>
		/// <param name="cfg">The configuration to apply to the pagefile</param>
		/// <returns>Whether the Operation were successful</returns>
		private static bool ChangePagefile(Pagefile cfg) =>
			Wrapper.ExecuteExecuteable(WmicPath,
				$"pagefileset where where \" name=\'{cfg.Drive.LocalDrive.Name}\\pagefile.sys\' \" set InitialSize={cfg.MinSize},MaximumSize={cfg.MaxSize}",
				out _, out int _, out _, waitforexit: true, hidden: true, admin: true);

		/// <summary>
		/// Deletes the pagefile on one specified drive
		/// </summary>
		/// <param name="drive">The drive to delete the pagefile from</param>
		/// <returns>Whether the Operation were successful</returns>
		private static bool DeletePagefile(DriveInfo drive) =>
			Wrapper.ExecuteExecuteable(WmicPath,
				$"pagefileset where \" name=\'{drive.Name}\\pagefile.sys\' \" DELETE /NOINTERACTIVE");

// Maybe I will write some WMIC GET API in the future...
		/// <summary>
		///  Checks wether pagefiles are currently system-manged
		/// </summary>
		/// <param name="systemManaged">Whether pagefiles are system managed</param>
		/// <returns>Whether the operation where successfull</returns>
		private static bool GetSystemManaged(out bool systemManaged) {
			systemManaged = false;
			if (!Wrapper.ExecuteExecuteable(
				WmicPath, "computersystem get AutomaticManagedPagefile /Value"
				, out string[] tmp, out int _, out int _, true, true, true, true)) {
				return false;
			}

			string data = string.Join("", tmp);

			string wmicKey = "AutomaticManagedPagefile=";
			int occurence = data.IndexOf(wmicKey, /*required because culture specific otherwise*/ StringComparison.Ordinal);
			if (occurence == -1) {
				return false;
			}

			if (!bool.TryParse(string.Concat(data.Skip(occurence + wmicKey.Length).TakeWhile(c => c != ' ')), out systemManaged)) {
				return false;
			}

			return true;
		}

		/// <summary>
		///  Changes the systems pagefile settings
		/// </summary>
		/// <param name="currentSelection">The selected partition entry</param>
		/// <param name="maxSize">The maximum Size of the Pagefile in MB</param>
		/// <param name="minSize">The minimum Size of the Pagefile in MB</param>
		/// <returns>Whether the Operation were successfull</returns>
		public static bool ChangePagefileSettings(string currentSelection, int maxSize, int minSize) {
			List<string> tempDriveInfoList = FileSystem.Drives.Select(OperatingMethods.GetDriveInfoDescription).ToList();
			int selectedPartitionIndex;
			if (tempDriveInfoList.Contains(currentSelection)) //Tests whether the selected partition is available
			{
				selectedPartitionIndex = tempDriveInfoList.IndexOf(currentSelection);
			}
			else {
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
		public static bool ChangePagefileSettings(DriveInfo toUse, int maxSize, int minSize) {
			if (maxSize < minSize) //Tests whether the maxSize is smaller than the minSize
			{
				MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_MinGreaterMax,
					OperatingMethodsStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (toUse.TotalFreeSpace < minSize * BytesInMegabyte) //Tests whether enough space is available
			{
				MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_NotEnoughSpace,
					OperatingMethodsStrings.Error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (Wrapper.ExecuteExecuteable(
				WmicPath, "computersystem get AutomaticManagedPagefile /Value"
				, out string[] tmp, out int _, out int _, true, true, true, true)) //Tests
			{
				if (bool.Parse(tmp[2].Split('=')[1])) {
					Wrapper.ExecuteCommand(
						WmicPath
						+ Environment.ExpandEnvironmentVariables(
							" computersystem where \"name='%computername%' \" set AutomaticManagedPagefile=False")
						, true, true, out _); //Disables automatic Pagefile  management
					Wrapper.ExecuteExecuteable(
						WmicPath
						, "computersystem get AutomaticManagedPagefile /Value"
						, out tmp, out int _, out _, waitforexit: true, hidden: true, admin: true);
					if (!bool.Parse(tmp[2].Split('=')[1])) {
						MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_CouldntDisableManagement,
							OperatingMethodsStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
				}
			}

			Wrapper.ExecuteExecuteable(WmicPath,
				"pagefileset delete /NOINTERACTIVE", out _, out int _, out _, waitforexit: true, hidden: true,
				admin: true); //Deletes all Pagefiles

			Wrapper.ExecuteExecuteable(WmicPath,
				$"pagefileset create name=\"{Path.Combine(toUse.Name, "Pagefile.sys")}\"", out _,
				out int _, out _, waitforexit: true, hidden: true, admin: true); //Creates new Pagefile

			Wrapper.ExecuteExecuteable(WmicPath,
				$"pagefileset where name=\"{Path.Combine(toUse.Name, "Pagefile.sys")}\" set InitialSize={minSize},MaximumSize={maxSize}",
				out _, out int _, out _, waitforexit: true, hidden: true, admin: true);
			// Sets Pagefile Size

			Wrapper.ExecuteExecuteable(WmicPath,
				" get", out tmp, out int _, out int _, true, true,
				true, true); //Checks wether there is exactly 1 pagefile existing
			if (tmp.Length != 2) {
				switch (MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_Not1Pagefile,
					OperatingMethodsStrings.Error,
					MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)) {
					case DialogResult.Cancel: return false;
					case DialogResult.Retry: return ChangePagefileSettings(toUse, maxSize, minSize);
				}
			}

			return true;
		}
	}
}