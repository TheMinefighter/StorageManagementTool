using System.Collections.Generic;

namespace StorageManagementCore.Configuration {
	public class PagefileSysConfiguration {
		public bool SystemManaged;
		public List<Pagefile> Pagefiles;
			
		public class Pagefile {
			public ConfiguredDrive Drive;
			public int MaxSize;
			public int MinSize;

			public Pagefile(ConfiguredDrive drive, int maxSize, int minSize) {
				Drive = drive;
				MaxSize = maxSize;
				MinSize = minSize;
			}
		}
	}
}