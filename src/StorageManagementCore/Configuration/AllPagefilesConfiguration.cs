using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	public class AllPagefilesConfiguration  {
		public bool EnableHibfilSys;

		[NotNull]
		public PagefileSysConfiguration PagefileSysSettings;

		[NotNull]
		public SwapfileSysConfiguration SwapfileSysSettings;

		public AllPagefilesConfiguration() {
			SwapfileSysSettings= new SwapfileSysConfiguration();
			PagefileSysSettings= new PagefileSysConfiguration();
		}
	}
}