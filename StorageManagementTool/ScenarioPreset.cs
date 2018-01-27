using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.Devices;
using static StorageManagementTool.GlobalizationRessources.ApplyPresetStrings;

namespace StorageManagementTool
{
    public class ScenarioPreset
    {
        public static List<ScenarioPreset> AvailablePresets;
        public bool HDDRequired;
        public string Name;
        public Action<DriveInfo, DriveInfo> toRun;


        private static void LocalSSDAndHDD(DriveInfo SSD, DriveInfo HDD)
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
                if (Directory.Exists(Path.Combine(HDD.RootDirectory.FullName, $"SSD {i}")))
                {
                    empty = true;
                }

                i++;
            } while (!empty);

            DirectoryInfo baseDir = HDD.RootDirectory.CreateSubdirectory($"SSD {i}");
            Session.Singleton.CfgJson.DefaultHDDPath = baseDir.FullName;
            Session.Singleton.SaveCfg();
            DirectoryInfo userDir = baseDir.CreateSubdirectory(Environment.UserName);
            foreach (KeyValuePair<string, string> currentPair in usfToMove)
            {
                UserShellFolder moving = UserShellFolder.GetUSFById(currentPair.Key);
                OperatingMethods.ChangeUserShellFolder(moving.GetPath(), userDir.CreateSubdirectory(currentPair.Value), moving,
                    OperatingMethods.QuestionAnswer.Yes, OperatingMethods.QuestionAnswer.Yes);
            }

            int memory = (int) (new ComputerInfo().TotalPhysicalMemory / 1048576L);
            OperatingMethods.ChangePagefileSettings(HDD, memory, memory * 2);
        }

        public static void LoadPresets()
        {
            AvailablePresets.Add(new ScenarioPreset {HDDRequired = true, Name = Presets_LocalsHDDAndSSD, toRun = LocalSSDAndHDD});
        }
    }
}