using StorageManagementCore.Configuration;

namespace StorageManagementCore.WPFGUI {
	public class MainViewModel {
		public PagefileSysConfiguration PagefileConfiguration { get; set; }

		public MainViewModel() {
			PagefileConfiguration= new PagefileSysConfiguration();
		}
	}
}