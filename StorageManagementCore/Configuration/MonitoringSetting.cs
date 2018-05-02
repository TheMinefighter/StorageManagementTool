using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Container for all settings for SSD Monitoring
	/// </summary>
	public class MonitoringSetting : IEquatable<MonitoringSetting> {
		/// <summary>
		///  The MonitoredFolders configured in this MonitoringSetting
		/// </summary>
		public List<MonitoredFolder> MonitoredFolders;

		/// <summary>
		///  Creates a new MonitoringSetting
		/// </summary>
		public MonitoringSetting() => MonitoredFolders = new List<MonitoredFolder>();


		public bool Equals(MonitoringSetting other) {
			if (ReferenceEquals(null, other)) {
				return false;
			}

			if (ReferenceEquals(this, other)) {
				return true;
			}

			return Equals(MonitoredFolders, other.MonitoredFolders);
		}


		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}