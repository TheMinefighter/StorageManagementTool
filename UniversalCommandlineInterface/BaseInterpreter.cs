using System.Collections.Generic;
using System.Linq;

namespace UniversalCommandlineInterface
{
   public class BaseInterpreter
   {
      private BaseInterpreter()
      {
         
      }

      protected  BaseInterpreter(CommandlineOptionInterpreter top,int offset=0)
      {
         Offset = offset;
         ParentInterpreters= new List<BaseInterpreter>{this};
         TopInterpreter=top;
         DirectParent = null;
      }


      protected BaseInterpreter(BaseInterpreter parent, int offset=0)
      {
         TopInterpreter = parent.TopInterpreter;
         DirectParent = parent;
         List<BaseInterpreter> parentInterpreters = parent.ParentInterpreters.ToList();
         parentInterpreters.Add(parent);
            ParentInterpreters = parentInterpreters;
         Offset = offset;
      }
internal void PrintHelp() {}
      
      public int Offset { get; }
     public CommandlineOptionInterpreter TopInterpreter { get; }
      public BaseInterpreter DirectParent { get; }
      public List<BaseInterpreter> ParentInterpreters { get; }
   }
}