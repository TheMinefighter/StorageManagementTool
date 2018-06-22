using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Win32;

namespace StorageManagementCore.Backend {
	/// <summary>
	///  Class for storing a windows registry path
	/// </summary>
	public struct RegistryValue {
		public string SubKey;
		public RegistryHive Hive;
		/// <summary>
		///  The where the value is stored
		/// </summary>
		public string RegistryKeyName  =>RegistryMethods.RegistryRootKeys[Hive]+'\\'+ SubKey;

		/// <summary>
		///  The name of the value
		/// </summary>
		public string ValueName;

		
		public RegistryValue(string registryKey, string valueName) {
			int indexOf = registryKey.IndexOf('\\');
			Hive = RegistryMethods.RegistryRootKeys[ registryKey.Substring(0,indexOf)];
			SubKey = registryKey.Substring(indexOf + 1);
			//RegistryKey = registryKey;
			ValueName = valueName;
		}

		public RegistryValue(RegistryHive hive, string subKey, string valueName) {
			Hive = hive;
			SubKey = subKey;
			ValueName = valueName;
		}

		public override string ToString() => RegistryKeyName + '\\' + ValueName;
	}
}