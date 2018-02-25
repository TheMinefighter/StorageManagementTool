namespace UniversalCommandlineInterface
{
   public class ActionInterpreter : BaseInterpreter
   {
      public ActionInterpreter(CommandlineOptionInterpreter top,int i) : base(top)
      {
         i++;
      }
internal void PrintHelp() {}
      
   }
}