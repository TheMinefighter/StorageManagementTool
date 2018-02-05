using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedMessageBoxLibary;

namespace ExtendedMessageBoxLibrary
{
    public class ExtendedMessageBox
    {
        public static List<ExtendedMessageBoxDefault> Defaults { get; set; }
        public ExtendedMessageBoxResult ToBeReturned { get; set; }
        public ExtendedMessageBoxConfiguration ExMsgBoxcfg { get; set; }

        public ExtendedMessageBox(ExtendedMessageBoxConfiguration ExMsgBoxBtns)
        {
            ExMsgBoxcfg = ExMsgBoxBtns;
        }

        public ExtendedMessageBox(
            string[] text, string title, string[] buttons, int defaultButton = -1,
            string defaultIdentifier = "NoDefault"
        )
        {
            ExMsgBoxcfg = new ExtendedMessageBoxConfiguration(text, title, buttons,defaultButton, defaultIdentifier);
        }

        /// <summary>
        ///     Connect an given String Array with a given Connector String
        /// </summary>
        /// <param name="strings">The Strings to connect</param>
        /// <param name="connector">The connector to use</param>
        /// <returns>The strings connected with the given connector</returns>
        public static string ConnectStringArrayWithConnector(string[] strings, string connector)
        {
            if (strings.GetLength(0) == 0)
            {
                return "";
            }

            string ret = strings[0];
            for (int i = 1; i < strings.GetLength(0); i++)
            {
                ret = ret + connector + strings[i];
            }

            return ret;
        }

        public static int GetAnswerNumberOfDefaultbyIdentifier(string defaultIdentifierToSearch)
        {
            foreach (ExtendedMessageBoxDefault item in Defaults)
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
            return Defaults.Any(item => item.DefaultIdentifier == defaultIdentifierToSearch);
        }

        public static ExtendedMessageBoxResult Show(ExtendedMessageBoxConfiguration buttons)
        {
            return new ExtendedMessageBox(buttons).Show();
        }

        public ExtendedMessageBoxResult Show()
        {
            if (ExMsgBoxcfg.DefaultIdentifier != "NoDefault")
            {
                if (ExistDefaultIdentifier(ExMsgBoxcfg.DefaultIdentifier))
                {
                    return new ExtendedMessageBoxResult(ExMsgBoxcfg, GetAnswerNumberOfDefaultbyIdentifier(ExMsgBoxcfg.DefaultIdentifier));
                }
            }

            ExtendedMessageBoxDialog dialog = new ExtendedMessageBoxDialog
            {
                Tag = this
            };
            dialog.ShowDialog();
            if (ToBeReturned == null)
            {
                ToBeReturned = Show(ExMsgBoxcfg);
            }

            if (ExMsgBoxcfg.DefaultIdentifier != "NoDefault")
            {
                Defaults.Add(new ExtendedMessageBoxDefault(ToBeReturned.NumberOfClickedButton, ExMsgBoxcfg.DefaultIdentifier));
            }

            return ToBeReturned;
        }
    }
}