using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;
using StorageManagementCore.WPFGUI.Views;
using static StorageManagementCore.WPFGUI.GlobalizationRessources.SettingsStrings;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void SettingsTi_OnLoaded(object sender, RoutedEventArgs e) {
			List<object> cultureInfos = Program.AvailableSpecificCultures.SelectMany(x => x)
				.Select(x => (object) new SelectableUICulture {Value = x}).ToList();
			string defaultlang = "Systemsprache";
			cultureInfos.Insert(0, defaultlang);
			SelectLanguageCmb.Items.Clear();
			SelectLanguageCmb.ItemsSource = cultureInfos;
			SelectLanguageCmb.SelectedItem = Session.Singleton.Configuration.DefaultHDDPath == null
				? defaultlang
				: (object) new SelectableUICulture {
					Value = CultureInfo.CreateSpecificCulture(Session.Singleton.Configuration.LanguageOverride)
				};
			LocalizeSettings();
			DefaultHDDPathChanged();
			CredentialsManager.OnCredentialsChanged += (s, a) => { };
			EnOrDisableCredentialsOnStartupCb.IsChecked = Session.Singleton.UnpriviligedSymlinksAvailable;
			//TODO IsAdministratorTb
		}

		private void LocalizeSettings()
		{
			EnOrDisableCredentialsOnStartupCb.Content = EnOrDisableCredentialsOnStartupCbContent;
            DeleteConfigurationBtn.Content = DeleteConfigurationBtnContent;
			AuthorizationGb.Header = AuthorizationGbHeader;
			AutomaticSetupBtn.Content = AutomaticSetupBtnContent;
			EnOrDisableSendToHDDCb.Content = EnOrDisableSendToHDDCbContent;
			//TODO Autolocalize that filename by desktop.ini
			RestartAsAdministratorBtn.Content = RestartAsAdministratorBtnContent;
			RestartAsAdministratorBtn.IsEnabled = !Session.Singleton.IsAdmin;
			IsAdministratorLbl.Text = Session.Singleton.IsAdmin ? IsAdministratorLblText : IsNoAdministratorLblText;
			SettingsTi.Header = SettingsTiText;
		}

		private void DisposeCredentialsBtn_Click(object sender, RoutedEventArgs e) {
			CredentialsManager.DisposeCredentials();
		}

		private void DefaultHDDPathChanged() {
			EnOrDisableSendToHDDCb.IsEnabled = HDDPathValid();
		}

		private void RestartAsAdministratorBtn_OnClick(object sender, RoutedEventArgs e) {
			Wrapper.RestartProgram(true);
		}

		private void EnOrDisableCredentialsOnStartupCb_OnChecked(object sender, RoutedEventArgs e)
		{
			Session.Singleton.Configuration.CredentialsOnStartup = EnOrDisableCredentialsOnStartupCb.IsChecked.Value;
				Session.Singleton.SaveCfg();
		}

		private void EnOrDisableSendToHDDCb_OnChecked(object sender, RoutedEventArgs e) {
			if (HDDPathValid()) {
				OperatingMethods.EnableSendToHDD();
			}
		}

		private static bool HDDPathValid() => 
			Session.Singleton.Configuration.DefaultHDDPath != null &&
		                                      Directory.Exists(Session.Singleton.Configuration.DefaultHDDPath);

		private void EnOrDisableSendToHDDCb_OnUnchecked(object sender, RoutedEventArgs e) {
			OperatingMethods.EnableSendToHDD(false);
		}

		private void SetLanguageAndRestartBtn_Click(object sender, RoutedEventArgs e) {
			if (SelectLanguageCmb.SelectionBoxItem is SelectableUICulture c) {
				Session.Singleton.Configuration.LanguageOverride = c.Value.ToString();
			}
			else {
				Session.Singleton.Configuration.LanguageOverride = null;
			}

			Session.Singleton.SaveCfg();
			Wrapper.RestartProgram(false);
		}

		private void ChangeCredentialsBtn_OnClick(object sender, RoutedEventArgs e) {
			CredentialsManager.GetCredentials(false, out Credentials _);
		}
		private void AutomaticSetupBtn_Click(object sender, RoutedEventArgs e)
		{
			new MainGUI.ApplyPreset().ShowDialog();
		}
		private void DeleteConfigurationBtn_Click(object sender, RoutedEventArgs e)
		{
			OperatingMethods.EnableSendToHDD(false);
			FileAndFolder.DeleteFile(new FileInfo(Session.Singleton.ConfigurationPath), false, false);
				Environment.Exit(0);
		}
    }
}