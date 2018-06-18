namespace ExtendedMessageBoxLibrary
{
	public class ExtendedMessageBoxConfiguration
	{
		public int DefaultButton;
		public string DefaultIdentifier;
		public int NumberOfBbuttons;
		public bool ShowCheckBox;
		public string Text;
		public string Title;
		public string[] Buttons { get; set; }

		public ExtendedMessageBoxConfiguration(string[] text, string title, string[] buttons, int defaultButton = -1,
			string defaultIdentifier = "NoDefault")
		{
			ShowCheckBox = defaultIdentifier != "NoDefault";
			NumberOfBbuttons = buttons.Length;
			Buttons = buttons;
			Text = string.Join(" ", text);
			Title = title;
			DefaultIdentifier = defaultIdentifier;
			DefaultButton = defaultButton;
		}

		public ExtendedMessageBoxConfiguration(string text, string title, string[] buttons, int defaultButton = -1,
			string defaultIdentifier = "NoDefault")
		{
			Buttons = buttons;
			ShowCheckBox = defaultIdentifier != "NoDefault";
			NumberOfBbuttons = buttons.Length;
			Text = text;
			Title = title;
			DefaultIdentifier = defaultIdentifier;
			DefaultButton = defaultButton;
		}
	}
}