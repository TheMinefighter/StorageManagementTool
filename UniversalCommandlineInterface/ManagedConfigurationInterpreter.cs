using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniversalCommandlineInterface;
using UniversalCommandlineInterface.Attributes;

namespace UniversalCommandlineInterface
{
   public class ManagedConfigurationInterpreter : BaseInterpreter
   {
      private Dictionary<CmdConfigurationNamespaceAttribute,MemberInfo> _namespaces;
      private Dictionary<CmdConfigurationValueAttribute,MemberInfo> _values;
      private 
      protected ManagedConfigurationInterpreter(CommandlineOptionInterpreter top, int offset = 0) : base(top, offset)
      {
      }

      protected ManagedConfigurationInterpreter(BaseInterpreter parent, int offset = 0) : base(parent, TODO, offset)
      {
      }

      internal override void PrintHelp()
      {
         int maxlength = new int[] {_namespaces.Keys.Select(x => x.Help.Length).Max(), _values.Keys.Select(x => x.Help.Length).Max()}.Max()+1;
   
         foreach (CmdConfigurationNamespaceAttribute cmdConfigurationNamespaceAttribute in _namespaces.Keys)
         {
            TopInterpreter.ConsoleIO.WriteLineToConsole($"Syntax: {Path} {}");
            TopInterpreter.ConsoleIO.WriteLineToConsole(cmdConfigurationNamespaceAttribute.Name.PadRight(maxlength)+
                                                             cmdConfigurationNamespaceAttribute.Help);
         }
         throw new System.NotImplementedException();
         
      }

      internal override void Interpret()
      {
         throw new System.NotImplementedException();
      }
   }
}