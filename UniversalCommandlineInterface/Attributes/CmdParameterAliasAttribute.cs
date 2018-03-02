using System;
using System.Collections.Generic;

namespace UniversalCommandlineInterface.Attributes {
   [AttributeUsage(AttributeTargets.GenericParameter | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field,
      AllowMultiple = true)]
   public class CmdParameterAliasAttribute : Attribute {
      public IEnumerable<string> ExtendedHelp;
      public string Help;

      public string Name;

      public object Value;

      public CmdParameterAliasAttribute(string name, object value, string help = "", string[] extendedHelp = null) {
         Name = name;
         Value = value;
         Help = help;
         ExtendedHelp = extendedHelp as IEnumerable<string> ?? new List<string>();
      }
   }
}