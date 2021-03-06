﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Security;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using StorageManagementCore.GlobalizationResources;
using StorageManagementCore.Operation;

namespace StorageManagementCore.Backend {
	/// <summary>
	///  Contains system functionalities, which are not specific for this project
	/// </summary>
	public static class Wrapper {
		/// <summary>
		///  The file extensions, which are executable as standalone
		/// </summary>
		private static readonly IEnumerable<string>
			executableExtensions = new[] {".exe", ".pif", ".com", ".bat", ".cmd"};

		/// <summary>
		///  The path of the Windows Explorer
		/// </summary>
		public static readonly string ExplorerPath =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe");


		//    public static (T1, T2) ToDirectTuple<T1, T2>(this Tuple<T1, T2> myTuple) => (myTuple.Item1, myTuple.Item2);

		/// <summary>
		///  Executes an executable
		/// </summary>
		/// <param name="filename">The name of the File to execute</param>
		/// <param name="parameters">The parameters to use when starting the file</param>
		/// <param name="admin">Whether the file should be executed with</param>
		/// <param name="hidden">Whether the Main Window of this executable (if exists) should be shown</param>
		/// <param name="waitForExit">Whether the code should wait until the executable exited</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool Execute(string filename, string parameters, bool admin = false,
			bool hidden = false, bool waitForExit = false) => Execute(filename, parameters, out string[] _, out int _,
			out _,
			waitForExit: waitForExit, hidden: hidden,
			admin: admin);

