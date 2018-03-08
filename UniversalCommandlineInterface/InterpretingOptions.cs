
namespace UniversalCommandlineInterface {
   public class InterpretingOptions {
      public static InterpretingOptions DefaultOptions = new InterpretingOptions() {
         IgnoreParameterCase = true,
         PreferredArgumentPrefix = '/'
      };
      public char PreferredArgumentPrefix='/';
      public bool IgnoreParameterCase=true;
      public string InteractiveOption = "Interactive";
      public string RootName=".";
   }
}