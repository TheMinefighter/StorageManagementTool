using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace StorageManagementTool
{
    /// <summary>
    /// Stores session data
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Reference to the Session Object
        /// </summary>
        public static Session Singleton;

        /// <summary>
        /// The List of drives currently available
        /// </summary>
        public List<DriveInfo> CurrentDrives;

        /// <summary>
        /// The current JSON configuration
        /// </summary>
        public JSONConfig CfgJson;

        private MainWindow _mainForm;

        /// <summary>
        /// Whether the program runs as administrator
        /// </summary>
        public bool Isadmin;

        /// <summary>
        /// The path of the configuration file
        /// </summary>
        public string CfgPath;

        /// <summary>
        /// The stadium of the swapfile
        /// </summary>
        public int Swapstadium;

        /// <summary>
        /// Creates a new Session
        /// </summary>
        public Session()
        {
            Singleton = this;
        }

        /// <summary>
        /// Refreshes the current Stadium of the Swapfile Movement
        /// </summary>
        public void RefreshSwapfileStadium()
        {
            if (Wrapper.IsPathSymbolic(@"C:\swapfile.sys"))
            {
                Wrapper.GetRegValue(new RegPath(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SwapFileControl"), out object regValue);
                Swapstadium =
                    (int?) regValue == null || (int?) regValue == 1
                        ? 4
                        : 3;
            }
            else
            {
                Wrapper.GetRegValue(new RegPath(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SwapFileControl"), out object regValue);
                Swapstadium =
                    (int?) regValue == null || (int?) regValue == 1
                        ? 1
                        : 2;
            }
        }

        /// <summary>
        /// Fills an given Listbox with information about the available Drives
        /// </summary>
        /// <param name="toFill"></param>
        public void FillWithDriveInfo(ListBox toFill)
        {
            toFill.Items.Clear();
            foreach (DriveInfo item in CurrentDrives)
            {
                toFill.Items.Add(OperatingMethods.DriveInfoAsString(item));
            }
        }

        /// <summary>
        /// Stores the configuration in a JSON file
        /// </summary>
        public void SaveCfg()
        {
            File.WriteAllText(
                CfgPath, JsonConvert.SerializeObject(CfgJson));
        }

        /// <summary>
        /// Creates an IEnumerable with all current DriveInfos
        /// </summary>
        /// <returns>All DriveInfos</returns>
        public IEnumerable<string> FillWithDriveInfo()
        {
            return CurrentDrives.Select(OperatingMethods.DriveInfoAsString).ToList();
        }

        public void RefreshDriveInformation()
        {
            CurrentDrives = FileSystem.Drives.ToList();
        }

        public void StandardLaunch()
        {
            RefreshDriveInformation();
            Isadmin = Wrapper.IsUserAdministrator();
            RefreshSwapfileStadium();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _mainForm = new MainWindow();
            Application.Run(_mainForm);
        }
    }
}