using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes {
   [AttributeUsage(AttributeTargets.GenericParameter | AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
   public class CmdParameterAttribute : Attribute {
      private bool _loaded;
      public bool AvailableWithoutAlias;
      public bool DeclerationNeeded;
      public bool IsParameter;
      public ICustomAttributeProvider MyInfo;
      public string Name;
      public IEnumerable<CmdParameterAliasAttribute> ParameterAliases;


      public CmdParameterAttribute(string name, bool availableWithoutAlias, bool declerationNeeded) {
         Name = name;
         AvailableWithoutAlias = availableWithoutAlias;
         DeclerationNeeded = declerationNeeded;
      }

      public void LoadAlias() {
         if (!_loaded) {
            ParameterAliases = MyInfo.GetCustomAttributes(typeof(CmdParameterAliasAttribute), false).Cast<CmdParameterAliasAttribute>();
         }
      }
   }
}