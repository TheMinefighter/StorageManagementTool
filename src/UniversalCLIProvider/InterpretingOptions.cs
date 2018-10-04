﻿namespace UniversalCLIProvider {
	public class InterpretingOptions {
		public static InterpretingOptions DefaultOptions = new InterpretingOptions {
			IgnoreParameterCase = true,
			PreferredArgumentPrefix = '/'
		};

		public bool IgnoreParameterCase = true;
		public string InteractiveOption = "Master:Interactive";
		public string HexOption= "Master:Hex";
		
		public char PreferredArgumentPrefix = '/';
		public string RootName = ".";

		public ContextDefaultAction StandardDefaultAction;
	}
}