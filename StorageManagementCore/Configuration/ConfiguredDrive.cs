using System.IO;
using System.Linq;

namespace StorageManagementCore.Configuration {
	public class ConfiguredDrive {
		public char Letter;
		public KnownDrive Type;
		public char LocalDriveLetter => Letter;
		public DriveInfo LocalDrive => new DriveInfo(new string(LocalDriveLetter, 1));

		public ConfiguredDrive() { }

		public ConfiguredDrive(char driveLetter) => Letter = driveLetter;

		public ConfiguredDrive(DriveInfo drive) => Letter = drive.Name.First();
	}
}