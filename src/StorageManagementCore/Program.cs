using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using StorageManagementCore.WPFGUI;
using UniversalCLIProvider;
using UniversalCLIProvider.Interpreters;

//TODO Rename Directory methods to Folder
namespace StorageManagementCore {
	/// <summary>
	///  Main class of this Program
	/// </summary>
	public static class Program {
		/// <summary>
		///  The version tag of this Version
		/// </summary>
		/// <remarks>This should be kept equivalent to the Git Release tag</remarks>
		public const string VersionTag = "1.1-b-1.1";

		public static readonly CultureInfo[][] AvailableSpecificCultures =
			{new[] {CultureInfo.CreateSpecificCulture("en-US")}, new[] {CultureInfo.CreateSpecificCulture("de-DE")}};

		/// <summary>
		///  A reference to an object containig methods for the console IO operations
		/// </summary>
		public static ConsoleIO ConsoleIOObject { get; set; }

//		/// <summary>
//		///  Whether the Programm runs from any shell / commandline
//		/// </summary>
//		public static bool CommandLineMode { get; private set; }

		//based upon https://www.dotnetperls.com/list-equals last access 
		public static bool UnorderedEqual<T>(this ICollection<T> a, ICollection<T> b) {
			// 1
			// Require that the counts are equal
			if (a.Count != b.Count) {
				return false;
			}

			// 2
			// Initialize new Dictionary of the type
			Dictionary<T, int> d = new Dictionary<T, int>();
			// 3
			// Add each key's frequency from collection A to the Dictionary
			foreach (T item in a) {
				if (d.TryGetValue(item, out int c)) {
					d[item] = c + 1;
				}
				else {
					d.Add(item, 1);
				}
			}

			// 4
			// Add each key's frequency from collection B to the Dictionary
			// Return early if we detect a mismatch
			foreach (T item in b) {
				if (d.TryGetValue(item, out int c)) {
					if (c == 0) {
						return false;
					}
					else {
						d[item] = c - 1;
					}
				}
				else {
					// Not in dictionary
					return false;
				}
			}

			// 5
			// Verify that all frequencies are zero
			foreach (int v in d.Values) {
				if (v != 0) {
					return false;
				}
			}

			// 6
			// We know the collections are equal
			return true;
		}

		public static char GetDriveLetter(this DriveInfo d) => d.Name.First();

