using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using IWshRuntimeLibrary;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32.SafeHandles;
using Microsoft.WindowsAPICodePack.Dialogs;
using File = System.IO.File;

namespace StorageManagementCore.WPFGUI.DataProviders {}

namespace StorageManagementCore.Backend {
	public static class FileAndFolder {
		public enum FileOrFolder : byte {
			Neither,
			File,
			Folder
		}

		public static void OpenFolder(DirectoryInfo info) {
			//TODO Add option to mark in parent
			Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath, info.FullName);
		}

		public static FileOrFolder IsFileOrFolder(string path) {
			if (Directory.Exists(path)) {
				return FileOrFolder.Folder;
			}

			if (File.Exists(path)) {
				return FileOrFolder.File;
			}

			return FileOrFolder.Neither;
		}

		public static DirectoryInfo SelectDirectory(string description = "") {
#if MITMode
				using (FolderBrowserDialog fbd = new FolderBrowserDialog {Description = description}) {
					fbd.ShowDialog();
					return new DirectoryInfo(fbd.SelectedPath);
				}
#else
			CommonOpenFileDialog dlg = new CommonOpenFileDialog(description) {IsFolderPicker = true};
			dlg.ShowDialog();
			return new DirectoryInfo(dlg.FileName);

#endif
		}

		public static IEnumerable<DirectoryInfo> SelectDirectories(string description = "") {
#if MITMode
				using (FolderBrowserDialog fbd = new FolderBrowserDialog {Description = description}) {
					fbd.ShowDialog();
					return new[] {new DirectoryInfo(fbd.SelectedPath)};
				}
#else
			CommonOpenFileDialog dlg = new CommonOpenFileDialog(description) {IsFolderPicker = true, Multiselect = true};
			dlg.ShowDialog();
			return dlg.FileNames.Select(x => new DirectoryInfo(x));

#endif
		}

		public static FileInfo SelectFile(string description = "") {
#if MITMode
				using (OpenFileDialog fd = new OpenFileDialog() {Title = description}) {
					fd.ShowDialog();
					return new FileInfo(fd.FileName);
				}
#else
			CommonOpenFileDialog dlg = new CommonOpenFileDialog(description);
			dlg.ShowDialog();
			return new FileInfo(dlg.FileName);
#endif
		}

		public static IEnumerable<FileInfo> SelectFiles(string description = "") {
#if MITMode
				using (OpenFileDialog fd = new OpenFileDialog {Title = description, Multiselect = true}) {
					fd.ShowDialog();
					return fd.FileNames.Select(x => new FileInfo(x));
				}
#else
			CommonOpenFileDialog dlg = new CommonOpenFileDialog(description) {Multiselect = true};
			dlg.ShowDialog();
			return dlg.FileNames.Select(x => new FileInfo(x));

#endif
		}

		/// <summary>
		///  Copies a Directory
		/// </summary>
		/// <param name="src">The Directory to copy from</param>
		/// <param name="target">The Directory, where the contents of src should be copied to</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool CopyDirectory(DirectoryInfo src, DirectoryInfo target) {
			try {
				FileSystem.CopyDirectory(src.FullName, target.FullName, UIOption.AllDialogs);
			}
			catch (Exception) {
				return false;
			}

