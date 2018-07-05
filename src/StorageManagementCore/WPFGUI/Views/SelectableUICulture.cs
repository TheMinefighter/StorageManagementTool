using System.Globalization;

namespace StorageManagementCore.WPFGUI.Views {
	//TODO Replace withe Datatemplate
	public struct SelectableUICulture {
		
		public CultureInfo Value;

		public override string ToString() => Value.NativeName;
	}
}