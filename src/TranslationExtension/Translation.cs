using System;
using System.Windows;
using System.Windows.Controls;

namespace TranslationExtension {
	public static class Translation {
		public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.RegisterAttached("ResourceFile",
			typeof(string), typeof(Translation), new FrameworkPropertyMetadata(null));

		public static string GetResourceFile(UIElement element) {
			if (element == null) {
				throw new ArgumentNullException(nameof(element));
			}

			return (string) element.GetValue(MyPropertyProperty);
		}

		public static void SetResourceFile(UIElement element, string value) {
			if (element == null) {
				throw new ArgumentNullException(nameof(element));
			}

			if (element is Panel p) {
				foreach (UIElement pChild in p.Children) {
					SetResourceFile(pChild, value);
				}
			}

			element.SetValue(MyPropertyProperty, value);
		}
	}
}