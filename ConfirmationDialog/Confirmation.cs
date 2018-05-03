using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ConfirmationDialog {
	public static class Confirmation {
		public static bool Skip;

		public static ModifierRequirement Alt {
			set => Requirements[ModifierKeys.Alt] = value;
		}

		public static ModifierRequirement Shift {
			set => Requirements[ModifierKeys.Shift] = value;
		}
		public static ModifierRequirement Control {
			set => Requirements[ModifierKeys.Control] = value;
		}
		public static ModifierRequirement Windows {
			set => Requirements[ModifierKeys.Windows] = value;
		}

		private static readonly Dictionary<ModifierKeys, ModifierRequirement> Requirements = new Dictionary<ModifierKeys, ModifierRequirement> {
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

		private static bool InternalConfirm(string text = null) {
			return true;
		}
	}
}