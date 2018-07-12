using System;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <inheritdoc />
	/// <summary>
	///  Container for all settings for SSD Monitoring
	/// </summary>
	public class MonitoringConfiguration : ICloneable {
		/// <summary>
		///  The SelectedMonitoredFolder configured in this MonitoringSetting
		/// </summary>
		public ObservableCollection<MonitoredFolder> MonitoredFolders;

		/// <summary>
		///  Creates a new MonitoringSetting
		/// </summary>
		public MonitoringConfiguration() => MonitoredFolders = new ObservableCollection<MonitoredFolder>();

		public object Clone() {
			return new MonitoringConfiguration {
				MonitoredFolders =
					new ObservableCollection<MonitoredFolder>(MonitoredFolders.Select(x => x.Clone()).Cast<MonitoredFolder>())
			};
		}


		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}