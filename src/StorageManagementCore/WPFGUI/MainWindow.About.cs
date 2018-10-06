using System;
using System.IO;
using System.Reflection;
using System.Windows;
using static StorageManagementCore.WPFGUI.GlobalizationResources.AboutStrings;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		public Visibility Lol { get; set; }

		private void AboutTextWb_OnLoaded(object sender, RoutedEventArgs e) {
			AboutTi.Header = AboutTiText;
			Assembly current = Assembly.GetExecutingAssembly();
			const string res = "StorageManagementCore.Ressources.About.html";
			using (Stream stream = current.GetManifestResourceStream(res)) {
				using (StreamReader rd =
					new StreamReader(stream ?? throw new Exception("An internal ressource could not be loaded"))) {
					AboutTextWb.NavigateToString(rd.ReadToEnd());
					//AboutTextWb.NavigateToStream(stream ?? throw new Exception("An internal ressource could not be loade"));
				}
			}

			AboutTextWb.UpdateLayout();
//         AboutTextWb.
//            AboutTextWb.Refresh();
		}
	}
}