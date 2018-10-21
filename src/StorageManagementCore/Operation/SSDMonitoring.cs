using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Xml.Linq;
using StorageManagementCore.Backend;

namespace StorageManagementCore.Operation {
	public static class SSDMonitoring {
		/// <summary>
		///  Name of the monitoring task
		/// </summary>
		private const string SSDMonitoringTaskName = "StorageManagementTool_SSDMonitoring";

		/// <summary>
		///  Path to the SCHTASKS.exe
		/// </summary>
		private static readonly string SchtasksPath =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "SCHTASKS.exe");

		/// <summary>
		///  Initalizes SSD monitoring
		/// </summary>
		/// <returns>Whether the initalization process were successful</returns>
		public static bool InitalizeSSDMonitoring() {
			XNamespace taskNamespace =
				XNamespace.Get("http://schemas.microsoft.com/windows/2004/02/mit/task");
			XElement taskContents = new XElement(taskNamespace + "Task", new XAttribute("version", "1.4"),
				new XElement(taskNamespace + "RegistrationInfo",
					new XElement(taskNamespace + "Date", Wrapper.DateTimeToWin32Format(DateTime.Now)),
					new XElement(taskNamespace + "Author", WindowsIdentity.GetCurrent().Name),
					new XElement(taskNamespace + "Description",
						"Monitors a list of configured paths"),
					new XElement(taskNamespace + "URI", $"\\{SSDMonitoringTaskName}")),
				new XElement(taskNamespace + "Triggers",
					new XElement(taskNamespace + "LogonTrigger", new XElement(taskNamespace + "Enabled", true))),
				new XElement(taskNamespace + "Principals",
					new XElement(taskNamespace + "Principal", new XAttribute("id", "Author"),
						new XElement(taskNamespace + "GroupId", "S-1-5-32-545"),
						new XElement(taskNamespace + "RunLevel", "HighestAvailable"))),
				new XElement(taskNamespace + "Settings",
					new XElement(taskNamespace + "MultipleInstancesPolicy", "Parallel"),
					new XElement(taskNamespace + "DisallowStartIfOnBatteries", false),
					new XElement(taskNamespace + "StopIfGoingOnBatteries", false),
					new XElement(taskNamespace + "AllowHardTerminate", false),
					new XElement(taskNamespace + "StartWhenAvailable", false),
					new XElement(taskNamespace + "RunOnlyIfNetworkAvailable", false),
					new XElement(taskNamespace + "IdleSettings", new XElement(taskNamespace + "StopOnIdleEnd", true),
						new XElement(taskNamespace + "RestartOnIdle", false)),
					new XElement(taskNamespace + "AllowStartOnDemand", true),
					new XElement(taskNamespace + "Enabled", true), new XElement(taskNamespace + "Hidden", false),
					new XElement(taskNamespace + "RunOnlyIfIdle", false),
					new XElement(taskNamespace + "DisallowStartOnRemoteAppSession", false),
					new XElement(taskNamespace + "UseUnifiedSchedulingEngine", true),
					new XElement(taskNamespace + "WakeToRun", false),
					new XElement(taskNamespace + "ExecutionTimeLimit", "PT0S"),
					new XElement(taskNamespace + "Priority", 7)),
				new XElement(taskNamespace + "Actions", new XAttribute("Context", "Author"),
					new XElement(taskNamespace + "Exec",
						new XElement(taskNamespace + "Command", Process.GetCurrentProcess().MainModule.FileName),
						new XElement(taskNamespace + "Arguments", "/background"))));
			string taskString = new XDocument(new XDeclaration("1.0", "UTF-16", null),
				taskContents).ToString();

			string tempLocation = Path.Combine(Path.GetTempPath(), "StorageManagementToolTask.xml");
			File.WriteAllText(tempLocation, taskString);
			bool tempShown;
			if (tempShown=!Program.isConsoleVisible) {
				Program.SetConsoleVisibility(true);
				Console.Clear();
			}

			string tmp = Console.Title;
			Console.Title = SchtasksPath;
			if (!Wrapper.ExecuteCommand(
				$"\"{SchtasksPath}\" /Create /XML \"{tempLocation}\" /TN {SSDMonitoringTaskName} /RP * /RU {Environment.UserName}",
				true, false
			)) {
				return false;
			}

			if (tempShown) {
				Program.SetConsoleVisibility(false);
			}

			Console.Title = tmp;
			return true;
		}

		/// <summary>
		///  Checks whether SSD monitoring has been initialized
		/// </summary>
		/// <param name="initalized">Whether SSD monitoring has been initialized</param>
		/// <returns>Whether the check were successful</returns>
		public static bool SSDMonitoringInitalized(out bool initalized) {
			initalized = false;
			if (!Wrapper.Execute(SchtasksPath, $"/QUERY /TN {SSDMonitoringTaskName}", out string[] _,
				out int returnCode, out int _, true, true, true)) {
				return false;
			}

			initalized = returnCode == 0;
			return true;
		}

		/// <summary>
		///  Checks whether SSD monitoring has been initalized
		/// </summary>
		/// <returns>Whether SSD monitoring has been initalized</returns>
		public static bool SSDMonitoringInitalized() => SSDMonitoringInitalized(out bool enabled) && enabled;

		/// <summary>
		///  Checks if SSD monitoring is enabled
		/// </summary>
		/// <param name="enabled">Whether SSD monitoring</param>
		/// <returns>Whether the check were successful</returns>
		public static bool SSDMonitoringEnabled(out bool enabled) {
			enabled = false;
			//From https://superuser.com/a/1035052 last access 10.02.2018
			if (!Wrapper.RunPowershellCommand(out IEnumerable<string> ret,
				$"(Get-ScheduledTask | Where TaskName -eq {SSDMonitoringTaskName} ).State")) {
				return false;
			}

			using (IEnumerator<string> enumerator = ret.GetEnumerator()) {
				if (!enumerator.MoveNext()) {
					return false;
				}

				enabled = enumerator.Current == "Enabled" || enumerator.Current == "Ready";
				return true;
			}
		}

		/// <summary>
		///  Checks if SSD monitoring is enabled
		/// </summary>
		/// <returns>Whether SSD monitoring</returns>
		public static bool SSDMonitoringEnabled() {
			bool success = SSDMonitoringEnabled(out bool enabled);
			return success && enabled;
		}

		/// <summary>
		///  Sets whether SSD monitoring is enabled
		/// </summary>
		/// <param name="enable">Whether the monitoring should be enabled or disabled</param>
		/// <param name="checkForInitialize">
		///  Whether to check if SSD monitoring were already initialized if it were not initialized
		///  it will be
		/// </param>
		/// <returns>Whether the operation were successful</returns>
		public static bool SetSSDMonitoring(bool enable, bool checkForInitialize = true) {
			if (checkForInitialize) {
				if (!SSDMonitoringInitalized(out bool initialized)) {
					return false;
				}

				if (!initialized) {
					if (!InitalizeSSDMonitoring()) {
						return false;
					}
				}
			}

			return Wrapper.Execute(SchtasksPath,
				$"/CHANGE /TN {SSDMonitoringTaskName} {(enable ? "/ENABLE" : "/DISABLE")}", out string[] _,out int _,out int _,waitForExit: true,admin: true);
		}
	}
}