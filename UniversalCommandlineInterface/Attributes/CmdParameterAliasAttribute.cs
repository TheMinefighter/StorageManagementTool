using System;

namespace UniversalCommandlineInterface.Attributes
{
   [AttributeUsage(AttributeTargets.GenericParameter|AttributeTargets.Parameter,AllowMultiple = true )]
   public class CmdParameterAliasAttribute : Attribute
   {
      
   }
}