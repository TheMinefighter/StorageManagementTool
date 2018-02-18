using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.Devices;
using static StorageManagementTool.GlobalizationRessources.ScenarioPresetStrings;

namespace StorageManagementTool
{/// <summary>
 /// Stores the presets for the scenarios
 /// </summary>
   public struct ScenarioPreset
   {
      public static ScenarioPreset[] AvailablePresets;
      public override string ToString()
      {
         return Id;
      }

      /// <summary>
      ///    Whether a HDD is required for this preset
      /// </summary>
      public bool HDDRequired;

      public string Id;

      /// <summary>
      ///    The name of the preset
      /// </summary>
      public string ViewedName;

      /// <summary>
      ///    Whether a SSD is required for this preset
      /// </summary>
      public bool SSDRequired;

      /// <summary>
      ///    the action to run
      /// </summary>
      public Action<DriveInfo, DriveInfo> ToRun;

      private static void LocalSSDAndNAS(DriveInfo ssd, DriveInfo hdd)
      {
         Dictionary<string, string> usfToMove = new Dictionary<string, string>
         {
            {"Personal", "Documents"},
            {"My Music", "Music"},
            {"My Pictures", "Pictures"},
            {"AppData", "AppData\\Roaming"},
            {"{374DE290-123F-4565-9164-39C4925E467B}", "Downloads"},
            {"Desktop", "Desktop"}
         };
         bool empty = false;
         int i = 0;
         do
         {
            if (Directory.Exists(Path.Combine(hdd.RootDirectory.FullName, $"SSD{i}")))
            {
               empty = true;
            }

            i++;
         } while (!empty);

         DirectoryInfo baseDir = hdd.RootDirectory.CreateSubdirectory($"SSD{i}");

         Session.Singleton.CurrentConfiguration.DefaultHDDPath = baseDir.FullName;
         Session.Singleton.SaveCfg();
         DirectoryInfo userDir = baseDir.CreateSubdirectory(Environment.UserName);
         foreach (KeyValuePair<string, string> currentPair in usfToMove)
         {
            UserShellFolder moving = UserShellFolder.GetUserShellFolderById(currentPair.Key);
            OperatingMethods.ChangeUserShellFolder(moving.GetPath(), userDir.CreateSubdirectory(currentPair.Value),
               moving,
               OperatingMethods.QuestionAnswer.Yes, OperatingMethods.QuestionAnswer.Yes);
         }
         Dictionary<string, string> csfToMove = new Dictionary<string, string>
         {
            {"ProgramFilesDir (x86)", "Program Files (x86)"},
            {"ProgramFilesDir", "Program Files"},
            {"Common Desktop","Common Desktop" }
         };
         DirectoryInfo commonDir = baseDir.CreateSubdirectory("Common Data");
         foreach (KeyValuePair<string, string> currentPair in csfToMove)
         {
            UserShellFolder moving = UserShellFolder.GetUserShellFolderById(currentPair.Key);
            OperatingMethods.ChangeUserShellFolder(moving.GetPath(), commonDir.CreateSubdirectory(currentPair.Value),
               moving,
               OperatingMethods.QuestionAnswer.Yes, OperatingMethods.QuestionAnswer.Yes);
         }
         int memory = (int)(new ComputerInfo().TotalPhysicalMemory / 1048576L);
         OperatingMethods.ChangePagefileSettings(hdd, memory, memory * 2);
         OperatingMethods.EnableSendToHDD();
         OperatingMethods.SetHibernate(false);
         OperatingMethods.SetSearchDataPath(baseDir.CreateSubdirectory("WindowsSearchData"));
      }

      private static void LocalSSDAndHDD(DriveInfo ssd, DriveInfo hdd)
      {
         Dictionary<string, string> usfToMove = new Dictionary<string, string>
         {
            {"Personal", "Documents"},
            {"My Music", "Music"},
            {"My Pictures", "Pictures"},
            {"AppData", "AppData\\Roaming"},
            {"{374DE290-123F-4565-9164-39C4925E467B}", "Downloads"},
            {"Desktop", "Desktop"}
         };
         bool empty = false;
         int i = 0;
         do
         {
            if (Directory.Exists(Path.Combine(hdd.RootDirectory.FullName, $"SSD{i}")))
            {
               empty = true;
            }

            i++;
         } while (!empty);

         DirectoryInfo baseDir = hdd.RootDirectory.CreateSubdirectory($"SSD{i}");
         Session.Singleton.CurrentConfiguration.DefaultHDDPath = baseDir.FullName;
         Session.Singleton.SaveCfg();
         DirectoryInfo userDir = baseDir.CreateSubdirectory(Environment.UserName);
         foreach (KeyValuePair<string, string> currentPair in usfToMove)
         {
            UserShellFolder moving = UserShellFolder.GetUserShellFolderById(currentPair.Key);
            OperatingMethods.ChangeUserShellFolder(moving.GetPath(), userDir.CreateSubdirectory(currentPair.Value),
               moving,
               OperatingMethods.QuestionAnswer.Yes, OperatingMethods.QuestionAnswer.Yes);
         }

         int memory = (int) (new ComputerInfo().TotalPhysicalMemory / 1048576L);
         OperatingMethods.ChangePagefileSettings(hdd, memory, memory * 2);
         OperatingMethods.SetHibernate(false);
         OperatingMethods.EnableSendToHDD();
         OperatingMethods.SetSearchDataPath(baseDir.CreateSubdirectory("WindowsSearchData"));
      }

      /// <summary>
      ///    Loads all configured presets
      /// </summary>
      public static void LoadPresets()
      {

         AvailablePresets = new[]
         {
            new ScenarioPreset
            {
               HDDRequired = true,
               ViewedName = Presets_LocalHDDAndSSD,
               ToRun = LocalSSDAndHDD,
               Id = "LocalSSDAndHDD"

            }, new ScenarioPreset
            {
               HDDRequired = true,
               SSDRequired = true,
               ViewedName = Presets_LocalSSDAndNAS,
               ToRun = LocalSSDAndNAS,
               Id = "LocalSSDAndNAS"
            } 
         };
      }
   }
}