using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes
{
   [AttributeUsage(AttributeTargets.GenericParameter| AttributeTargets.Parameter )]
   public class CmdParameterAttribute : Attribute
   {
      public ParameterInfo myInfo;
      public string Name; 
      public bool AvailableWithoutAlias; 
      public IEnumerable<CmdParameterAliasAttribute> ParameterAlias; 
      public bool DeclerationNeeded;


      public CmdParameterAttribute( string name, bool availableWithoutAlias, bool declerationNeeded)
      {
         Name = name;
         AvailableWithoutAlias = availableWithoutAlias;
         DeclerationNeeded = declerationNeeded;
      }

      public void LoadAlias()
      {
         ParameterAlias = myInfo.GetCustomAttributes(typeof(CmdParameterAliasAttribute), false).Cast<CmdParameterAliasAttribute>();
      }
   }
}