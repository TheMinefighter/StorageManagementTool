﻿using System.IO;
using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	public struct ConfiguredDrive {
		[NotNull]
		public DriveInfo LocalDrive;

		public KnownDrive Type;

		public ConfiguredDrive(DriveInfo drive) {
			LocalDrive = drive;
			Type = KnownDrive.Unknown;
		}
	}
}