			return true;
		}

		/// <summary>
		///  Deletes a Directory
		/// </summary>
		/// <param name="toBeDeleted">The Folder to delete</param>
		/// <param name="deletePermanent">Whether the Folder should be deleted permanently</param>
		/// <param name="ask">Whether to ask for confirmation</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool DeleteDirectory(DirectoryInfo toBeDeleted, bool deletePermanent = true, bool ask = true) {
			try {
				FileSystem.DeleteDirectory(toBeDeleted.FullName, ask ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs,
					deletePermanent ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin);
			}
			catch (Exception) {
				return false;
			}

			return true;
		}

		#region From https://stackoverflow.com/a/26473940/6730162 access on 30.9.2017

		/// <summary>
		///  Tests whether a File is a symlink
		/// </summary>
		/// <param name="path">The path of the file to test</param>
		/// <returns>Whether the file is a symlink</returns>
		public static bool IsPathSymbolic(string path) {
			FileInfo pathInfo = new FileInfo(path);
			return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
		}

		#endregion

		/// <summary>
		///  Copies a file
		/// </summary>
		/// <param name="src">The location to copy from</param>
		/// <param name="to">The location to copy to</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool CopyFile(FileInfo src, FileInfo to) {
			try {
				FileSystem.CopyFile(src.FullName, to.FullName, UIOption.AllDialogs);
			}
			catch (Exception) {
				return false;
			}

			return true;
		}

		/// <summary>
		///  Deletes a file
		/// </summary>
		/// <param name="toDelete">The file to delete</param>
		/// <param name="showFullDialog"></param>
		/// <param name="deletePermanent">Whether it should be deleted permanently</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool DeleteFile(FileInfo toDelete, bool showFullDialog = true, bool deletePermanent = true) {
			try {
				FileSystem.DeleteFile(toDelete.FullName,
					showFullDialog ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs,
					deletePermanent ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin);
			}
			catch (Exception) {
				return false;
			}

			return true;
		}

		public static bool MoveDirectory(DirectoryInfo toMove, DirectoryInfo destination, bool preserveACL = true) {
			DirectorySecurity security = null;
			if (preserveACL) {
				security = toMove.GetAccessControl();
			}

			try {
				FileSystem.MoveDirectory(toMove.FullName, destination.FullName, UIOption.AllDialogs);
				if (preserveACL) {
					CalculateACLInheritance(security);
					destination.SetAccessControl(security);
				}
			}
			catch (OperationCanceledException) {
				return true;
			}

			catch (Exception) {
				return false;
			}

			return true;
		}

		public static bool MoveFile(FileInfo toMove, FileInfo destination, bool preserveACL = true) {
			FileSecurity security = null;
			if (preserveACL) {
				security = toMove.GetAccessControl();
			}

			try {
				FileSystem.MoveFile(toMove.FullName, destination.FullName, UIOption.AllDialogs);
				if (preserveACL) {
					CalculateACLInheritance(security);
					destination.SetAccessControl(security);
				}
			}
			catch (Exception) {
				return false;
			}

			return true;
		}

		public static bool CreateFolderSymlink(DirectoryInfo dir, DirectoryInfo newLocation) {
			if (true && (Session.Singleton.IsAdmin || Session.Singleton.UnpriviligedSymlinksAvailable)) {
				if (CreateSymbolicLink(dir.FullName, newLocation.FullName + '\\',
					    (Session.Singleton.UnpriviligedSymlinksAvailable
						    ? SymbolicLinkFlags.SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE
						    : SymbolicLinkFlags.None) |
					    SymbolicLinkFlags.SYMBOLIC_LINK_FLAG_DIRECTORY) == 1) {
					return true;
				}
				else {
#if DEBUG
					Marshal.GetLastWin32Error();
#endif
					return false;
				}
			}
			else {
				return Wrapper.ExecuteCommand($"mklink /D \"{dir.FullName}\" \"{newLocation.FullName}\"", true, true);
			}
		}

		[DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", CharSet = CharSet.Unicode)]
		private static extern int CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLinkFlags dwFlags);

		public static bool CreateFileSymlink(FileInfo file, FileInfo newLocation) {
			if (true && (Session.Singleton.IsAdmin || Session.Singleton.UnpriviligedSymlinksAvailable)) {
				if (CreateSymbolicLink(file.FullName, newLocation.FullName,
					    Session.Singleton.UnpriviligedSymlinksAvailable
						    ? SymbolicLinkFlags.SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE
						    : SymbolicLinkFlags.None) == 1) {
					return true;
				}
				else {
#if DEBUG
					Marshal.GetLastWin32Error();
#endif
					return false;
				}
			}
			else {
				return Wrapper.ExecuteCommand($"mklink \"{file.FullName}\" \"{newLocation.FullName}\"", true, true, out _);
			}
		}

		/// <summary>
		///  Calculates the changes in ACL to do when the target is not effected by inheritance anymore
		/// </summary>
		/// <remarks>
		///  When moving a file / folder from a folder, where it is effected by inheritance,
		///  to a target where it is not anymore and you want the access control to stay the same,
		///  these inherited rules must be converted so that they stay. This conversion is done by this method
		/// </remarks>
		/// <code>
		/// DirectorySecurity currentControl = toMove.GetAccessControl();
		/// CalculateACLInheritance(currentControl);
		/// newLocation.SetAccessControl(currentControl);
		/// </code>
		/// <param name="currentControl"></param>
		public static void CalculateACLInheritance(FileSystemSecurity currentControl) {
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

		public static void CreateShortcut(string arguments, FileInfo shortcutLocation, string description = "",
			string targetPath = null) {
			IWshShortcut shortcut = (IWshShortcut) new WshShell().CreateShortcut(shortcutLocation.FullName);
			shortcut.Description = description;
			shortcut.TargetPath = targetPath ?? Process.GetCurrentProcess().MainModule.FileName;
			shortcut.Arguments = arguments;
			shortcut.Save();
		}

		[Flags]
		private enum SymbolicLinkFlags : uint {
			None = 0x00,
			SYMBOLIC_LINK_FLAG_DIRECTORY = 0x01,
			SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE = 0x02
		}

		#region From https://stackoverflow.com/a/38308957/6730162 access on 30.9.2017

		[DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode,
			IntPtr SecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", EntryPoint = "GetFinalPathNameByHandleW", CharSet = CharSet.Unicode,
			SetLastError = true)]
		private static extern int GetFinalPathNameByHandle([In] IntPtr hFile, [Out] StringBuilder lpszFilePath,
			[In] int cchFilePath, [In] int dwFlags);

		private const int CREATION_DISPOSITION_OPEN_EXISTING = 3;
		private const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;

		/// <summary>
		///  Reads the TargetPath stored in the a symlink
		/// </summary>
		/// <param name="path">The path of the symlink</param>
		/// <returns>The path stored in the symlink</returns>
		public static string GetRealPath(string path) {
			if (!Directory.Exists(path) && !File.Exists(path)) {
				throw new IOException("TargetPath not found");
			}

			DirectoryInfo symlink = new DirectoryInfo(path); // No matter if it's a file or folder
			SafeFileHandle directoryHandle = CreateFile(symlink.FullName, 0, 2, IntPtr.Zero,
				CREATION_DISPOSITION_OPEN_EXISTING,
				FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero); //Handle file / folder
			if (directoryHandle.IsInvalid) {
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}

			StringBuilder result = new StringBuilder(512);
			int mResult = GetFinalPathNameByHandle(directoryHandle.DangerousGetHandle(), result, result.Capacity, 0);
			if (mResult < 0) {
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}

			if (result.Length >= 4 && result[0] == '\\' && result[1] == '\\' && result[2] == '?' && result[3] == '\\') {
				return result.ToString().Substring(4); // "\\?\" remove
			}

			return result.ToString();
		}

		#endregion
	}
}