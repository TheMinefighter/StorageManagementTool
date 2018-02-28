using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UniversalCommandlineInterface.Attributes;

namespace UniversalCommandlineInterface
{
   public class ActionInterpreter : BaseInterpreter
   {
      private CmdActionAttribute action;
      private Dictionary<CmdParameterAttribute, ParameterInfo> parameters;


      public ActionInterpreter(CommandlineOptionInterpreter top, int i) : base(top)
      {
         i++;
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
         foreach (CmdParameterAttribute cmdParameterAttribute in parameters.Keys)
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