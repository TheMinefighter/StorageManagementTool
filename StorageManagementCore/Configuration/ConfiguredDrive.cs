using System.IO;
using System.Linq;

namespace StorageManagementCore.Configuration {
	public struct ConfiguredDrive {
		public char Letter;
		public KnownDrive Type;
		public char LocalDriveLetter => Letter;
		public DriveInfo LocalDrive => new DriveInfo(new string(LocalDriveLetter, 1));

		public ConfiguredDrive(char driveLetter) {
			Letter = driveLetter;
			Type = KnownDrive.Unknown;
		}

		public ConfiguredDrive(DriveInfo drive) {
			Letter = drive.DriveLetter();
			Type = KnownDrive.Unknown;
		}
	}
}