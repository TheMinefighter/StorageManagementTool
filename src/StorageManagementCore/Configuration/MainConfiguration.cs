using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Represents the JSON serializable configuration data of program
	/// </summary>
	public class MainConfiguration {
		/// <summary>
		/// The settings for all pagefiles
		/// </summary>
		public AllPagefilesConfiguration AllPagefilesSettings;

		/// <summary>
		/// Whether to ask for credentials on GUI startup
		/// </summary>
		public bool CredentialsOnStartup;

		/// <summary>
		///  The default path to move data to
		/// </summary>
		public string DefaultHDDPath;

		/// <summary>
		/// Whether to completely disable unprivileged symlink creation 
		/// </summary>
		/// <remarks>
		/// This setting should be used when there are compatibility issues with unprivileged symlinks,
		/// which existence is completely denied in the official documentation, lol
		/// </remarks>
		public bool DisableUnprivilgedLinkCreation;

		/// <summary>
		///  Overrides the UI language if not null, otherwise equivalent to system language
		/// </summary>
		public string LanguageOverride;

		/// <summary>
		///  The configured SSD MonitoringSetting
		/// </summary>
		public MonitoringConfiguration MonitoringSettings;


		/// <summary>
		/// 
		/// </summary>
		/// <remarks> Not used yet, might be removed in future </remarks>
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