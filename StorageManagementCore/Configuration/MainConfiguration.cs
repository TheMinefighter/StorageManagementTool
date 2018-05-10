using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Represents the JSON serializable configuration data of program
	/// </summary>
	public class MainConfiguration {
		/// <summary>
		/// </summary>
		public AllPagefilesConfiguration AllPagefilesSettings;

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
		public MonitoringConfiguration MonitoringSettings;

		/// <summary>
		/// </summary>
		public List<ShellFolderConfiguration> ShellFolderSettings;

		/// <summary>
		///  The Version of the Configuration
		/// </summary>
		public string Version;


		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}