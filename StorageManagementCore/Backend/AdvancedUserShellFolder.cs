using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Csv;

namespace StorageManagementCore.Backend {
	public class AdvancedUserShellFolder {
		public static AdvancedUserShellFolder[] AllUSF;
		public string DefaultValue;
		public bool IsUserSpecific;
		public string LocalizedName;
		public string Name;
		public bool ShouldBeEdited;
		public bool Undefined;
		public Guid WindowsIdentifier;

		private AdvancedUserShellFolder() { }

		public static AdvancedUserShellFolder GetUSF(string name) {
			return AllUSF.First(x => x.Name == name);
		}

		public static AdvancedUserShellFolder GetUSF(Guid windowsIdentifier) {
			return AllUSF.First(x => x.WindowsIdentifier == windowsIdentifier);
		}

		public static void LoadUSF() {
			Assembly current = Assembly.GetExecutingAssembly();
			const string res = "StorageManagementCore.Backend.AdvancedUserShellFolderData.csv";
			MessageBox.Show(string.Join(";", current.GetManifestResourceNames()));
			string[] manifestResourceNames = current.GetManifestResourceNames();
			manifestResourceNames.ToString();
			using (Stream stream = current.GetManifestResourceStream(res)) {
				IEnumerable<ICsvLine> data = CsvReader.ReadFromStream(stream);
				AllUSF = data.Select(x => new AdvancedUserShellFolder {
					Name = x[0],
					WindowsIdentifier = new Guid(x[1]),
					Undefined = x[2][0] == '1',
					IsUserSpecific = x[3][0] == '1',
					ShouldBeEdited = x[4][0] == '1',
					LocalizedName = x[0]
				}).ToArray();
			}
		}
	}
}