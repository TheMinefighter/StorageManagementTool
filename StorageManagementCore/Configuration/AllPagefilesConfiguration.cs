using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	public class AllPagefilesConfiguration {
		public bool EnableHibfilSys;
		[NotNull]
		public PagefileSysConfiguration PagefileSysSettings;
		[NotNull]
		public SwapfileSysConfiguration SwapfileSysSettings;
	}
}