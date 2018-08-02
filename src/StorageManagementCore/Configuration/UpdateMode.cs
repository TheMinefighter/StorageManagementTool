namespace StorageManagementCore.Configuration {
	public enum UpdateMode : byte {
		NoUpdates,
		DownloadAndInstallOnStartup,
		DownloadOnStartupInstallNext,
		DownloadOnStartupInstallManual
	}
}