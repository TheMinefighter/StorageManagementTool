using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <inheritdoc />
	/// <summary>
	///  Container for all settings for SSD Monitoring
	/// </summary>
	public class MonitoringConfiguration: ICloneable {
		/// <summary>
		///  The MonitoredFolders configured in this MonitoringSetting
		/// </summary>
		public List<MonitoredFolder> MonitoredFolders;

		/// <summary>
		///  Creates a new MonitoringSetting
		/// </summary>
		public MonitoringConfiguration() => MonitoredFolders = new List<MonitoredFolder>();


		public override string ToString() => JsonConvert.SerializeObject(this);
		public object Clone()
		{
			return new MonitoringConfiguration(){MonitoredFolders = MonitoredFolders.Select(x=>x.Clone()).Cast<MonitoredFolder>().ToList()};
		}
	}
}