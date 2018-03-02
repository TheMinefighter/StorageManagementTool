﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UniversalCommandlineInterface.Attributes;
using UniversalCommandlineInterface.Interpreters;

namespace UniversalCommandlineInterface {
   public class ManagedConfigurationInterpreter : BaseInterpreter {
      private Dictionary<CmdConfigurationNamespaceAttribute, MemberInfo> _namespaces;
      private Dictionary<CmdConfigurationValueAttribute, MemberInfo> _values;

      protected ManagedConfigurationInterpreter(CommandlineOptionInterpreter top, int offset = 0) : base(top, offset) {
      }

      protected ManagedConfigurationInterpreter(BaseInterpreter parent, string name, int offset = 0) : base(parent, name, offset) {
      }

      internal override void PrintHelp() {
         int maxlength =
            new int[] {_namespaces.Keys.Select(x => x.Help.Length).Max(), _values.Keys.Select(x => x.Help.Length).Max()}.Max() + 1;
         StringBuilder ConsoleStack = new StringBuilder();
         TopInterpreter.ConsoleIO.WriteLineToConsole($"Syntax: {Path} ");
         foreach (CmdConfigurationNamespaceAttribute cmdConfigurationNamespaceAttribute in _namespaces.Keys) {
            //  TopInterpreter.ConsoleIO.WriteLineToConsole
            ConsoleStack.Append(cmdConfigurationNamespaceAttribute.Name.PadRight(maxlength) +
                                cmdConfigurationNamespaceAttribute.Help);
            ConsoleStack.Append(Environment.NewLine);
         }

         TopInterpreter.ConsoleIO.WriteToConsole(ConsoleStack.ToString());
         throw new NotImplementedException();
      }

      internal override bool Interpret(bool printErrors = true) {
         throw new NotImplementedException();
      }
   }
}