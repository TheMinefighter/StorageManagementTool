﻿using System;

namespace UniversalCLIProvider.Attributes {
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class CmdDefaultActionAttribute : Attribute {
		private bool IsDirect;
		private ContextDefaultAction toRun;
	}
}