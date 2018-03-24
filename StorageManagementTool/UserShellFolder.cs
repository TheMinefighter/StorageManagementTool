using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtendedMessageBoxLibary;
using ExtendedMessageBoxLibrary;
using Microsoft.Win32;
using StorageManagementTool.GlobalizationRessources;
using static StorageManagementTool.GlobalizationRessources.UserShellFolderStrings;

namespace StorageManagementTool {
   public struct UserShellFolder {
      struct RegistryValueWithDefault {
         Re
      }

      public string ViewedName;
      public (string, RegistryValue)[] RegistryValues;
      public bool MoveExistingFiles;
      public bool isUserSpecific;
      public bool AccessAsUser;
      public string Identifier;

      private const string UserShellFolderRoot =
         @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders";

      private const string CommonShellFolderRooot =
         @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders";

      private const string CommonUserShellFolderRoot =
         @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders";

      private const string ShellFolderRoot =
         @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders";

      private const string ProgramPathDefinitionRoot =
         @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion";

      private const string PublicTempRoot = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment";

      private const string UserTempRoot = @"HKEY_CURRENT_USER\Environment";

      private const string UserTempDefault = @"%USERPROFILE%\AppData\Temp";
      
      private const string PublicTempDefault= @"%SYSTEMROOT%\TEMP";

      public bool isMultiuser() => 
   //   public RegistryValue[] GetRegistryValues(bool DefaultUser = false) => RegistryValues.Select(x => x.RegistryKey = x.RegistryKey)

      private UserShellFolder(string name, (string,RegistryValue)[] registryValues, bool moveExistingFiles = true,
         bool accessAsUser = false, string identifier = null, string defaultValue = null) {
         if (registryValues.Length == 0) {
            throw new ArgumentException("At least one required");
         }

         ViewedName = name;
         RegistryValues = registryValues;
         MoveExistingFiles = moveExistingFiles;
         AccessAsUser = accessAsUser;
         Identifier = identifier ?? registryValues[0].Item2.ValueName;
      }

      private static UserShellFolder NormalUSF(string name, string id, bool user = true, bool moveExistingFiles = true) =>
         new UserShellFolder {
            ViewedName = name,
            RegistryValues = (user
               ? new[] {new RegistryValue(ShellFolderRoot, id), new RegistryValue(UserShellFolderRoot, id)}
               : new[] {new RegistryValue(ShellFolderRoot, id)}).Select(x => Tuple.Create(string.Empty, x).ToValueTuple()).ToArray(),
            MoveExistingFiles = moveExistingFiles,
            Identifier = id,

            isUserSpecific = true
         };

      private static UserShellFolder CommonUSF(string name, string id, string path, bool user = true, bool moveExistingFiles = true,
         bool asUser = true) {
         (string, RegistryValue) commonShellFolder = (Environment.ExpandEnvironmentVariables(path), new RegistryValue(CommonShellFolderRooot, id));
         (string, RegistryValue) commonUserShellFolder = (path, new RegistryValue(CommonUserShellFolderRoot, id));
         return new UserShellFolder {
            ViewedName = name,
            RegistryValues = user
               ? new [] {
                  commonShellFolder,
                  commonUserShellFolder
               }
               : new[] {commonShellFolder},
            MoveExistingFiles = moveExistingFiles,
            Identifier = id,
            AccessAsUser = asUser,
            isUserSpecific = false,
           };
      }


