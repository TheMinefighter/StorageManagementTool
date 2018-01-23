using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace StorageManagementTool
{
    public static class OperatingMethods
    {
        /// <summary>
        /// Creates a string representation of an DriveInfo
        /// </summary>
        /// <param name="item">The DriveInfo object to represent</param>
        /// <returns>The string representation</returns>
        public static string DriveInfoAsString(DriveInfo item)
        {
            return item.IsReady
                ? item.VolumeLabel + " (" + item.Name + " ; " +
                  GermanDriveType(item.DriveType) + ')'
                : item.Name;
        }

        /// <summary>
        /// Reads the whole content of an StreamReader
        /// </summary>
        /// <param name="reader">The StreamReader to read from</param>
        /// <returns>The Strings saved in the StreamReader</returns>
        public static string[] FromStream(this StreamReader reader)
        {
            List<string> ret = new List<string>();
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ret.Add(line);
                }
            }
            return ret.ToArray();
        }

        public static bool MoveFolder(DirectoryInfo dir, DirectoryInfo newLocation)
        {
            if (dir == newLocation)
            {
                if (MessageBox.Show("Fehler", "Fehler: Die Pfade dürfen nicht gleich sein",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    MoveFolder(dir, newLocation);
            }

            if (!newLocation.Parent.Exists)
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

            return Wrapper.ExecuteCommand($"mklink /D \"{dir.FullName}\" \"{newLocation.FullName}\"", true,true);
            throw new NotImplementedException();
        }

        public static bool MoveFile(FileInfo file, FileInfo newLocation)
        {
            if (file == newLocation)
            {
                if (
                    MessageBox.Show("Fehler", "Fehler: Die Pfade dürfen nicht gleich sein",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    MoveFile(file, newLocation);
                else return false;
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

        /// <summary>
        /// Moves an file or Directory from an old path to a new path using .symlink
        /// </summary>
        /// <param name="newPath">The new path of the data</param>
        /// <param name="oldPath">The old path of the data to move</param>
        /// <param name="file">Whether its a file or a directory</param>
        /// <returns></returns>
        public static bool MoveFolderOrFile(string newPath, string oldPath, bool file)
        {
            if (oldPath == "" || newPath == "")
            {
                if (
                    MessageBox.Show("Fehler", "Fehler: Der Pfad darf nicht leer sein", MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    return MoveFolderOrFile(newPath, oldPath, file);
                }
            }

            if (oldPath == newPath)
            {
                if (
                    MessageBox.Show("Fehler", "Fehler: Die Ordner dürfen nicht gleich sein",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    return false;
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            if (!Directory.Exists(Path.GetDirectoryName(newPath)))
            {
                if (
                    MessageBox.Show("Fehler",
                        "Fehler: Der Pfad auf der NewPath existiert nicht und kann nicht erzeugt werden",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    return MoveFolderOrFile(newPath, oldPath, file);
                }
            }

            if (!file)
            {
                Directory.CreateDirectory(oldPath);
                if (!Directory.Exists(oldPath))
                {
                    if (
                        MessageBox.Show("Fehler",
                            "Fehler: Der zu verschiebende Ordner existiert nicht und kann nicht erzeugt werden",
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    {
                        return MoveFolderOrFile(newPath, oldPath, file);
                    }
                }
            }

            if (file)
            {
                File.Copy(oldPath, newPath, true);
                File.Delete(oldPath);
                Wrapper.ExecuteCommand($"mklink \"{oldPath}\" \"{newPath}\"", true, true);
                return true;
            }
            else
            {
                try
                {
                    if (!Wrapper.CopyDirectory(new DirectoryInfo(oldPath), new DirectoryInfo(newPath)))
                    {
                        return false;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    if (MessageBox.Show("Das Programm ist aktuell nicht berechtigt die folgende" +
                                        " Operation" +
                                        ". Möchten sie das Programm mit Administartorrechten neustarten?", "Fehler",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        Wrapper.RestartAsAdministrator();
                    }
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Eine Datei wurde während des Kopier-Vorgangs entfernt", "Fehler");
                }

                Wrapper.DeleteDirectory(new DirectoryInfo(oldPath));
                Wrapper.ExecuteCommand($"mklink /D \"{oldPath}\\\" \"{newPath}\"", true,
                    true);
                return true;
            }
        }

        public static bool ChangeSwapfileStadium(int currentStadium, bool fwd)
        {
            if (currentStadium == 1 && !fwd)
            {
                MessageBox.Show("Fehler", "Es gibt kein Stadium vor dem ersten", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            if (currentStadium == 4 && fwd)
            {
                MessageBox.Show("Fehler", "Es gibt kein Stadium nach dem letzten", MessageBoxButtons.OK,
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
                        List<char> HDDList = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys")
                            .ToArray().ToList();
                        HDDList.RemoveAt(1);
                        if (Session.Singleton.CfgJson.DefaultHDDPath == "")
                        {
                            if (MessageBox.Show(
                                    "Fehler", "Es wurde kein Pfad für den NewPath Speicherort eingegeben",
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
                            new DirectoryInfo(Session.Singleton.CfgJson.DefaultHDDPath);
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

                        string newPath = (Session.Singleton.CfgJson.DefaultHDDPath + "\\" + new string(HDDList.ToArray()));
                        string oldPath = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys");
                        MoveFile(new FileInfo(oldPath),new FileInfo(newPath) );
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
            else
            {
                switch (Session.Singleton.Swapstadium)
                {
                    case 2:
                        Registry.SetValue(
                            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                            "SwapFileControl", 1, RegistryValueKind.DWord);
                        break;
                    case 3:
                        File.Delete(Path.Combine(Session.Singleton.CfgJson.DefaultHDDPath,
                            Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys").Remove(1, 1)));
                        break;
                    case 4:
                        Registry.SetValue(
                            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                            "SwapFileControl", 0, RegistryValueKind.DWord);
                        break;
                }
            }

            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                "SwapFileControl", 1, RegistryValueKind.DWord);

            return false;
        }

        /// <summary>
        /// Recommends Paths to move to NewPath
        /// </summary>
        /// <returns>The recommended Paths</returns>
        public static string[] GetRecommendedPaths()
        {
            List<string> ret = new List<string>();
            if (
                (!Wrapper.IsPathSymbolic((Environment.ExpandEnvironmentVariables(@"%AppData%")))))
            {
                ret.Add(Environment.ExpandEnvironmentVariables(@"%AppData%"));
            }

            List<string> blacklist = new List<string>
            {
                (Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData")),
                (Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Local\Microsoft")),
                (Environment.ExpandEnvironmentVariables(@"%temp%")),
                (Environment.ExpandEnvironmentVariables(@"%tmp%"))
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
        /// Gets German Names for DriveTypes
        /// </summary>
        /// <param name="toName">The DriveType Object, which name should be returned</param>
        /// <returns>The German name of the DriveType Object</returns>
        public static string GermanDriveType(DriveType toName)
        {
            switch (toName)
            {
                // Namen von https://msdn.microsoft.com/de-de/library/system.io.drivetype.aspx
                case DriveType.CDRom: return "CD-ROM/DVD";
                case DriveType.Fixed: return "Fester Datenträger";
                case DriveType.Network: return "Netzwerk Datenträger";
                case DriveType.Ram: return "RAM-Datenträger";
                case DriveType.Removable: return "Wechseldatenträger";
                case DriveType.NoRootDirectory: return "Fehler: kein Stammverzeichniss";
                case DriveType.Unknown:
                default: return "Unbekannter Datenträger";
            }
        }

        /// <summary>
        /// Changes the systems pagefile settings
        /// </summary>
        /// <param name="currentSelection">The selected partition entry</param>
        /// <param name="maxSize">The maximum Size of the Pagefile</param>
        /// <param name="minSize">The minimum Size of the Pagefile</param>
        /// 
        /// <returns>Whether the Operation were successfull</returns>
        public static bool ChangePagefilesettings(string currentSelection, int maxSize, int minSize)
        {
            Session.Singleton.RefreshDriveInformation();
            List<string> tempDriveInfoList = Session.Singleton.FillWithDriveInfo().ToList();
            int selectedPartitionIndex;
            if (tempDriveInfoList.Contains(currentSelection)) //Tests whether the selected partition is available
            {
                selectedPartitionIndex = tempDriveInfoList.IndexOf(currentSelection);
                currentSelection = tempDriveInfoList[selectedPartitionIndex];
            }
            else
            {
                MessageBox.Show("Die ausgewählte Partition konnte nichtmehr gefunden werden", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            DriveInfo toUse = Session.Singleton.CurrentDrives[selectedPartitionIndex];
            if (maxSize < minSize) //Tests whether the maxSize is smaller than the minSize
            {
                MessageBox.Show("Die minimale Größe der Pagefile kann nicht größer sein als derren die maximale Größe!",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (toUse.AvailableFreeSpace < minSize * 1048576L) //Tests whether enough space is available
            {
                MessageBox.Show("Auf der ausgewählten Partition ist nicht genug Speicherplatz verfügbar.", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Wrapper.ExecuteExecuteable(
                Environment.ExpandEnvironmentVariables("%windir%\\system32\\wbem\\wmic.exe") //Tests
                ,
                "computersystem get AutomaticManagedPagefile /Value"
                , returnData: out string[] tmp, exitCode: out int _, readReturnData: true, waitforexit: true,
                hidden: true, admin: true, asUser: false))
            {
                if (bool.Parse(tmp[2].Split('=')[1]))
                {
                    Wrapper.ExecuteCommand(
                        cmd: Environment.ExpandEnvironmentVariables("%windir%\\system32\\wbem\\wmic.exe")
                             + Environment.ExpandEnvironmentVariables(
                                 " computersystem where \"name='%computername%' \" set AutomaticManagedPagefile=False")
                        , admin: true, hidden: true, returnData: out _); //Disables automatic Pagefile  management
                    Wrapper.ExecuteExecuteable(
                        Environment.ExpandEnvironmentVariables("%windir%\\system32\\wbem\\wmic.exe")
                        , "computersystem get AutomaticManagedPagefile /Value"
                        , returnData: out tmp, exitCode: out int _, waitforexit: true, hidden: true, admin: true,
                        asUser: false);
                    if (!bool.Parse(tmp[2].Split('=')[1]))
                    {
                        MessageBox.Show("Parameter \"AutomaticManagedPagefile\" konnte nicht auf False gesetzt werden",
                            "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            Wrapper.ExecuteExecuteable(Environment.ExpandEnvironmentVariables("%windir%\\system32\\wbem\\wmic.exe"),
                "pagefileset delete /NOINTERACTIVE", returnData: out _, exitCode: out int _, waitforexit: true,
                hidden: true, admin: true, asUser: false); //Deletes all Pagefiles

            Wrapper.ExecuteExecuteable(Environment.ExpandEnvironmentVariables("%windir%\\system32\\wbem\\wmic.exe"),
                $"pagefileset create name=\"{Path.Combine(toUse.Name, "Pagefile.sys")}\"", returnData: out _,
                exitCode: out int _,
                waitforexit: true, hidden: true, admin: true, asUser: false); //Creates new Pagefile

            Wrapper.ExecuteExecuteable(Environment.ExpandEnvironmentVariables("%windir%\\system32\\wbem\\wmic.exe"),
                $"pagefileset where name=\"{Path.Combine(toUse.Name, "Pagefile.sys")}\" set InitialSize={minSize},MaximumSize={maxSize}",
                returnData: out _, exitCode: out int _, waitforexit: true,
                hidden: true, admin: true, asUser: false); // Sets Pagefile Size

            Wrapper.ExecuteExecuteable(Environment.ExpandEnvironmentVariables("%windir%\\system32\\wbem\\wmic.exe"),
                " get", returnData: out tmp, exitCode: out int _, readReturnData: true, waitforexit: true,
                hidden: true); //Checks wether there is exactly 1 pagefile existing
            if (tmp.Length != 2)
            {
                switch (MessageBox.Show("Beim ändern der Pagefile-Konfiguration ist ein Fehler aufgetreten.", "Fehler",
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Cancel: return false;
                    case DialogResult.Retry: return ChangePagefilesettings(currentSelection, maxSize, minSize);
                }
            }

            return true;
        }

        /// <summary>
        /// Moves an User ShellFolder to a new Location 
        /// </summary>
        /// <param name="oldDir">The old Directory of</param>
        /// <param name="newDir">The new Directory of the new </param>
        /// <param name="usfIndex"></param>
        /// <returns>Whether the Operation were successful</returns>
        public static bool ChangeUserShellFolder(DirectoryInfo oldDir, DirectoryInfo newDir, UserShellFolder usf)
        {
            if (newDir.Exists)
            {
                if (usf.RegPaths.All(x =>
                    Wrapper.SetRegistryValue(x, newDir.FullName, RegistryValueKind.String, usf.AccessAsUser)))
                {
                    if (newDir.Exists && oldDir.Exists && usf.MoveExistingFiles && MessageBox.Show(
                            "Der UserShellFolder wurde erfolgreich geändert, sollen dessen Inhalte" +
                            " an seinen neuen Speicherort verschoben werden?",
                            "Dateien an neuen Speicherort verschieben",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button1) ==
                        DialogResult.Yes)
                    {
                        if (Wrapper.CopyDirectory(oldDir, newDir))
                        {
                            if (MessageBox.Show($"Soll der Ordner {oldDir.FullName} gelöscht werden," +
                                                $" da seine Inhalte in {newDir.FullName} gespeichert sind?",
                                    "Alte Dateien löschen?",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                if (Wrapper.DeleteDirectory(oldDir) && !oldDir.Exists)
                                {
                                    Wrapper.ExecuteCommand($"mklink /D {oldDir.FullName}\\ {newDir.FullName}", true,
                                        true);
                                }
                            }
                        }
                    }

                    if (MessageBox.Show("Sollen die Änderungen durch einen Neustart des Explorers angewendet werden? " +
                                        "Dies kann dazu führen dass der Bildschirm für kurze Zeit ein falsches oder garkein Bild anzeigt," +
                                        " außerdem Werden Änderungen an der Anordnung von Objekten auf dem Desktop und im Startmenü zurückgesetzt," +
                                        " wenn der Benutzer nach der Änderung noch nicht abgemeldet wurde." +
                                        " Tipp: Die Ändeungen können auch durch das Ab- und wieder Anmelden des Benutzers angewendet werden.",
                            "Änderungen anwenden und Explorer neustarten?", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Wrapper.ExecuteCommand("taskkill /IM explorer.exe /F & explorer.exe", false, true);
                    }

                    return true;
                }

                return false;
            }
            else
            {
                MessageBox.Show("Der angegebene Pfad existiert nicht", "Fehler", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                return false;
            }
        }
    }
}