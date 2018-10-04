namespace UniversalCLIProvider {
	public class InterpretingOptions {
		public static InterpretingOptions DefaultOptions = new InterpretingOptions {
			IgnoreParameterCase = true,
			PreferredArgumentPrefix = '/'
		};

		public bool IgnoreParameterCase = true;
		public string InteractiveOption = "Master:Interactive";
		public string BinOption= "Master:Bin";
		
		public char PreferredArgumentPrefix = '/';
		public string RootName = ".";

		public ContextDefaultAction StandardDefaultAction;
	}
}