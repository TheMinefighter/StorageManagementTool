using System.Collections.Generic;
using System.Linq;

namespace UniversalCommandlineInterface.Interpreters
{
   public abstract class BaseInterpreter
   {
      public string Name { get; }
      public int Offset { get; }
      public CommandlineOptionInterpreter TopInterpreter { get; }
      public BaseInterpreter DirectParent { get; }
      public List<BaseInterpreter> ParentInterpreters { get; }

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


      protected BaseInterpreter(BaseInterpreter parent, string name, int offset = 0)
      {
         TopInterpreter = parent.TopInterpreter;
         DirectParent = parent;
         List<BaseInterpreter> parentInterpreters = parent.ParentInterpreters.ToList();
         parentInterpreters.Add(parent);
            ParentInterpreters = parentInterpreters;
         Offset = offset;
         Name = name;
      }

      internal abstract void PrintHelp();
      internal abstract void Interpret();


      public void PrintEror(string argName=null   )
      {
         TopInterpreter.ConsoleIO.WriteToConsole(
            $"An error occurred while parsing argument {argName??Name} use {TopInterpreter.Options.preferredArgumentPrefix}? for help");
      }

      public List<string> Path
      {
         get {
            if (Name==null)
            {
               return new List<string>();
            }

            List<string> tmpList = DirectParent.Path;
               tmpList.Add(Name);
            return tmpList;
         }
      }
   }
}