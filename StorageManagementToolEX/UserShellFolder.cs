using System.IO;
using System.Linq;

namespace StorageManagementTool
{
    public struct UserShellFolder
    {
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

        public UserShellFolder(string Name, RegPath[] Regpaths, bool moveExistingFiles = true,
            bool accessAsUser = false)
        {
            ViewedName = Name;
            RegPaths = Regpaths;
            MoveExistingFiles = moveExistingFiles;
            AccessAsUser = accessAsUser;
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
                MoveExistingFiles = moveExistingFiles
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
                MoveExistingFiles = moveExistingFiles
            };
        }

        public string ViewedName;
        public RegPath[] RegPaths;
        public bool MoveExistingFiles;
        public bool AccessAsUser;

        public static void LoadEditable()
        {
        }

        public static UserShellFolder[] AllEditableUserUserShellFolders =
        {
            #region Based upon https://support.microsoft.com/en-us/help/931087/how-to-redirect-user-shell-folders-to-a-specified-path-by-using-profil access on 22.01.2017

            NormalUSF("Desktop", "Desktop"),
            NormalUSF("Eigene Dokumente", "Personal"),
            NormalUSF("Eigene Videos", "My Video"),
            NormalUSF("Eigene Musik", "My Music"),
            NormalUSF("Eigene Bilder", "My Pictures"),
            NormalUSF("Senden an", "SendTo"),
            NormalUSF("Anwendungsdaten\\Local", "Local AppData"),
            NormalUSF("Anwendungsdaten\\Roaming", "AppData"),
            NormalUSF("Startmenü Inhalt", "Programs"),
            NormalUSF("Startmenü", "Start Menu"),
            NormalUSF("Autostart", "Startup"),
            NormalUSF("Verlauf", "History"),
            NormalUSF("Favoriten", "Favorites"),
            NormalUSF("Schriftarten", "Fonts", false),
            NormalUSF("Zuletzt verwendet", "Recent"),
            NormalUSF("Vorlagen", "Templates"),
            NormalUSF("Administratorenwerkzeuge", "Administrative Tools", false),
            NormalUSF("Cookies", "Cookies", false),
            NormalUSF("Netzwerkverknüpfungen", "NetHood", false),
            NormalUSF("Druckerverknüpfung", "PrintHood", false),
            NormalUSF("Internetcache", "Cache", false),
            NormalUSF("Cache zum CD brennen", "CD Burning", false),
            NormalUSF("Downloads", "{374DE290-123F-4565-9164-39C4925E467B}"),

            #endregion

            //Common 
            CommonUSF("Öffentliche Videos", "CommonVideo"),
            CommonUSF("Öffentliche Dokumente", "Common Documents"),
            CommonUSF("Öffentliche Bilder", "CommonPictures"),
            CommonUSF("Öffentliche Musik", "CommonMusic"),
            CommonUSF("Öffentlicher Desktop", "Common Desktop"),
            CommonUSF("Öffentliche Anwendungsdaten", "Common AppData"),
            CommonUSF("Öffentlicher Autostart", "Common Start Menu"),
            CommonUSF("Öffentliche Starmenü Programme", "Common Programs"),
            CommonUSF("Öffentliche Vorlagen", "Common Templates"),
            CommonUSF("Öffentliches Startmenü", "Common Start Menu"),
            CommonUSF("Hersteller Links", "OEM Links", false),
            CommonUSF("Öffentliche Administratorenwerkzeuge", "OEM Links", false),
            //No real USF
            new UserShellFolder("Installationspfad für x86 basierte Programme",
                new[]
                {
                    new RegPath(ProgramPathDefinitionRoot, "ProgramFilesDir (x86)")
                }, false, true),
            new UserShellFolder("Installationspfad für x64 basierte Programme",
                new[]
                {
                    new RegPath(ProgramPathDefinitionRoot, "ProgramFilesDir"),
                    new RegPath(ProgramPathDefinitionRoot, "ProgramW6432Dir")
                }, false, true)
        };

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
            return new DirectoryInfo((string) regValue ?? "Fehler");
        }
    }
}