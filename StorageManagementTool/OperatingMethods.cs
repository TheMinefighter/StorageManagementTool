using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using ExtendedMessageBoxLibary;
using ExtendedMessageBoxLibrary;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using static StorageManagementTool.GlobalizationRessources.OperatingMethodsStrings;
using File = System.IO.File;

namespace StorageManagementTool
{
   public static partial class OperatingMethods
   {
      public enum QuestionAnswer
      {
         Yes,
         No,
         Ask
      }

      public static readonly RegPath SearchDatatDirectoryRegPath = new RegPath(
         @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "DataDirectory");

      /// <summary>
      ///    Creates a string representation of an DriveInfo
      /// </summary>
      /// <param name="item">The DriveInfo object to represent</param>
      /// <returns>The string representation</returns>
      public static string DriveInfo2String(DriveInfo item)
      {
         return item.IsReady
            ? item.VolumeLabel + " (" + item.Name + " ; " +
              DriveType2String(item.DriveType) + ')'
            : item.Name;
      }

      public static bool MoveFolder(DirectoryInfo dir, DirectoryInfo newLocation)
      {
         if (dir == newLocation)
         {
            if (MessageBox.Show(Error, MoveFolderOrFile_PathsEqual,
                   MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
            {
               MoveFolder(dir, newLocation);
            }
         }

         if (newLocation.Parent==null)
         {
            newLocation.Parent.Create();
         }

         if (dir.Exists)
         {
            if (Wrapper.CopyDirectory(dir, newLocation))
            {
               if (!Wrapper.DeleteDirectory(dir))
               {
                  return false;
               }
            }
            else
            {
               return false;
            }
         }

         return Wrapper.ExecuteCommand($"mklink /D \"{dir.FullName}\" \"{newLocation.FullName}\"", true, true);
      }

      public static bool MoveFile(FileInfo file, FileInfo newLocation)
      {
         if (file == newLocation)
         {
            if (
               MessageBox.Show(Error, MoveFolderOrFile_PathsEqual,
                  MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
            {
               MoveFile(file, newLocation);
            }
            else
            {
               return false;
            }
         }

         if (!newLocation.Directory.Exists)
         {
            newLocation.Directory.Create();
         }

         if (file.Exists)
         {
            if (Wrapper.CopyFile(file, newLocation))
            {
               if (!Wrapper.DeleteFile(file))
               {
                  return false;
               }
            }
            else
            {
               return false;
            }
         }

         return Wrapper.ExecuteCommand($"mklink \"{file.FullName}\" \"{newLocation.FullName}\"", true, true);
         //throw new NotImplementedException();
      }

      public static bool ChangeSwapfileStadium(int currentStadium, bool fwd)
      {
         if (currentStadium == 1 && !fwd)
         {
            MessageBox.Show(Error, SetStadium_ErrorNoneBeforeFirst, MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            return false;
         }

         if (currentStadium == 4 && fwd)
         {
            MessageBox.Show(Error, SetStadium_ErrorNoneAfterLast, MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            return false;
         }

         if (fwd)
         {
            switch (Session.Singleton.Swapstadium)
            {
               case 1:
                  try
                  {
                     Registry.SetValue(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                        "SwapFileControl", 1, RegistryValueKind.DWord);
                  }
                  catch (Exception)
                  {
                     return false;
                  }

                  break;
               case 2:
                  string HDDPath = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys").Remove(1, 1);
                  
                  if (Session.Singleton.CurrentConfiguration.DefaultHDDPath == "")
                  {
                     if (MessageBox.Show(
                            Error, SetStadium_NoNewPathGiven,
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                     {
                        ChangeSwapfileStadium(currentStadium, fwd);
                     }
                     else
                     {
                        return false;
                     }
                  }

                  try
                  {
                     new DirectoryInfo(Session.Singleton.CurrentConfiguration.DefaultHDDPath);
                  }
                  catch (Exception)
                  {
                     if (MessageBox.Show(
                            "Fehler", "Es wurde ein ungültiger Pfad für den NewPath Speicherort eingegeben",
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                     {
                        ChangeSwapfileStadium(currentStadium, fwd);
                     }
                     else
                     {
                        return false;
                     }
                  }

                  string newPath =Path.Combine( Session.Singleton.CurrentConfiguration.DefaultHDDPath ,
                                   HDDPath);
                  string oldPath = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys");
                  MoveFile(new FileInfo(oldPath), new FileInfo(newPath));
                  break;
               case 3:
                  try
                  {
                     Registry.SetValue(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                        "SwapFileControl", 0, RegistryValueKind.DWord);
                     break;
                  }
                  catch (Exception)
                  {
                     return false;
                  }
            }

            return true;
         }

         switch (Session.Singleton.Swapstadium)
         {
            case 2:
               Registry.SetValue(
                  @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                  "SwapFileControl", 1, RegistryValueKind.DWord);
               break;
            case 3:
               File.Delete(Path.Combine(Session.Singleton.CurrentConfiguration.DefaultHDDPath,
                  Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys").Remove(1, 1)));
               break;
            case 4:
               Registry.SetValue(
                  @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                  "SwapFileControl", 0, RegistryValueKind.DWord);
               break;
         }

         Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
            "SwapFileControl", 1, RegistryValueKind.DWord);

         return false;
      }

      /// <summary>
      ///    Recommends Paths to move to NewPath
      /// </summary>
      /// <returns>The recommended Paths</returns>
      public static string[] GetRecommendedPaths()
      {
         List<string> ret = new List<string>();
         if (
            !Wrapper.IsPathSymbolic(Environment.ExpandEnvironmentVariables(@"%AppData%")))
         {
            ret.Add(Environment.ExpandEnvironmentVariables(@"%AppData%"));
         }

         List<string> blacklist = new List<string>
         {
            Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData"),
            Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local\Microsoft"),
            Environment.ExpandEnvironmentVariables(@"%temp%"),
            Environment.ExpandEnvironmentVariables(@"%tmp%")
         };
         string[] currentsubfolders =
            Directory.GetDirectories(Environment.ExpandEnvironmentVariables(@"%userprofile%"));
         for (int i = 0; i < currentsubfolders.GetLength(0); i++)
         {
            if (!Wrapper.IsPathSymbolic(currentsubfolders[i]) && !blacklist.Contains(currentsubfolders[i]))
            {
               ret.Add(currentsubfolders[i]);
            }
         }

         currentsubfolders =
            Directory.GetDirectories(Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local"));
         for (int i = 0; i < currentsubfolders.GetLength(0); i++)
         {
            if (!Wrapper.IsPathSymbolic(currentsubfolders[i]) && !blacklist.Contains(currentsubfolders[i]))
            {
               ret.Add(currentsubfolders[i]);
            }
         }

         return ret.ToArray();
      }

      /// <summary>
      ///    Gets names of DriveTypes
      /// </summary>
      /// <param name="toName">The DriveType Object, which name should be returned</param>
      /// <returns>The  name of the DriveType Object</returns>
      public static string DriveType2String(DriveType toName)
      {
         switch (toName)
         {
            case DriveType.CDRom: return DriveType2String_CDRom;
            case DriveType.Fixed: return DriveType2String_Fixed;
            case DriveType.Network: return DriveType2String_Network;
            case DriveType.Ram: return DriveType2String_RAM;
            case DriveType.Removable: return DriveType2String_Removable;
            case DriveType.NoRootDirectory: return DriveType2String_NoRootDirectory;
            default: return DriveType2String_Unknown;
         }
      }

      /// <summary>
      ///    Changes the systems pagefile settings
      /// </summary>
      /// <param name="currentSelection">The selected partition entry</param>
      /// <param name="maxSize">The maximum Size of the Pagefile in MB</param>
      /// <param name="minSize">The minimum Size of the Pagefile in MB</param>
      /// <returns>Whether the Operation were successfull</returns>
      public static bool ChangePagefileSettings(string currentSelection, int maxSize, int minSize)
      {
         Session.Singleton.RefreshDriveInformation();
         List<string> tempDriveInfoList = Session.Singleton.FillWithDriveInfo().ToList();
         int selectedPartitionIndex;
         if (tempDriveInfoList.Contains(currentSelection)) //Tests whether the selected partition is available
         {
            selectedPartitionIndex = tempDriveInfoList.IndexOf(currentSelection);
         }
         else
         {
            MessageBox.Show(ChangePagefileSettings_SelectedPartitionMissing,
               Error,
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }

         DriveInfo toUse = Session.Singleton.CurrentDrives[selectedPartitionIndex];
         return ChangePagefileSettings(toUse, maxSize, minSize);
      }

      public static bool ChangePagefileSettings(DriveInfo toUse, int maxSize, int minSize)
      {
         string wmicPath = Path.Combine(Wrapper.System32Path, @"wbem\\wmic.exe");
         if (maxSize < minSize) //Tests whether the maxSize is smaller than the minSize
         {
            MessageBox.Show(ChangePagefileSettings_MinGreaterMax,
               Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }

         if (toUse.AvailableFreeSpace < minSize * 1048576L) //Tests whether enough space is available
         {
            MessageBox.Show(ChangePagefileSettings_NotEnoughSpace,
               Error,
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }

         if (Wrapper.ExecuteExecuteable(
            wmicPath, "computersystem get AutomaticManagedPagefile /Value"
            , out string[] tmp, out int _, true, true, true, true)) //Tests
         {
            if (bool.Parse(tmp[2].Split('=')[1]))
            {
               Wrapper.ExecuteCommand(
                  wmicPath
                  + Environment.ExpandEnvironmentVariables(
                     " computersystem where \"name='%computername%' \" set AutomaticManagedPagefile=False")
                  , true, true, out _); //Disables automatic Pagefile  management
               Wrapper.ExecuteExecuteable(
                  wmicPath
                  , "computersystem get AutomaticManagedPagefile /Value"
                  , out tmp, out int _, waitforexit: true, hidden: true, admin: true);
               if (!bool.Parse(tmp[2].Split('=')[1]))
               {
                  MessageBox.Show(ChangePagefileSettings_CouldntDisableManagement,
                     Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return false;
               }
            }
         }

         Wrapper.ExecuteExecuteable(wmicPath,
            "pagefileset delete /NOINTERACTIVE", out _, out int _, waitforexit: true,
            hidden: true, admin: true); //Deletes all Pagefiles

         Wrapper.ExecuteExecuteable(wmicPath,
            $"pagefileset create name=\"{Path.Combine(toUse.Name, "Pagefile.sys")}\"", out _,
            out int _, waitforexit: true, hidden: true, admin: true); //Creates new Pagefile

         Wrapper.ExecuteExecuteable(wmicPath,
            $"pagefileset where name=\"{Path.Combine(toUse.Name, "Pagefile.sys")}\" set InitialSize={minSize},MaximumSize={maxSize}",
            out _, out int _, waitforexit: true, hidden: true, admin: true); // Sets Pagefile Size

         Wrapper.ExecuteExecuteable(wmicPath,
            " get", out tmp, out int _, true, true,
            true); //Checks wether there is exactly 1 pagefile existing
         if (tmp.Length != 2)
         {
            switch (MessageBox.Show(ChangePagefileSettings_Not1Pagefile,
               Error,
               MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
            {
               case DialogResult.Cancel: return false;
               case DialogResult.Retry: return ChangePagefileSettings(toUse, maxSize, minSize);
            }
         }

         return true;
      }

      /// <summary>
      ///    Moves an User ShellFolder to a new Location
      /// </summary>
      /// <param name="oldDir">The old Directory of</param>
      /// <param name="newDir">The new Directory of the new </param>
      /// <param name="usf">The UserShellFolder to edit</param>
      /// <param name="copyContents"></param>
      /// <param name="deleteOldContents"></param>
      /// <returns>Whether the Operation were successful</returns>
      public static bool ChangeUserShellFolder(DirectoryInfo oldDir, DirectoryInfo newDir, UserShellFolder usf,
         QuestionAnswer copyContents = QuestionAnswer.Ask, QuestionAnswer deleteOldContents = QuestionAnswer.Ask)
      {
         if (!newDir.Exists)
         {
            newDir.Create();
         }

         DirectoryInfo currentPath = usf.GetPath();
         Dictionary<UserShellFolder, DirectoryInfo> childs = UserShellFolder
            .AllEditableUserUserShellFolders
            .Select(x => new KeyValuePair<UserShellFolder, DirectoryInfo>(x, x.GetPath()))
            .Where(x => Wrapper.IsSubfolder(currentPath, x.Value)).ToDictionary();
         bool MoveAll = false;

         foreach (KeyValuePair<UserShellFolder, DirectoryInfo> child in childs)
         {
            //Add strings
            bool move = false;
            if (!MoveAll)
            {
               //No;Yes;YesAll
               ExtendedMessageBoxResult result = ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
                  string.Format(ChangeUserShellFolder_SubfolderFound_Text, child.Key.ViewedName),
                  ChangeUserShellFolder_SubfolderFound_Title,
                  childs.Count == 1
                     ? new[] {ChangeUserShellFolder_SubfolderFound_Yes, ChangeUserShellFolder_SubfolderFound_No}
                     : new[]
                     {
                        ChangeUserShellFolder_SubfolderFound_YesAll, ChangeUserShellFolder_SubfolderFound_Yes,
                        ChangeUserShellFolder_SubfolderFound_No
                     }, 0));
               if (result.NumberOfClickedButton == 2)
               {
                  MoveAll = true;
               }
               else
               {
                  move = result.NumberOfClickedButton == 1;
               }
            }

            if (MoveAll || move)
            {
               string newPathOfChild = Path.Combine(newDir.FullName,
                  child.Value.FullName.Skip(currentPath.FullName.Length).AsString());
               foreach (RegPath x in child.Key.RegPaths)
               {
                  bool retry;
                  bool skip = false;
                  do
                  {
                     retry = false;
                     if (!Wrapper.RegistryMethods.SetRegistryValue(x, newPathOfChild, RegistryValueKind.String, usf.AccessAsUser))
                     {
                        switch (ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
                           string.Format(ChangeUserShellFolder_ErrorChangeSubfolder_Text, child.Key.ViewedName,
                              x.ValueName, x.RegistryKey, newPathOfChild), Error,
                           new[]
                           {
                              ChangeUserShellFolder_ErrorChangeSubfolder_Retry,
                              string.Format(ChangeUserShellFolder_ErrorChangeSubfolder_Skip, child.Key.ViewedName),
                              ChangeUserShellFolder_ErrorChangeSubfolder_Ignore,
                              ChangeUserShellFolder_ErrorChangeSubfolder_Abort
                           }, 0)).NumberOfClickedButton)
                        {
                           case 0:
                              retry = true;
                              break;
                           case 1:
                              skip = true;
                              break;
                           case 2: break;
                           case 3: return false;
                        }
                     }
                  } while (retry);

                  if (skip)
                  {
                     break;
                  }
               }
            }
         }

         if (usf.RegPaths.All(x =>
            Wrapper.RegistryMethods.SetRegistryValue(x, newDir.FullName, RegistryValueKind.String, usf.AccessAsUser)))
         {
            if (newDir.Exists && oldDir.Exists && usf.MoveExistingFiles &&
                (copyContents == QuestionAnswer.Yes || copyContents == QuestionAnswer.Ask && MessageBox.Show(
                    ChangeUserShellFolder_MoveContent_Text,
                    ChangeUserShellFolder_MoveContent_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                    MessageBoxDefaultButton.Button1) ==
                 DialogResult.Yes))
            {
               if (Wrapper.CopyDirectory(oldDir, newDir))
               {
                  if (deleteOldContents == QuestionAnswer.Yes || deleteOldContents == QuestionAnswer.Ask &&
                      MessageBox.Show(
                         string.Format(
                            ChangeUserShellFolder_DeleteContent_Text,
                            oldDir.FullName, newDir.FullName),
                         ChangeUserShellFolder_DeleteContent_Title,
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                  {
                     if (Wrapper.DeleteDirectory(oldDir) && !oldDir.Exists)
                     {
                        Wrapper.ExecuteCommand($"mklink /D \"{oldDir.FullName}\\\" \"{newDir.FullName}\"", true,
                           true);
                     }
                  }
               }
            }

            if (MessageBox.Show(
                   ChangeUserShellFolder_RestartExplorer_Text,
                   ChangeUserShellFolder_RestartExplorer_Title, MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
               Wrapper.ExecuteCommand("taskkill /IM explorer.exe /F & explorer.exe", false, true);
            }

            return true;
         }

         return false;
      }

      public static void EnableSendToHDD(bool enable = true)
      {
         if (enable)
         {
            #region From https://stackoverflow.com/a/4909475/6730162 access on 5.11.2017 

            WshShell shell = new WshShell();
            string shortcutAddress = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
               "Auf HDD Speichern.lnk");
            IWshShortcut shortcut = (IWshShortcut) shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Lagert den Speicherort der gegebenen Datei aus";
            shortcut.TargetPath = Process.GetCurrentProcess().MainModule.FileName;
            shortcut.Arguments = " -move -auto-detect -SrcPath";
            shortcut.Save();

            #endregion
         }
         else
         {
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo),
               "Auf HDD Speichern.lnk"));
         }
      }

      public static bool SetSearchDataPath(DirectoryInfo newPath)
      {
         if (newPath.Exists)
         {
            if (Wrapper.RegistryMethods.SetRegistryValue(SearchDatatDirectoryRegPath,
               newPath.CreateSubdirectory("Search").CreateSubdirectory("Data").FullName,
               RegistryValueKind.String,
               true))
            {
               if (!Session.Singleton.IsAdmin)
               {
                  if (MessageBox.Show(
                         SetSearchDataPath_RestartNoAdmin,
                         SetSearchDataPath_RestartNow_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                      DialogResult.Yes)
                  {
                     Wrapper.RestartComputer();
                  }
               }

               ServiceController wSearch = new ServiceController("WSearch");
               if (!RecursiveServiceRestart(wSearch))
               {
                  if (MessageBox.Show(string.Format(
                            SetSearchDataPath_RestartErrorService, wSearch.DisplayName),
                         SetSearchDataPath_RestartNow_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                      DialogResult.Yes)
                  {
                     Wrapper.RestartComputer();
                  }
               }

               return true;
            }

            return false;
         }

         MessageBox.Show(SetSearchDataPath_InvalidPath, Error, MessageBoxButtons.OK,
            MessageBoxIcon.Error);
         return false;
      }

      /// <summary>
      ///    Restarts a service and all depending services
      /// </summary>
      /// <param name="toRestart">The service to restart</param>
      /// <returns>Whether the operation were successful</returns>
      private static bool RecursiveServiceRestart(ServiceController toRestart)
      {
         return RecursiveServiceKiller(toRestart) && RecursiveServiceStarter(toRestart);
      }

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

      private static bool RecursiveServiceStarter(ServiceController toStart)
      {
         IEnumerable<ServiceController> childs = toStart.DependentServices;
         try
         {
            toStart.Start();
         }
         catch (Exception)
         {
            return false;
         }

         return childs.All(RecursiveServiceStarter);
      }

      public static void SetHibernate(bool enable)
      {
         Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path, "powercfg.exe"), $"/h {(enable ? "on" : "off")}",
            true, true,
            true);
      }
   }
}