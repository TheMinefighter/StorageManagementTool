using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes
{
   [AttributeUsage(AttributeTargets.Class)]
   public class CmdContextAttribute : Attribute
   {
      public string Name;

      public TypeInfo MyInfo;
      public IList<CmdContextAttribute> subCtx=new List<CmdContextAttribute>();
      public IList<CmdContextParameterAttribute> ctxParameters= new List<CmdContextParameterAttribute>();
      public IList<CmdActionAttribute> ctxActions=new List<CmdActionAttribute>();
      private bool loaded;
      public void LoadChilds()
      {

         if (!loaded)
         {
            IEnumerable<MemberInfo> members = MyInfo.DeclaredFields.Cast<MemberInfo>().Concat(MyInfo.DeclaredProperties);
            foreach (MemberInfo memberInfo in members)
            {
               CmdContextAttribute contextAttribute = memberInfo.GetCustomAttribute<CmdContextAttribute>();
               if (contextAttribute != null)
               {
                  contextAttribute.MyInfo = CommandlineMethods.GetTypeInfo(memberInfo) ;
                  subCtx.Add(contextAttribute);
               }

               CmdContextParameterAttribute parameterAttribute = memberInfo.GetCustomAttribute<CmdContextParameterAttribute>();
               if (parameterAttribute!=null)
               {
                  parameterAttribute.MyInfo = memberInfo;
                  ctxParameters.Add(parameterAttribute);
               }
            
            }

            foreach (MethodInfo methodInfo in MyInfo.DeclaredMethods)
            {
               CmdActionAttribute actionAttribute = methodInfo.GetCustomAttribute<CmdActionAttribute>();
               if (actionAttribute!=null)
               {
                  actionAttribute.MyInfo = methodInfo;
               }
            }

            loaded = true;
         }
      }

      public CmdContextAttribute()
      {
         
      }
   }
}