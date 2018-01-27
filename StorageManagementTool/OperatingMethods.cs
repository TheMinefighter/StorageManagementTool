using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using StorageManagementTool.GlobalizationRessources;

namespace StorageManagementTool
{
    public static class OperatingMethods
    {
        public enum QuestionAnswer
        {
            Yes,
            No,
            Ask
        }

        /// <summary>
        ///     Creates a string representation of an DriveInfo
        /// </summary>
        /// <param name="item">The DriveInfo object to represent</param>
        /// <returns>The string representation</returns>
        public static string DriveInfoAsString(DriveInfo item)
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
                if (MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.MoveFolderOrFile_PathsEqual,
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    MoveFolder(dir, newLocation);
                }
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

            return Wrapper.ExecuteCommand($"mklink /D \"{dir.FullName}\" \"{newLocation.FullName}\"", true, true);
            // throw new NotImplementedException();
        }

        public static bool MoveFile(FileInfo file, FileInfo newLocation)
        {
            if (file == newLocation)
            {
                if (
                    MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.MoveFolderOrFile_PathsEqual,
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

                        string newPath = Session.Singleton.CfgJson.DefaultHDDPath + "\\" +
                                         new string(HDDList.ToArray());
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
                    File.Delete(Path.Combine(Session.Singleton.CfgJson.DefaultHDDPath,
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
        ///     Recommends Paths to move to NewPath
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
        ///     Gets names of DriveTypes
        /// </summary>
        /// <param name="toName">The DriveType Object, which name should be returned</param>
        /// <returns>The  name of the DriveType Object</returns>
        public static string DriveType2String(DriveType toName)
        {
            switch (toName)
            {
                case DriveType.CDRom: return OperatingMethodsStrings.DriveType2String_CDRom;
                case DriveType.Fixed: return OperatingMethodsStrings.DriveType2String_Fixed;
                case DriveType.Network: return OperatingMethodsStrings.DriveType2String_Network;
                case DriveType.Ram: return OperatingMethodsStrings.DriveType2String_RAM;
                case DriveType.Removable: return OperatingMethodsStrings.DriveType2String_Removable;
                case DriveType.NoRootDirectory: return OperatingMethodsStrings.DriveType2String_NoRootDirectory;
                case DriveType.Unknown:
                default: return OperatingMethodsStrings.DriveType2String_Unknown;
            }
        }

        /// <summary>
        ///     Changes the systems pagefile settings
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
                currentSelection = tempDriveInfoList[selectedPartitionIndex];
            }
            else
            {
                MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_SelectedPartitionMissing,
                    OperatingMethodsStrings.Error,
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
                MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_MinGreaterMax,
                    OperatingMethodsStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (toUse.AvailableFreeSpace < minSize * 1048576L) //Tests whether enough space is available
            {
                MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_NotEnoughSpace,
                    OperatingMethodsStrings.Error,
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
                        MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_CouldntDisableManagement,
                            OperatingMethodsStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                switch (MessageBox.Show(OperatingMethodsStrings.ChangePagefileSettings_Not1Pagefile,
                    OperatingMethodsStrings.Error,
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Cancel: return false;
                    case DialogResult.Retry: return ChangePagefileSettings(toUse, maxSize, minSize);
                }
            }

            return true;
        }

        /// <summary>
        ///     Moves an User ShellFolder to a new Location
        /// </summary>
        /// <param name="oldDir">The old Directory of</param>
        /// <param name="newDir">The new Directory of the new </param>
        /// <param name="usf">The UserShellFolder to edit</param>
        /// <returns>Whether the Operation were successful</returns>
        public static bool ChangeUserShellFolder(DirectoryInfo oldDir, DirectoryInfo newDir, UserShellFolder usf,
            QuestionAnswer CopyContents = QuestionAnswer.Ask, QuestionAnswer DeleteOldContents = QuestionAnswer.Ask)
        {
            if (!newDir.Exists)
            {
                newDir.Create();
            }

            if (usf.RegPaths.All(x =>
                Wrapper.SetRegistryValue(x, newDir.FullName, RegistryValueKind.String, usf.AccessAsUser)))
            {
                if (newDir.Exists && oldDir.Exists && usf.MoveExistingFiles &&
                    (CopyContents == QuestionAnswer.Yes || CopyContents == QuestionAnswer.Ask && MessageBox.Show(
                         OperatingMethodsStrings.ChangeUserShellFolder_MoveContent_Text,
                         OperatingMethodsStrings.ChangeUserShellFolder_MoveContent_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                         MessageBoxDefaultButton.Button1) ==
                     DialogResult.Yes))
                {
                    if (Wrapper.CopyDirectory(oldDir, newDir))
                    {
                        if (DeleteOldContents == QuestionAnswer.Yes || DeleteOldContents == QuestionAnswer.Ask && MessageBox.Show(
                                string.Format(
                                    OperatingMethodsStrings.ChangeUserShellFolder_DeleteContent_Text,
                                    oldDir.FullName, newDir.FullName),
                                OperatingMethodsStrings.ChangeUserShellFolder_DeleteContent_Title,
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
                        OperatingMethodsStrings.ChangeUserShellFolder_RestartExplorer_Text,
                        OperatingMethodsStrings.ChangeUserShellFolder_RestartExplorer_Title, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Wrapper.ExecuteCommand("taskkill /IM explorer.exe /F & explorer.exe", false, true);
                }

                return true;
            }

            return false;
        }
    }
}