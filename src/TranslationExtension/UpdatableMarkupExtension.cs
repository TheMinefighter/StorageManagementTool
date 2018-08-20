using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace TranslationExtension {
//The following class is NOT my own work and instead from https://www.thomaslevesque.com/2009/07/28/wpf-a-markup-extension-that-can-update-its-target/
//According to https://www.thomaslevesque.com/about/#comment-105 no further licensing stuff is required
	public abstract class UpdatableMarkupExtension : MarkupExtension {
		private object _targetObject;
		private object _targetProperty;

		protected object TargetObject => _targetObject;

		protected object TargetProperty => _targetProperty;

		public sealed override object ProvideValue(IServiceProvider serviceProvider) {
			if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target) {
				_targetObject = target.TargetObject;
				_targetProperty = target.TargetProperty;
			}

			return ProvideValueInternal(serviceProvider);
		}

		protected void UpdateValue(object value) {
			if (_targetObject != null) {
				if (_targetProperty is DependencyProperty prop1) {
					DependencyObject obj = _targetObject as DependencyObject;

					void UpdateAction	() => obj.SetValue(prop1, value);

					// Check whether the target object can be accessed from the
					// current thread, and use Dispatcher.Invoke if it can't
					if (obj is null) {
						throw new InvalidOperationException();
					}
					if (obj.CheckAccess()) {
						UpdateAction();
					}
					else {
						obj.Dispatcher.Invoke((Action) UpdateAction);
					}
				}
				else // _targetProperty is PropertyInfo
				{
					PropertyInfo prop = _targetProperty as PropertyInfo;
					if (prop == null) {
						throw new InvalidOperationException();
					}

					prop.SetValue(_targetObject, value, null);
				}
			}
		}

		protected abstract object ProvideValueInternal(IServiceProvider serviceProvider);
	}
}