      public static void LoadEditable() {
         AllEditableUserUserShellFolders = new[] {
            #region Based upon https://support.microsoft.com/en-us/help/931087/how-to-redirect-user-shell-folders-to-a-specified-path-by-using-profil access on 22.01.2017

            NormalUSF(Desktop_Name, "Desktop"),
            NormalUSF(Personal_Name, "Personal"),
            NormalUSF(My_Video_Name, "My Video"),
            NormalUSF(My_Music_Name, "My Music"),
            NormalUSF(My_Pictures_Name, "My Pictures"),
            NormalUSF(SendTo_Name, "SendTo"),
            NormalUSF(Local_AppData_Name, "Local AppData"),
            NormalUSF(Appdata_Name, "AppData"),
            NormalUSF(Programs_Name, "Programs"),
            NormalUSF(Start_Menu_Name, "Start Menu"),
            NormalUSF(Startup_Name, "Startup"),
            NormalUSF(History_Name, "History"),
            NormalUSF(Favorites_Names, "Favorites"),
            NormalUSF(Fonts_Name, "Fonts", false),
            NormalUSF(Recent_Name, "Recent"),
            NormalUSF(Templates_Name, "Templates"),
            NormalUSF(Administrative_Tools_Name, "Administrative Tools", false),
            NormalUSF(Cookies_Name, "Cookies", false),
            NormalUSF(NetHood_Name, "NetHood", false),
            NormalUSF(PrintHood_Name, "PrintHood", false),
            NormalUSF(Cache_Name, "Cache", false),
            NormalUSF(CD_Burning_Name, "CD Burning", false),
            NormalUSF(Downloads_Name, "{374DE290-123F-4565-9164-39C4925E467B}"),
            NormalUSF(Libraries_Name, "{1B3EA5DC-B587-4786-B4EF-BD1DC332AEAE}", false),

            #endregion

            //Common 
            CommonUSF(Common_Documents_Name, "Common Documents", "%PUPLIC%\\Documennts"),
            CommonUSF(CommonVideo_Name, "CommonVideo", "%PUPLIC%\\Video"),

            CommonUSF(CommonPictures_Name, "CommonPictures", "%PUPLIC%\\Pictures"),
            CommonUSF(CommonMusic_Name, "CommonMusic", "%PUPLIC%\\Music"),
            CommonUSF(Common_Desktop_Name, "Common Desktop", "%PUPLIC%\\Desktop"),
            CommonUSF(Common_AppData_Name, "Common AppData", "%PUPLIC%\\Roaming"),
            CommonUSF(Common_Startup_Name, "Common Startup", "%PROGRAMDATA%"),
            CommonUSF(Common_Programs_Name, "Common Programs", "%PROGRAMDATA%\\Microsoft\\Windows\\Start Menu\\Programs"),
            CommonUSF(Common_Templates_Name, "Common Templates", "%PROGRAMDATA%\\Microsoft\\Windows\\Templates"),
            CommonUSF(Common_Start_Menu_Name, "Common Start Menu", "%PROGRAMDATA%\\Microsoft\\Windows\\Start Menu"),
            CommonUSF(OEM_Links_Name, "OEM Links", "%PROGRAMDATA%\\OEM\\Links", false, true, true),
            CommonUSF(Common_Administrative_Tools_Name, "Common Administrative Tools","%PROGRAMDATA%\\Microsoft\\Windows\\Start Menu\\Programs\\Administartive Tools",  false),
            //No real USF
            //TODO Readd with commons
//            new UserShellFolder(ProgramFilesDir_x86_Name,
//               new[] {
//                  new RegistryValue(ProgramPathDefinitionRoot, "ProgramFilesDir (x86)")
//               }, false, true),
//            new UserShellFolder(ProgramFilesDir_Name,
//               new[] {
//                  new RegistryValue(ProgramPathDefinitionRoot, "ProgramFilesDir"),
//                  new RegistryValue(ProgramPathDefinitionRoot, "ProgramW6432Dir")
//               }, false, true, null),
            new UserShellFolder {
               Identifier = "PrivateTemp",
               AccessAsUser = false,
               RegistryValues = new (string, RegistryValue)[] {(UserTempDefault, new RegistryValue(UserTempRoot, "TEMP")),(UserTempRoot, new RegistryValue(UserTempRoot, "TMP"))},
               MoveExistingFiles = false,
               ViewedName = PrivateTemp,
               isUserSpecific = true
            },
            new UserShellFolder(PublicTemp, new [] { (PublicTempDefault, new RegistryValue(PublicTempRoot, "TEMP")), (PublicTempDefault, new RegistryValue(PublicTempRoot, "TMP"))},
               false, true, "PublicTemp")
         };
      }

      public static UserShellFolder[] AllEditableUserUserShellFolders;

      public static UserShellFolder GetUserShellFolderById(string id) {
         return AllEditableUserUserShellFolders.First(x => x.Identifier == id);
      }

