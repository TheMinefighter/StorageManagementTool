using System.Collections.Generic;

namespace StorageManagementCore.Configuration {
	public class PagefileSysConfiguration {
		public List<Pagefile> Pagefiles;
		public bool SystemManaged;

		public PagefileSysConfiguration() => Pagefiles= new List<Pagefile>();
	}
}