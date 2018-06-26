using System;
using System.IO;
using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	public struct ConfiguredDrive : IEquatable<ConfiguredDrive> {
		[NotNull]
		public DriveInfo LocalDrive;

		public KnownDrive Type;

		public ConfiguredDrive(DriveInfo drive) {
			LocalDrive = drive;
			Type = KnownDrive.Unknown;
		}

//TODO If Non Letter Drives are support (don't know) to be changed
		public bool Equals(ConfiguredDrive other) =>
			Type == other.Type && LocalDrive.GetDriveLetter() == other.LocalDrive.GetDriveLetter();

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}

			return obj is ConfiguredDrive drive && Equals(drive);
		}

		public override int GetHashCode() {
			unchecked {
				return ((int) Type * 397) ^ LocalDrive.GetDriveLetter().GetHashCode();
			}
		}
	}
}