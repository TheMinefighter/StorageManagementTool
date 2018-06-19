﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Csv;
using Microsoft.VisualBasic.FileIO;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.GlobalizationRessources;

namespace StorageManagementCore.Operation {
	public static class PagefileManagement {
		private static readonly string WmicPath =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "wbem\\wmic.exe");

//		public static bool ApplyConfiguration(Configuration.PagefileSysConfiguration cfg) { }
//
		/// <summary>
		/// Reads the current <exception cref="PagefileSysConfiguration"></exception>
		/// </summary>
		/// <param name="cfg">The current <see cref="PagefileSysConfiguration"/></param>
		/// <returns>Whether the operation were successful</returns>
		public static bool GetCurrentPagefileConfiguration(out PagefileSysConfiguration cfg) {
			cfg = new PagefileSysConfiguration();
			if (!Wrapper.ExecuteExecuteable(
				WmicPath, "pagefileset list /FORMAT:CSV"
				, out string[] tmp, out int _, out int _, true, true, true, true)) {
				return false;
			}

			foreach (ICsvLine line in CsvReader.Read(new StringReader(string.Join(Environment.NewLine, tmp)))) {
				cfg.Pagefiles.Add(new Pagefile(new ConfiguredDrive(line["Name"].First()), int.Parse(line["MaximumSize"]),
					int.Parse(line["MaximumSize"])));
			}

			if (!GetSystemManaged(out bool val)) {
				return false;
			}

			cfg.SystemManaged = val;
			return true;
		}

//
		/// <summary>
		/// Deletes all pagefiles
		/// </summary>
		/// <returns>Whether the operation were successful</returns>
	//	private static bool DeleteAllPagefiles() { }
//
//		private static bool SetSystemManaged(bool SystemManaged) { }
//
//		private static bool ChangePagefile(Configuration.Pagefile cfg) { }
//
//		private static bool DeletePagefile(DriveInfo drive) { }
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

			if (toUse.AvailableFreeSpace < minSize * 1048576L) //Tests whether enough space is available
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