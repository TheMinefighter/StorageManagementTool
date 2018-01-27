namespace ExtendedMessageBoxLibary
{
    public class ExtendedMessageBoxDefault
    {
        public int AnswerNumber { get; set; }
        public string DefaultIdentifier { get; set; }

        public ExtendedMessageBoxDefault(int AnswerNumber, string DefaultIdentifier)
        {
            this.AnswerNumber = AnswerNumber;
            this.DefaultIdentifier = DefaultIdentifier;
        }
    }
}