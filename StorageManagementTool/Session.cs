﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace StorageManagementTool
{
   /// <summary>
   ///    Stores session data
   /// </summary>
   public class Session
   {
      /// <summary>
      ///    Reference to the Session Object
      /// </summary>
      public static Session Singleton;

      /// <summary>
      ///    The path of the configuration file
      /// </summary>
      public string ConfigurationPath;

      /// <summary>
      ///    The current JSON configuration
      /// </summary>
      public JSONConfig CurrentConfiguration;

      /// <summary>
      ///    The List of drives currently available
      /// </summary>
      public List<DriveInfo> CurrentDrives;

      /// <summary>
      ///    Whether the program runs as administrator
      /// </summary>
      public bool IsAdmin;

      /// <summary>
      ///    The stadium of the swapfile
      /// </summary>
      public int Swapstadium;

      /// <summary>
      ///    Creates a new Session
      /// </summary>
      public Session()
      {
         Singleton = this;
         ConfigurationPath = Path.Combine(Environment.GetFolderPath(
               Environment.SpecialFolder.MyDocuments),
            "StorageManagementToolConfiguration.json");
         CurrentConfiguration = File.Exists(ConfigurationPath)
            ? JsonConvert.DeserializeObject<JSONConfig>(File.ReadAllText(ConfigurationPath))
            : new JSONConfig();
         Thread.CurrentThread.CurrentUICulture =
            CultureInfo.GetCultureInfo(CurrentConfiguration.LanguageOverride ?? CultureInfo.CurrentUICulture.Name);
         ScenarioPreset.LoadPresets();
         UserShellFolder.LoadEditable();
      }

      /// <summary>
      ///    Refreshes the current Stadium of the Swapfile Movement
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
      ///    Fills an given Listbox with information about the available Drives
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
      ///    Stores the configuration in a JSON file
      /// </summary>
      public void SaveCfg()
      {
         File.WriteAllText(
            ConfigurationPath, JsonConvert.SerializeObject(CurrentConfiguration));
      }

      /// <summary>
      ///    Creates an IEnumerable with all current DriveInfos
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
         IsAdmin = Wrapper.IsUserAdministrator();
         RefreshSwapfileStadium();
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new MainWindow());
      }
   }
}