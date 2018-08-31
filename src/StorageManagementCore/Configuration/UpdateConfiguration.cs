namespace StorageManagementCore.Configuration {
	public class UpdateConfiguration {
		public bool UsePreReleases;
		public UpdateMode Mode;
		public UpdateConfiguration(bool usePreReleases, UpdateMode mode) {
			UsePreReleases = usePreReleases;
			Mode = mode;
		}

		public UpdateConfiguration() {
			
		}
		public static UpdateConfiguration Default => new UpdateConfiguration {
			UsePreReleases = true,
			Mode = UpdateMode.DownloadOnStartupInstallManual
		};
	}
}