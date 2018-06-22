using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using StorageManagementCore.GlobalizationRessources;

namespace StorageManagementCore.Backend {
	public static class RegistryMethods {
		#region Based upon https://en.wikipedia.org/wiki/Windows_Registry#Keys_and_values and https://msdn.microsoft.com/en-us/library/windows/desktop/bb773476(v=vs.85).aspx last access 14.02.2018

		public static readonly Dictionary<RegistryValueKind, string> ToWin32Api = new Dictionary<RegistryValueKind, string>() {
			{RegistryValueKind.String, "REG_SZ"},
			{RegistryValueKind.ExpandString, "REG_EXPAND_SZ"},
			{RegistryValueKind.Binary, "REG_BINARY"},
			{RegistryValueKind.DWord, "REG_DWORD"},
			{RegistryValueKind.MultiString, "REG_MULTI_SZ"},
			{RegistryValueKind.QWord, "REG_QWORD"},
			//Technically not the only solution but in 99% of the cases where this really iounlikely case is used it is correct
			{RegistryValueKind.Unknown, "REG_RESSOURCE_LIST"}
		};

		public static readonly Dictionary<string, RegistryValueKind> FromWin32Api= new Dictionary<string, RegistryValueKind> {
	{"REG_BINARY",RegistryValueKind.Binary},
	{"REG_DWORD_LITTLE_ENDIAN",RegistryValueKind.DWord },
	{"REG_DWORD_BIG_ENDIAN",RegistryValueKind.DWord },
	{"REG_DWORD",RegistryValueKind.DWord},
	{"REG_EXPAND_SZ",RegistryValueKind.ExpandString},
	{"REG_NONE",RegistryValueKind.None},
	{"REG_QWORD_LITTLE_ENDIAN", RegistryValueKind.QWord },
	{"REG_QWORD",RegistryValueKind.QWord},
	{"REG_SZ",RegistryValueKind.String},
	{"REG_RESOURCE_LIST", RegistryValueKind.Unknown},
	{"REG_RESOURCE_REQUIREMENTS_LIST", RegistryValueKind.Unknown},
	{"REG_FULL_RESOURCE_DESCRIPTOR", RegistryValueKind.Unknown},
	{"REG_LINK",RegistryValueKind.Unknown},
	{"REG_NONE", RegistryValueKind.None}
};
		#endregion

		public static readonly Map<RegistryHive, string> RegistryRootKeys = new Map<RegistryHive, string> {
			{RegistryHive.ClassesRoot, "HKEY_CLASSES_ROOT"},
			{RegistryHive.CurrentConfig, "HKEY_CURRENT_CONFIG"},
			{RegistryHive.CurrentUser, "HKEY_CURRENT_USER"},
			{RegistryHive.DynData, "HKEY_DYN_DATA"},
			{RegistryHive.LocalMachine, "HKEY_LOCAL_MACHINE"},
			{RegistryHive.PerformanceData, "HKEY_PERFORMANCE_DATA"},
			{RegistryHive.Users, "HKEY_USERS"}
		};

		public static bool GetRegistryValue(RegistryValue path, out object toReturn, bool asUser = false) {
			
			toReturn = null;
			if (asUser) {
				if (!Wrapper.ExecuteExecuteable(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"reg.exe"),
					$" query \"{path.RegistryKeyName}\" /v \"{path.ValueName}\"", out string[] ret, out int _, out _, true,
					true, true, true,
					true)) {
					return false;
				}

				if (ret.Length == 2) {
					return false;
				}

				string thirdLine = ret[2];
				RegistryValueKind kind = FromWin32Api[thirdLine.Substring(8 + path.ValueName.Length).Split(' ')[0]];
				string data = thirdLine.Substring(12 + path.ValueName.Length + kind.ToString().Length).Trim();
				toReturn = RegistryObjectFromString(data, kind);

				return true;
			}

			try {
				RegistryKey.OpenBaseKey(path.Hive,RegistryView.Default).OpenSubKey(path.ValueName)
//				RegistryKey.OpenBaseKey(RegistryRootKeys[path.RegistryKey.Split('\\')[0]],
//					RegistryView.Registry64).OpenSubKey()
				toReturn = Registry.GetValue(path.RegistryKeyName, path.ValueName, -1);
			}
			catch (Exception e) {
				return MessageBox.Show(
					       string.Format(WrapperStrings.GetRegistryValue_Exception,
						       path.ValueName, path.RegistryKeyName, e.Message),
					       WrapperStrings.Error, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) ==
				       DialogResult.Retry && GetRegistryValue(path, out toReturn, asUser);
			}

			toReturn = RegistryNumberFixGet(toReturn);

