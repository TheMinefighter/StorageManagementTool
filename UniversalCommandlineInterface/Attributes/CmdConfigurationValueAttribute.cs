using System;

namespace UniversalCommandlineInterface.Attributes {
	[AttributeUsage(AttributeTargets.Parameter)]
	public class CmdConfigurationValueAttribute : Attribute {
		public string ExtendedHelp;
		public string Help;
		public bool IsReadonly;


		public CmdConfigurationValueAttribute(Predicate<string> test, string help = null, string extendedHelp = null, bool isReadonly = false) {
			IsReadonly = isReadonly;
			Help = help;
			ExtendedHelp = extendedHelp;
		}
	}
}