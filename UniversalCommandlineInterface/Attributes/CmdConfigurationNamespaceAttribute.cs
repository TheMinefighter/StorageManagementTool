using System;
using System.Collections.Generic;

namespace UniversalCommandlineInterface.Attributes
{
   
   [AttributeUsage(AttributeTargets.Class)]
   public class CmdConfigurationNamespaceAttribute : Attribute
   {
      public bool IsReadonly;
      public string Name;
      public string Help;
      public string ExtendedHelp;

      public CmdConfigurationNamespaceAttribute([CmdParameter("gg",  false,parameterAliasDic: new []{"fgf","fgfg"}, declerationNeeded: false)] bool isReadonly, string name, string help, string extendedHelp)
      {
            IsReadonly = isReadonly;
         Name = name;
         Help = help;
         ExtendedHelp = extendedHelp;
      }
   }
}