		/// <summary>
		///  Executes an executable
		/// </summary>
		/// <param name="filename">The name of the file to execute</param>
		/// <param name="parameters">The parameters to use when starting the file</param>
		/// <param name="returnData"> The String returned by the file</param>
		/// <param name="exitCode"> The exit code returned by the executable, only available if waitForExit=true</param>
		/// <param name="pid"></param>
		/// <param name="readReturnData">Whether to Read the output of the executable started</param>
		/// <param name="waitForExit">Whether the code should wait until the executable exited</param>
		/// <param name="hidden">Whether the main window of this executable (if existing) should be hidden</param>
		/// <param name="admin">Whether the file should be executed with</param>
		/// <param name="getPID"></param>
		/// <returns>Whether the operation were successful</returns>
		public static bool Execute(string filename, string parameters, out string[] returnData,
			out int exitCode, out int pid, bool readReturnData = false, bool waitForExit = false, bool hidden = false,
			bool admin = false, bool getPID = false) {
			if (Session.Singleton.IsAdmin) {
				admin = false;
			}
			pid = 0;
			FileInfo toRun = new FileInfo(filename);
			exitCode = 0;
			returnData = null;
			if (!toRun.Exists) {
				if (MessageBox.Show(
					    string.Format(WrapperStrings.ExecuteExecutable_FileNotFound_Text, toRun.FullName),
					    WrapperStrings.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
					OpenFileDialog alternativeExecutableSel = new OpenFileDialog {
						Filter =
							$"{WrapperStrings.ExecuteExecutable_FileNotFound_SelectionFilter}|*{string.Join(";*", executableExtensions)}",
						Title = string.Format(WrapperStrings.ExecuteExecutable_FileNotFound_SelectionTitle, toRun.Name)
					};
					alternativeExecutableSel.ShowDialog();
					return Execute(alternativeExecutableSel.FileName, parameters, out returnData,
						out exitCode, out pid, waitForExit: waitForExit, hidden: hidden, admin: admin,
						getPID: getPID);
				}
			}

			if (!executableExtensions.Contains(toRun.Extension)) {
				if (new DialogResult[] {DialogResult.No, DialogResult.None}.Contains(MessageBox.Show(
					string.Format(WrapperStrings.ExecuteExecutable_WrongEnding,
						toRun.FullName, toRun.Extension),
					WrapperStrings.Error,
					MessageBoxButtons.YesNo, MessageBoxIcon.Error))) {
					return false;
				}
			}

			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo {
				WindowStyle = hidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
				FileName = toRun.FullName,
				Arguments = parameters,
				RedirectStandardOutput = readReturnData
			};
			if (readReturnData && admin) { }

			startInfo.UseShellExecute = false;
			if (admin) {
				startInfo.Verb = "runas";
				startInfo.UseShellExecute = true;
			}

			if (!waitForExit) {
				process.Exited += (emitter, args) => ((Process) emitter).Dispose();
			}

			process.StartInfo = startInfo;
			try {
				process.Start();
			}
			catch (Win32Exception) {
				DialogResult retry = MessageBox.Show(
					string.Format(
						WrapperStrings.ExecuteExecutable_AdminError,
						toRun.FullName),
					WrapperStrings.Error, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
				switch (retry) {
					case DialogResult.Retry:
						Execute(filename, parameters, admin, hidden, waitForExit);
						break;
					case DialogResult.Ignore:
						Execute(filename, parameters, false, hidden, waitForExit);
						break;
					default: return false;
				}
			}

			if (getPID) {
				pid = process.Id;
			}

			if (waitForExit) {
				process.WaitForExit();
				if (readReturnData) {
					returnData = process.StandardOutput.FromStream();
				}

				exitCode = process.ExitCode;

				process.Dispose();
			}

			return true;
		}

		public static IEnumerable<DriveInfo> GetDrives() => FileSystem.Drives;

		/// <summary>
		///  Executes an Command using Windows Commandline
		/// </summary>
		/// <param name="cmd">The Command to Execute</param>
		/// <param name="admin">Whether the Command should be executed with</param>
		/// <param name="hidden">Whether to hide the Commandline </param>
		/// <param name="waitforexit">
		///  Whether to wait until the command execution completed
		/// </param>
		/// <param name="asUser"></param>
		/// <param name="debug">
		///  Whether to run the command in debug mode
		/// </param>
		/// <returns>Whether the operation were successful</returns>
		public static bool ExecuteCommand(string cmd, bool admin, bool hidden, bool waitforexit = true,
			bool asUser = false,
			bool debug = false) => ExecuteCommand(cmd, admin, hidden, out string[] _, waitforexit, debug, asUser);

		/// <summary>
		///  Adds a Backslash for each quotation mark and backslash
		/// </summary>
		/// <param name="source">The string to add backslashes to</param>
		/// <returns>The  source with backslashes</returns>
		public static string AddBackslashes(string source) => source.Replace("\\", "\\\\").Replace("\"", "\\\"");

		/// <summary>
		///  Checks if one Path is the parent of another
		/// </summary>
		/// <param name="parentPath">The parent path</param>
		/// <param name="childPath">The child path</param>
		/// <returns>Whether parentPath is a paren of childPath</returns>
		public static bool IsSubfolder(DirectoryInfo parentPath, DirectoryInfo childPath) =>
			parentPath.FullName.StartsWith(childPath.FullName + '\\');

		/// <summary>
		///  Executes an Command using Windows Commandline
		/// </summary>
		/// <param name="cmd">The Command to Execute</param>
		/// <param name="admin">Whether the Command should be executed with</param>
		/// <param name="hidden">Whether to hide the Commandline </param>
		/// <param name="returnData">The Data returned by the executable</param>
		/// <param name="waitforexit">
		///  Whether to wait until the command execution completed
		/// </param>
		/// <param name="debug">
		///  Whether to run the command in debug mode
		/// </param>
		/// <param name="readReturnData">Whether to read the output of the Application</param>
		/// <param name="asUser">Whether to run the command as user</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool ExecuteCommand(string cmd, bool admin, bool hidden, out string[] returnData,
			bool waitforexit = true,
			bool debug = false, bool readReturnData = false, bool asUser = false) =>
			Execute(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
				$"/S {(debug ? "/K " : "/C ")}\"{cmd}\"", out returnData, out int tmp, out int _, readReturnData, waitforexit, hidden, admin)
			&& tmp == 0;

		#region From https://stackoverflow.com/a/3600342/6730162 access on 30.9.2017

		/// <summary>
		///  Tests whether the program is being executed with Admin privileges
		/// </summary>
		/// <returns>Whether the Program is being executed with Admin privileges</returns>
		public static bool IsCurrentUserAdministrator() {
			try {
				return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
				//   new UserPrincipal().IsMemberOf(new )
			}
			catch (Exception) {
				return false;
			}
		}

		#endregion

		/// <summary>
		///  Restarts Program as Administrator
		/// </summary>
		public static void RestartProgram(bool administrator,string[] parameters= null,bool hexWrapper=false) {
			parameters=parameters?? new string[0];
			if (Execute(Process.GetCurrentProcess().MainModule.FileName,(hexWrapper?"-Master:Hex "+UniversalCLIProvider.CommandlineMethods.toHexArgumentString(parameters):string.Join(" ", parameters)) ,
				administrator)) {
				Environment.Exit(0);
			}
		}
		
		/// <summary>
		///  Reads the whole content of an StreamReader
		/// </summary>
		/// <param name="reader">The StreamReader to read from</param>
		/// <returns>The strings saved in the StreamReader</returns>
		private static string[] FromStream(this TextReader reader) {
			List<string> ret = new List<string>();
			string line;
			while ((line = reader.ReadLine()) != null) {
				ret.Add(line);
			}

			return ret.ToArray();
		}

		/// <summary>
		///  Checks whether a local user with the given name exists
		/// </summary>
		/// <param name="name">The name of the user</param>
		/// <returns>Whether the username exists</returns>
		public static bool IsUser(string name) =>
			UserPrincipal.FindByIdentity(GetPrincipalContext(), IdentityType.SamAccountName, name) != null;

		/// <summary>
		///  Checks whether a given user is a local administrator
		/// </summary>
		/// <param name="username">The username</param>
		/// <returns></returns>
		public static bool IsAdmin(string username) {
			UserPrincipal user = UserPrincipal
				.FindByIdentity(GetPrincipalContext(), IdentityType.SamAccountName, username);

			if (user is null) {
				return false;
			}

			return user.IsMemberOf(GetPrincipalContext(), IdentityType.Sid, "S-1-5-32-544");
		}

		/// <summary>
		///  Converts a IEnumerable of KeyValuePairs to the appropriate dictionary
		/// </summary>
		/// <typeparam name="TKey">The Key Type of the dictionary</typeparam>
		/// <typeparam name="TValue">The Value type of the dictionary</typeparam>
		/// <param name="source">The IEnumerable of KeyValuePairs to use</param>
		/// <returns>A dictionary containing all KeyValuePairs of the source</returns>
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
			this IEnumerable<KeyValuePair<TKey, TValue>> source) {
			return source.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
		}

