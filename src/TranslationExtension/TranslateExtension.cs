using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using JetBrains.Annotations;

namespace TranslationExtension {
	public class TranslateExtension : UpdatableMarkupExtension {
		private Func<string> getValue;

		public TranslateExtension() { }

		public TranslateExtension(string overrideSource) {
			int index = overrideSource.LastIndexOf('.');
			if (index==-1) {
				
			}
			else {
				Initialize(overrideSource.Substring(0,index),overrideSource.Substring(index+1));
			}
		}

		public TranslateExtension([NotNull] string type, [NotNull] string propertyName) {
			Initialize(type, propertyName);
		}

		private void Initialize(string type, string propertyName) {
#if DEBUG
			Type resourceType = Assembly.GetEntryAssembly().GetType(type);
			if (resourceType is null) {
				throw new ArgumentException("Couldn't find type specified", nameof(type));
			}

			PropertyInfo resourceProperty = resourceType.GetProperty(propertyName);
			if (resourceProperty is null) {
				throw new ArgumentException("Could not find the resource specified (but the resource containing type)",
					nameof(propertyName));
			}

			if (resourceProperty.PropertyType != typeof(string)) {
				throw new ArgumentException("The resource specified is not of type string", nameof(propertyName));
			}

			getValue = () => (string) resourceProperty.GetValue(null);
#else
// ReSharper disable once PossibleNullReferenceException
			getValue = () => Assembly.GetEntryAssembly().GetType(type).GetProperty(propertyName).GetValue(null);
#endif
		}


		protected override object ProvideValueInternal(IServiceProvider serviceProvider) {
			Console.WriteLine("Reached point");
			if (getValue==null) {
				return "Test2";
			}
			return getValue();
		}
	}
}