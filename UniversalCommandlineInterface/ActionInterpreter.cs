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
         object value = null;
         Dictionary<CmdParameterAttribute, object> invokationArguments = new Dictionary<CmdParameterAttribute, object>();
         foreach (CmdParameterAttribute cmdParameterAttribute in parameters.Keys)
         {
            if (cmdParameterAttribute.AvailableWithoutAlias && IsParameterEqual(cmdParameterAttribute.Name, search))
            {
               if (!GetAliasValue(out value, cmdParameterAttribute, TopInterpreter.args.ElementAt(Offset + 1)))
               {
                  if (!GetValueFromString(TopInterpreter.args.ElementAt(Offset + 1), cmdParameterAttribute.GetType(), out value))
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
               bool success = GetAliasValue(out value, cmdParameterAttribute, search);

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
         }

         method.Invoke(null, new[] {value});
         throw new System.NotImplementedException();
      }

      public static bool GetAliasValue(out object value, CmdParameterAttribute cmdParameterAttribute, string search)
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
               }
         }

         return success;
      }

      internal static bool GetValueFromString(string source, Type expectedType, out object value)
      {
         value = null;
         if (expectedType == typeof(sbyte))
         {
            bool parsed = sbyte.TryParse(source, out sbyte tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(byte))
         {
            bool parsed = byte.TryParse(source, out byte tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(short))
         {
            bool parsed = short.TryParse(source, out short tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(ushort))
         {
            bool parsed = ushort.TryParse(source, out ushort tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(int))
         {
            bool parsed = int.TryParse(source, out int tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(uint))
         {
            bool parsed = uint.TryParse(source, out uint tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(long))
         {
            bool parsed = long.TryParse(source, out long tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(ulong))
         {
            bool parsed = ulong.TryParse(source, out ulong tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(bool))
         {
            bool parsed = bool.TryParse(source, out bool tmp);
            value = tmp;
            return parsed;
         }
         else if (expectedType == typeof(string))
         {
            value = source;
            return true;
         }
         else if (expectedType == typeof(object))
         {
            return false;
         }
         else if (expectedType.IsEnum)
         {
            bool parseable = Enum.IsDefined(expectedType, source);
            if (parseable)
            {
               value = Enum.Parse(expectedType, source);
            }

            return parseable;
         }
         else if (source.StartsWith("{") && source.EndsWith("}"))
         {
            try
            {
               JsonConvert.DeserializeObject(source, expectedType);
            }
            catch (Exception)
            {
               return false;
            }

            return true;
         }
         else
         {
            return false;
         }
      }

      internal static bool IsParameterEqual(string expected, string given)
      {
         return '/' + expected == given || '-' + expected == given;
      }
   }
}