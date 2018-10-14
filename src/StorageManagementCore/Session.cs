using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Win32;
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

		public static EventHandler LanguageChanged = (a, b) => { };
		/// <summary>
		///  The current JSON configuration
		/// </summary>
		public MainConfiguration Configuration;

		/// <summary>
		///  The path of the configuration file
		/// </summary>
		public string ConfigurationPath;

		public bool Compat;
		

		/// <summary>
		///  Whether the program runs as administrator
		/// </summary>
		public bool IsAdmin;

		/// <summary>
		///  Whether Unpriviliged Symlinks available
		/// </summary>
		public bool UnpriviligedSymlinksAvailable;

		/// <summary>
		///  Creates a new Session
		/// </summary>
		public Session() {
			Singleton = this;
			//Potential to be changed in future
			Compat = false;
			string configurationFolder= null;
			try {
				configurationFolder = Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.ApplicationData), "StorageManagementTool");
				ConfigurationPath = Path.Combine(configurationFolder,
					"MainConfiguration.json");
			}
			catch (Exception ) {
				Compat = true;
				Configuration = MainConfiguration.Default;
			}

			if (!Compat) {
				if (File.Exists(ConfigurationPath)) {
					Configuration = JsonConvert.DeserializeObject<MainConfiguration>(File.ReadAllText(ConfigurationPath));
				}
				else {
					Configuration = MainConfiguration.Default;
					if (!Directory.Exists(configurationFolder)) {
						Directory.CreateDirectory(configurationFolder);
					}

					Singleton.SaveCfg();
				}
			}

			CultureInfo requestedCulture =
				CultureInfo.GetCultureInfo(Configuration.LanguageOverride ?? CultureInfo.CurrentUICulture.Name);
			CultureInfo toUseCultureInfo = BestPossibleCulture(requestedCulture, Program.AvailableSpecificCultures);
			Thread.CurrentThread.CurrentUICulture = toUseCultureInfo;
			ScenarioPreset.LoadPresets();
			IsAdmin = Wrapper.IsCurrentUserAdministrator();
			UnpriviligedSymlinksAvailable =
				!Configuration.DisableUnprivilgedLinkCreation &&
				RegistryMethods.GetRegistryValue(
					new RegistryValue(RegistryHive.LocalMachine,
						"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\AppModelUnlock",
						"AllowDevelopmentWithoutDevLicense"), out object isDevobject) &&
				isDevobject is string isDevString &&
				uint.TryParse(isDevString, out uint isDev) &&
				isDev != 0 && Environment.OSVersion.Version.Major > 10 ||
				Environment.OSVersion.Version.Major == 10 &&
				RegistryMethods.GetRegistryValue(
					new RegistryValue(RegistryHive.LocalMachine,
						"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
						"ReleaseId"),
					out object release) &&
				release is string releaseString &&
				uint.TryParse(releaseString, out uint releaseId) &&
				releaseId >= 1703;
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
			if (!Compat) {
				File.WriteAllText(
					ConfigurationPath, JsonConvert.SerializeObject(Configuration));
			}
		}
	}
}