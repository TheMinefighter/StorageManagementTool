﻿using System.Collections.Generic;

namespace UniversalCommandlineInterface.Interpreters {
	public class ConfigurationNamespaceInterpreter {
		private bool _loaded;
		public ManagedConfigurationInterpreter ConfigurationInterpreter;
		public ConfigurationNamespaceInterpreter parent;

		public IList<ConfigurationNamespaceInterpreter> path {
			get {
				if (parent != null) {
					IList<ConfigurationNamespaceInterpreter> parentPath = parent.path;
					parentPath.Add(this);
					return parentPath;
				}

				return new List<ConfigurationNamespaceInterpreter> {this};
			}
		}
	}
}