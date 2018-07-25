using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Csv;
using ExtendedMessageBoxLibrary;
using JetBrains.Annotations;
using Microsoft.Win32;
using StorageManagementCore.GlobalizationRessources;
using StorageManagementCore.Operation;

namespace StorageManagementCore.Backend {
	public partial class ShellFolder : INotifyPropertyChanged {
		public static ShellFolder[] AllShellFolders =
			typeof(KnownShellFolders).GetFields().Select(x => x.GetValue(null)).Cast<ShellFolder>().ToArray();

		public static Dictionary<string, ShellFolder> ByName = AllShellFolders.ToDictionary(x => x.Name);

		public static Dictionary<Guid, ShellFolder> ByGuid = AllShellFolders.ToDictionary(x => x.WindowsIdentifier);

		//TODO add
		[CanBeNull]
		public string DefaultValue;
		public bool IsUserSpecific { get; }

		//TODO Localize
		[NotNull]
		public string LocalizedName => Name;
		[NotNull]
		public string Name { get; }
		public bool ShouldBeEdited { get; }
		public bool Undefined { get; }
		public Guid WindowsIdentifier { get; }

		// ReSharper disable once NotNullMemberIsNotInitialized
		// Justified by being private
		private ShellFolder() {
			Session.Singleton.LanguageChanged += (a, b) => OnPropertyChanged(nameof(LocalizedName));
		}

		private ShellFolder([NotNull] string name, [NotNull] string windowsIdentifier, bool undefined, bool isUserSpecific,
			bool shouldBeEdited, [CanBeNull] string defaultValue) : this() {
			IsUserSpecific = isUserSpecific;
			Name = name;
			ShouldBeEdited = shouldBeEdited;
			Undefined = undefined;
			WindowsIdentifier = new Guid(windowsIdentifier);
			DefaultValue = defaultValue;
		}

		public static ShellFolder GetUSF(Guid windowsIdentifier) {
			return AllShellFolders.First(x => x.WindowsIdentifier == windowsIdentifier);
		}

		//TODO Fix that method
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
			OperatingMethods.QuestionAnswer copyContents = OperatingMethods.QuestionAnswer.Ask,
			OperatingMethods.QuestionAnswer deleteOldContents = OperatingMethods.QuestionAnswer.Ask) {
			if (!newDir.Exists) {
				newDir.Create();
			}

			DirectoryInfo currentPath = toChange.GetPath();
			Dictionary<ShellFolder, DirectoryInfo> childs = AllShellFolders
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
				    (copyContents == OperatingMethods.QuestionAnswer.Yes ||
				     copyContents == OperatingMethods.QuestionAnswer.Ask &&
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

		public static void DebugAllFolders() {
			StringBuilder sb = new StringBuilder();
			foreach (ShellFolder sf in ShellFolder.AllShellFolders) {
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

		[CanBeNull]
		public DirectoryInfo GetPath() {
			string specialFolderPath = GetSpecialFolderPath(WindowsIdentifier);

			return string.IsNullOrEmpty(specialFolderPath) ? new DirectoryInfo(specialFolderPath) : null;
		}

		public bool SetPath(DirectoryInfo newPath) => SetSpecialFolderPath(WindowsIdentifier, newPath.FullName);

		public static bool SetSpecialFolderPath(Guid folderId, string newPath) {
			if (SetSpecialFolderPathInternal(folderId, newPath) == 0) {
				return true;
			}
#if DEBUG
			Marshal.GetLastWin32Error();
#endif
			return false;
		}

		private static int SetSpecialFolderPathInternal(Guid folderId, string fPrgTt) =>
			Win32ShellFolders.SHSetKnownFolderPath(folderId, 0, IntPtr.Zero, fPrgTt);

		[CanBeNull]
		public static string GetSpecialFolderPath(Guid kFolderID) {
			string sRet = null;

			if (Win32ShellFolders.SHGetKnownFolderPath(kFolderID, 0, IntPtr.Zero, out IntPtr pPath) == 0) {
				sRet = Marshal.PtrToStringUni(pPath);
			}
			else {
#if DEBUG
				Marshal.GetLastWin32Error();
#endif
			}

			Marshal.FreeCoTaskMem(pPath);
			return sRet;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}