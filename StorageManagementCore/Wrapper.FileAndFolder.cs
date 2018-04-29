using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32.SafeHandles;

namespace StorageManagementCore
{
	public static partial class Wrapper
	{
		public static class FileAndFolder
		{
			/// <summary>
			///  Copies a Directory
			/// </summary>
			/// <param name="src">The Directory to copy from</param>
			/// <param name="target">The Directory, where the contents of src should be copied to</param>
			/// <returns>Whether the operation were successful</returns>
			public static bool CopyDirectory(DirectoryInfo src, DirectoryInfo target)
			{
				try
				{
					FileSystem.CopyDirectory(src.FullName, target.FullName, UIOption.AllDialogs);
				}
				catch (Exception)
				{
					return false;
				}

				return true;
			}

			/// <summary>
			///  Deletes a Directory
			/// </summary>
			/// <param name="toBeDeleted">The Folder to delete</param>
			/// <param name="deletePermanent">Whether the Folder should be deleted permanently</param>
			/// <returns>Whether the operation were sucessful</returns>
			public static bool DeleteDirectory(DirectoryInfo toBeDeleted, bool deletePermanent = true, bool ask = true)
			{
				try
				{
					FileSystem.DeleteDirectory(toBeDeleted.FullName, ask ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs,
						deletePermanent ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin);
				}
				catch (Exception)
				{
					return false;
				}

				return true;
			}

			/// <summary>
			///  Tests whether a File is a symlink
			/// </summary>
			/// <param name="path">The path of the file to test</param>
			/// <returns>Whether the file is a symlink</returns>
			public static bool IsPathSymbolic(string path)
			{
				FileInfo pathInfo = new FileInfo(path);
				return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
			}

			/// <summary>
			///  Copies a file
			/// </summary>
			/// <param name="src">The location to copy from</param>
			/// <param name="to">The location to copy to</param>
			/// <returns>Whether the operation were successful</returns>
			public static bool CopyFile(FileInfo src, FileInfo to)
			{
				try
				{
					FileSystem.CopyFile(src.FullName, to.FullName, UIOption.AllDialogs);
				}
				catch (Exception)
				{
					return false;
				}

				return true;
			}

			/// <summary>
			///  Deletes a file
			/// </summary>
			/// <param name="toDelete">The file to delete</param>
			/// <param name="ShowFullDialog"></param>
			/// <param name="deletePermanent">Whether it should be deleted permanently</param>
			/// <returns>Whether the operation were successful</returns>
			public static bool DeleteFile(FileInfo toDelete, bool ShowFullDialog = true, bool deletePermanent = true)
			{
				try
				{
					FileSystem.DeleteFile(toDelete.FullName,
						ShowFullDialog ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs,
						deletePermanent ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin);
				}
				catch (Exception)
				{
					return false;
				}

				return true;
			}

			public static bool MoveDirectory(DirectoryInfo toMove, DirectoryInfo destination)
			{
				try
				{
					FileSystem.MoveDirectory(toMove.FullName, destination.FullName, UIOption.AllDialogs);
				}
				catch (OperationCanceledException)
				{
					return true;
				}

				catch (Exception)
				{
					return false;
				}

				return true;
			}

			public static bool MoveFile(FileInfo toMove, FileInfo destination)
			{
				try
				{
					FileSystem.MoveFile(toMove.FullName, destination.FullName, UIOption.AllDialogs);
				}
				catch (Exception)
				{
					return false;
				}

				return true;
			}

			public static bool CreateFolderSymlink(DirectoryInfo dir, DirectoryInfo newLocation)
			{
				return ExecuteCommand($"mklink /D \"{dir.FullName}\" \"{newLocation.FullName}\"", true, true);
			}

			public static bool CreateFileSymlink(FileInfo file, FileInfo newLocation)
			{
				return ExecuteCommand($"mklink \"{file.FullName}\" \"{newLocation.FullName}\"", true, true, out _);
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
			public static string GetRealPath(string path)
			{
				if (!Directory.Exists(path) && !File.Exists(path))
				{
					throw new IOException("TargetPath not found");
				}

				DirectoryInfo symlink = new DirectoryInfo(path); // No matter if it's a file or folder
				SafeFileHandle directoryHandle = CreateFile(symlink.FullName, 0, 2, IntPtr.Zero,
					CREATION_DISPOSITION_OPEN_EXISTING,
					FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero); //Handle file / folder
				if (directoryHandle.IsInvalid)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}

				StringBuilder result = new StringBuilder(512);
				int mResult = GetFinalPathNameByHandle(directoryHandle.DangerousGetHandle(), result, result.Capacity, 0);
				if (mResult < 0)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}

				if (result.Length >= 4 && result[0] == '\\' && result[1] == '\\' && result[2] == '?' && result[3] == '\\')
				{
					return result.ToString().Substring(4); // "\\?\" remove
				}

				return result.ToString();
			}

			#endregion
		}
	}
}