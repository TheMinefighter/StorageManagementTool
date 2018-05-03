using System.Collections.Generic;

namespace StorageManagementCore.Configuration {
	public class PagefileSysConfiguration {
		public List<Pagefile> Pagefiles;
		public bool SystemManaged;

		/// <summary>
		/// </summary>
		public class Pagefile {
			/// <summary>
			///  The drive to store on
			/// </summary>
			public ConfiguredDrive Drive;

			/// <summary>
			///  Maximum size in MB
			/// </summary>
			public int MaxSize;

			/// <summary>
			///  Minimum sie in MB
			/// </summary>
			public int MinSize;

			public Pagefile(ConfiguredDrive drive, int maxSize, int minSize) {
				Drive = drive;
				MaxSize = maxSize;
				MinSize = minSize;
			}
		}
	}
}