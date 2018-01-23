using System.Linq;

namespace ExtendedMessageBoxLibary
{
    public class ExtendedMessageBoxConfiguration
   {
      public string[] Buttons { get; set; }
      public bool ShowCheckBox { get; set; }
      public int NumberOfBbuttons { get; set; }
      public string[] Text { get; set; }
      public string Title { get; set; }
      public string DefaultIdentifier { get; set; }
      public ExtendedMessageBoxConfiguration(string[] text,string title,string[] buttons,string defaultIdentifier ="NoDefault")
      {
         this.ShowCheckBox = defaultIdentifier != "NoDefault";
         this.NumberOfBbuttons = buttons.Count();
          this.Buttons = buttons;
         this.Text = text;
         this.Title = title;
         this.DefaultIdentifier = defaultIdentifier;

      }
   }
   } 