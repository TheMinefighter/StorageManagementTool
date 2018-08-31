using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using JetBrains.Annotations;

namespace LocalizationExtension {
//The following class is NOT my own work and instead from https://www.thomaslevesque.com/2009/07/28/wpf-a-markup-extension-that-can-update-its-target/
//According to https://www.thomaslevesque.com/about/#comment-105 no further licensing stuff is required
	/// <summary>
	/// A base class for making <see cref="MarkupExtension"/>s updateable without the need for an internal <see cref="Binding"/>
	/// </summary>
	public abstract class UpdateableMarkupExtension : MarkupExtension {
		/// <summary>
		///  The targeted object, null until first evaluation
		/// </summary>
		[CanBeNull]
		protected object TargetObject { get; private set; }
/// <summary>
/// Whether the target is readonly
/// </summary>
		protected bool IsReadOnly{ get; private set; }
		/// <summary>
		///  The targeted property, null until first evaluation
		/// </summary>
		[CanBeNull]
		protected object TargetProperty { get; private set; }
 
		/// <summary>
		/// Provides the value request and (if needed) sets update information
		/// </summary>
		/// <param name="serviceProvider">The <see cref="T:System.IServiceProvider" /> providing additional data</param>
		/// <returns>The result of the <see cref="T:System.Windows.Markup.MarkupExtension" /></returns>
		public sealed override object ProvideValue(IServiceProvider serviceProvider) {
			if (TargetObject is null && serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target) {
				TargetObject = target.TargetObject;
				TargetProperty = target.TargetProperty;
				switch (TargetProperty) {
					case DependencyProperty dependencyProperty:
						IsReadOnly = dependencyProperty.ReadOnly;
						break;
					case PropertyInfo propertyInfo:
						IsReadOnly = propertyInfo.CanWrite;
						break;
					default:
						throw new ArgumentException("The targetProperty provided is not a DependencyProperty neither of PropertyInfo", nameof(serviceProvider));
				}
			}

			return ProvideValueInternal(serviceProvider);
		}

		/// <summary>
		/// Updates the value of the target
		/// </summary>
		/// <param name="value"> the value to update to</param>
		/// <exception cref="InvalidOperationException"> Thrown when the current instance has not been evaluated before</exception>
		protected void UpdateValue(object value) {
			if (TargetObject != null&& !IsReadOnly ) {
				if (TargetProperty is DependencyProperty prop1) {
					DependencyObject obj = TargetObject as DependencyObject;

					void UpdateAction() => obj.SetValue(prop1, value);

					// Check whether the target object can be accessed from the
					// current thread, and use Dispatcher.Invoke if it can't
					if (obj is null) {
						throw new InvalidOperationException();
					}

					if (obj.CheckAccess()) {
						UpdateAction();
					}
					else {
						obj.Dispatcher.Invoke(UpdateAction);
					}
				}
				else // _targetProperty is PropertyInfo
				{
					PropertyInfo prop = TargetProperty as PropertyInfo;
					if (prop == null) {
						throw new InvalidOperationException();
					}

					prop.SetValue(TargetObject, value, null);
				}
			}
		}

		/// <summary>
		/// Provides the value of the <see cref="MarkupExtension"/>
		/// </summary>
		/// <param name="serviceProvider"> The <see cref="IServiceProvider"/> providing additional data</param>
		/// <returns> The result of the <see cref="MarkupExtension"/></returns>
		protected abstract object ProvideValueInternal(IServiceProvider serviceProvider);
	}
}