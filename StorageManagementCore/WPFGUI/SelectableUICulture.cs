using System.Globalization;

namespace StorageManagementCore.WPFGUI
{
	public struct SelectableUICulture
	{
		public CultureInfo Value;
		public override string ToString()
		{
	
			return Value.NativeName;
		}
	}
}