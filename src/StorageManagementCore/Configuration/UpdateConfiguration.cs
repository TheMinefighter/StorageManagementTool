using System;
using System.Collections.Generic;
using System.Linq;
using StorageManagementCore.Backend;

namespace StorageManagementCore.Configuration {
	public class UpdateConfiguration {
		private static ReadOnlyMap<UpdateMode, string> _updateModeByLocalizedName;

		public static ReadOnlyMap<UpdateMode, string> UpdateModeByLocalizedName => _updateModeByLocalizedName ??
			(_updateModeByLocalizedName =new ReadOnlyMap<UpdateMode, string>(Enum.GetValues(typeof(UpdateMode)).Cast<UpdateMode>()
				.Select(x => new KeyValuePair<UpdateMode, string>(x, LocalizedModeName(x)))));

		public static string LocalizedModeName(UpdateMode updateMode) {
			switch (updateMode) {
				case UpdateMode.NoUpdates:
					return WPFGUI.GlobalizationRessources.SettingsStrings.UpdateMode_NoUpdates;
				case UpdateMode.DownloadAndInstallOnStartup:
					return WPFGUI.GlobalizationRessources.SettingsStrings.UpdateMode_DownloadAndInstallOnStartup;
				case UpdateMode.DownloadOnStartupInstallNext:
					return WPFGUI.GlobalizationRessources.SettingsStrings.UpdateMode_DownloadOnStartupInstallNext;
				case UpdateMode.DownloadOnStartupInstallManual:
					return WPFGUI.GlobalizationRessources.SettingsStrings.UpdateMode_DownloadOnStartupInstallManual;
				default:
					throw new ArgumentOutOfRangeException(nameof(updateMode), updateMode, null);
			}
		}

		public bool UsePreReleases;
		public UpdateMode Mode;

		public UpdateConfiguration(bool usePreReleases, UpdateMode mode) {
			UsePreReleases = usePreReleases;
			Mode = mode;
		}

		public UpdateConfiguration() { }

		static UpdateConfiguration() {
			Session.LanguageChanged += (a, b) => _updateModeByLocalizedName = null; }

		public static UpdateConfiguration Default => new UpdateConfiguration {
			UsePreReleases = true,
			Mode = UpdateMode.DownloadOnStartupInstallManual
		};
	}
}