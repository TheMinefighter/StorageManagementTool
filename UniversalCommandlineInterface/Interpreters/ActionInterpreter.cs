using System;
using System.Collections.Generic;
using System.Linq;
using UniversalCommandlineInterface.Attributes;

namespace UniversalCommandlineInterface.Interpreters {
   public class ActionInterpreter : BaseInterpreter {
      public CmdActionAttribute MyActionAttribute;
      private IEnumerable<CmdParameterAttribute> parameters;


      public ActionInterpreter(CommandlineOptionInterpreter top, int i) : base(top) {
         i++;
      }

      public ActionInterpreter(CmdActionAttribute myActionAttribute, BaseInterpreter parent, int offset = 0) : base(parent,
         myActionAttribute.Name, offset) {
         MyActionAttribute = myActionAttribute;
      }

      internal override void PrintHelp() {
      }

      internal override bool Interpret(bool printErrors=true) {
         Dictionary<CmdParameterAttribute, object> invokationArguments = new Dictionary<CmdParameterAttribute, object>();
         int currentOffset = Offset;
         string search = TopInterpreter.args.ElementAt(currentOffset);
         if (GetValue(search, invokationArguments, out object value)) {
            return true;
         }

         MyActionAttribute.MyInfo.Invoke(null, new[] {value});
         throw new NotImplementedException();
      }

      private bool GetValue(string search, IDictionary<CmdParameterAttribute, object> invokationArguments, out object value) {
         value = null;
         foreach (CmdParameterAttribute cmdParameterAttribute in parameters) {
            if (cmdParameterAttribute.AvailableWithoutAlias && CommandlineMethods.IsParameterEqual(cmdParameterAttribute.Name, search)) {
               if (!CommandlineMethods.GetAliasValue(out value, cmdParameterAttribute, TopInterpreter.args.ElementAt(Offset + 1))) {
                  if (!CommandlineMethods.GetValueFromString(TopInterpreter.args.ElementAt(Offset + 1), cmdParameterAttribute.GetType(),
                     out value)) {
                     PrintHelp();
                     return true;
                  }

                  invokationArguments.Add(cmdParameterAttribute, value);
                  break;
               }
               else {
                  invokationArguments.Add(cmdParameterAttribute, value);
                  break;
               }
            }

            if (!cmdParameterAttribute.DeclerationNeeded) {
               if (!CommandlineMethods.GetAliasValue(out value, cmdParameterAttribute, search)) {
                  PrintHelp();
                  return true;
               }
               else {
                  invokationArguments.Add(cmdParameterAttribute, value);
                  break;
               }
            }

            //   invokationArguments.Add();
         }

         return false;
      }
   }
}