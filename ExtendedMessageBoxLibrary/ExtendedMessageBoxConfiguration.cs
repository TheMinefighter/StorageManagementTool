using System.Security.Principal;

namespace ExtendedMessageBoxLibrary
{
   public class ExtendedMessageBoxConfiguration
   {
      public string[] Buttons { get; set; }
      public bool ShowCheckBox;
      public int NumberOfBbuttons;
      public string Text;
      public string Title;
      public string DefaultIdentifier;
      public int DefaultButton;

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

      public ExtendedMessageBoxConfiguration(string text, string title, string[] buttons, int defaultButton = -1, string defaultIdentifier = "NoDefault")
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