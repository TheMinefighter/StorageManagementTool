using System;

namespace UniversalCommandlineInterface.Attributes
{[AttributeUsage(AttributeTargets.Parameter)]
   public class CmdConfigurationValueAttribute : Attribute
   {
      public bool IsReadonly;
      public string Help;
      public string ExtendedHelp;

      public CmdConfigurationValueAttribute(string help=null, string extendedHelp = null, bool isReadonly=false)
      {
         IsReadonly = isReadonly;
         Help = help;
         ExtendedHelp = extendedHelp;
      }
   }
}