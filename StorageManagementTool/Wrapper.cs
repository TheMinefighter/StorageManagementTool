using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using static StorageManagementTool.GlobalizationRessources.WrapperStrings;

namespace StorageManagementTool
{
    /// <summary>
    ///     Contains system functionalities, which are not specific made for this project
    /// </summary>
    public static class Wrapper
    {
        private static readonly string[] ExecuteableExtensions = {".exe", ".pif", ".com", ".bat", ".cmd"};
        public static readonly string WinPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        public static readonly string System32Path = Environment.GetFolderPath(Environment.SpecialFolder.System);
        public static readonly string ExplorerPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe");

        /// <summary>
        ///     Executes an executeable
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
        ///     Gives the WIN32APi Representation of an given RegistryValueKind
        /// </summary>
        /// <param name="kind"> The RegistryValueKind to represent</param>
        /// <returns>The WIN32API Representation of the given RegistryValueKind</returns>
        public static string Win32ApiRepresentation(this RegistryValueKind kind)
        {
            switch (kind)
            {
                case RegistryValueKind.String: return "REG_SZ";
                case RegistryValueKind.ExpandString: return "REG_EXPAND_SZ";
                case RegistryValueKind.Binary: return "REG_BINARY";
                case RegistryValueKind.DWord: return "REG_DWORD";
                case RegistryValueKind.MultiString: return "REG_MULTI_SZ";
                case RegistryValueKind.QWord: return "REG_QWORD";
                case RegistryValueKind.Unknown: return "REG_RESSOURCE_LIST";
                case RegistryValueKind.None: return "REG_NONE";
                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        public static bool GetRegistryValue(RegPath path, out object toReturn, bool asUser = false)
        {
            toReturn = null;
            if (asUser)
            {
                if (!ExecuteExecuteable(Path.Combine(System32Path, @"reg.exe"),
                    $" query \"{path.RegistryKey}\" /v \"{path.ValueName}\"", out string[] ret, out int _,
                    true, true, true, false, true))
                {
                    return false;
                }

                if (ret.Length == 2)
                {
                    return true;
                }

                char[] tmp = ret[2].ToCharArray();
                string toProcess = new string(tmp).Substring(8 + path.ValueName.Length);
                int i = 0;
                StringBuilder Builder = new StringBuilder();
                char c;
                do
                {
                    c = toProcess[i];
                    Builder.Append(c);
                    i++;
                } while (c != ' ');

                string toSwitch = Builder.ToString();
                RegistryValueKind kind = Enum.GetValues(typeof(RegistryValueKind)).Cast<RegistryValueKind>()
                    .FirstOrDefault(x => toSwitch.Contains(x.Win32ApiRepresentation()));
                string data = new string(tmp.Skip(12 + path.ValueName.Length + kind.ToString().Length).ToArray());
                toReturn = data;
                switch (kind)
                {
                    case RegistryValueKind.DWord:
                        toReturn = uint.Parse(new string(data.Skip(2).ToArray()), NumberStyles.HexNumber);
                        break;
                    case RegistryValueKind.String:
                        toReturn = data;
                        break;
                    case RegistryValueKind.ExpandString: break;
                    case RegistryValueKind.Binary: break;
                    case RegistryValueKind.MultiString:
                        toReturn = data.Split('\0');
                        break;
                    case RegistryValueKind.QWord:
                        toReturn = ulong.Parse(new string(data.Skip(2).ToArray()), NumberStyles.HexNumber);
                        break;
                    case RegistryValueKind.Unknown:
                        toReturn = data;
                        break;
                    case RegistryValueKind.None: return false;
                }

                return true;
            }

            try
            {
                toReturn = Registry.GetValue(path.RegistryKey, path.ValueName, null);
            }
            catch (Exception e)
            {
                return MessageBox.Show(
                           string.Format(GetRegistryValue_Exception,
                               path.ValueName, path.RegistryKey, e.Message),
                           Error, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) ==
                       DialogResult.Retry &&
                       GetRegistryValue(path, out toReturn, asUser);
            }

            return true;
        }

        /// <summary>
        ///     Executes an Executeable
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
            exitCode = 0;
            returnData = null;
            if (!File.Exists(filename))
            {
                if (MessageBox.Show(
                        string.Format(ExecuteExecuteable_FileNotFound, filename),
                        Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    OpenFileDialog alternativeExecuteableSel = new OpenFileDialog
                    {
                        Filter = $"Programme|*{string.Join(";*", ExecuteableExtensions)}"
                    };
                    alternativeExecuteableSel.ShowDialog();
                    return ExecuteExecuteable(alternativeExecuteableSel.FileName, parameters, out returnData,
                        out exitCode, waitforexit: waitforexit, hidden: hidden, admin: admin, asUser: asUser);
                }
            }

            if (!ExecuteableExtensions.Contains(new FileInfo(filename).Extension))
            {
                if (new DialogResult[] {DialogResult.No, DialogResult.None}.Contains(MessageBox.Show(
                    string.Format(ExecuteExecuteable_WrongEnding,
                        filename, new FileInfo(filename).Extension),
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
                FileName = filename,
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
                        filename),
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

        /// <summary>
        ///     Executes an Command using Windows Commandline
        /// </summary>
        /// <param name="cmd">The Command to Execute</param>
        /// <param name="admin">Whether the Command should be executed with</param>
        /// <param name="hidden">Whether to hide the Commandline </param>
        /// <param name="waitforexit">
        ///     Whether to wait until the command execution completed
        /// </param>
        /// <param name="debug">
        ///     Whether to run the command in debug mode
        /// </param>
        /// <returns>Whether the operation were successful</returns>
        public static bool ExecuteCommand(string cmd, bool admin, bool hidden, bool waitforexit = true,
            bool debug = false)
        {
            return ExecuteCommand(cmd, admin, hidden, out string[] _, waitforexit, debug);
        }
        /// <summary>
        /// Checks if one Path is the parent of another
        /// </summary>
        /// <param name="parentPath">The parent path</param>
        /// <param name="childPath">The child path</param>
        /// <returns>Whether parentPath is a paren of childPath</returns>
        public static bool IsSubfolder(DirectoryInfo parentPath, DirectoryInfo childPath)
        {
            return parentPath.FullName.StartsWith(childPath.FullName + Path.DirectorySeparatorChar);
        }
        /// <summary>
        ///     Executes an Command using Windows Commandline
        /// </summary>
        /// <param name="cmd">The Command to Execute</param>
        /// <param name="admin">Whether the Command should be executed with</param>
        /// <param name="hidden">Whether to hide the Commandline </param>
        /// <param name="returnData">The Data returned by the executeable</param>
        /// <param name="waitforexit">
        ///     Whether to wait until the command execution completed
        /// </param>
        /// <param name="debug">
        ///     Whether to run the command in debug mode
        /// </param>
        /// <param name="readReturnData">Whether to read the output of the Application</param>
        /// <returns>Whether the operation were successful</returns>
        public static bool ExecuteCommand(string cmd, bool admin, bool hidden, out string[] returnData,
            bool waitforexit = true, bool debug = false, bool readReturnData = false)
        {
            if (ExecuteExecuteable(Path.Combine(System32Path, @"cmd.exe"),
                (debug ? " /K " : " /C ") + cmd, out returnData, out int tmp, readReturnData, waitforexit, hidden,
                admin, false))
            {
                return tmp == 0;
            }

            return false;
        }

        #region From https://stackoverflow.com/a/3600342/6730162 access on 30.9.2017

        /// <summary>
        ///     Tests whether the program is being executed with Admin privileges
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

        /// <summary>
        ///     Copies a Directory
        /// </summary>
        /// <param name="src">The Directory to copy from</param>
        /// <param name="target">The Directory, where the contents of src should be copied to</param>
        /// <returns>Whether the operation were successful</returns>
        public static bool CopyDirectory(DirectoryInfo src, DirectoryInfo target)
        {
            try
            {
                FileSystem.CopyDirectory(src.FullName, target.FullName, UIOption.AllDialogs);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Deletes a Directory
        /// </summary>
        /// <param name="toBeDeleted">The Folder to delete</param>
        /// <param name="deletePermanent">Whether the Folder should be deleted permanently</param>
        /// <returns>Whether the operation were sucessful</returns>
        public static bool DeleteDirectory(DirectoryInfo toBeDeleted, bool deletePermanent = true)
        {
            try
            {
                FileSystem.DeleteDirectory(toBeDeleted.FullName, UIOption.AllDialogs,
                    deletePermanent ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Sets an Registry Value
        /// </summary>
        /// <param name="valueLocation">The Location of the Value to change</param>
        /// <param name="content">The content to write into the content</param>
        /// <param name="registryValueKind">The type of the content</param>
        /// <param name="asUser"></param>
        /// <returns></returns>
        public static bool SetRegistryValue(RegPath valueLocation, object content, RegistryValueKind registryValueKind,
            bool asUser = false)
        {
            if (asUser)
            {
                string value;
                switch (registryValueKind)
                {
                    case RegistryValueKind.DWord:
                        value = ((uint) content).ToString();
                        break;
                    case RegistryValueKind.QWord:
                        value = ((ulong) content).ToString();
                        break;
                    case RegistryValueKind.String:
                        value = ((string) content).Replace("\"", "\"\"");
                        break;
                    case RegistryValueKind.MultiString:
                        value = string.Join("\0", (string[]) content).Replace("\"", "\"\"");
                        break;
                    case RegistryValueKind.ExpandString:
                        value = ((string) content).Replace("\"", "\"\"");
                        break;
                    default:
                        value = (string) content;
                        break;
                }

                string kind = RegistryValueKind.String.ToString();
                if (!ExecuteExecuteable(Path.Combine(System32Path, "reg.exe"),
                        $" add \"{valueLocation.RegistryKey}\" /v \"{valueLocation.ValueName}\" /t {kind} /d \"{value}\"",
                        out string[] ret, out int tmpExitCode, true, true, true, false,
                        true) || tmpExitCode == 1)
                {
                }

                return true;
            }

            try
            {
                Registry.SetValue(valueLocation.ValueName, valueLocation.ValueName, content, registryValueKind);
            }
            catch (SecurityException)
            {
                if (MessageBox.Show(
                        string.Format(
                            SetRegistryValue_Security,
                            valueLocation.ValueName, valueLocation.RegistryKey, content, registryValueKind),
                        Error, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    return SetRegistryValue(valueLocation, content, registryValueKind);
                }

                return false;
            }
            catch (UnauthorizedAccessException)
            {
                if (MessageBox.Show(
                        string.Format(
                            SetRegistryValue_UnauthorizedAccess,
                            valueLocation.ValueName, valueLocation.RegistryKey, content, registryValueKind),
                        Error,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    RestartAsAdministrator();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                if (MessageBox.Show(
                        string.Format(
                            SetRegistry_Exception,
                            valueLocation.ValueName, valueLocation.ValueName, content, registryValueKind, e.Message),
                        Error, MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    return SetRegistryValue(valueLocation, content, registryValueKind);
                }

                return false;
            }

            return true;
        }

        #region From https://stackoverflow.com/a/26473940/6730162 access on 30.9.2017

        /// <summary>
        ///     Tests whether a File is a symlink
        /// </summary>
        /// <param name="path">The path of the file to test</param>
        /// <returns>Whether the file is a symlink</returns>
        public static bool IsPathSymbolic(string path)
        {
            FileInfo pathInfo = new FileInfo(path);
            return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }

        #endregion

        /// <summary>
        ///     Runs a given Action for each Element of an IEnumerable
        /// </summary>
        /// <param name="Base">The IEnumerable to perform Actions with</param>
        /// <param name="action">The Action to perform with each Element of the Base</param>
        /// <typeparam name="T">The Type of the IEnumerable</typeparam>
        public static void ForEach<T>(this IEnumerable<T> Base, Action<T> action)
        {
            foreach (T variable in Base)
            {
                action(variable);
            }
        }

        /// <summary>
        ///     Restarts Program as Administartor
        /// </summary>
        public static void RestartAsAdministrator()
        {
            if (ExecuteExecuteable(Process.GetCurrentProcess().MainModule.FileName, "", true, false, false))
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Copies a file 
        /// </summary>
        /// <param name="src">The location to copy from</param>
        /// <param name="to">The location to copy to</param>
        /// <returns>Whether the operation were successful</returns>
        public static bool CopyFile(FileInfo src, FileInfo to)
        {
            try
            {
                FileSystem.CopyFile(src.FullName, to.FullName, UIOption.AllDialogs);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="toDelete">The file to delete</param>
        /// <param name="deletePermanent">Whether it should be deleted permanently</param>
        /// <returns>Whether the operation were successful</returns>
        public static bool DeleteFile(FileInfo toDelete, bool deletePermanent = true)
        {
            try
            {
                FileSystem.DeleteFile(toDelete.FullName, UIOption.AllDialogs,
                    deletePermanent ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Reads the whole content of an StreamReader
        /// </summary>
        /// <param name="reader">The StreamReader to read from</param>
        /// <returns>The strings saved in the StreamReader</returns>
        public static string[] FromStream(this StreamReader reader)
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
            return (UserPrincipal.FindByIdentity(GetPrincipalContext(), IdentityType.SamAccountName, name) != null);
        }
        public static bool IsAdmin(string Username)
        {
            SecurityIdentifier id = new SecurityIdentifier("S-1-5-32-544");
            return UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, Username).GetGroups()
                .Any(x => x.Sid == id);
        }
        #region From https://stackoverflow.com/a/38308957/6730162 access on 30.9.2017

        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode,
            IntPtr SecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", EntryPoint = "GetFinalPathNameByHandleW", CharSet = CharSet.Unicode,
            SetLastError = true)]
        private static extern int GetFinalPathNameByHandle([In] IntPtr hFile, [Out] StringBuilder lpszFilePath,
            [In] int cchFilePath, [In] int dwFlags);

        private const int CREATION_DISPOSITION_OPEN_EXISTING = 3;
        private const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;

        /// <summary>
        ///     Reads the TargetPath stored in the a symlink
        /// </summary>
        /// <param name="path">The path of the symlink</param>
        /// <returns>The path stored in the symlink</returns>
        public static string GetRealPath(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new IOException("TargetPath not found");
            }

            DirectoryInfo symlink = new DirectoryInfo(path); // No matter if it's a file or folder
            SafeFileHandle directoryHandle = CreateFile(symlink.FullName, 0, 2, IntPtr.Zero,
                CREATION_DISPOSITION_OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero); //Handle file / folder
            if (directoryHandle.IsInvalid)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            StringBuilder result = new StringBuilder(512);
            int mResult = GetFinalPathNameByHandle(directoryHandle.DangerousGetHandle(), result, result.Capacity, 0);
            if (mResult < 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            if (result.Length >= 4 && result[0] == '\\' && result[1] == '\\' && result[2] == '?' && result[3] == '\\')
            {
                return result.ToString().Substring(4); // "\\?\" remove
            }

            return result.ToString();
        }

        #endregion

        #region From https://stackoverflow.com/a/3774508/6730162 access on 02.10.2017

        /// <summary>
        ///     Checks whether a user is Part of a localgroup
        /// </summary>
        /// <param name="username">The Name of the User to search for</param>
        /// <param name="localGroup">The localgroup to search in</param>
        /// <returns>Whether the user is in the logalgroup</returns>
        public static bool IsUserInLocalGroup(string username, string localGroup)
        {
            GroupPrincipal oGroupPrincipal = GetGroup(localGroup);
            PrincipalSearchResult<Principal> oPrincipalSearchResult = oGroupPrincipal.GetMembers();
            return oPrincipalSearchResult.Any(principal => principal.Name == username);
        }

        private static GroupPrincipal GetGroup(string sGroupName)
        {
            PrincipalContext oPrincipalContext = GetPrincipalContext();
            GroupPrincipal oGroupPrincipal = GroupPrincipal.FindByIdentity(oPrincipalContext, sGroupName);
            return oGroupPrincipal;
        }

        private static PrincipalContext GetPrincipalContext()
        {
            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Machine);
            return oPrincipalContext;
        }

        #endregion

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> src)
        {
            return src.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }

        public static string AsString(this IEnumerable<char> src)
        {
            StringBuilder builder= new StringBuilder();
            foreach (char c in src)
            {
                builder.Append(c);
            }
            return builder.ToString();
        }

        public static string ToWin32Format(this DateTime toConvert)
        { 
            return $"{toConvert.Year:0000}-{toConvert.Month:00}-{toConvert.Day:00}T{toConvert.Hour:00}:{toConvert.Minute:00}:{toConvert.Second:00}.{toConvert.Millisecond:000}0000";
        }
    }
}