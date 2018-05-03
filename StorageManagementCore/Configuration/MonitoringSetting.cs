using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Container for all settings for SSD Monitoring
	/// </summary>
	public class MonitoringSetting {
		/// <summary>
		///  The MonitoredFolders configured in this MonitoringSetting
		/// </summary>
		public List<MonitoredFolder> MonitoredFolders;

		/// <summary>
		///  Creates a new MonitoringSetting
		/// </summary>
		public MonitoringSetting() => MonitoredFolders = new List<MonitoredFolder>();


		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}