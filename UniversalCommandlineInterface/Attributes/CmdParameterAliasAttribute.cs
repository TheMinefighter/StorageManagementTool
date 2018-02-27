using System;
using System.Collections.Generic;

namespace UniversalCommandlineInterface.Attributes
{
   [AttributeUsage(AttributeTargets.GenericParameter|AttributeTargets.Parameter,AllowMultiple = true )]
   public class CmdParameterAliasAttribute : Attribute
   {
      public CmdParameterAliasAttribute(string name, object value, string help="", string[] extendedHelp=null)
      {
         Name = name;
         this.Value = value;
         this.Help = help;
         this.ExtendedHelp = extendedHelp as IEnumerable<string> ??new List<string>();
      }

      public string Name;

      public object Value;
      public string Help;
      public IEnumerable<string> ExtendedHelp;
   }
}