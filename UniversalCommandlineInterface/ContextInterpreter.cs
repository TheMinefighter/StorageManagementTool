using System;

namespace UniversalCommandlineInterface
{
   public class ContextInterpreter : BaseInterpreter
   {
      public Type Type { get; set; }
      public object O { get; set; }

      protected ContextInterpreter(CommandlineOptionInterpreter top, int offset = 0) : base(top, offset)
      {
      }

      protected ContextInterpreter(BaseInterpreter parent, int offset = 0) : base(parent, offset)
      {
      }
   }
}