using System;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes {
   [AttributeUsage(AttributeTargets.Method)]
   public class CmdActionAttribute : Attribute {
      private bool _loaded;
      public MethodInfo MyInfo;
      public string Name;

      public CmdActionAttribute(string name) => Name = name;

      public void LoadParametersAndAlias() {
         foreach (ParameterInfo parameterInfo in MyInfo.GetParameters()) {
            CmdParameterAttribute[] parmeterAttribs =
               parameterInfo.GetCustomAttributes(typeof(CmdParameterAttribute), false).Cast<CmdParameterAttribute>().ToArray();
            foreach (CmdParameterAttribute parameterAttribute in parmeterAttribs) {
               parameterAttribute.MyInfo = parameterInfo;
               parameterAttribute.LoadAlias();
            }
         }
      }
   }
}