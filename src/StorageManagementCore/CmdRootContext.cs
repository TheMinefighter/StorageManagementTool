using System;
using System.IO;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;
using UniversalCommandlineInterface;
using UniversalCommandlineInterface.Attributes;

// ReSharper disable UnusedMember.Global

namespace StorageManagementCore {
	[CmdContext]
	public abstract class CmdRootContext {
		public enum FileOrFolder {
			File,
			Folder,
			Automatic
		}

		internal const string StartupProceedFileName = "StorageManagementProceed.lnk";

		[CmdAction("Admin")]
		public static void RestartAsAdministrator([CmdParameter("Arguments")] string[] args = null) {
			Wrapper.RestartProgram(true, args ?? new string[] { });
		}

		[CmdAction("ContinueSwapfile")]
		public static void ContinueSwapfile([CmdParameter("Drive")] string drive,
			[CmdParameter("Forward", CmdParameterAttribute.CmdParameterUsage.OnlyDirectAlias)]
			[CmdParameterAlias("forward", true)]
			[CmdParameterAlias("backward", false)]
			bool forward) {
			FileAndFolder.DeleteFile(
				new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
					StartupProceedFileName)), false);
			Swapfile.ChangeSwapfileStadium(forward, Swapfile.SwapfileState.Disabled, new DriveInfo(drive));
		}

		[CmdAction("Move")]
		public static void Move(
			[CmdParameter("Srcpath")] string[] oldPaths,
			[CmdParameterAlias("File", FileOrFolder.File)]
			[CmdParameterAlias("Folder", FileOrFolder.Folder)]
			[CmdParameterAlias("Auto-detect", FileOrFolder.Automatic)]
			[CmdParameter("Type")]
			FileOrFolder moveFileOrFolder = FileOrFolder.Automatic, [CmdParameter("newpath")] string newPath = null
		) {
			if (newPath == null) {
				newPath = Session.Singleton.Configuration.DefaultHDDPath;
			}

			foreach (string oldPath in oldPaths) {
				bool fileOrFolder;
				switch (moveFileOrFolder) {
					case FileOrFolder.File:
						fileOrFolder = true;
						break;
					case FileOrFolder.Folder:
						fileOrFolder = false;
						break;
					case FileOrFolder.Automatic:
						if (File.Exists(oldPath)) {
							fileOrFolder = true;
						}
						else if (Directory.Exists(oldPath)) {
							fileOrFolder = false;
						}
						else {
							//    ArgumentError(args);
							continue;
						}

						break;
					default: continue;
				}

				if (fileOrFolder) {
					OperatingMethods.MoveFilePhysically(new FileInfo(oldPath), new DirectoryInfo(newPath));
				}
				else {
					OperatingMethods.MoveFolderPhysically(new DirectoryInfo(oldPath), new DirectoryInfo(newPath), true);
				}
			}
		}

		[CmdAction("Background")]
		public static void Back() {
			BackgroundNotificationCreator.Initalize();
		}

		[CmdContext("SendTo")]
		public abstract class SendTo {
			[CmdAction("Set")]
			public static void SetSendTo(
				[CmdParameter("Enabled")]
				[CmdParameterAlias("Enable", true)]
				[CmdParameterAlias("Disable", false)]
				bool enable = true) {
				OperatingMethods.EnableSendToHDD(enable);
			}

			[CmdAction("Get")]
			public static void GetSendTo() {
				bool isSendToHddEnabled = OperatingMethods.IsSendToHDDEnabled();
				ConsoleIO.WriteLine(isSendToHddEnabled.ToString());
				ConsoleIO.WriteLine($"SendTo feature is{(isSendToHddEnabled ? string.Empty : " not")} enabled");
			}
		}
	}
}