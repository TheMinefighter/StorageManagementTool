namespace UniversalCommandlineInterface
{
   public class ManagedConfigurationInterpreter : BaseInterpreter
   {
      private CommandlineOptionInterpreter parent;
      public void Interpret(){}

      protected ManagedConfigurationInterpreter(CommandlineOptionInterpreter top, int offset = 0) : base(top, offset)
      {
      }

      protected ManagedConfigurationInterpreter(BaseInterpreter parent, int offset = 0) : base(parent, offset)
      {
      }

      internal override void PrintHelp()
      {
         throw new System.NotImplementedException();
      }
   }
}