namespace StorageManagementCore.Configuration {
	public class SwapfileConfiguration {
		public enum SwapfileState {
			Untouched,
			Disabled,
			Moved
		}

		public SwapfileState SwapfileConfigured;
		public ConfiguredDrive SwapfileDrive;
	}
}