		/// <summary>
		///  Converts a DateTime to its Win32 representation
		/// </summary>
		/// <param name="toConvert">The DateTime to convert</param>
		/// <returns>The Win32 representation of the DateTime object</returns>
		public static string DateTimeToWin32Format(DateTime toConvert) =>
			$"{toConvert.Year:0000}-{toConvert.Month:00}-{toConvert.Day:00}T{toConvert.Hour:00}:{toConvert.Minute:00}:{toConvert.Second:00}.{toConvert.Millisecond:000}0000";

		/// <summary>
		///  Loads the local PrincipalContext
		/// </summary>
		/// <returns>The local PrincipalContext</returns>

		#region Based upon https://stackoverflow.com/a/3681442/6730162 last access 18.02.2018

		private static PrincipalContext GetPrincipalContext() {
			return new PrincipalContext(ContextType.Machine);
		}

		#endregion

		/// <summary>
		///  Runs a stack of powershell commands
		/// </summary>
		/// <param name="ret">What the commands returned</param>
		/// <param name="command">The commands to run</param>
		/// <returns>Whether the invokation of the commands were successful</returns>

		#region Based upon https://blogs.msdn.microsoft.com/kebab/2014/04/28/executing-powershell-scripts-from-c/ last access 10.02.2018

		public static bool RunPowershellCommand(out IEnumerable<string> ret, params string[] command) {
			ret = new[] {""};
			IEnumerable<PSObject> returned;
			using (PowerShell powerShellInstance = PowerShell.Create()) {
				foreach (string s in command) {
					powerShellInstance.AddScript(s);
				}

				try {
					{
						returned = powerShellInstance.Invoke();
					}
				}
				catch (Exception) {
					return false;
				}
			}

			ret = returned.Where(x => x != null).Select(x => x.ToString());
			return true;
		}

		#endregion

		public static bool RestartComputer() => Execute(
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shutdown.exe"), "/R /T 1", false,
			true);
	}
}