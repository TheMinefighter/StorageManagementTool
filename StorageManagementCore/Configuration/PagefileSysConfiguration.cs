using System.Collections.Generic;
using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	public class PagefileSysConfiguration {
		[NotNull] [ItemNotNull]
		public List<Pagefile> Pagefiles;

		public bool SystemManaged;

		public PagefileSysConfiguration() => Pagefiles = new List<Pagefile>();
	}
}