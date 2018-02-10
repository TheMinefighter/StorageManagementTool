using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Xml.Linq;
using File = System.IO.File;

namespace StorageManagementTool
{
   public static partial class OperatingMethods
   {
      public static class SSDMonitoring
      {
         private const string SSDMonitoringTaskName = "SSDMonitoring";
         private static readonly string SchtasksPath = Path.Combine(Wrapper.System32Path + "SCHTASKS.exe");

         private static readonly XNamespace TaskNamespace =
            XNamespace.Get("http://schemas.microsoft.com/windows/2004/02/mit/task");

         public static bool InitalizeSSDMonitoring()
         {
            string task = new XDocument(new XDeclaration("1.0", "UTF-16", null),
               new XElement(TaskNamespace + "Task", new XAttribute("version", "1.4"),
                  new XElement(TaskNamespace + "RegistrationInfo",
                     new XElement(TaskNamespace + "Date", DateTime.Now.ToWin32Format()),
                     new XElement(TaskNamespace + "Author", WindowsIdentity.GetCurrent().Name),
                     new XElement(TaskNamespace + "Description",
                        "Monitors a list of configured paths"),
                     new XElement(TaskNamespace + "URI", $"\\{SSDMonitoringTaskName}")),
                  new XElement(TaskNamespace + "Triggers",
                     new XElement(TaskNamespace + "LogonTrigger", new XElement(TaskNamespace + "Enabled", "true"))),
                  new XElement(TaskNamespace + "Principals",
                     new XElement(TaskNamespace + "Principal", new XAttribute("ID", "Author"),
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
                        new XElement(TaskNamespace + "Arguments", "/background"))))).ToString();

            File.WriteAllText(Path.Combine(Path.GetTempPath(), "StorageManagementTool.Task.xml"), task);
            return Wrapper.ExecuteExecuteable(SchtasksPath,
               $"/Create /XML \"{Path.Combine(Path.GetTempPath(), "StorageManagementTool.Task.xml")}\" /TN {SSDMonitoringTaskName} /RP * /RU {Environment.UserName}",
               true, false, true);
         }

         public static bool SSDMonitoringEnabled(out bool enabled)
         {
            enabled = false;
            //From https://superuser.com/a/1035052 last access 10.02.2018
            if (!Wrapper.RunPowershellCommand($"(Get-ScheduledTask | Where TaskName -eq {SSDMonitoringTaskName} ).State",
               out IEnumerable<string> ret))
            {

               return false;
            }

            if (!ret.Any())
            {
               return false;
            }
            enabled = ret.ElementAt(0) == "Enabled";
            return true;

         }

         public static bool SSDMonitoringEnabled()
         {
            bool success = SSDMonitoringEnabled(out bool enabled);
            return success && enabled;
         }

         public static bool SSDMonitoringInitalized(out bool initalized)
         {
            int returnCode;
            initalized = false;
            try
            {
               if (!Wrapper.ExecuteExecuteable(SchtasksPath, $"/QUERY /TN {SSDMonitoringTaskName}", out string[] _,
                  out returnCode, true, true, true))
               {
                  return false;
               }
            }
            catch (Exception )
            {
               return false;
            }

            initalized = returnCode == 0;
            return true;
         }

         public static bool SSDMonitoringInitalized()
         {
            bool success = SSDMonitoringInitalized(out bool enabled);
            return success && enabled;
         }

         public static bool SetSSDMonitoring(bool enable, bool checkForInitalize=true)
         {
            if (checkForInitalize)
            {
               if (!Wrapper.ExecuteExecuteable(SchtasksPath, $"/QUERY /TN {SSDMonitoringTaskName}", out string[] _,out int returnCode,true,true, true))
               {
                  return false;
               }

               if (returnCode!=0)
               {
                  if (!InitalizeSSDMonitoring())
                  {
                     return false;
                  }
               
               }
            }

            return Wrapper.ExecuteExecuteable(SchtasksPath,
               $"/EDIT /TN {SSDMonitoringTaskName} {(enable ? "/Enable" : "/DISABLE")}");
         }
      }
   }
}