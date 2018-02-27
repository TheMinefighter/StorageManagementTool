using System;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes
{
   [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
   public class CmdContextParameterAttribute :Attribute
   {
      public MemberInfo MyInfo; 
      
   }
  
}