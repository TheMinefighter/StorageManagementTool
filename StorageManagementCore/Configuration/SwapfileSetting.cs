namespace StorageManagementCore.Configuration {
	public class SwapfileSetting {
		public bool SwapfileConfigured;
		public ConfiguredDrive SwapfileDrive;
		public enum SwapfileState {
			Untouched, 
			Disabled,
			Moved
		}
	}
}