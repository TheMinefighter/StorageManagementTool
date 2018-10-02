using System;
using System.Windows;
using JetBrains.Annotations;

namespace LocalizationExtension {
	//Class based upon https://stackoverflow.com/a/5782985/6730162
	/// <summary>
	///  Provides additional Localization related settings
	/// </summary>
	public static class Settings {
		/// <summary>
		///  A <see cref="DependencyProperty" /> indicating which resource file is used for translations
		/// </summary>
		/// <remarks>
		///  This Property is Inherited (<seealso cref="FrameworkPropertyMetadataOptions.Inherits" />) and can not be bound (
		///  <seealso cref="FrameworkPropertyMetadataOptions.NotDataBindable" />)
		/// </remarks>
		public static readonly DependencyProperty ResourceFileProperty = DependencyProperty.RegisterAttached("ResourceFile",
			typeof(Type), typeof(Settings),
			new FrameworkPropertyMetadata(null,
				FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.NotDataBindable));

		/// <summary>
		///  Gets the <see cref="ResourceFileProperty" /> of a <see cref="DependencyObject" />
		/// </summary>
		/// <param name="element">The <see cref="DependencyObject" /> to read the <see cref="ResourceFileProperty" /> from</param>
		/// <returns> The <see cref="ResourceFileProperty" /> of the <see cref="DependencyObject" /> specified</returns>
		/// <exception cref="ArgumentNullException"> If element is null</exception>
		[CanBeNull]
		public static Type GetResourceFile(DependencyObject element) {
			if (element is null) {
				throw new ArgumentNullException(nameof(element));
			}

			return (Type) element.GetValue(ResourceFileProperty);
		}

		/// <summary>
		///  Sets the <see cref="ResourceFileProperty" /> of a <see cref="DependencyObject" />
		/// </summary>
		/// <param name="element">The element which <see cref="ResourceFileProperty" /> to set</param>
		/// <param name="value"> The value to set the elements <see cref="ResourceFileProperty" /> to</param>
		/// <exception cref="ArgumentNullException">If element is null </exception>
		public static void SetResourceFile([NotNull] DependencyObject element, [CanBeNull] Type value) {
			Console.WriteLine("Test");
			if (element is null) {
				throw new ArgumentNullException(nameof(element));
			}

			element.SetValue(ResourceFileProperty, value);
		}
	}
}