﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using System.Management.Automation;
using System.ServiceProcess;
using Microsoft.VisualBasic.FileIO;
using static StorageManagementTool.GlobalizationRessources.WrapperStrings;

namespace StorageManagementTool
{
   /// <summary>
   ///    Contains system functionalities, which are not specific for this project
   /// </summary>
   public static partial class Wrapper
   {
      private static readonly IEnumerable<string> ExecuteableExtensions =new [] {".exe", ".pif", ".com", ".bat", ".cmd"};
      public static readonly string System32Path = Environment.GetFolderPath(Environment.SpecialFolder.System);

      public static readonly string ExplorerPath =
         Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe");

      /// <summary>
      ///    Executes an executeable
      /// </summary>
      /// <param name="filename">The name of the File to execute</param>
      /// <param name="parameters">The parameters to use when satrting the file</param>
      /// <param name="admin">Whether the file should be executed with</param>
      /// <param name="hidden">Whether the Main Window of this executeable (if exists) should be shown</param>
      /// <param name="waitforexit">Whether the code should wait until the executeable exited</param>
      /// <returns>Whether the operation were successfull</returns>
      public static bool ExecuteExecuteable(string filename, string parameters, bool admin = false,
         bool hidden = false, bool waitforexit = false)
      {
         return ExecuteExecuteable(filename, parameters, out string[] _, out int _, waitforexit: waitforexit,
            hidden: hidden, admin: admin);
      }

