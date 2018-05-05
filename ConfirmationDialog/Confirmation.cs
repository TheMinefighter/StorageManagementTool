using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;

namespace ConfirmationDialog {
	public static class Confirmation {
		internal static bool Skip;


		internal static readonly Dictionary<ModifierKeys, ModifierRequirement> Requirements = new Dictionary<ModifierKeys, ModifierRequirement> {
			{ModifierKeys.Alt, ModifierRequirement.Ignored},
			{ModifierKeys.Control, ModifierRequirement.Required},
			{ModifierKeys.Shift, ModifierRequirement.Ignored},
			{ModifierKeys.Windows, ModifierRequirement.Ignored}
		};

		public static bool Confirm(string text = null) {
			if (Skip) {
				return true;
			}

			ModifierKeys toUse = Keyboard.Modifiers;
			foreach (KeyValuePair<ModifierKeys,ModifierRequirement> modifierRequirement in Requirements) {
				switch (modifierRequirement.Value) {
					case ModifierRequirement.Required:
						if ((toUse & modifierRequirement.Key) != modifierRequirement.Key) {
							return InternalConfirm(text);}
						break;
					case ModifierRequirement.MustNot:
						if ((toUse & modifierRequirement.Key) != modifierRequirement.Key) {
							return InternalConfirm(text);}
						break;
				}
			}
			return true;
		}

		private static bool InternalConfirm(string text = null)
		{
			ConfirmationWindow window = new ConfirmationWindow(text ?? "The following action might be bad. Continue only if you know what you are doing!");
			window.ShowDialog();
			return window.Tag is bool b && b;
		}
	}
}