using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;

namespace StorageManagementCore.Backend {
	#region Classes used as return values

	public class FolderProperties {
		#region Private class variables

		#endregion

		#region Public class properties

		public string Path { get; set; } = "";

		public Icon Icon { get; set; } = null;

		#endregion
	}

	#endregion

	public static class SpecialFolders {
		#region Enums

		public enum FolderList {
			None,
			AdminTools,
			ApplicationData,
			CDBurning,
			CommonAdminTools,
			CommonApplicationData,
			CommonDesktopDirectory,
			CommonDocuments,
			CommonMusic,
			CommonOemLinks,
			CommonPictures,
			CommonProgramFiles,
			CommonProgramFilesX86,
			CommonPrograms,
			CommonStartMenu,
			CommonStartup,
			CommonTemplates,
			CommonVideos,
			Cookies,
			Desktop,
			DesktopDirectory,
			Favorites,
			Fonts,
			History,
			InternetCache,
			LocalApplicationData,
			LocalizedResources,
			MyComputer,
			MyDocuments,
			MyMusic,
			MyPictures,
			MyVideos,
			NetworkShortcuts,
			Personal,
			PrinterShortcuts,
			ProgramFiles,
			ProgramFilesX86,
			Programs,
			Recent,
			Resources,
			SendTo,
			StartMenu,
			Startup,
			System,
			SystemX86,
			Templates,
			UserProfile,
			Windows,
			Links
		}

		public enum FolderType {
			None,
			System,
			Custom
		}

		#endregion

		#region Public static class methods

		public static FolderProperties GetPath(string sFolderKey) {
			//Overloaded
			FolderList folderKey = FolderList.None;
			//Determine the folder type
			switch (sFolderKey) {
				#region System's Environment.SpecialFolder elements

				//There was more code here but had to
				//remove due to SO's character limit
				//Full code: http://pastebin.com/dE0Y6tFB
				case "System":
					folderKey = FolderList.System;
					break;
				case "SystemX86":
					folderKey = FolderList.SystemX86;
					break;
				case "Templates":
					folderKey = FolderList.Templates;
					break;
				case "UserProfile":
					folderKey = FolderList.UserProfile;
					break;
				case "Windows":
					folderKey = FolderList.Windows;
					break;

				#endregion

				#region Custom elements

				case "Links":
					folderKey = FolderList.Links;
					break;

				#endregion
			}

			return GetPath(folderKey);
		}

		public static FolderProperties GetPath(FolderList folderKey) {
			FolderProperties fp = new FolderProperties();
			FolderType sfType = FolderType.None;
			Environment.SpecialFolder sf = Environment.SpecialFolder.AdminTools;

			//Determine the folder type
			switch (folderKey) {
				#region System's Environment.SpecialFolder elements

				//There was more code here but had to
				//remove due to SO's character limit
				//Full code: http://pastebin.com/dE0Y6tFB
				case FolderList.System:
					sfType = FolderType.System;
					sf = Environment.SpecialFolder.System;
					break;
				case FolderList.SystemX86:
					sfType = FolderType.System;
					sf = Environment.SpecialFolder.SystemX86;
					break;
				case FolderList.Templates:
					sfType = FolderType.System;
					sf = Environment.SpecialFolder.Templates;
					break;
				case FolderList.UserProfile:
					sfType = FolderType.System;
					sf = Environment.SpecialFolder.UserProfile;
					break;
				case FolderList.Windows:
					sfType = FolderType.System;
					sf = Environment.SpecialFolder.Windows;
					break;

				#endregion

				#region Custom elements

				case FolderList.Links:
					sfType = FolderType.Custom;
					break;

				#endregion
			}

			//Build the folder object's path
			switch (sfType) {
				case FolderType.System:
					fp.Path = Environment.GetFolderPath(sf);
					break;
				case FolderType.Custom:
					fp.Path = "LINKS";
					fp.Path = GetSpecialFolderPath(Win32ShellFolders.KnownFolder.Links);
					break;
			}

			//Build the folder object's icon
			//more to be done here...

			return fp;
		}

		public static void DebugShowAllFolders() {
			StringBuilder sb = new StringBuilder();
			foreach (AdvancedUserShellFolder sf in AdvancedUserShellFolder.AllUSF) {
				sb.Append(sf.Name);
				sb.Append(Environment.NewLine);
				sb.Append(GetSpecialFolderPath(sf.WindowsIdentifier));
				sb.Append(Environment.NewLine);
				//      Console .WriteLine("SpecialFolder: " + sf + "\n  Path: " + GetSpecialFolderPath(sf.WindowsIdentifier) + "\n");
			}

			File.WriteAllText(Environment.CurrentDirectory + "\\Data.txt", sb.ToString());
			string s = sb.ToString();
			Guid folderId = new Guid("9E52AB10-F80D-49DF-ACB8-4330F5687855");
			string fPrgTt = "F:\\Prg\\TT";
			int shSetKnownFolderPath = SetSpecialFolderPathInternal(folderId, fPrgTt);
		}

		public static bool SetSpecialFolderPath(AdvancedUserShellFolder folderId, string newPath) =>
			SetSpecialFolderPathInternal(folderId.WindowsIdentifier, newPath) == 0;

		public static int SetSpecialFolderPathInternal(Guid folderId, string fPrgTt) =>
			Win32ShellFolders.SHSetKnownFolderPath(folderId, 0, IntPtr.Zero, fPrgTt);

		[CanBeNull]
		public static string GetSpecialFolderPath(Guid kFolderID) {
			string sRet = null;

			if (Win32ShellFolders.SHGetKnownFolderPath(kFolderID, 0, IntPtr.Zero, out IntPtr pPath) == 0) {
				sRet = Marshal.PtrToStringUni(pPath);
				Marshal.FreeCoTaskMem(pPath);
			}

			return sRet;
		}

		#endregion
	}
}