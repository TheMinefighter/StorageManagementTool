using System.Collections.Generic;

namespace UniversalCommandlineInterface {
   public class CommandlineOptionInterpreter {
      public IEnumerable<string> args;
      public ConsoleIO ConsoleIO;

      public InterpretingOptions Options;
   }
}