		[STAThread, PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public static void Main(string[] args) {
			FileInfo parentName = new FileInfo(Process.GetCurrentProcess().ProcessName);
			//CommandLineMode = parentName.Name == "cmd.exe" || parentName.Name == "powershell.exe";
			//SetConsoleVisibility(CommandLineMode);

			new Session();
			ProcessCommandlineArguments(args);
		}

		/// <summary>
		///  Processes arguments
		/// </summary>
		/// <param name="args">The arguments to process</param>
		private static void ProcessCommandlineArguments(string[] args) {
			if (false) {
				//For debugging purposes only
				MessageBox.Show(string.Join(" , ", args));
			}

			new CommandlineOptionInterpreter(args) {Options = new InterpretingOptions {RootName = "StorageManagementTool"}}
				.Interpret<CmdRootContext>(() => new MainWindow().ShowDialog());
			/*
			if (args.Count == 0) {
			   Session.Singleton.StandardLaunch();
			   Environment.Exit(0);
			}
			else {
			   switch (args[0]) {
			      case "-?":
			         if (CommandLineMode) {
			            ConsoleIO.WriteLine("Help: ");
			            ConsoleIO.WriteLine("Hint: use subcontexts with -? to show help for the subcontext");
			            ConsoleIO.WriteLine("-?                        Shows this help");
			            ConsoleIO.WriteLine("-background               Starts SSDHDDTool in background mode");
			            ConsoleIO.WriteLine("-move                     Moves a file or a folder");
			            ConsoleIO.WriteLine("-config                   Edits or Views Config");
			         }

			         Environment.Exit(-10);
			         //Help
			         break;
			      case "-background":
			         if (args.Count == 1) {
			            BackgroundNotificationCreator.Initalize();
			         }

			         else if (args[1] == "-?") {
			            ConsoleIO.WriteLine("Help for -background");
			            ConsoleIO.WriteLine("-?       Shows this help");
			            ConsoleIO.WriteLine("else    Starts SSDHDDTool in background mode");
			         }
			         else {
			            ArgumentError(args);
			         }

			         break;
			      case "-move":
			         if (args.Count >= 2) {
			            if (args[1] == "-?") {
			               ConsoleIO.WriteLine("Help for -background");
			               ConsoleIO.WriteLine("-?              Shows this help");
			               ConsoleIO.WriteLine("-file           Stores a file at a new location");
			               ConsoleIO.WriteLine("-folder         Stores a folder at a new location");
			               ConsoleIO.WriteLine(
			                  "-auto-detect    Detects automatically, whether the given object is a file or a folder");
			               ConsoleIO.WriteLine("-srcpath        (Path of the file or folder to move)");
			               ConsoleIO.WriteLine("[-NewPath]      (Path, where file or folder should be stored");
			            }
			            else if (new[] {
			               "-file", "-folder", "-auto-detect"
			            }.Contains(args[1])) {
			               MoveObjectFromCommandline(args);
			            }
			            else {
			               ArgumentError(args);
			            }
			         }

			         break;
			      case "-config":
			         if (args.Count >= 2) {
			            switch (args[1]) {
			               case "-?" when args.Count == 2:
			                  ConsoleIO.WriteLine("Help for -config");
			                  ConsoleIO.WriteLine("-?       Shows this help");
			                  ConsoleIO.WriteLine("-set     sets a config value");
			                  ConsoleIO.WriteLine("-get     gets a config value");
			                  break;
			               case "-get":
			                  if (args[2] == "-?") {
			                     ConsoleIO.WriteLine("Help for -config -get");
			                     ConsoleIO.WriteLine("-?       Shows this help");
			                     ConsoleIO.WriteLine("-ViewedName=(ViewedName of the config value to get)");
			                  }
			                  else if (args[2].StartsWith("-ViewedName=")) {
			                     switch (args[2].Split('=')[1]) {
			                        case "Root.DefaultHDDPath":
			                           ConsoleIO.WriteLine("Value of Root.DefaultHDDPath:");
			                           ConsoleIO.WriteLine(Session.Singleton.CurrentConfiguration.DefaultHDDPath);

			                           break;
			                        default:
			                           ArgumentError(args);
			                           break;
			                     }
			                  }
			                  else if (args[2] == "-ViewedName" && args[3] == " -?") {
			                     ConsoleIO.WriteLine("Help for -ViewedName Argument in -config -get context");
			                     ConsoleIO.WriteLine("-ViewedName can be have the Following Values:");
			                     ConsoleIO.WriteLine("Root.DefaultHDDPath");
			                  }
			                  else {
			                     ArgumentError(args);
			                  }

			                  break;
			               case "-set":
			                  if (args.Count >= 3) {
			                     if (args[2].StartsWith("-name") && args.Count == 6 &&
			                         args[4].StartsWith("-value")) {
			                        switch (args[3]) {
			                           case "Root.DefaultHDDPath":
			                              try {
			                                 Session.Singleton.CurrentConfiguration.DefaultHDDPath = args[5];
			                              }
			                              catch (Exception) {
			                                 ConsoleIO.WriteLine("Operation failed!");
			                                 return;
			                              }

			                              ConsoleIO.WriteLine("Operation completed");
			                              break;
			                           //case "Root.SSDMonitoring.Enabled":
			                           //   try
			                           //   {
			                           //      Session.Singleton.CurrentConfiguration.MonitoringSettings
			                           //            .SSDMonitoringEnabled =
			                           //         bool.Parse(args[5]);
			                           //   }
			                           //   catch (Exception)
			                           //   {
			                           //      ConsoleIO.WriteLine("Operation failed!");
			                           //      return;
			                           //   }

			                           //   ConsoleIO.WriteLine("Operation completed");
			                           //   break;
			                           default:
			                              ArgumentError(args);
			                              break;
			                        }

			                        ArgumentError(args);
			                     }
			                     else {
			                        switch (args[2]) {
			                           case "-?":
			                              ConsoleIO.WriteLine("Help for -config -set");
			                              ConsoleIO.WriteLine("-?       Shows this help");
			                              ConsoleIO.WriteLine("-ViewedName=(ViewedName of the config value to get)");
			                              ConsoleIO.WriteLine("-Value=(New Value for the Config Setting)");
			                              break;
			                           case "-value" when args[1] == "-?":
			                              ConsoleIO.WriteLine(
			                                 "Help for -ViewedName Argument in -config -value context");
			                              ConsoleIO.WriteLine("-ViewedName can be have the Following Values:");
			                              ConsoleIO.WriteLine("Root.DefaultHDDPath");
			                              ConsoleIO.WriteLine("Root.SSDMonitoring.Enabled");
			                              break;
			                           case "-name" when args[3] == " -?":
			                              ConsoleIO.WriteLine(
			                                 "Help for -ViewedName Argument in -config -get context");
			                              ConsoleIO.WriteLine("-ViewedName can be have the Following Values:");
			                              ConsoleIO.WriteLine("Root.DefaultHDDPath");
			                              ConsoleIO.WriteLine("Root.SSDMonitoring.Enabled");
			                              break;
			                           default:
			                              ArgumentError(args);
			                              break;
			                        }
			                     }

			                     break;
			                  }

			                  ArgumentError(args);
			                  break;
			               default:
			                  ArgumentError(args);
			                  break;
			            }
			         }

			         break;
			      default:
			         ArgumentError(args);
			         break;
			   }
			}
}

/// <summary>
///    Processes arguments for -move
/// </summary>
/// <param name="args">The arguments to process</param>
private static void MoveObjectFromCommandline(List<string> args) {
			if (args.Count >= 3) {
			   string oldPath;
			   if (args[2] == "-srcpath") {
			      oldPath = args[3];
			   }
			   else {
			      ArgumentError(args);
			      return;
			   }

			   bool fileOrFolder;
			   switch (args[1]) {
			      case "-file":
			         fileOrFolder = true;
			         break;
			      case "-folder":
			         fileOrFolder = false;
			         break;
			      case "-auto-detect":
			         if (File.Exists(oldPath)) {
			            fileOrFolder = true;
			         }
			         else if (Directory.Exists(oldPath)) {
			            fileOrFolder = false;
			         }
			         else {
			            ArgumentError(args);
			            return;
			         }

			         break;
			      default: return;
			   }

			   string newPath;
			   if (args.Count == 6) {
			      if (args[3].StartsWith("-newpath")) {
			         newPath = args[5];
			      }
			      else {
			         ArgumentError(args);
			         return;
			      }
			   }
			   else {
			      newPath = Path.Combine(
			         Session.Singleton.CurrentConfiguration.DefaultHDDPath, oldPath.Remove(1, 1));
			   }

			   if (fileOrFolder) {
			      OperatingMethods.MoveFile(new FileInfo(oldPath), new FileInfo(newPath));
			   }
			   else {
			      OperatingMethods.MoveFolderPhysically(new DirectoryInfo(oldPath), new DirectoryInfo(newPath));
			   }

			   //   OperatingMethods.MoveFolderOrFile(newPath, oldPath, fileOrFolder);
			}
			else {
			   ArgumentError(args);
			}
			*/
		}

		public static void SetConsoleVisibility(bool visible) {
			ShowWindow(GetConsoleWindow(), visible ? SW_SHOW : SW_HIDE);
		}

		#region From https://stackoverflow.com/a/3571628/6730162 access on 08.01.2018

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public const int SW_HIDE = 0;
		public const int SW_SHOW = 5;

		#endregion
	}
}