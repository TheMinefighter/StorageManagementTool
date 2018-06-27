using System;

namespace StorageManagementCore.Configuration {
	/// <summary>
	/// </summary>
	public class Pagefile : IEquatable<Pagefile> {
		/// <summary>
		///  The drive to store on
		/// </summary>
		public ConfiguredDrive Drive;

		/// <summary>
		///  Maximum size in MB
		/// </summary>
		public int MaxSize;

		/// <summary>
		///  Minimum size in MB
		/// </summary>
		public int MinSize;

		public Pagefile(ConfiguredDrive drive, int maxSize, int minSize) {
			Drive = drive;
			MaxSize = maxSize;
			MinSize = minSize;
		}

		public override string ToString() => Drive.ToString() + '(' + MinSize + '-' + MaxSize + ')';

		public bool Equals(Pagefile other) => Drive.Equals(other.Drive) && MaxSize == other.MaxSize && MinSize == other.MinSize;

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}

			if (ReferenceEquals(this, obj)) {
				return true;
			}

			if (obj.GetType() != GetType()) {
				return false;
			}

			return Equals((Pagefile) obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = Drive.GetHashCode();
				hashCode = (hashCode * 397) ^ MaxSize;
				hashCode = (hashCode * 397) ^ MinSize;
				return hashCode;
			}
		}
	}
}