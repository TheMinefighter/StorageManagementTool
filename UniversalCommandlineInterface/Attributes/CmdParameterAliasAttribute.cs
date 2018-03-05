using System;
using System.Collections.Generic;

namespace UniversalCommandlineInterface.Attributes {
   [AttributeUsage(AttributeTargets.GenericParameter | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field,
      AllowMultiple = true)]
   public class CmdParameterAliasAttribute : Attribute {
      public string[] ExtendedHelp;
      public string Help;

      public readonly string Name;

      public readonly object Value;

      public CmdParameterAliasAttribute(string name=null, object value=null, string help = "", string[] extendedHelp = null) {
         Name = name;
         Value = value;
         Help = help;
         ExtendedHelp = extendedHelp ?? new string[]{};
      }
   }
}