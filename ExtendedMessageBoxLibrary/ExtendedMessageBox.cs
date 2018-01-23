using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedMessageBoxLibary
{
   public class ExtendedMessageBox
   {
        /// <summary>
        /// Connect an given String Array with a given Connector String
        /// </summary>
        /// <param name="strings">The Strings to connect</param>
        /// <param name="connector">The connector to use</param>
        /// <returns>The strings connected with the given connector</returns>
       public static string ConnectStringArrayWithConnector(string[] strings, string connector)
       {
           if (strings.GetLength(0) == 0) { return ""; }
           string ret = strings[0];
           for (int i = 1; i < strings.GetLength(0); i++)
           {
               ret = ret + connector + strings[i];
           }
           return ret;
       }
        public static List<ExtendedMessageBoxDefault> Defaults { get; set; }
      public ExtendedMessageBoxResult ToBeReturned { get; set; }
      public ExtendedMessageBoxConfiguration ExMsgBoxcfg { get; set; }
      public ExtendedMessageBox(ExtendedMessageBoxConfiguration ExMsgBoxBtns)
      {
         this.ExMsgBoxcfg = ExMsgBoxBtns;
      }
      public ExtendedMessageBox(
         string[] text, string title, string[] buttons,
         string defaultIdentifier = "NoDefault"
         )
      {
         this.ExMsgBoxcfg = new ExtendedMessageBoxConfiguration(text, title, buttons);
      }
      public static int GetAnswerNumberOfDefaultbyIdentifier(string defaultIdentifierToSearch)
      {
         foreach (ExtendedMessageBoxDefault item in ExtendedMessageBox.Defaults)
         {
            if (item.DefaultIdentifier == defaultIdentifierToSearch)
            {
               return item.AnswerNumber;
            }
         }
         throw new Exception("Der gesuchte Wert konnte nicht gefunden werden");
      }
      public static bool ExistDefaultIdentifier(string defaultIdentifierToSearch)
      {
          return ExtendedMessageBox.Defaults.Any(item => item.DefaultIdentifier == defaultIdentifierToSearch);
      }
      public static ExtendedMessageBoxResult Show(ExtendedMessageBoxConfiguration buttons)
      {
         return new ExtendedMessageBox(buttons).Show();
      }
      public ExtendedMessageBoxResult Show()
      {
         if (this.ExMsgBoxcfg.DefaultIdentifier != "NoDefault")
         {
            if (ExtendedMessageBox.ExistDefaultIdentifier(this.ExMsgBoxcfg.DefaultIdentifier))
            {
               return new ExtendedMessageBoxResult(this.ExMsgBoxcfg, ExtendedMessageBox.GetAnswerNumberOfDefaultbyIdentifier(this.ExMsgBoxcfg.DefaultIdentifier));
            }
         }
         ExtendedMessageBoxDialog dialog = new ExtendedMessageBoxDialog()
         {
            Tag = this
         };
         dialog.ShowDialog();
         if (ToBeReturned == null)
         {
            ToBeReturned = ExtendedMessageBox.Show(ExMsgBoxcfg);
         }
         if (this.ExMsgBoxcfg.DefaultIdentifier != "NoDefault")
         {
            ExtendedMessageBox.Defaults.Add(new ExtendedMessageBoxDefault(ToBeReturned.NumberOfClickedButton, this.ExMsgBoxcfg.DefaultIdentifier));
         }
         return ToBeReturned;
      }
   }
}
