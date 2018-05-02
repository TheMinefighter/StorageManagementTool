using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration
{
	/// <summary>
	///  Represents the JSON serializable configuration data of program
	/// </summary>
	public class MainConfiguration :IEquatable<MainConfiguration>
	{
		/// <summary>
      /// The Version of the Configuration
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
		public 

		public override string ToString() => JsonConvert.SerializeObject(this);


		public bool Equals(MainConfiguration other) {
			if (ReferenceEquals(null, other)) {
				return false;
			}

			if (ReferenceEquals(this, other)) {
				return true;
			}

			return string.Equals(Version, other.Version) && string.Equals(DefaultHDDPath, other.DefaultHDDPath) && string.Equals(LanguageOverride, other.LanguageOverride) && Equals(MonitoringSettings, other.MonitoringSettings);
		}
	}
}