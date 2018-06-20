using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace StorageManagementCore {
	/// <summary>
	///  Stores session data
	/// </summary>
	public class Session {
		/// <summary>
		///  Reference to the Session Object
		/// </summary>
		public static Session Singleton;

		private readonly string ConfigurationFolder;

		/// <summary>
		///  The current JSON configuration
		/// </summary>
		public MainConfiguration Configuration;

		/// <summary>
		///  The path of the configuration file
		/// </summary>
		public string ConfigurationPath;

		/// <summary>
		///  Whether the program runs as administrator
		/// </summary>
		public bool IsAdmin;

		private bool UnpriviligedSymlinksAvailable;

		/// <summary>
		///  Creates a new Session
		/// </summary>
		public Session() {
			Singleton = this;
			UnpriviligedSymlinksAvailable =
				RegistryMethods.GetRegistryValue(
					new RegistryValue(
						"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\AppModelUnlock",
						"AllowDevelopmentWithoutDevLicense"), out object val) &&
				int.TryParse((string) val, out int isDev) &&
				isDev!=0&&
				RegistryMethods.GetRegistryValue(
					new RegistryValue(
						"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
						"ReleaseId"),
					out object release) &&
				int.TryParse((string) release, out int releaseId) &&
				releaseId >= 1703;

			ConfigurationFolder = Path.Combine(Environment.GetFolderPath(
				Environment.SpecialFolder.ApplicationData), "StorageManagementTool");
			ConfigurationPath = Path.Combine(ConfigurationFolder,
				"MainConfiguration.json");
			if (File.Exists(ConfigurationPath)) {
				Configuration = JsonConvert.DeserializeObject<MainConfiguration>(File.ReadAllText(ConfigurationPath));
			}
			else {
				Configuration = MainConfiguration.DefaultSettings();
				if (!Directory.Exists(ConfigurationFolder)) {
					Directory.CreateDirectory(ConfigurationFolder);
				}

				Singleton.SaveCfg();
			}

			CultureInfo requestedCulture =
				CultureInfo.GetCultureInfo(Configuration.LanguageOverride ?? CultureInfo.CurrentUICulture.Name);
			CultureInfo toUseCultureInfo = BestPossibleCulture(requestedCulture, Program.AvailableSpecificCultures);
			Thread.CurrentThread.CurrentUICulture = toUseCultureInfo;
			ScenarioPreset.LoadPresets();
			UserShellFolder.LoadEditable();
			IsAdmin = Wrapper.IsCurrentUserAdministrator();
		}

		private static CultureInfo BestPossibleCulture(CultureInfo requestedCulture,
			CultureInfo[][] availableSpecificCultures) {
			CultureInfo requestedParent = requestedCulture.Parent;
			CultureInfo toUseCultureInfo = null;
			foreach (CultureInfo[] baseCulture in availableSpecificCultures) {
				if (baseCulture[0].Parent.Equals(requestedParent)) {
					foreach (CultureInfo cultureInfo in baseCulture) {
						if (cultureInfo.Equals(requestedCulture)) {
							toUseCultureInfo = cultureInfo;
						}
					}

					toUseCultureInfo = toUseCultureInfo ?? baseCulture[0];
					break;
				}
			}

			toUseCultureInfo = toUseCultureInfo ?? availableSpecificCultures[0][0];
			return toUseCultureInfo;
		}

		/// <summary>
		///  Stores the configuration in a JSON file
		/// </summary>
		public void SaveCfg() {
			File.WriteAllText(
				ConfigurationPath, JsonConvert.SerializeObject(Configuration));
		}
	}
}