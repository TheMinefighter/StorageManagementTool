using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration
{
	/// <summary>
	///  Represents the JSON serializable configuration data of program
	/// </summary>
	public class JSONConfig :IEquatable<JSONConfig>
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

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}

		/// <summary>
		///  Container for all settings for SSD Monitoring
		/// </summary>
		public class MonitoringSetting :IEquatable<MonitoringSetting>
		{


			/// <summary>
			///  The MonitoredFolders configured in this MonitoringSetting
			/// </summary>
			public List<MonitoredFolder> MonitoredFolders;

			/// <summary>
			///  Creates a new MonitoringSetting
			/// </summary>
			public MonitoringSetting()
			{
				MonitoredFolders = new List<MonitoredFolder>();
			}


			public override string ToString() => JsonConvert.SerializeObject(this);



			public bool Equals(MonitoringSetting other) {
				if (ReferenceEquals(null, other)) {
					return false;
				}

				if (ReferenceEquals(this, other)) {
					return true;
				}

				return Equals(MonitoredFolders, other.MonitoredFolders);
			}

			public override bool Equals(object obj) {
				if (ReferenceEquals(null, obj)) {
					return false;
				}

				if (ReferenceEquals(this, obj)) {
					return true;
				}

				if (obj.GetType() != this.GetType()) {
					return false;
				}

				return Equals((MonitoringSetting) obj);
			}

		}

		public bool Equals(JSONConfig other) {
			if (ReferenceEquals(null, other)) {
				return false;
			}

			if (ReferenceEquals(this, other)) {
				return true;
			}

			return string.Equals(Version, other.Version) && string.Equals(DefaultHDDPath, other.DefaultHDDPath) && string.Equals(LanguageOverride, other.LanguageOverride) && Equals(MonitoringSettings, other.MonitoringSettings);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}

			if (ReferenceEquals(this, obj)) {
				return true;
			}

			if (obj.GetType() != this.GetType()) {
				return false;
			}

			return Equals((JSONConfig) obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = (Version != null ? Version.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (DefaultHDDPath != null ? DefaultHDDPath.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (LanguageOverride != null ? LanguageOverride.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (MonitoringSettings != null ? MonitoringSettings.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}