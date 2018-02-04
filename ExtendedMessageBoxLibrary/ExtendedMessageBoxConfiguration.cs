namespace ExtendedMessageBoxLibrary
{
    public class ExtendedMessageBoxConfiguration
    {
        public string[] Buttons { get; set; }
        public bool ShowCheckBox { get; set; }
        public int NumberOfBbuttons { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string DefaultIdentifier { get; set; }

        public ExtendedMessageBoxConfiguration(string[] text, string title, string[] buttons, string defaultIdentifier = "NoDefault")
        {
            ShowCheckBox = defaultIdentifier != "NoDefault";
            NumberOfBbuttons = buttons.Length;
            Buttons = buttons;
            Text = string.Join(" ",text);
            Title = title;
            DefaultIdentifier = defaultIdentifier;
        }

        public ExtendedMessageBoxConfiguration(string text, string title, string[] buttons, string defaultIdentifier = "NoDefault")
        {
            Buttons = buttons;
            ShowCheckBox = defaultIdentifier != "NoDefault";
            NumberOfBbuttons = buttons.Length;
            Text = text;
            Title = title;
            DefaultIdentifier = defaultIdentifier;
        }
    }
}