      /// <summary>
      ///    Executes an Executeable
      /// </summary>
      /// <param name="filename">The name of the file to execute</param>
      /// <param name="parameters">The parameters to use when satrting the file</param>
      /// <param name="returnData"> The String returned by the file</param>
      /// <param name="exitCode"> The exit code returned by the executable, only available if waitforexit=true</param>
      /// <param name="readReturnData">Whether to Read the output of the executeable started</param>
      /// <param name="waitforexit">Whether the code should wait until the executeable exited</param>
      /// <param name="hidden">Whether the main window of this executeable (if existing) should be hidden</param>
      /// <param name="admin">Whether the file should be executed with</param>
      /// <param name="asUser"></param>
      /// <returns>Whether the operation were successfull</returns>
      public static bool ExecuteExecuteable(string filename, string parameters, out string[] returnData,
         out int exitCode, bool readReturnData = false, bool waitforexit = false, bool hidden = false,
         bool admin = false, bool asUser = false)
      {
         FileInfo toRun= new FileInfo(filename);
         exitCode = 0;
         returnData = null;
         if (!toRun.Exists)
         {
            if (MessageBox.Show(
                   string.Format(ExecuteExecuteable_FileNotFound_Text, toRun.FullName),
                   Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
               OpenFileDialog alternativeExecuteableSel = new OpenFileDialog
               {
                  Filter = $"{ExecuteExecutable_FileNotFound_SelectionFilter}|*{string.Join(";*", ExecuteableExtensions)}",
                  Title = string.Format(ExecuteExecuteable_FileNotFound_SelectionTitle,toRun.Name)
               };
               alternativeExecuteableSel.ShowDialog();
               return ExecuteExecuteable(alternativeExecuteableSel.FileName, parameters, out returnData,
                  out exitCode, waitforexit: waitforexit, hidden: hidden, admin: admin, asUser: asUser);
            }
         }

         if (!ExecuteableExtensions.Contains(toRun.Extension))
         {
            if (new DialogResult[] {DialogResult.No, DialogResult.None}.Contains(MessageBox.Show(
               string.Format(ExecuteExecuteable_WrongEnding,
                  toRun.FullName, toRun.Extension),
               Error,
               MessageBoxButtons.YesNo, MessageBoxIcon.Error)))
            {
               return false;
            }
         }

         Process process = new Process();
         ProcessStartInfo startInfo = new ProcessStartInfo
         {
            WindowStyle = hidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
            FileName = toRun.FullName,
            Arguments = parameters,
            RedirectStandardOutput = readReturnData
         };
         if (readReturnData && admin)
         {
            asUser = true;
         }

         startInfo.UseShellExecute = false;
         if (asUser)
         {
            if (!EnterCredentials.GetCredentials(admin, out EnterCredentials.Credentials tmp))
            {
               return false;
            }

            startInfo.Password = tmp.Password;
            startInfo.UserName = tmp.Username;
         }
         else
         {
            if (admin)
            {
               startInfo.Verb = "runas";
               startInfo.UseShellExecute = true;
            }
         }

         process.StartInfo = startInfo;
         try
         {
            process.Start();
         }
         catch (Win32Exception)
         {
            DialogResult retry = MessageBox.Show(
               string.Format(
                  ExecuteExecuteable_AdminError,
                  toRun.FullName),
               Error, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            switch (retry)
            {
               case DialogResult.Retry:
                  ExecuteExecuteable(filename, parameters, admin, hidden, waitforexit);
                  break;
               case DialogResult.Ignore:
                  ExecuteExecuteable(filename, parameters, false, hidden, waitforexit);
                  break;
               case DialogResult.Abort:
               default: return false;
            }
         }

         if (waitforexit)
         {
            process.WaitForExit();
            if (readReturnData)
            {
               returnData = process.StandardOutput.FromStream();
            }

            exitCode = process.ExitCode;
            process.Dispose();
         }

         return true;
      }

      public static IEnumerable<DriveInfo> getDrives()
      {
         return FileSystem.Drives;
      }
      
      /// <summary>
      ///    Executes an Command using Windows Commandline
      /// </summary>
      /// <param name="cmd">The Command to Execute</param>
      /// <param name="admin">Whether the Command should be executed with</param>
      /// <param name="hidden">Whether to hide the Commandline </param>
      /// <param name="waitforexit">
      ///    Whether to wait until the command execution completed
      /// </param>
      /// <param name="debug">
      ///    Whether to run the command in debug mode
      /// </param>
      /// <returns>Whether the operation were successful</returns>
      public static bool ExecuteCommand(string cmd, bool admin, bool hidden, bool waitforexit = true,
         bool debug = false)
      {
         return ExecuteCommand(cmd, admin, hidden, out string[] _, waitforexit, debug);
      }

      /// <summary>
      ///    Checks if one Path is the parent of another
      /// </summary>
      /// <param name="parentPath">The parent path</param>
      /// <param name="childPath">The child path</param>
      /// <returns>Whether parentPath is a paren of childPath</returns>
      public static bool IsSubfolder(DirectoryInfo parentPath, DirectoryInfo childPath)
      {
         return parentPath.FullName.StartsWith(childPath.FullName + Path.DirectorySeparatorChar);
      }

      /// <summary>
      ///    Executes an Command using Windows Commandline
      /// </summary>
      /// <param name="cmd">The Command to Execute</param>
      /// <param name="admin">Whether the Command should be executed with</param>
      /// <param name="hidden">Whether to hide the Commandline </param>
      /// <param name="returnData">The Data returned by the executeable</param>
      /// <param name="waitforexit">
      ///    Whether to wait until the command execution completed
      /// </param>
      /// <param name="debug">
      ///    Whether to run the command in debug mode
      /// </param>
      /// <param name="readReturnData">Whether to read the output of the Application</param>
      /// <returns>Whether the operation were successful</returns>
      public static bool ExecuteCommand(string cmd, bool admin, bool hidden, out string[] returnData,
         bool waitforexit = true, bool debug = false, bool readReturnData = false)
      {
         if (ExecuteExecuteable(Path.Combine(System32Path, @"cmd.exe"),
            (debug ? " /K " : " /C ") + cmd, out returnData, out int tmp, readReturnData, waitforexit, hidden,
            admin))
         {
            return tmp == 0;
         }

         return false;
      }

      #region From https://stackoverflow.com/a/3600342/6730162 access on 30.9.2017

      /// <summary>
      ///    Tests whether the program is being executed with Admin privileges
      /// </summary>
      /// <returns>Whether the Program is being executed with Admin privileges</returns>
      public static bool IsUserAdministrator()
      {
         try
         {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
         }
         catch (Exception)
         {
            return false;
         }
      }

      #endregion

      #region From https://stackoverflow.com/a/26473940/6730162 access on 30.9.2017

      #endregion

      /// <summary>
      ///    Restarts Program as Administartor
      /// </summary>
      public static void RestartAsAdministrator()
      {
         if (ExecuteExecuteable(Process.GetCurrentProcess().MainModule.FileName, " ", true))
         {
            Environment.Exit(0);
         }
      }

      /// <summary>
      ///    Reads the whole content of an StreamReader
      /// </summary>
      /// <param name="reader">The StreamReader to read from</param>
      /// <returns>The strings saved in the StreamReader</returns>
      private static string[] FromStream(this TextReader reader)
      {
         List<string> ret = new List<string>();
         string line;
         while ((line = reader.ReadLine()) != null)
         {
            ret.Add(line);
         }

         return ret.ToArray();
      }

      public static bool IsUser(string name)
      {
         return UserPrincipal.FindByIdentity(GetPrincipalContext(), IdentityType.SamAccountName, name) != null;
      }

      public static bool IsAdmin(string username)
      {
         SecurityIdentifier id = new SecurityIdentifier("S-1-5-32-544");
         UserPrincipal user = UserPrincipal
            .FindByIdentity(GetPrincipalContext(), IdentityType.SamAccountName, username);
         if (user == null)
         {
            return false;
         }

         return user
            .GetGroups()
            .Any(x => x.Sid == id);
      }

      public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
         this IEnumerable<KeyValuePair<TKey, TValue>> src)
      {
         return src.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
      }

      public static string DateTimeToWin32Format(DateTime toConvert)
      {
         return
            $"{toConvert.Year:0000}-{toConvert.Month:00}-{toConvert.Day:00}T{toConvert.Hour:00}:{toConvert.Minute:00}:{toConvert.Second:00}.{toConvert.Millisecond:000}0000";
      }

      /// <summary>
      ///    Checks whether a user is Part of a localgroup
      /// </summary>
      /// <param name="username">The ViewedName of the User to search for</param>
      /// <param name="localGroup">The localgroup to search in</param>
      /// <returns>Whether the user is in the logalgroup</returns>
//      public static bool IsUserInLocalGroup(string username, string localGroup)
//      {
//         GroupPrincipal oGroupPrincipal = GetGroup(localGroup);
//         PrincipalSearchResult<Principal> oPrincipalSearchResult = oGroupPrincipal.GetMembers();
//         return oPrincipalSearchResult.Any(principal => principal.Name == username);
//      }
//
//      private static GroupPrincipal GetGroup(string sGroupName)
//      {
//         PrincipalContext oPrincipalContext = GetPrincipalContext();
//         GroupPrincipal oGroupPrincipal = GroupPrincipal.FindByIdentity(oPrincipalContext, sGroupName);
//         return oGroupPrincipal;
//      }

      private static PrincipalContext GetPrincipalContext()
      {
         PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Machine);
         return oPrincipalContext;
      }


