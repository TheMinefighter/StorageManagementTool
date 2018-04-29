using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace StorageManagementTool {
   /// <summary>
   ///  Stores session data
   /// </summary>
   public class Session {
      /// <summary>
      ///  Reference to the Session Object
      /// </summary>
      public static Session Singleton;

      /// <summary>
      ///  The path of the configuration file
      /// </summary>
      public string ConfigurationPath;

      /// <summary>
      ///  The current JSON configuration
      /// </summary>
      public JSONConfig CurrentConfiguration;

      /// <summary>
      ///  Whether the program runs as administrator
      /// </summary>
      public bool IsAdmin;

      /// <summary>
      ///  Creates a new Session
      /// </summary>
      public Session() {
			IEnumerable<IEnumerable<CultureInfo>> availableSpecificCultures = new[]
				{new[] {CultureInfo.CreateSpecificCulture("en-US")}, new[] {CultureInfo.CreateSpecificCulture("de-DE")}};
			Singleton = this;
			ConfigurationPath = Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDocuments),
				"StorageManagementToolConfiguration.json");
			CurrentConfiguration = File.Exists(ConfigurationPath)
				? JsonConvert.DeserializeObject<JSONConfig>(File.ReadAllText(ConfigurationPath))
				: new JSONConfig();
			CultureInfo requestedCulture =
				CultureInfo.GetCultureInfo(CurrentConfiguration.LanguageOverride ?? CultureInfo.CurrentUICulture.Name);
			CultureInfo toUseCultureInfo = BestPossibleCulture(requestedCulture, availableSpecificCultures);
			Thread.CurrentThread.CurrentUICulture = toUseCultureInfo;
			ScenarioPreset.LoadPresets();
			UserShellFolder.LoadEditable();
			IsAdmin = Wrapper.IsCurrentUserAdministrator();
		}

		private static CultureInfo BestPossibleCulture(CultureInfo requestedCulture,
			IEnumerable<IEnumerable<CultureInfo>> availableSpecificCultures) {
			CultureInfo requestedParent = requestedCulture.Parent;
			CultureInfo toUseCultureInfo = null;
			for (int i = 0; i < availableSpecificCultures.Count(); i++) {
				if (availableSpecificCultures.ElementAt(i).ElementAt(0).Parent.Equals(requestedParent)) {
					foreach (CultureInfo cultureInfo in availableSpecificCultures.ElementAt(i)) {
						if (cultureInfo.Equals(requestedCulture)) {
							toUseCultureInfo = cultureInfo;
						}
					}

					toUseCultureInfo = toUseCultureInfo ?? availableSpecificCultures.ElementAt(i).ElementAt(0);
					break;
				}
			}

			toUseCultureInfo = toUseCultureInfo ?? availableSpecificCultures.ElementAt(0).ElementAt(0);
			return toUseCultureInfo;
		}

      /// <summary>
      ///  Refreshes the current Stadium of the Swapfile Movement
      /// </summary>
      /// <summary>
      ///  Fills an given Listbox with information about the available Drives
      /// </summary>
      /// <param name="toFill"></param>
      public void FillWithDriveInfo(ListBox toFill) {
			toFill.Items.Clear();
			foreach (DriveInfo item in Wrapper.getDrives()) {
				toFill.Items.Add(OperatingMethods.GetDriveInfoDescription(item));
			}
		}

      /// <summary>
      ///  Stores the configuration in a JSON file
      /// </summary>
      public void SaveCfg() {
			File.WriteAllText(
				ConfigurationPath, JsonConvert.SerializeObject(CurrentConfiguration));
		}
	}
}