using System;
using System.IO;
using System.Linq;
using static StorageManagementTool.GlobalizationRessources.UserShellFolderStrings;

namespace StorageManagementTool
{
   public struct UserShellFolder
   {

      public string ViewedName;
      public RegistryValue[] RegistryValues;
      public bool MoveExistingFiles;
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
      private UserShellFolder(string name, RegistryValue[] registryValues, bool moveExistingFiles = true,
         bool accessAsUser = false)
      {
         if (registryValues.Length == 0)
         {
            throw new ArgumentException("At least on required");
         }
         ViewedName = name;
         RegistryValues = registryValues;
         MoveExistingFiles = moveExistingFiles;
         AccessAsUser = accessAsUser;
         Identifier = registryValues[0].ValueName;
      }

      //   public UserShellFolder()  {  }

      private static UserShellFolder NormalUSF(string name, string id, bool user = true, bool moveExistingFiles = true)
      {
         return new UserShellFolder
         {
            ViewedName = name,
            RegistryValues = user
               ? new[] { new RegistryValue(ShellFolderRoot, id), new RegistryValue(UserShellFolderRoot, id) }
               : new[] { new RegistryValue(ShellFolderRoot, id) },
            MoveExistingFiles = moveExistingFiles,
            Identifier = id
         };
      }

      private static UserShellFolder CommonUSF(string name, string id, bool user = true, bool moveExistingFiles = true,bool asUser=true)
      {
         return new UserShellFolder
         {
            ViewedName = name,
            RegistryValues = user
               ? new[] { new RegistryValue(CommonShellFolderRooot, id), new RegistryValue(CommonUserShellFolderRoot, id) }
               : new[] { new RegistryValue(CommonShellFolderRooot, id) },
            MoveExistingFiles = moveExistingFiles,
            Identifier = id,
            AccessAsUser = asUser
         };
      }


      public static void LoadEditable()
      {
         AllEditableUserUserShellFolders = new[]
         {
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
            CommonUSF(Common_Documents_Name, "Common Documents"),
            CommonUSF(CommonVideo_Name, "CommonVideo"),

            CommonUSF(CommonPictures_Name, "CommonPictures"),
            CommonUSF(CommonMusic_Name, "CommonMusic"),
            CommonUSF(Common_Desktop_Name, "Common Desktop"),
            CommonUSF(Common_AppData_Name, "Common AppData"),
            CommonUSF(Common_Startup_Name, "Common Startup"),
            CommonUSF(Common_Programs_Name, "Common Programs"),
            CommonUSF(Common_Templates_Name, "Common Templates"),
            CommonUSF(Common_Start_Menu_Name, "Common Start Menu"),
            CommonUSF(OEM_Links_Name, "OEM Links", false,true,true),
            CommonUSF(Common_Administrative_Tools_Name, "Common Administrative Tools", false),
            //No real USF
            new UserShellFolder(ProgramFilesDir_x86_Name,
               new[]
               {
                  new RegistryValue(ProgramPathDefinitionRoot, "ProgramFilesDir (x86)")
               }, false, true),
            new UserShellFolder(ProgramFilesDir_Name,
               new[]
               {
                  new RegistryValue(ProgramPathDefinitionRoot, "ProgramFilesDir"),
                  new RegistryValue(ProgramPathDefinitionRoot, "ProgramW6432Dir")
               }, false, true),
            new UserShellFolder{Identifier = "PrivateTemp",AccessAsUser = false,
               RegistryValues = new []{new RegistryValue(UserTempRoot,"TEMP"), new RegistryValue(UserTempRoot,"TMP"), },
               MoveExistingFiles = false, ViewedName = PrivateTemp},
            new UserShellFolder{Identifier = "PublicTemp",AccessAsUser = true,
            RegistryValues = new []{new RegistryValue(PublicTempRoot,"TEMP"), new RegistryValue(PublicTempRoot,"TMP"), },
               MoveExistingFiles = false, ViewedName = PublicTemp,},
         };
      }

      public static UserShellFolder[] AllEditableUserUserShellFolders;

      public static UserShellFolder GetUserShellFolderById(string id)
      {
         return AllEditableUserUserShellFolders.First(x => x.Identifier == id);
      }

      public static UserShellFolder GetUserShellFolderByName(string name)
      {
         return AllEditableUserUserShellFolders.First(x => x.ViewedName == name);
      }

      public DirectoryInfo GetPath()
      {
         Wrapper.RegistryMethods.GetRegistryValue(RegistryValues[0], out object regValue, AccessAsUser);
         return new DirectoryInfo((string)regValue ?? Error);
      }

      public static DirectoryInfo GetPath(UserShellFolder currentUSF)
      {
         return currentUSF.GetPath();
      }
   }
}