			return true;
		}

		/// <summary>
		///  Fix for numbers stored in the registry, explaination available at https://github.com/dotnet/corefx/issues/26936
		/// </summary>
		/// <param name="source">The registry object to fix</param>
		/// <returns>The fixed object</returns>
		private static object RegistryNumberFixGet(object source) {
			if (source is int) {
				source = BitConverter.ToUInt32(BitConverter.GetBytes((int) source), 0);
			}

			if (source is long) {
				source = BitConverter.ToUInt64(BitConverter.GetBytes((long) source), 0);
			}

			return source;
		}

		/// <summary>
		///  Fix for numbers stored in the registry, explaination available at https://github.com/dotnet/corefx/issues/26936
		/// </summary>
		/// <param name="toReturn">The registry object to fix</param>
		/// <returns>The fixed object</returns>
		private static object RegistryNumberFixSet(object toReturn) {
			if (toReturn is int) {
				toReturn = BitConverter.ToUInt32(BitConverter.GetBytes((int) toReturn), 0);
			}

			if (toReturn is long) {
				toReturn = BitConverter.ToUInt64(BitConverter.GetBytes((long) toReturn), 0);
			}

			return toReturn;
		}

		private static object RegistryObjectFromString(string data, RegistryValueKind kind) {
			object toReturn = data;
			switch (kind) {
				case RegistryValueKind.DWord:

					toReturn = uint.Parse(data.Substring(2), NumberStyles.HexNumber);
					break;
				case RegistryValueKind.String:
					toReturn = data;
					break;
				case RegistryValueKind.ExpandString: break;
				case RegistryValueKind.Binary: break;
				case RegistryValueKind.MultiString:
					toReturn = data.Split('\0');
					break;
				case RegistryValueKind.QWord:
					toReturn = ulong.Parse(data.Substring(2), NumberStyles.HexNumber);
					break;
				case RegistryValueKind.Unknown:
					toReturn = data;
					break;
				case RegistryValueKind.None:
					toReturn = "";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return toReturn;
		}

		/// <summary>
		///  Sets an Registry Value
		/// </summary>
		/// <param name="valueLocation">The Location of the Value to change</param>
		/// <param name="content">The content to write into the content</param>
		/// <param name="registryValueKind">The type of the content</param>
		/// <param name="asUser"></param>
		/// <returns></returns>
		public static bool SetRegistryValue(RegistryValue valueLocation, object content,
			RegistryValueKind registryValueKind,
			bool asUser = false) {
			if (asUser) {
				SetProtectedRegistryValue(valueLocation, content, registryValueKind);
				return true;
			}

			if (!Session.Singleton.IsAdmin) {
				string value;
				switch (registryValueKind) {
					case RegistryValueKind.DWord:
						value = content is int ? ((int) content).ToString() : ((uint) content).ToString();
						break;
					case RegistryValueKind.QWord:
						value = content is long ? ((long) content).ToString() : ((ulong) content).ToString();
						break;
					case RegistryValueKind.String:
						value = ((string) content).Replace("\"", "\\\"");
						break;
					case RegistryValueKind.MultiString:
						value = string.Join("\0", (string[]) content).Replace("\"", "\\\"");
						break;
					case RegistryValueKind.ExpandString:
						value = ((string) content).Replace("\"", "\\\"");
						break;
					default:
						value = (string) content;
						break;
				}

				string kind = ToWin32Api[registryValueKind];
				if (!Wrapper.ExecuteExecuteable(
					    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "reg.exe"),
					    $" add \"{valueLocation.RegistryKeyName}\" /v \"{valueLocation.ValueName}\" /t {kind} /d \"{value}\" /f",
					    out string[] _, out int tmpExitCode, out _, true, true, true, true, asUser) || tmpExitCode == 1) {
					return false;
				}

				return true;
			}

