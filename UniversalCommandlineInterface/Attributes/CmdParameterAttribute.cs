using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalCommandlineInterface.Attributes
{
   [AttributeUsage(AttributeTargets.GenericParameter| AttributeTargets.Parameter )]
   public class CmdParameterAttribute : Attribute
   { 
      public string Name; 
      public bool AvailableWithoutAlias; 
      public IEnumerable<CommandlineParameterAlias> ParameterAlias; 
      public bool DeclerationNeeded;

      public CmdParameterAttribute(string name, bool availableWithoutAlias, IEnumerable<CommandlineParameterAlias> parameterAlias, bool declerationNeeded)
      {
         Name = name;
         AvailableWithoutAlias = availableWithoutAlias;
         ParameterAlias = parameterAlias;
         DeclerationNeeded = declerationNeeded;
      }
      public CmdParameterAttribute( string name, bool availableWithoutAlias,string[] parameterAliasDic, bool declerationNeeded)
      {
         Name = name;
         AvailableWithoutAlias = availableWithoutAlias;
     //    ParameterAlias = parameterAliasDic.Select(x => new CommandlineParameterAlias(x.Key, x.Value));
         
         DeclerationNeeded = declerationNeeded;
      }
   }
}