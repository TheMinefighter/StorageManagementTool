using System;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Represents the JSON serializable configuration data of program
	/// </summary>
	public class MainConfiguration {
		/// <summary>
		///  The Version of the Configuration
		/// </summary>
		public string Version;

		/// <summary>
		///  The default path to move data to
		/// </summary>
		public string DefaultHDDPath;

		/// <summary>
		///  Overrides the UI language if not null
		/// </summary>
		public string LanguageOverride;

		/// <summary>
		///  The configured SSD MonitoringSetting
		/// </summary>
		public MonitoringSetting MonitoringSettings;

		public PagefilesSetting PagefilesSettings;



		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}