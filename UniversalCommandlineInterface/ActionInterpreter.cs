using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniversalCommandlineInterface.Attributes;

namespace UniversalCommandlineInterface
{
   public class ActionInterpreter : BaseInterpreter
   {
      private CmdActionAttribute action;
      private MethodInfo method;
      private Dictionary<CmdParameterAttribute, ParameterInfo> parameters;


      public ActionInterpreter(CommandlineOptionInterpreter top, int i) : base(top)
      {
         i++;
      }

      internal override void PrintHelp()
      {
         action.
      }

      internal override void Interpret()
      {
         int currentOffset = Offset;
         string search = TopInterpreter.args.ElementAt(currentOffset);
         foreach (CmdParameterAttribute cmdParameterAttribute in parameters.Keys)
         {
            if (cmdParameterAttribute.AvailableWithoutAlias && IsParameterEqual(cmdParameterAttribute.Name, search))
            {
               foreach (var VARIABLE in COLLECTION)
               {
               }
            }

            if (!cmdParameterAttribute.DeclerationNeeded)
            {
               bool success = GetAliasValue(out object value, cmdParameterAttribute, search);

               if (!success)
               {
                  PrintHelp();
                  return;
               }

               method.Invoke(null, new[] {value});
            }
         }

         throw new System.NotImplementedException();
      }

      private static bool GetAliasValue(out object value, CmdParameterAttribute cmdParameterAttribute, string search)
      {
         value = null;
         bool success = false;
         foreach (CommandlineParameterAlias commandlineParameterAlias in cmdParameterAttribute.ParameterAlias)
         {
            if (IsParameterEqual(commandlineParameterAlias.Name, search))
            {
               success = true;
               value = commandlineParameterAlias.value;
               break;
               // use Value
            }
         }

         return success;
      }

      internal static bool IsParameterEqual(string expected, string given)
      {
         return '/' + expected == given || '-' + expected == given;
      }
   }
}