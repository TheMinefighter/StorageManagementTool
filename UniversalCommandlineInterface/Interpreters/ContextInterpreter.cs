using System;
using System.Linq;
using UniversalCommandlineInterface.Attributes;
using UniversalCommandlineInterface.Interpreters;

namespace UniversalCommandlineInterface {
   public class ContextInterpreter : BaseInterpreter {
      public CmdContextAttribute MyContextAttribute;

      internal ContextInterpreter(CommandlineOptionInterpreter top, int offset = 0) : base(top, offset) {
      }

      internal ContextInterpreter(BaseInterpreter parent, CmdContextAttribute attribute, int offset = 0) : base(parent, attribute.Name,
         offset) {
         MyContextAttribute = attribute;
      }

      internal override void PrintHelp() {
      }

      internal override bool Interpret(bool printErrors = true) {
         string search = TopInterpreter.Args.ElementAt(Offset);
         foreach (CmdContextAttribute cmdContextAttribute in MyContextAttribute.subCtx) {
            if (CommandlineMethods.IsParameterEqual(cmdContextAttribute.Name, search)) {
               ContextInterpreter subInterpreter = new ContextInterpreter(this, cmdContextAttribute, Offset + 1);
               cmdContextAttribute.LoadChilds();
               subInterpreter.Interpret();
               return true;
            }
         }

         foreach (CmdActionAttribute cmdActionAttribute in MyContextAttribute.ctxActions) {
            if (CommandlineMethods.IsParameterEqual(cmdActionAttribute.Name, search)) {
               IncreaseOffset();
               ActionInterpreter actionInterpreter = new ActionInterpreter(cmdActionAttribute, this, Offset);
               actionInterpreter.Interpret();
               return true;
            }
         }

         foreach (CmdParameterAttribute cmdParameterAttribute in MyContextAttribute.ctxParameters) {
         }

         return false;
         //   throw new NotImplementedException();
      }
   }
}