			try {
				Registry.SetValue(valueLocation.RegistryKeyName, valueLocation.ValueName, content, registryValueKind);
			}
			catch (SecurityException) {
				if (MessageBox.Show(
					    string.Format(
						    WrapperStrings.SetRegistryValue_Security,
						    valueLocation.ValueName, valueLocation.RegistryKeyName, content, registryValueKind),
					    WrapperStrings.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
					Wrapper.RestartProgram(true);
					Environment.Exit(0);
				}

				return false;
			}
			catch (UnauthorizedAccessException) {
				if (MessageBox.Show(
					    string.Format(
						    WrapperStrings.SetRegistryValue_UnauthorizedAccess,
						    valueLocation.ValueName, valueLocation.RegistryKeyName, content, registryValueKind),
					    WrapperStrings.Error,
					    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
					Wrapper.RestartProgram(true);
					return true;
				}

				return false;
			}
			catch (Exception e) {
				if (MessageBox.Show(
					    string.Format(
						    WrapperStrings.SetRegistry_Exception,
						    valueLocation.ValueName, valueLocation.ValueName, content, registryValueKind, e.Message),
					    WrapperStrings.Error, MessageBoxButtons.RetryCancel,
					    MessageBoxIcon.Error) == DialogResult.Retry) {
					return SetRegistryValue(valueLocation, content, registryValueKind);
				}

				return false;
			}

			return true;
		}

		/// <summary>
		///  Writes a protected registry value
		/// </summary>
		/// <param name="toSet">The value to set</param>
		/// <param name="content">The content to set the value to</param>
		/// <param name="kind">The RegistyValueKind of the content</param>
		public static void SetProtectedRegistryValue(RegistryValue toSet, object content, RegistryValueKind kind) {
			const string folderName = "StorageManagementToolRegistryData";
			string path = Path.Combine(Path.GetTempPath(), folderName, "OpenThisFileToApplyRegistryChages.reg");
			new FileInfo(path).Directory.Create();
			string toWrite = GetRegFileContent(content, kind);
			File.WriteAllLines(path,
				new[] {
					"Windows Registry Editor Version 5.00",
					"",
					$"[{toSet.RegistryKeyName}]",
					$"\"{Wrapper.AddBackslahes(toSet.ValueName)}\"={toWrite}"
				});
			// I know that the following is exploiting UAC a bit, but the Warning will not be suppressed, so I don´t see any real reasons not to do that
			ApplyRegfile();
		}

		/// <summary>
		///  Applies a Regfile generated using SetProtectedRegistryValue
		/// </summary>
		public static void ApplyRegfile() {
			const string folderName = "StorageManagementToolRegistryData";
			Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath,
				$" /select,\"{Path.Combine(Path.GetTempPath(), folderName, "OpenThisFileToApplyRegistryChages.reg")}\"");
			//Thread.Sleep(1000);
			//SendKeys.SendWait("{ENTER}");
			//Thread.Sleep(1000);
			//ExecuteExecuteable(Path.Combine(System32Path, "taskkill.exe"),
			//   $"/F /FI \"WINDOWTITLE eq {folderName}\" /IM explorer.exe");
			////For people, who have the "Display full name in titlebar" option enabled
			//ExecuteExecuteable(Path.Combine(System32Path, "taskkill.exe"),
			//   $"/F /FI \"WINDOWTITLE eq {Path.Combine(Path.GetTempPath(), folderName)}\" /IM explorer.exe");
		}

		/// <summary>
		///  Generates a value to write to a .reg file for Windows Registry Editor Version 5.00
		/// </summary>
		/// <param name="content">The object to be written in the file</param>
		/// <param name="kind">The RegistryValueKind of the content</param>
		/// <returns> A value to use in a .reg file</returns>
		private static string GetRegFileContent(object content, RegistryValueKind kind) {
			string toWrite;
			switch (kind) {
				case RegistryValueKind.String:
					toWrite = $"\"{Wrapper.AddBackslahes((string) content)}\"";
					break;
				case RegistryValueKind.ExpandString:
					toWrite = "hex(2):" + string.Join(",",
						          Encoding.Unicode.GetBytes((string) content + "\0")
							          .Select(x => x.ToString("X2").ToLower()));
					break;
				case RegistryValueKind.Binary:
					toWrite = "hex:" + string.Join(",",
						          Encoding.Unicode.GetBytes((string) content).Select(x => x.ToString("X2").ToLower()));
					break;
				case RegistryValueKind.DWord:
					toWrite = "dword:" + ((uint) RegistryNumberFixGet(content)).ToString("D8");
					break;
				case RegistryValueKind.MultiString:
					toWrite = "hex(7):" + string.Join(",",
						          Encoding.Unicode.GetBytes(string.Join("\0", (string[]) content) + "\0\0")
							          .Select(x => x.ToString("X2").ToLower()));
					break;
				case RegistryValueKind.QWord:
					toWrite = "hex(b):" + string.Join(",",
						          BitConverter.GetBytes((ulong) RegistryNumberFixGet(content))
							          .Select(x => x.ToString("X2").ToLower()));
					break;
				case RegistryValueKind.None:
					toWrite = "hex(0):" + string.Join(",",
						          Encoding.Unicode.GetBytes((string) content).Select(x => x.ToString("X2").ToLower()));
					break;
				case RegistryValueKind.Unknown:
				default:
					throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
			}

			return toWrite;
		}
	}
}