﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	/// <inheritdoc cref="Window" />
	/// <summary>
	///  Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private MainViewModel ViewModel => (MainViewModel) Resources["ViewModel"];

		public MainWindow() {
			Program.SetConsoleVisibility(false);
#if DEBUG
			try {
				InitializeComponent();
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
#else
			InitializeComponent();
#endif

			//PagefilesTi.DataContext = ProposedPagefileConfiguration;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			Updater.InvokeUpdateProcess(Session.Singleton.Configuration.UpdateSettings);
			if (!Session.Singleton.IsAdmin && Session.Singleton.Configuration.CredentialsOnStartup) {
				List<string> args = Environment.GetCommandLineArgs().ToList();
				args.RemoveAt(0);
				Wrapper.RestartProgram(true, args.ToArray());
			}

			if (Session.Singleton.IsAdmin) {
				Title += " (Administrator)";
			}

			if (Tag is Dictionary<GUIModifier, object> modifierDictionary) {
				foreach (KeyValuePair<GUIModifier, object> modifierPair in modifierDictionary) {
					switch (modifierPair.Key) {
						case GUIModifier.UIProperties: {
							if (modifierPair.Value is Dictionary<string, object> propertiesToSet) {
								foreach (KeyValuePair<string, object> propertyPair in propertiesToSet) {
									FieldInfo elementProperty = typeof(MainWindow).GetField(propertyPair.Key.Split(',')[0],BindingFlags.NonPublic| BindingFlags.Public|BindingFlags.Instance);
									object element = elementProperty.GetValue(this);
									PropertyInfo targetProperty = element.GetType().GetProperty(propertyPair.Key.Split(',')[1]);
									if (targetProperty.PropertyType== typeof(int) && propertyPair.Value is long propertyPairValue) {
										targetProperty.SetValue(element,unchecked ((int) propertyPairValue));
									}
									else {
										targetProperty.SetValue(element, propertyPair.Value);
									}
								}
							}

							break;
						}
						default: throw new InvalidOperationException();
					}
				}
			}

		
		}


		private void BaseTc_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
	}
}