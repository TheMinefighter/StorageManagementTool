using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace StorageManagementCore
{
	/// <summary>
	///  Stores session data
	/// </summary>
	public class Session
	{
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

		/// <summary>
		///  Creates a new Session
		/// </summary>
		public Session()
		{
			Singleton = this;
			ConfigurationFolder = Path.Combine(Environment.GetFolderPath(
				Environment.SpecialFolder.ApplicationData), "StorageManagementTool");
			ConfigurationPath = Path.Combine(ConfigurationFolder,
				"MainConfiguration.json");
			if (File.Exists(ConfigurationPath))
			{
				Configuration = JsonConvert.DeserializeObject<MainConfiguration>(File.ReadAllText(ConfigurationPath));
			}
			else
			{
				Configuration = MainConfiguration.DefaultSettings();
				if (!Directory.Exists(ConfigurationFolder))
				{
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
			CultureInfo[][] availableSpecificCultures)
		{
			CultureInfo requestedParent = requestedCulture.Parent;
			CultureInfo toUseCultureInfo = null;
			for (int i = 0; i < availableSpecificCultures.Length; i++)
			{
				if (availableSpecificCultures[i][0].Parent.Equals(requestedParent))
				{
					foreach (CultureInfo cultureInfo in availableSpecificCultures[i])
					{
						if (cultureInfo.Equals(requestedCulture))
						{
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
		///  Stores the configuration in a JSON file
		/// </summary>
		public void SaveCfg()
		{
			File.WriteAllText(
				ConfigurationPath, JsonConvert.SerializeObject(Configuration));
		}
	}
}