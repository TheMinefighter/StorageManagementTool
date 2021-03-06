using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;

namespace StorageManagementCore.Backend {
	public partial class ShellFolder : INotifyPropertyChanged {
		private static ReadOnlyMap<string, ShellFolder> _byLocalizedName;

		public static ReadOnlyCollection<ShellFolder> AllShellFolders;
		private static Dictionary<string, ShellFolder> _byName;

		private static Dictionary<Guid, ShellFolder> _byGuid;

		public static ReadOnlyMap<string, ShellFolder> ByLocalizedName =>
			_byLocalizedName ?? (_byLocalizedName = new ReadOnlyMap<string, ShellFolder>(
				AllShellFolders.Select(x => new KeyValuePair<string, ShellFolder>(x.LocalizedName, x))));

		//Must be an on demand initialization, because C#s unchangeable object initialization order creating NullReferences otherwise 
		public static Dictionary<string, ShellFolder> ByName {
			get { return _byName ?? (_byName = AllShellFolders.ToDictionary(x => x.Name)); }
		}

		//Must be an on demand initialization, because C#s unchangeable object initialization order creating NullReferences otherwise
		public static Dictionary<Guid, ShellFolder> ByGuid {
			get { return _byGuid ?? (_byGuid = AllShellFolders.ToDictionary(x => x.WindowsIdentifier)); }
		}

		//TODO add
		[CanBeNull]
		public string DefaultValue { get; }

		public bool IsUserSpecific { get; }

		//TODO Localize
		[NotNull]
		public string LocalizedName => Name;

		[NotNull]
		public string Name { get; }

		public bool ShouldBeEdited { get; }

		public bool Defined => DefaultValue != null;

		public bool Undefined => DefaultValue is null;

		public Guid WindowsIdentifier { get; }

		static ShellFolder() {
			Session.LanguageChanged += (a, b) => _byLocalizedName = null;
			AllShellFolders = Array.AsReadOnly(typeof(KnownShellFolders).GetFields().Select(x => x.GetValue(null))
				.Cast<ShellFolder>().ToArray());
		}


		// ReSharper disable once NotNullMemberIsNotInitialized
		// Justified by being private
		private ShellFolder() {
			Session.LanguageChanged += (a, b) => OnPropertyChanged(nameof(LocalizedName));
		}

		private ShellFolder([NotNull] string name, [NotNull] string windowsIdentifier, bool isUserSpecific,
			bool shouldBeEdited, [CanBeNull] string defaultValue) : this() {
			IsUserSpecific = isUserSpecific;
			Name = name;
			ShouldBeEdited = shouldBeEdited;
			WindowsIdentifier = new Guid(windowsIdentifier);
			DefaultValue = defaultValue;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public static ShellFolder GetUSF(Guid windowsIdentifier) {
			return AllShellFolders.First(x => x.WindowsIdentifier == windowsIdentifier);
		}

		public static void DebugAllFolders() {
			StringBuilder sb = new StringBuilder();
			foreach (ShellFolder sf in AllShellFolders) {
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

			return string.IsNullOrEmpty(specialFolderPath) ? null: new DirectoryInfo(specialFolderPath);
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

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}