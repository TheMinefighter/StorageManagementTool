namespace ConfirmationDialog {
	internal class ConfirmationTag {
		public string Text;
		public string Confirmation;
		public bool Confirmed;
		public ConfirmationTag(string text=null, string confirmation=null) {
			Text = text??ConfirmationStrings.DefaultText;
			Confirmation = confirmation??ConfirmationStrings.DefaultConfirm ;
		}
//		public static  implicit operator ConfirmationTag((string, string) source) => new ConfirmationTag(source.Item1,source.Item2);
//		public static  implicit operator ConfirmationTag(string source) => new ConfirmationTag(source);
	}
}