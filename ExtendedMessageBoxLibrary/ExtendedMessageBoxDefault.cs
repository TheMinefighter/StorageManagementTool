namespace ExtendedMessageBoxLibrary
{
   public class ExtendedMessageBoxDefault
   {
      public int AnswerNumber { get; set; }
      public string DefaultIdentifier { get; set; }

      public ExtendedMessageBoxDefault(int answerNumber, string defaultIdentifier)
      {
         this.AnswerNumber = answerNumber;
         this.DefaultIdentifier = defaultIdentifier;
      }
   }
}