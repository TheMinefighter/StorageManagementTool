using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Container for all settings for SSD Monitoring
	/// </summary>
	public class MonitoringConfiguration {
		/// <summary>
		///  The MonitoredFolders configured in this MonitoringSetting
		/// </summary>
		public List<MonitoredFolder> MonitoredFolders;

		/// <summary>
		///  Creates a new MonitoringSetting
		/// </summary>
		public MonitoringConfiguration() => MonitoredFolders = new List<MonitoredFolder>();


		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}