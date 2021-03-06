﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using StorageManagementCore.GlobalizationRessources;

namespace StorageManagementCore.Backend {
	public static partial class Wrapper {
		/// <summary>
		///  Starts all ServiceControllers depending on the given one
		/// </summary>
		/// <param name="toStart"></param>
		/// <returns>Whether the operation were successful</returns>
		private static bool RecursiveServiceStarter(ServiceController toStart) {
			IEnumerable<ServiceController> childs = toStart.DependentServices;
			if (!childs.All(RecursiveServiceStarter)) {
				return false;
			}

			try {
				toStart.Start();
			}
			catch (Exception) {
				return false;
			}

			return true;
		}

		public static class RegistryMethods {
			/// <summary>
			///  Gives the WIN32APi Representation of an given RegistryValueKind
			/// </summary>
			/// <param name="kind"> The RegistryValueKind to represent</param>
			/// <returns>The WIN32API Representation of the given RegistryValueKind</returns>
			private static string Win32ApiRepresentation(RegistryValueKind kind) {
				switch (kind) {
					case RegistryValueKind.String: return "REG_SZ";
					case RegistryValueKind.ExpandString: return "REG_EXPAND_SZ";
					case RegistryValueKind.Binary: return "REG_BINARY";
					case RegistryValueKind.DWord: return "REG_DWORD";
					case RegistryValueKind.MultiString: return "REG_MULTI_SZ";
					case RegistryValueKind.QWord: return "REG_QWORD";
					case RegistryValueKind.Unknown: return "REG_RESSOURCE_LIST";
					case RegistryValueKind.None: return "REG_NONE";
					default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
				}
			}

			public static bool GetRegistryValue(RegistryValue path, out object toReturn, bool asUser = false) {
				toReturn = null;
				if (asUser) {
					if (!ExecuteExecuteable(
						Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"reg.exe"),
						$" query \"{path.RegistryKey}\" /v \"{path.ValueName}\"", out string[] ret, out int _, out _, true,
						true, true, true,
						true)) {
						return false;
					}

					if (ret.Length == 2) {
						return false;
					}

					string thirdLine = ret[2];
					RegistryValueKind kind = FromWin32Api(thirdLine.Substring(8 + path.ValueName.Length).Split(' ')[0]);
					string data = thirdLine.Substring(12 + path.ValueName.Length + kind.ToString().Length).Trim();
					toReturn = RegistryObjectFromString(data, kind);

					return true;
				}

				try {
					toReturn = Registry.GetValue(path.RegistryKey, path.ValueName, null);
				}
				catch (Exception e) {
					return MessageBox.Show(
						       string.Format(WrapperStrings.GetRegistryValue_Exception,
							       path.ValueName, path.RegistryKey, e.Message),
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

			private static RegistryValueKind FromWin32Api(string src) {
				switch (src) {
					#region Based upon https://en.wikipedia.org/wiki/Windows_Registry#Keys_and_values and https://msdn.microsoft.com/en-us/library/windows/desktop/bb773476(v=vs.85).aspx last access 14.02.2018

					case "REG_BINARY": return RegistryValueKind.Binary;
					case "REG_DWORD_LITTLE_ENDIAN":
					case "REG_DWORD_BIG_ENDIAN":
					case "REG_DWORD": return RegistryValueKind.DWord;
					case "REG_EXPAND_SZ": return RegistryValueKind.ExpandString;
					case "REG_NONE": return RegistryValueKind.None;
					case "REG_QWORD_LITTLE_ENDIAN":
					case "REG_QWORD": return RegistryValueKind.QWord;
					case "REG_SZ": return RegistryValueKind.String;
					case "REG_RESOURCE_LIST":
					case "REG_RESOURCE_REQUIREMENTS_LIST":
					case "REG_FULL_RESOURCE_DESCRIPTOR":
					case "REG_LINK": return RegistryValueKind.Unknown;

					#endregion

					default: throw new ArgumentOutOfRangeException();
				}
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

					string kind = Win32ApiRepresentation(registryValueKind);
					if (!ExecuteExecuteable(
						    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "reg.exe"),
						    $" add \"{valueLocation.RegistryKey}\" /v \"{valueLocation.ValueName}\" /t {kind} /d \"{value}\" /f",
						    out string[] _, out int tmpExitCode, out _, true, true, true, true, asUser) || tmpExitCode == 1) {
						return false;
					}

					return true;
				}

				try {
					Registry.SetValue(valueLocation.RegistryKey, valueLocation.ValueName, content, registryValueKind);
				}
				catch (SecurityException) {
					if (MessageBox.Show(
						    string.Format(
							    WrapperStrings.SetRegistryValue_Security,
							    valueLocation.ValueName, valueLocation.RegistryKey, content, registryValueKind),
						    WrapperStrings.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
						RestartAsAdministrator();
						Environment.Exit(0);
					}

					return false;
				}
				catch (UnauthorizedAccessException) {
					if (MessageBox.Show(
						    string.Format(
							    WrapperStrings.SetRegistryValue_UnauthorizedAccess,
							    valueLocation.ValueName, valueLocation.RegistryKey, content, registryValueKind),
						    WrapperStrings.Error,
						    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
						RestartAsAdministrator();
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
						$"[{toSet.RegistryKey}]",
						$"\"{AddBackslahes(toSet.ValueName)}\"={toWrite}"
					});
				// I know that the following is exploiting UAC a bit, but the Warning will not be suppressed, so I don´t see any real reasons not to do that
				ApplyRegfile();
			}

			/// <summary>
			///  Applies a Regfile generated using SetProtectedRegistryValue
			/// </summary>
			public static void ApplyRegfile() {
				const string folderName = "StorageManagementToolRegistryData";
				ExecuteExecuteable(ExplorerPath,
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
						toWrite = $"\"{AddBackslahes((string) content)}\"";
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
}