using System;
using System.Reflection;
using System.Windows;
using JetBrains.Annotations;

namespace TranslationExtension {
	/// <summary>
	/// Provides easy WPF Localization
	/// </summary>
	public class LocalizeExtension : UpdateableMarkupExtension {
		/// <summary>
		/// Shall be called when the language changes at runtime
		/// </summary>
		public static EventHandler LanguageChanged = (e, args) => { };

		private Func<string> _getValue;
		private readonly Action<IServiceProvider> _getValueOnDemandInitializer;

		private LocalizeExtension(DBNull internalBase) => LanguageChanged += OnLanguageChanged;

		/// <summary>
		/// Creates a new <see cref="LocalizeExtension"/> 
		/// </summary>
		/// <exception cref="InvalidOperationException">When the <see cref="Settings.ResourceFileProperty"/> is not set</exception>
		public LocalizeExtension() : this(DBNull.Value) {
			_getValueOnDemandInitializer = p => {
				Type toUse = Settings.GetResourceFile(TargetObject as UIElement);
				if (toUse is null) {
					throw new InvalidOperationException("No resource type specified");
				}

				UIElement uiElement = (UIElement) TargetObject;

				string resourceName;
				if (uiElement.Uid == "") {
					resourceName = uiElement.GetValue(FrameworkElement.NameProperty) as string;
				}
				else {
					resourceName = uiElement.Uid;
				}

				if (TargetProperty is DependencyProperty d) {
					resourceName += d.Name;
				}
				else {
					resourceName += ((PropertyInfo) TargetProperty).Name;
				}

				Initialize(toUse, resourceName);
			};
		}

		/// <summary>
		/// Creates a new <see cref="LocalizeExtension"/> overriding the resource name
		/// </summary>
		/// <param name="resourceName">The name of the resource to use</param>
		/// <exception cref="InvalidOperationException">When the <see cref="Settings.ResourceFileProperty"/> is not set</exception>
		public LocalizeExtension(string resourceName) : this(DBNull.Value) {
			_getValueOnDemandInitializer = p => {
				Type toUse = Settings.GetResourceFile(TargetObject as UIElement);
				if (toUse is null) {
					throw new InvalidOperationException("No resource type specified");
				}

				Initialize(toUse, resourceName);
			};
		}

		/// <summary>
		/// Creates a new <see cref="LocalizeExtension"/> overriding the resource type as well as the resource name
		/// </summary>
		/// <param name="resourceType">The type in which the resource to use is</param>
		/// <param name="resourceName">The name of the resource to use</param>
		public LocalizeExtension([NotNull] Type resourceType, [NotNull] string resourceName) : this(DBNull.Value) {
			Initialize(resourceType, resourceName);
		}

		/// <summary>
		/// Handles a runtime language change
		/// </summary>
		/// <param name="sender">The event sender</param>
		/// <param name="eventArgs"> The event arguments</param>
		private void OnLanguageChanged(object sender, EventArgs eventArgs) {
			UpdateValue(GetValue(null));
		}

		/// <summary>
		/// Initializes a instance of this class
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyName"></param>
		/// <exception cref="ArgumentException">When the given Property does not exist in the type provided</exception>
		private void Initialize(Type type, string propertyName) {
#if DEBUG
			PropertyInfo resourceProperty = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static);
			if (resourceProperty is null) {
				throw new ArgumentException($"Could not find the resource specified (\"{propertyName}\")",
					nameof(propertyName));
			}

			if (resourceProperty.PropertyType != typeof(string)) {
				throw new ArgumentException("The resource specified is not of type string", nameof(propertyName));
			}

			_getValue = () => (string) resourceProperty.GetValue(null);
#else
			PropertyInfo propertyInfo = type.GetProperty(propertyName);
			// ReSharper disable once PossibleNullReferenceException
			_getValue = () => (string) propertyInfo.GetValue(null);
#endif
		}

		/// <summary>
		/// The <see cref="ProvideValueInternal"/> Implementation
		/// </summary>
		/// <param name="provider">The service provider, providing additional information</param>
		/// <returns>The localized string</returns>
		/// <exception cref="InvalidOperationException">If the current instance were not initialized correctly</exception>
		private string GetValue(IServiceProvider provider) {
			if (_getValue is null) {
				if (_getValueOnDemandInitializer is null) {
					throw new InvalidOperationException();
				}

				if (provider is null) {
					//No exception thrown, due to the fact that that might be a temporary state
					return "Error: Language updated before the first value request";
				}

				_getValueOnDemandInitializer(provider);
				if (_getValue is null) {
					throw new InvalidOperationException();
				}
			}

			return _getValue();
		}

		/// <summary>
		/// Provides the translation text
		/// </summary>
		/// <param name="serviceProvider">The service provider, providing additional information</param>
		/// <returns>The localized string</returns>
		protected override object ProvideValueInternal(IServiceProvider serviceProvider) => GetValue(serviceProvider);
	}
}