using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes {
   [AttributeUsage(AttributeTargets.Method)]
   public class CmdActionAttribute : Attribute {
      private bool _loaded;
      public MethodInfo MyInfo;
      public string Name;

      public CmdActionAttribute(string name) {
         Name = name;
      }

      public void LoadParametersAndAlias() {
         foreach (ParameterInfo parameterInfo in MyInfo.GetParameters()) {
            IEnumerable<CmdParameterAttribute> parmeterAttribs =
               parameterInfo.GetCustomAttributes(typeof(CmdParameterAttribute), false).Cast<CmdParameterAttribute>();
            if (parmeterAttribs.Any()) {
               CmdParameterAttribute cmdParameterAttribute = parmeterAttribs.ElementAt(0);
               cmdParameterAttribute.MyInfo = parameterInfo;
               cmdParameterAttribute.LoadAlias();
            }
         }
      }
   }
}