namespace ExtendedMessageBoxLibrary {
	public class ExtendedMessageBoxResult {
		public ExtendedMessageBoxConfiguration MessageBoxButtonsUsed { get; set; }
		public string ClickedText { get; set; }
		public int NumberOfClickedButton { get; set; }

		public ExtendedMessageBoxResult(ExtendedMessageBoxConfiguration extendedMessageBoxButtons,
			int numberOfClickedButton) {
			ClickedText = extendedMessageBoxButtons.Buttons[numberOfClickedButton];
			MessageBoxButtonsUsed = extendedMessageBoxButtons;
			NumberOfClickedButton = numberOfClickedButton;
		}
	}
}