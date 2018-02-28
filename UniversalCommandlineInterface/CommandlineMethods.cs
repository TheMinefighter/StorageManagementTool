using System;
using System.Reflection;
using Newtonsoft.Json;
using UniversalCommandlineInterface.Attributes;

namespace UniversalCommandlineInterface
{
   public static class CommandlineMethods
   {
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

      public static bool GetValueFromString(string source, Type expectedType, out object value)
      {
         value = null;
         switch (Type.GetTypeCode(expectedType))
         {
            case TypeCode.SByte:
            { 
               bool parsed = SByte.TryParse(source, out sbyte tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.Byte:
            {
               bool parsed = Byte.TryParse(source, out byte tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.Int16:
            {
               bool parsed = Int16.TryParse(source, out short tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.UInt16:
            {
               bool parsed = UInt16.TryParse(source, out ushort tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.Int32:
            {
               bool parsed = Int32.TryParse(source, out int tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.UInt32:
            {
               bool parsed = UInt32.TryParse(source, out uint tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.Int64:
            {
               bool parsed = Int64.TryParse(source, out long tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.UInt64:
            {
               bool parsed = UInt64.TryParse(source, out ulong tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.Boolean:
            {
               bool parsed = Boolean.TryParse(source, out bool tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.Single:
            {
               bool parsed = Single.TryParse(source, out float tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.Double:
            {
               bool parsed = Double.TryParse(source, out double tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.Decimal:
            {
               bool parsed = Decimal.TryParse(source, out decimal tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.DateTime:
            {
               bool parsed = DateTime.TryParse(source, out DateTime tmp);
               value = tmp;
               return parsed;
            }
            case TypeCode.String:
            {
               value = source;
               return true;
            }
            case TypeCode.Char:
               value = source[0];
               return true;

            case TypeCode.Object:
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
            case TypeCode.Empty:
            case TypeCode.DBNull:
            default: return false;
         }

         value = null;
      }

      internal static bool IsParameterEqual(string expected, string given)
      {
         return '/' + expected == given || '-' + expected == given;
      }

      public static TypeInfo getTypeInfo(MemberInfo member)
      {
         switch (member)
         {
            case PropertyInfo propertyInfo:
               propertyInfo.PropertyType.GetTypeInfo();
               break;
            case FieldInfo fieldInfo:
               fieldInfo.FieldType.GetTypeInfo();
               break;
         }

         throw new ArgumentOutOfRangeException(nameof(member),member,"Must be  or FieldInfo");
      }
   }
}