      public static UserShellFolder GetUserShellFolderByName(string name) {
         return AllEditableUserUserShellFolders.First(x => x.ViewedName == name);
      }

      public DirectoryInfo GetPath() {
         Wrapper.RegistryMethods.GetRegistryValue(RegistryValues[0].Item2, out object regValue, AccessAsUser);
         return new DirectoryInfo((string) regValue ?? Error);
      }

      public static DirectoryInfo GetPath(UserShellFolder currentUSF) => currentUSF.GetPath();

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
         OperatingMethods.QuestionAnswer copyContents = OperatingMethods.QuestionAnswer.Ask,
         OperatingMethods.QuestionAnswer deleteOldContents = OperatingMethods.QuestionAnswer.Ask) {
         if (!newDir.Exists) {
            newDir.Create();
         }

         DirectoryInfo currentPath = usf.GetPath();
         Dictionary<UserShellFolder, DirectoryInfo> childs = AllEditableUserUserShellFolders
            .Select(x => new KeyValuePair<UserShellFolder, DirectoryInfo>(x, x.GetPath()))
            .Where(x => Wrapper.IsSubfolder(x.Value, currentPath)).ToDictionary();
         bool moveAll = false;

         foreach (KeyValuePair<UserShellFolder, DirectoryInfo> child in childs) {
            //Add strings
            bool move = false;
            if (!moveAll) {
               //No;Yes;YesAll
               ExtendedMessageBoxResult result = ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
                  string.Format(OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_Text, child.Key.ViewedName),
                  OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_Title,
                  childs.Count == 1
                     ? new[] {
                        OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_Yes,
                        OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_No
                     }
                     : new[] {
                        OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_YesAll,
                        OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_Yes,
                        OperatingMethodsStrings.ChangeUserShellFolder_SubfolderFound_No
                     }, 0));
               if (result.NumberOfClickedButton == 2) {
                  moveAll = true;
               }
               else {
                  move = result.NumberOfClickedButton == 1;
               }
            }

            if (moveAll || move) {
               string newPathOfChild = Path.Combine(newDir.FullName,
                  child.Value.FullName.Substring(currentPath.FullName.Length));
               foreach (RegistryValue x in child.Key.RegistryValues.Select(x=>x.Item2)) {
                  bool retry;
                  bool skip = false;
                  do {
                     retry = false;
                     if (!Wrapper.RegistryMethods.SetRegistryValue(x, newPathOfChild, RegistryValueKind.String, usf.AccessAsUser)) {
                        switch (ExtendedMessageBox.Show(new ExtendedMessageBoxConfiguration(
                           string.Format(OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Text, child.Key.ViewedName,
                              x.ValueName, x.RegistryKey, newPathOfChild), OperatingMethodsStrings.Error,
                           new[] {
                              OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Retry,
                              string.Format(OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Skip, child.Key.ViewedName),
                              OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Ignore,
                              OperatingMethodsStrings.ChangeUserShellFolder_ErrorChangeSubfolder_Abort
                           }, 0)).NumberOfClickedButton) {
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

                  if (skip) {
                     break;
                  }
               }
            }
         }

         if (usf.RegistryValues.All(x =>
            Wrapper.RegistryMethods.SetRegistryValue(x, newDir.FullName, RegistryValueKind.String, usf.AccessAsUser))) {
            if (newDir.Exists && oldDir.Exists && usf.MoveExistingFiles &&
                (copyContents == OperatingMethods.QuestionAnswer.Yes || copyContents == OperatingMethods.QuestionAnswer.Ask &&
                 MessageBox.Show(
                    OperatingMethodsStrings.ChangeUserShellFolder_MoveContent_Text,
                    OperatingMethodsStrings.ChangeUserShellFolder_MoveContent_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                    MessageBoxDefaultButton.Button1) ==
                 DialogResult.Yes)) {
               if (Wrapper.FileAndFolder.MoveDirectory(oldDir, newDir)) {
                  Wrapper.FileAndFolder.DeleteDirectory(oldDir, true, false);
                  Wrapper.ExecuteCommand($"mklink /D \"{oldDir.FullName}\\\" \"{newDir.FullName}\"", true, true);
               }
            }

            return true;
         }

         return false;
      }
   }
}