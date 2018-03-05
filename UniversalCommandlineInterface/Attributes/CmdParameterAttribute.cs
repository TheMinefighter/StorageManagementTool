using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes {
   [AttributeUsage(AttributeTargets.GenericParameter | AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
   public class CmdParameterAttribute : Attribute {
      private bool _loaded;
      public bool AvailableWithoutAlias;
      private bool _AvailableWithoutAliasExplictitlyDeclared;
      public bool DeclerationNeeded;
      private bool _DeclerationNeededExplicitlyDeclared;
      public bool IsParameter;
      public ICustomAttributeProvider MyInfo;
      public string Name;
      public IEnumerable<CmdParameterAliasAttribute> ParameterAliases;


      public CmdParameterAttribute(string name, bool? availableWithoutAlias=null, bool? declerationNeeded=null) {
         Name = name;
         AvailableWithoutAlias = availableWithoutAlias??true;
         _AvailableWithoutAliasExplictitlyDeclared = availableWithoutAlias != null;
         DeclerationNeeded = declerationNeeded??true;
         _DeclerationNeededExplicitlyDeclared = declerationNeeded != null;
      }

      public void LoadAlias() {
         if (!_loaded) {
            ParameterAliases = MyInfo.GetCustomAttributes(typeof(CmdParameterAliasAttribute), false).Cast<CmdParameterAliasAttribute>();
            if (!_AvailableWithoutAliasExplictitlyDeclared) {
               AvailableWithoutAlias = !ParameterAliases.Any();
            }

            if (!_DeclerationNeededExplicitlyDeclared) {
               DeclerationNeeded = !ParameterAliases.Any();
            }
            _loaded = true;
         }
      }

      enum CmdParameterUsage {
         RawValueWithDecleration,NoRawsButDecleration,OnlyDirectAlias
      }
   }
}