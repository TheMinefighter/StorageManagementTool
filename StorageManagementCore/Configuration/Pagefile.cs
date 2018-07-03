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
		public string ToDisplay => ToString();

		/// <summary>
		///  The drive to store on
		/// </summary>
		public ConfiguredDrive Drive {
			get => _drive;
			set {
				if (!Equals(value, _drive)) {
					_drive = value;
					OnPropertyChanged(nameof(Drive));
				}
			}
		}

		/// <summary>
		///  Maximum size in MB
		/// </summary>
		public int MaxSize {
			get => _maxSize;
			set {
				if (value != _maxSize) {
					_maxSize = value;
					OnPropertyChanged(nameof(MaxSize));
				}
			}
		}

		/// <summary>
		///  Minimum size in MB
		/// </summary>
		public int MinSize {
			get => _minSize;
			set {
				if (value != _minSize) {
					_minSize = value;
					OnPropertyChanged(nameof(MinSize));
				}
			}
		}

		public Pagefile(ConfiguredDrive drive, int maxSize, int minSize) {
			_drive = drive;
			_maxSize = maxSize;
			_minSize = minSize;
		}

		public bool Equals(Pagefile other) => other !=null && Drive.Equals(other.Drive) && MaxSize == other.MaxSize && MinSize == other.MinSize;

		public event PropertyChangedEventHandler PropertyChanged;

		public override string ToString() => $"{Drive} ({MinSize}-{MaxSize})";

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

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName]
			string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToDisplay)));
			
		}
	}
}