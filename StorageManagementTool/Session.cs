using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace StorageManagementTool
{
    /// <summary>
    ///     Stores session data
    /// </summary>
    public class Session
    {
        /// <summary>
        ///     Reference to the Session Object
        /// </summary>
        public static Session Singleton;

        private MainWindow _mainForm;

        /// <summary>
        ///     The current JSON configuration
        /// </summary>
        public JSONConfig CfgJson;

        /// <summary>
        ///     The path of the configuration file
        /// </summary>
        public string CfgPath;

        /// <summary>
        ///     The List of drives currently available
        /// </summary>
        public List<DriveInfo> CurrentDrives;

        public UIStrings.UILanguage CurrentLanguage;

        /// <summary>
        ///     Whether the program runs as administrator
        /// </summary>
        public bool Isadmin;

        /// <summary>
        ///     The stadium of the swapfile
        /// </summary>
        public int Swapstadium;

        /// <summary>
        ///     Creates a new Session
        /// </summary>
        public Session()
        {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "de":
                    CurrentLanguage = UIStrings.UILanguage.German;
                    break;
                default:
                    CurrentLanguage = UIStrings.UILanguage.English;
                    break;
            }

            Singleton = this;
            ScenarioPreset.LoadPresets();
            UserShellFolder.LoadEditable();
        }

        /// <summary>
        ///     Refreshes the current Stadium of the Swapfile Movement
        /// </summary>
        public void RefreshSwapfileStadium()
        {
            if (Wrapper.IsPathSymbolic(@"C:\swapfile.sys"))
            {
                Wrapper.GetRegistryValue(new RegPath(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SwapFileControl"), out object regValue);
                Swapstadium =
                    (int?) regValue == null || (int?) regValue == 1
                        ? 4
                        : 3;
            }
            else
            {
                Wrapper.GetRegistryValue(new RegPath(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                    "SwapFileControl"), out object regValue);
                Swapstadium =
                    (int?) regValue == null || (int?) regValue == 1
                        ? 1
                        : 2;
            }
        }

        /// <summary>
        ///     Fills an given Listbox with information about the available Drives
        /// </summary>
        /// <param name="toFill"></param>
        public void FillWithDriveInfo(ListBox toFill)
        {
            toFill.Items.Clear();
            foreach (DriveInfo item in CurrentDrives)
            {
                toFill.Items.Add(OperatingMethods.DriveInfo2String(item));
            }
        }

        /// <summary>
        ///     Stores the configuration in a JSON file
        /// </summary>
        public void SaveCfg()
        {
            File.WriteAllText(
                CfgPath, JsonConvert.SerializeObject(CfgJson));
        }

        /// <summary>
        ///     Creates an IEnumerable with all current DriveInfos
        /// </summary>
        /// <returns>All DriveInfos</returns>
        public IEnumerable<string> FillWithDriveInfo()
        {
            return CurrentDrives.Select(OperatingMethods.DriveInfo2String).ToList();
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