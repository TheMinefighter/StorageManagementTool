using System.Collections.Generic;

namespace UniversalCommandlineInterface {
   public class CommandlineParameterAlias {
      public IEnumerable<string> extendedHelp;
      public string help;

      public string Name;

      public object value;

      public CommandlineParameterAlias(string name, object value, string help = "", IEnumerable<string> extendedHelp = null) {
         Name = name;
         this.value = value;
         this.help = help;
         this.extendedHelp = extendedHelp ?? new List<string>();
      }
   }
}