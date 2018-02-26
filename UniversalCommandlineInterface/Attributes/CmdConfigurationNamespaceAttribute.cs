using System;

namespace UniversalCommandlineInterface.Attributes
{
   
   [AttributeUsage(AttributeTargets.Class)]
   public class CmdConfigurationNamespaceAttribute : Attribute
   {
      public bool IsReadonly;
      public string Name;
      public string Help;
      public string ExtendedHelp;
   }
}