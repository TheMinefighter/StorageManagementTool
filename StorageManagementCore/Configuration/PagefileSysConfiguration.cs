﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace StorageManagementCore.Configuration {
	public class PagefileSysConfiguration : INotifyPropertyChanged {
		[NotNull] [ItemNotNull]
		public List<Pagefile> Pagefiles;

		private bool _systemManaged;

		public bool SystemManaged
		{
			get => _systemManaged;
			set
			{
				_systemManaged = value;
				OnPropertyChanged(nameof(SystemManaged));

			}
		}

		public PagefileSysConfiguration() => Pagefiles = new List<Pagefile>();
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}