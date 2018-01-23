namespace ExtendedMessageBoxLibary
{
    public class ExtendedMessageBoxResult
   {
      public ExtendedMessageBoxConfiguration MessageBoxButtonsUsed { get; set; }
      public string ClickedText { get; set; }
      public int NumberOfClickedButton { get; set; }
      public ExtendedMessageBoxResult(ExtendedMessageBoxConfiguration extendedMessageBoxButtons,int numberOfClickedButton)
      {
         this.ClickedText = extendedMessageBoxButtons.Buttons[numberOfClickedButton];
         this.MessageBoxButtonsUsed = extendedMessageBoxButtons;
         this.NumberOfClickedButton = numberOfClickedButton;
      }
   }
}
