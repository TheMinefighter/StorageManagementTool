using System;
using System.Collections.Generic;

namespace UniversalCommandlineInterface.Attributes
{
   [AttributeUsage(AttributeTargets.GenericParameter)]
   public class CmdParameterAttribute : Attribute
   {
      public string Name; 
      public bool AvailableWithoutAlias; 
      public IEnumerable<CommandlineParameterAlias> ParameterAlias; 
      public bool DeclerationNeeded;
      
   }
}