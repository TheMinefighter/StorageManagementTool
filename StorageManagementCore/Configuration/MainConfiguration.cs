﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Represents the JSON serializable configuration data of program
	/// </summary>
	public class MainConfiguration {
		/// <summary>
		/// </summary>
		public AllPagefilesConfiguration AllPagefilesSettings;

		public bool CredentialsOnStartup;

		/// <summary>
		///  The default path to move data to
		/// </summary>
		public string DefaultHDDPath;

		/// <summary>
		///  Overrides the UI language if not null
		/// </summary>
		public string LanguageOverride;

		/// <summary>
		///  The configured SSD MonitoringSetting
		/// </summary>
		public MonitoringConfiguration MonitoringSettings;

		public bool DisableUnprivilgedLinkCreation;
		

		/// <summary>
		/// </summary>
		public Dictionary<Guid, string> ShellFolderSettings;

		/// <summary>
		///  The Version of the Configuration
		/// </summary>
		public string Version;


		public static MainConfiguration DefaultSettings() => new MainConfiguration {
			AllPagefilesSettings = new AllPagefilesConfiguration(),
			MonitoringSettings = new MonitoringConfiguration(),
			ShellFolderSettings = new Dictionary<Guid, string>(),
			Version = "1.1"
		};

		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}