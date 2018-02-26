using System;
using System.Collections.Generic;

namespace UniversalCommandlineInterface
{
   public class CommandlineParameterAlias
   {
      public CommandlineParameterAlias(string name, object value, string help, IEnumerable<string> extendedHelp)
      {
         Name = name;
         this.value = value;
         this.help = help;
         this.extendedHelp = extendedHelp;
      }

      public string Name;

      public object value;
      public string help;
      public IEnumerable<string> extendedHelp;
   }
}