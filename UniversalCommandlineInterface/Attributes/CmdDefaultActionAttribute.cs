using System;
using System.Net.Mail;

namespace UniversalCommandlineInterface.Attributes {
   [AttributeUsage(AttributeTargets.Method|AttributeTargets.Field|AttributeTargets.Parameter)]
   public class CmdDefaultActionAttribute :Attribute {
      private bool IsDirect;
      
   }
}