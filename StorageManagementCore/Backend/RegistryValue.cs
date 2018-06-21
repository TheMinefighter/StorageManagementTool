using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace StorageManagementCore.Backend {
	/// <summary>
	///  Class for storing a windows registry path
	/// </summary>
	public struct RegistryValue {
		public string SubKey;
		public RegistryHive hive;
		/// <summary>
		///  The where the value is stored
		/// </summary>
		public string RegistryKey  =>RegistryMethods.RegistryRootKeys[hive]+'\\'+ SubKey;

		/// <summary>
		///  The name of the value
		/// </summary>
		public string ValueName;

		public RegistryValue(string registryKey, string valueName) {
			int indexOf = registryKey.IndexOf('\\');
			hive = RegistryMethods.RegistryRootKeys[ registryKey.Substring(0,indexOf)];
			SubKey = registryKey.Substring(indexOf + 1);
			//RegistryKey = registryKey;
			ValueName = valueName;
		}

		public static implicit operator RegistryValue((string, string) s) => new RegistryValue(s.Item1, s.Item2);

		public static implicit operator (string, string)(RegistryValue s) => (s.RegistryKey, s.ValueName);

		public override string ToString() => RegistryKey + '\\' + ValueName;
	}
}