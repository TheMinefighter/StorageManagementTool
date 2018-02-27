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

      protected ContextInterpreter(BaseInterpreter parent,string name, int offset = 0) : base(parent, name, offset)
      {
      }

      internal override void PrintHelp()
      {}

      internal override void Interpret()
      {
         
         throw new NotImplementedException();
      }
   }
}