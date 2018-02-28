using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes
{
   [AttributeUsage(AttributeTargets.Method)]
   public class CmdActionAttribute : Attribute
   {
      public MethodInfo MyInfo;
      public string Name;
      private bool loaded;
      public void LoadParametersAndAlias()
      {
         foreach (ParameterInfo parameterInfo in MyInfo.GetParameters())
         {
            IEnumerable<CmdParameterAttribute> parmeterAttribs = parameterInfo.GetCustomAttributes(typeof(CmdParameterAttribute), false).Cast<CmdParameterAttribute>();
            if (parmeterAttribs.Any())
            {
               CmdParameterAttribute cmdParameterAttribute = parmeterAttribs.ElementAt(0);
               cmdParameterAttribute.myInfo = parameterInfo;
               cmdParameterAttribute.LoadAlias();
            }
         }
      }
   }
}