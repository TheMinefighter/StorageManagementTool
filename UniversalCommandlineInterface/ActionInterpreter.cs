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

            //   invokationArguments.Add();
         }

         method.Invoke(null, new[] {value});
         throw new System.NotImplementedException();
      }

      public static bool GetAliasValue(out object value, CmdParameterAttribute cmdParameterAttribute, string search)
      {
         value = null;
         bool success = false;
         foreach (CmdParameterAliasAttribute commandlineParameterAlias in cmdParameterAttribute.ParameterAlias)
         {
            if (IsParameterEqual(commandlineParameterAlias.Name, search))
            {
               success = true;
               value = commandlineParameterAlias.Value;
               break;
            }
         }

         return success;
      }

      internal static bool GetValueFromString(string source, Type expectedType, out object value)
      {
         value = null;
         switch (Type.GetTypeCode(expectedType))
         {
         
            case TypeCode.SByte:
            {
               bool parsed = sbyte.TryParse(source, out sbyte tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.Byte):
            {
               bool parsed = byte.TryParse(source, out byte tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.Int16):
            {
               bool parsed = short.TryParse(source, out short tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.UInt16):
            {
               bool parsed = ushort.TryParse(source, out ushort tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.Int32):
            {
               bool parsed = int.TryParse(source, out int tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.UInt32):
            {
               bool parsed = uint.TryParse(source, out uint tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.Int64):
            {
               bool parsed = long.TryParse(source, out long tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.UInt64):
            {
               bool parsed = ulong.TryParse(source, out ulong tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.Boolean):
            {
               bool parsed = bool.TryParse(source, out bool tmp);
               value = tmp;
               return parsed;
            }
            case (TypeCode.String):
            {
               value = source;
               return true;
            }
            case TypeCode.Char:
               value = source[0];
               return true;
            case (TypeCode.Object):
            {
               if (expectedType.IsEnum)
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

               return false;
            }
            default: return false;
         }
      }

      internal static bool IsParameterEqual(string expected, string given)
      {
         return '/' + expected == given || '-' + expected == given;
      }
   }
}