using System.Collections.Generic;

namespace UniversalCommandlineInterface {
   public class CommandlineOptionInterpreter {
      public string[] Args;
      public ConsoleIO ConsoleIO;
      internal int ArgsLengthMinus1; 
      public InterpretingOptions Options;
   }
}