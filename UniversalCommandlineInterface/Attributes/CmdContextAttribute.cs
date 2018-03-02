using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes {
   [AttributeUsage(AttributeTargets.Class)]
   public class CmdContextAttribute : Attribute {
      private bool _loaded;
      public IList<CmdActionAttribute> ctxActions = new List<CmdActionAttribute>();
      public IList<CmdParameterAttribute> ctxParameters = new List<CmdParameterAttribute>();

      public TypeInfo MyInfo;
      public string Name;
      public IList<CmdContextAttribute> subCtx = new List<CmdContextAttribute>();

      public void LoadChilds() {
         if (!_loaded) {
            IEnumerable<MemberInfo> members = MyInfo.DeclaredFields.Cast<MemberInfo>().Concat(MyInfo.DeclaredProperties);
            foreach (MemberInfo memberInfo in members) {
               CmdContextAttribute contextAttribute = memberInfo.GetCustomAttribute<CmdContextAttribute>();
               if (contextAttribute != null) {
                  contextAttribute.MyInfo = CommandlineMethods.GetTypeInfo(memberInfo);
                  subCtx.Add(contextAttribute);
               }

               CmdParameterAttribute parameterAttribute = memberInfo.GetCustomAttribute<CmdParameterAttribute>();
               if (parameterAttribute != null) {
                  parameterAttribute.MyInfo = memberInfo;
                  ctxParameters.Add(parameterAttribute);
               }
            }

            foreach (MethodInfo methodInfo in MyInfo.DeclaredMethods) {
               CmdActionAttribute actionAttribute = methodInfo.GetCustomAttribute<CmdActionAttribute>();
               if (actionAttribute != null) {
                  actionAttribute.MyInfo = methodInfo;
               }
            }

            _loaded = true;
         }
      }
   }
}