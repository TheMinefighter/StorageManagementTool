﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Xml.Linq;
using StorageManagementTool.GlobalizationRessources;
using StorageManagementTool.MainGUI.GlobalizationRessources;
using File = System.IO.File;

namespace StorageManagementTool
{
   public static partial class OperatingMethods
   {
      public static class SSDMonitoring
      {
         private const string SSDMonitoringTaskName = "SSDMonitoring";
         private static readonly string SchtasksPath = Path.Combine(Wrapper.System32Path,"SCHTASKS.exe");

         private static readonly XNamespace TaskNamespace =
            XNamespace.Get("http://schemas.microsoft.com/windows/2004/02/mit/task");

         public static bool InitalizeSSDMonitoring()
         {
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
            string task = new XDocument(new XDeclaration("1.0", "UTF-16", null),
               taskContents).ToString();

            string tempLocation = Path.Combine(Path.GetTempPath(), "StorageManagementToolTask.xml");
            File.WriteAllText(tempLocation, task);
            return Wrapper.ExecuteExecuteable(SchtasksPath,
               $"/Create /XML \"{tempLocation}\" /TN {SSDMonitoringTaskName} /RP * /RU {Environment.UserName}",
               out string[] tmp,out int tmp2,false,true,false,true);
         }

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
            enabled = ret.ElementAt(0) == "Enabled"||ret.ElementAt(0)=="Ready";
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
               if (!Wrapper.ExecuteExecuteable(SchtasksPath, $"/QUERY /TN {SSDMonitoringTaskName}", out string[] _,out int returnCode,true,true, true,false,false))
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
               $"/CHANGE /TN {SSDMonitoringTaskName} {(enable ? "/ENABLE" : "/DISABLE")}",true,true,true);
         }
      }

      public static bool TestCredentials(string username, SecureString password)
      {
         Process pProcess = new Process
         {
            StartInfo = new ProcessStartInfo(Path.Combine(Wrapper.System32Path, "cmd.exe"), " /C exit")
            {
               WindowStyle = ProcessWindowStyle.Hidden,
               UseShellExecute = false,
               Password = new SecureString(),
               UserName = username
            }
         };

         pProcess.StartInfo.Password = password;

         try
         {
            pProcess.Start();
         }
         catch (Exception)
         {

            return false;
         }

         return true;
      }

      public static bool IsSendToEnabled()
      {
         return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
            OperatingMethodsStrings.StoreOnHDDLinkName + ".lnk"));
      }
   }
}