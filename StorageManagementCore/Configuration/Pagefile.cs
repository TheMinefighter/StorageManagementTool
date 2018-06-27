using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	/// <summary>
	/// </summary>
	public class Pagefile : IEquatable<Pagefile>, INotifyPropertyChanged {
		private ConfiguredDrive _drive;
		private int _maxSize;
		private int _minSize;

		/// <summary>
		///  The drive to store on
		/// </summary>
		public ConfiguredDrive Drive {
			get => _drive;
			set { _drive = value; OnPropertyChanged(nameof(Drive)); }
		}

		/// <summary>
		///  Maximum size in MB
		/// </summary>
		public int MaxSize {
			get => _maxSize;
			set { _maxSize = value; OnPropertyChanged(nameof(MaxSize)); }
		}

		/// <summary>
		///  Minimum size in MB
		/// </summary>
		public int MinSize {
			get => _minSize;
			set { _minSize = value; OnPropertyChanged(nameof(MinSize)); }
		}

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

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName]
			string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}