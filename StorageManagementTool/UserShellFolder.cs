using System;
using System.IO;
using System.Linq;
using static StorageManagementTool.GlobalizationRessources.UserShellFolderStrings;
//using static StorageManagementTool.MainGUI.GlobalizationRessources.EditUserShellFolderStrings;

namespace StorageManagementTool
{
   public struct UserShellFolder
   {

      public string ViewedName;
      public RegPath[] RegPaths;
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

      public UserShellFolder(string name, RegPath[] regPaths, bool moveExistingFiles = true,
         bool accessAsUser = false)
      {
         if (regPaths.Length==0)
         {
            throw new ArgumentException("At least on required");
         }
         ViewedName = name;
         RegPaths = regPaths;
         MoveExistingFiles = moveExistingFiles;
         AccessAsUser = accessAsUser;
         Identifier = regPaths[0].ValueName;
      }

      //   public UserShellFolder()  {  }

      private static UserShellFolder NormalUSF(string name, string id, bool user = true, bool moveExistingFiles = true)
      {
         return new UserShellFolder
         {
            ViewedName = name,
            RegPaths = user
               ? new[] {new RegPath(ShellFolderRoot, id), new RegPath(UserShellFolderRoot, id)}
               : new[] {new RegPath(ShellFolderRoot, id)},
            MoveExistingFiles = moveExistingFiles,
            Identifier = id
         };
      }

      private static UserShellFolder CommonUSF(string name, string id, bool user = true, bool moveExistingFiles = true) 
      {
         return new UserShellFolder
         {
            ViewedName = name,
            RegPaths = user
               ? new[] {new RegPath(CommonShellFolderRooot, id), new RegPath(CommonUserShellFolderRoot, id)}
               : new[] {new RegPath(CommonShellFolderRooot, id)},
            MoveExistingFiles = moveExistingFiles,
            Identifier = id
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
            CommonUSF(OEM_Links_Name, "OEM Links", false),
            CommonUSF(Common_Administrative_Tools_Name, "Common Administrative Tools", false),
            //No real USF
            new UserShellFolder(ProgramFilesDir_x86_Name,
               new[]
               {
                  new RegPath(ProgramPathDefinitionRoot, "ProgramFilesDir (x86)")
               }, false, true),
            new UserShellFolder(ProgramFilesDir_Name,
               new[]
               {
                  new RegPath(ProgramPathDefinitionRoot, "ProgramFilesDir"),
                  new RegPath(ProgramPathDefinitionRoot, "ProgramW6432Dir")
               }, false, true)
         };
      }

      public static UserShellFolder[] AllEditableUserUserShellFolders;

      public static UserShellFolder GetUSFById(string id)
      {
         return AllEditableUserUserShellFolders.First(x => x.RegPaths.Any(y => y.ValueName == id));
      }

      public static UserShellFolder GetUSFByName(string name)
      {
         return AllEditableUserUserShellFolders.First(x => x.ViewedName == name);
      }

      public DirectoryInfo GetPath()
      {
         return GetPath(this);
      }

      public static DirectoryInfo GetPath(UserShellFolder currentUSF)
      {
         Wrapper.GetRegistryValue(currentUSF.RegPaths[0], out object regValue, currentUSF.AccessAsUser);
         return new DirectoryInfo((string) regValue ?? Error);
      }
   }
}