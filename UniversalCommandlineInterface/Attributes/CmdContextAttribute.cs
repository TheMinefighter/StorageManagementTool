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

      public void LoadChilds()
      {
         IEnumerable<MemberInfo> members = MyInfo.DeclaredFields.Cast<MemberInfo>().Concat(MyInfo.DeclaredProperties);
         foreach (MemberInfo memberInfo in members)
         {
            CmdContextAttribute contextAttribute = memberInfo.GetCustomAttribute<CmdContextAttribute>();
            if (contextAttribute != null)
            {
               contextAttribute.MyInfo =getTypeInfo(memberInfo) ;
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
      }

      public static TypeInfo getTypeInfo(MemberInfo member)
      {
         switch (member)
         {
            case PropertyInfo propertyInfo:
               propertyInfo.PropertyType.GetTypeInfo();
               break;
            case FieldInfo fieldInfo:
               fieldInfo.FieldType.GetTypeInfo();
               break;
         }

         throw new ArgumentOutOfRangeException(nameof(member),member,"Must be  or FieldInfo");
      }
      
      public CmdContextAttribute()
      {
         
      }
   }
}