      #region Based upon https://blogs.msdn.microsoft.com/kebab/2014/04/28/executing-powershell-scripts-from-c/ last access 10.02.2018

      public static bool RunPowershellCommand(out IEnumerable<string> ret, params string[] command)
      {
         ret = new[] {""};
         IEnumerable<PSObject> returned;
         using (PowerShell PowerShellInstance = PowerShell.Create())
         {
            foreach (string s in command)
            {
               PowerShellInstance.AddScript(s);
            }

            try
            {
               {
                  returned = PowerShellInstance.Invoke();
               }
            }
            catch (Exception)
            {
               return false;
            }
         }

         ret = returned.Where(x => x != null).Select(x => x.ToString());
         return true;
      }

      #endregion

      public static bool RestartComputer()
      {
         return ExecuteExecuteable(Path.Combine(System32Path, "shutdown.exe"), "/R /T 1", false, true);
      }

      /// <summary>
      /// Kills first all depnding ServiceControllers and then itselves 
      /// </summary>
      /// <param name="toKill">The ServiceController to kill</param>
      /// <returns>Whether the operation were successful</returns>
      private static bool RecursiveServiceKiller(ServiceController toKill)
      {
         
         IEnumerable<ServiceController> childs = toKill.DependentServices;
         if (!(childs.All(x => x.CanStop) && childs.All(RecursiveServiceKiller)))
         {
            return false;
         }

         try
         {
            toKill.Stop();
         }
         catch (Exception)
         {
            return false;
         }

         return true;
      }
   }
}