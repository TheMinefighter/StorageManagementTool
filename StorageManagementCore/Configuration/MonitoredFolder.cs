using System;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Contains information about a folder, which should be monitored
	/// </summary>
	public class MonitoredFolder : IEquatable<MonitoredFolder> {
		/// <summary>
		///  The MonitoringAction to execute for new files
		/// </summary>
		public MonitoringAction ForFiles;

		/// <summary>
		///  The MonitoringAction to execute for new folders
		/// </summary>
		public MonitoringAction ForFolders;

		/// <summary>
		///  The target path of this MonitoredFolder
		/// </summary>
		public string TargetPath;

		public MonitoredFolder() { }

		/// <summary>
		///  Creates a new MonitoredFolder object with a given target targetPath
		/// </summary>
		/// <param name="targetPath">The target targetPath</param>
		public MonitoredFolder(string targetPath) {
			TargetPath = targetPath;
			ForFiles = MonitoringAction.Ask;
			ForFolders = MonitoringAction.Ask;
		}

		public bool Equals(MonitoredFolder other) {
			if (ReferenceEquals(null, other)) {
				return false;
			}

			if (ReferenceEquals(this, other)) {
				return true;
			}

			return ForFiles == other.ForFiles && ForFolders == other.ForFolders && string.Equals(TargetPath, other.TargetPath);
		}


		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}