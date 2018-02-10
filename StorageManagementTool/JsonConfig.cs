using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace StorageManagementTool
{

   /// <summary>
   ///    Represents the JSON serializable configuration data of program
   /// </summary>
   public class JSONConfig
   {
      public override bool Equals(object obj)
      {
         if (obj is JSONConfig eq)
         {
           return eq.DefaultHDDPath == DefaultHDDPath && eq.LanguageOverride == LanguageOverride &&
               MonitoringSettings.Equals(eq.MonitoringSettings);
         }

         return false;
      }

      public override string ToString()
      {
         return JsonConvert.SerializeObject(this);
      }
      /// <summary>
      ///    The default path to move data to
      /// </summary>
      public string DefaultHDDPath;

      /// <summary>
      ///    Overrides the UI language if not null
      /// </summary>
      public string LanguageOverride;

      /// <summary>
      ///    The configured SSD MonitoringSetting
      /// </summary>
      public MonitoringSetting MonitoringSettings;

      /// <summary>
      ///    Container for all settings for SSD Monitoring
      /// </summary>
      public class MonitoringSetting
      {
         public override bool Equals(object obj)
         {
            if (obj is MonitoringSetting eq)
            {
               return MonitoredFolders.Count == eq.MonitoredFolders.Count && MonitoredFolders.SequenceEqual(eq.MonitoredFolders);
            }

            return false;
         }
         private class MonitoredFolderComparer:IEqualityComparer<MonitoredFolder>
         {
            public bool Equals(MonitoredFolder x, MonitoredFolder y)
            {
               throw new System.NotImplementedException();
            }

            public int GetHashCode(MonitoredFolder obj)
            {
               throw new System.NotImplementedException();
            }
         }
         public override string ToString()
         {
            return JsonConvert.SerializeObject(this);
         }
         /// <summary>
         ///    Represents all available actions for the event, when a new file or folder has been created
         /// </summary>
         public enum MonitoringAction
         {
            /// <summary>
            ///    Just does nothing
            /// </summary>
            Ignore,

            /// <summary>
            ///    Asks the user what to do
            /// </summary>
            Ask,

            /// <summary>
            ///    Moves the object to the configured location
            /// </summary>
            Move
         }

         /// <summary>
         ///    The MonitoredFolders configured in this MonitoringSetting
         /// </summary>
         public List<MonitoredFolder> MonitoredFolders;

         /// <summary>
         ///    Creates a new MonitoringSetting
         /// </summary>
         public MonitoringSetting()
         {
            MonitoredFolders = new List<MonitoredFolder>();
         }

         /// <summary>
         ///    Contains information about a folder, which should be monitored
         /// </summary>
         public class MonitoredFolder
         {
            public override bool Equals(object obj)
            {
               if (obj is MonitoredFolder eq)
               {
                  return TargetPath == eq.TargetPath && ForFiles == eq.ForFiles && ForFolders == eq.ForFolders;
               }
               return false;
            }

            public override string ToString()
            {
               return JsonConvert.SerializeObject(this);
            }

            /// <summary>
            ///    The MonitoringAction to execute for new files
            /// </summary>
            public MonitoringAction ForFiles;

            /// <summary>
            ///    The MonitoringAction to execute for new folders
            /// </summary>
            public MonitoringAction ForFolders;

            /// <summary>
            ///    The target path of this MonitoredFolder
            /// </summary>
            public string TargetPath;

            public MonitoredFolder()
            {
               
            }
            /// <summary>
            ///    Creates a new MonitoredFolder object with a given target targetPath
            /// </summary>
            /// <param name="targetPath">The target targetPath</param>
            public MonitoredFolder(string targetPath)
            {
               TargetPath = targetPath;
               ForFiles = MonitoringAction.Ask;
               ForFolders = MonitoringAction.Ask;
            }
         }
      }
   }
}