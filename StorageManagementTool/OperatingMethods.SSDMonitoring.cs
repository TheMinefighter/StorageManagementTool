using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Xml.Linq;
using StorageManagementTool.GlobalizationRessources;
using File = System.IO.File;

namespace StorageManagementTool
{
   public static partial class OperatingMethods
   {
      public static class SSDMonitoring
      {
         /// <summary>
         /// Name of the monitoring task
         /// </summary>
         private const string SSDMonitoringTaskName = "StorageManagementTool_SSDMonitoring";
         /// <summary>
         /// Path to the SCHTASKS.exe
         /// </summary>
         private static readonly string SchtasksPath = Path.Combine(Wrapper.System32Path, "SCHTASKS.exe");
         /// <summary>
         /// Initalizes SSD monitoring
         /// </summary>
         /// <returns>Whether the initalization process were successful</returns>
         public static bool InitalizeSSDMonitoring()
         {          XNamespace TaskNamespace =
            XNamespace.Get("http://schemas.microsoft.com/windows/2004/02/mit/task");
            XElement taskContents = new XElement(TaskNamespace + "Task", new XAttribute("version", "1.4"),
               new XElement(TaskNamespace + "RegistrationInfo",
                  new XElement(TaskNamespace + "Date", Wrapper.DateTimeToWin32Format(DateTime.Now)),
                  new XElement(TaskNamespace + "Author", WindowsIdentity.GetCurrent().Name),
                  new XElement(TaskNamespace + "Description",
                     "Monitors a list of configured paths"),
                  new XElement(TaskNamespace + "URI", $"\\{SSDMonitoringTaskName}")),
               new XElement(TaskNamespace + "Triggers",
                  new XElement(TaskNamespace + "LogonTrigger", new XElement(TaskNamespace + "Enabled", "true"))),
               new XElement(TaskNamespace + "Principals",
                  new XElement(TaskNamespace + "Principal", new XAttribute("id", "Author"),
                     new XElement(TaskNamespace + "GroupId", "S-1-5-32-545"),
                     new XElement(TaskNamespace + "RunLevel", "HighestAvailable"))),
               new XElement(TaskNamespace + "Settings",
                  new XElement(TaskNamespace + "MultipleInstancesPolicy", "Parallel"),
                  new XElement(TaskNamespace + "DisallowStartIfOnBatteries", "false"),
                  new XElement(TaskNamespace + "StopIfGoingOnBatteries", "false"),
                  new XElement(TaskNamespace + "AllowHardTerminate", "false"),
                  new XElement(TaskNamespace + "StartWhenAvailable", "false"),
                  new XElement(TaskNamespace + "RunOnlyIfNetworkAvailable", "false"),
                  new XElement(TaskNamespace + "IdleSettings", new XElement(TaskNamespace + "StopOnIdleEnd", "true"),
                     new XElement(TaskNamespace + "RestartOnIdle", "false")),
                  new XElement(TaskNamespace + "AllowStartOnDemand", "true"),
                  new XElement(TaskNamespace + "Enabled", "true"), new XElement(TaskNamespace + "Hidden", "false"),
                  new XElement(TaskNamespace + "RunOnlyIfIdle", "false"),
                  new XElement(TaskNamespace + "DisallowStartOnRemoteAppSession", "false"),
                  new XElement(TaskNamespace + "UseUnifiedSchedulingEngine", "true"),
                  new XElement(TaskNamespace + "WakeToRun", "false"),
                  new XElement(TaskNamespace + "ExecutionTimeLimit", "PT0S"),
                  new XElement(TaskNamespace + "Priority", "7")),
               new XElement(TaskNamespace + "Actions", new XAttribute("Context", "Author"),
                  new XElement(TaskNamespace + "Exec",
                     new XElement(TaskNamespace + "Command", Process.GetCurrentProcess().MainModule.FileName),
                     new XElement(TaskNamespace + "Arguments", "/background"))));
            string taskString = new XDocument(new XDeclaration("1.0", "UTF-16", null),
               taskContents).ToString();

            string tempLocation = Path.Combine(Path.GetTempPath(), "StorageManagementToolTask.xml");
            File.WriteAllText(tempLocation, taskString);
            return Wrapper.ExecuteExecuteable(SchtasksPath,
               $"/Create /XML \"{tempLocation}\" /TN {SSDMonitoringTaskName} /RP * /RU {Environment.UserName}",
               out string[] _, out int _, false, true, false, true);
         }
         /// <summary>
         /// Checks whether SSD monitoring has been initalized
         /// </summary>
         /// <param name="initalized">Whether SSD monitoring has been initalized</param>
         /// <returns>Whether the check were successful</returns>
         public static bool SSDMonitoringInitalized(out bool initalized)
         {
            initalized = false;
            if (!Wrapper.ExecuteExecuteable(SchtasksPath, $"/QUERY /TN {SSDMonitoringTaskName}", out string[] _,
               out int returnCode, true, true, true))
            {
               return false;
            }
            initalized = returnCode == 0;
            return true;
         }

         /// <summary>
         /// Checks whether SSD monitoring has been initalized
         /// </summary>
         /// <returns>Whether SSD monitoring has been initalized</returns>
         public static bool SSDMonitoringInitalized()
         {
            bool success = SSDMonitoringInitalized(out bool enabled);
            return success && enabled;
         }
         /// <summary>
         /// Checks if SSD monitoring is enabled
         /// </summary>
         /// <param name="enabled">Whether SSD monitoring</param>
         /// <returns>Whether the check were successful</returns>
         public static bool SSDMonitoringEnabled(out bool enabled)
         {
            enabled = false;
            //From https://superuser.com/a/1035052 last access 10.02.2018
            if (!Wrapper.RunPowershellCommand(out IEnumerable<string> ret, $"(Get-ScheduledTask | Where TaskName -eq {SSDMonitoringTaskName} ).State"))
            {

               return false;
            }

            if (!ret.Any())
            {
               return false;
            }
            enabled = ret.ElementAt(0) == "Enabled" || ret.ElementAt(0) == "Ready";
            return true;

         }
         /// <summary>
         /// Checks if SSD monitoring is enabled
         /// </summary>
         /// <returns>Whether SSD monitoring</returns>
         public static bool SSDMonitoringEnabled()
         {
            bool success = SSDMonitoringEnabled(out bool enabled);
            return success && enabled;
         }

         /// <summary>
         /// Sets whether SSD monitoring is enabled
         /// </summary>
         /// <param name="enable">Whether the monitoring should be enabled or disabled</param>
         /// <param name="checkForInitalize">Whether to check if SSD monitoring were allready initalized if it were not initalized it will be</param>
         /// <returns>Whether the operation were successful</returns>
         public static bool SetSSDMonitoring(bool enable, bool checkForInitalize = true)
         {
            if (checkForInitalize)
            {
               if (!SSDMonitoringInitalized(out bool initalized))
               {
                  return false;
               }

               if (!initalized)
               {
                  if (!InitalizeSSDMonitoring())
                  {
                     return false;
                  }
               }
            }

            return Wrapper.ExecuteExecuteable(SchtasksPath,
               $"/CHANGE /TN {SSDMonitoringTaskName} {(enable ? "/ENABLE" : "/DISABLE")}", true, true, true);
         }
      }
   }
}