using System;
using System.Collections.Generic;
using System.Linq;
using UniversalCommandlineInterface.Attributes;

namespace UniversalCommandlineInterface.Interpreters
{
   public class ActionInterpreter : BaseInterpreter
   {
      public CmdActionAttribute action;
      private IEnumerable<CmdParameterAttribute> parameters;


      public ActionInterpreter(CommandlineOptionInterpreter top, int i) : base(top)
      {
         i++;
      }

      public ActionInterpreter(CmdActionAttribute action, BaseInterpreter parent, int offset = 0) : base(parent, action.Name, offset)
      {
         this.action = action;
         
      }

      internal override void PrintHelp()
      {
      }

      internal override void Interpret()
      {
         int currentOffset = Offset;
         string search = TopInterpreter.args.ElementAt(currentOffset);
         object value = null;
         Dictionary<CmdParameterAttribute, object> invokationArguments = new Dictionary<CmdParameterAttribute, object>();
         foreach (CmdParameterAttribute cmdParameterAttribute in parameters)
         {
            if (cmdParameterAttribute.AvailableWithoutAlias && CommandlineMethods.IsParameterEqual(cmdParameterAttribute.Name, search))
            {
               if (!CommandlineMethods.GetAliasValue(out value, cmdParameterAttribute, TopInterpreter.args.ElementAt(Offset + 1)))
               {
                  if (!CommandlineMethods.GetValueFromString(TopInterpreter.args.ElementAt(Offset + 1), cmdParameterAttribute.GetType(), out value))
                  {
                     PrintHelp();
                     return;
                  }

                  ;
               }
               else
               {
                  PrintHelp();
                  return;
               }
            }

            if (!cmdParameterAttribute.DeclerationNeeded)
            {
               bool success = CommandlineMethods.GetAliasValue(out value, cmdParameterAttribute, search);

               if (!success)
               {
                  PrintHelp();
                  return;
               }
               else
               {
                  invokationArguments.Add(cmdParameterAttribute, value);
                  break;
               }
            }

            //   invokationArguments.Add();
         }

        action.MyInfo.Invoke(null, new[] {value});
         throw new NotImplementedException();
      }
   }
}