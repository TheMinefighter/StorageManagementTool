using System.IO;
using System.Linq;

namespace StorageManagementCore.Configuration {
	public struct ConfiguredDrive {
		public DriveInfo LocalDrive;
		public KnownDrive Type;
	
//		public ConfiguredDrive(char driveLetter) {
//			Letter = driveLetter;
//			Type = KnownDrive.Unknown;
//		}

		public ConfiguredDrive(DriveInfo drive) {
		 LocalDrive = drive;
			Type = KnownDrive.Unknown;
		}
	}
}