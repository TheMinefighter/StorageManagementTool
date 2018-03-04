using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;
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

      internal override bool Interpret(bool printErrors = true) {
         //Dictionary<CmdParameterAttribute, object> invokationArguments = new Dictionary<CmdParameterAttribute, object>();
         int currentOffset = Offset;
         string search = TopInterpreter.Args.ElementAt(currentOffset);
         if (!GetValues(out Dictionary<CmdParameterAttribute, object> invokationArguments)) {
            return true;
         }

         ParameterInfo[] allParameterInfos = MyActionAttribute.MyInfo.GetParameters();
         object[] invokers = new object[allParameterInfos.Length];
         bool[] invokersDeclared= new bool[allParameterInfos.Length];
         foreach (KeyValuePair<CmdParameterAttribute,object> invokationArgument in invokationArguments) {
            int position = (invokationArgument.Key.MyInfo as ParameterInfo).Position;
            invokers[position] = invokationArgument.Value;
            invokersDeclared[position] = true;
         }
         
         for (int i = 0; i < allParameterInfos.Length; i++) {
            if (!invokersDeclared[i]) {
               if (allParameterInfos[i].HasDefaultValue) {
                  invokers[i] = allParameterInfos[i].DefaultValue;
                  invokersDeclared[i] = true;
               }
               else {
                  //throw
               }
            }
         }
         MyActionAttribute.MyInfo.Invoke(null,invokers);
         throw new NotImplementedException();
      }

      private bool GetValues(out Dictionary<CmdParameterAttribute, object> invokationArguments) {
         invokationArguments= new Dictionary<CmdParameterAttribute, object>();
        // value = null;
         while (Offset!=TopInterpreter.ArgsLengthMinus1) {
            if (IsParameterDeclaration(out CmdParameterAttribute found)) {
               Offset++;

               Type parameterType = (found.MyInfo as ParameterInfo).ParameterType;
               if (IsAlias(found, out object aliasValue)) {
                  invokationArguments.Add(found, aliasValue);
               }
               else if (found.AvailableWithoutAlias && typeof(IEnumerable<>).IsAssignableFrom(parameterType)) {
                  #region From https://stackoverflow.com/a/2493258/6730162 last access 04.03.2018

                  Type realType = parameterType.GetGenericArguments()[0];
                  Type listGenericType = typeof(List<>);
                  Type specificList = listGenericType.MakeGenericType(realType);
                  ConstructorInfo ci = specificList.GetConstructor(new Type[] { });
                  object listOfRealType = ci.Invoke(new object[] { });

                  #endregion

                  MethodInfo addMethodInfo = typeof(List<>).GetMethod("Add");
                  Offset++;
                  while (!((IsAlias(out CmdParameterAttribute tmpParameterAttribute, out object _) &&
                            !tmpParameterAttribute.DeclerationNeeded) ||
                           IsParameterDeclaration(out CmdParameterAttribute _) ||
                           Offset == TopInterpreter.ArgsLengthMinus1)) {
                     if (!CommandlineMethods.GetValueFromString(TopInterpreter.Args[Offset], realType, out object toAppend)) {
                        //throw
                     }
                     else {
                        addMethodInfo.Invoke(listOfRealType, new object[] {toAppend});
                     }
                  }

                  if (new Type[] {
                     specificList, typeof(IList<>).MakeGenericType(realType), typeof(ICollection<>).MakeGenericType(realType),
                     typeof(IEnumerable<>).MakeGenericType(realType), typeof(IReadOnlyList<>).MakeGenericType(realType),
                     typeof(IReadOnlyCollection<>).MakeGenericType(realType), typeof(ReadOnlyCollection<>).MakeGenericType(realType)
                  }.Contains(parameterType)) {
                  }
                  else if (parameterType == realType.MakeArrayType()) {
                     object ArrayOfRealType = typeof(Enumerable).GetMethod("ToArray").Invoke(listOfRealType, new object[] { });
                     invokationArguments.Add(found,ArrayOfRealType);
                  }
               }
               else if (found.AvailableWithoutAlias &&
                        CommandlineMethods.GetValueFromString(TopInterpreter.Args[Offset], parameterType, out object given)) {
                  invokationArguments.Add(found, given);
               }
               else {
                  //throw
               }
            }
            else if (IsAlias(out CmdParameterAttribute aliasType, out object aliasValue) && !aliasType.DeclerationNeeded) {
               invokationArguments.Add(found, aliasValue);
            }
            else {
               //throw
            }
         }

/*
         foreach (CmdParameterAttribute cmdParameterAttribute in parameters) {
            if (cmdParameterAttribute.AvailableWithoutAlias && CommandlineMethods.IsParameterEqual(cmdParameterAttribute.Name, search)) {
               if (!CommandlineMethods.GetAliasValue(out value, cmdParameterAttribute, TopInterpreter.Args.ElementAt(Offset + 1))) {
                  // ReSharper disable once UseMethodIsInstanceOfType
                  // ReSharper disable once UseIsOperator.1
                  Type expectedType = cmdParameterAttribute.GetType();

                  if (typeof(IEnumerable<>).IsAssignableFrom(expectedType)) {
                     int i = 1;
                     Type realtType = expectedType.GetGenericArguments()[0];

                     Type listGenericType = typeof(List<>);

                     Type list = listGenericType.MakeGenericType(realtType);
                     ConstructorInfo ci = list.GetConstructor(new Type[] { });
                     object listInt = ci.Invoke(new object[] { });
                     MethodInfo addMethodInfo = typeof(List<>).GetMethod("Add").MakeGenericMethod(realtType);
                     while (CommandlineMethods.GetValueFromString(TopInterpreter.Args.ElementAt(Offset + i), expectedType,
                        out value)) {
                        i++;
                     }
                  }
                  else {
                     if (!CommandlineMethods.GetValueFromString(TopInterpreter.Args.ElementAt(Offset + 1), expectedType,
                        out value)) {
                        PrintHelp();
                        return true;
                     }

                     invokationArguments.Add(cmdParameterAttribute, value);
                  }

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
*/
         return false;
      }

      internal bool IsParameterDeclaration(out CmdParameterAttribute found, string search = null) {
         return BaseInterpreter.IsParameterDeclaration(out found, parameters, search ?? TopInterpreter.Args[Offset]);
      }

      internal bool IsAlias(CmdParameterAttribute expectedAliasType, out object value, string source = null) {
         return BaseInterpreter.IsAlias(expectedAliasType, out value, source ?? TopInterpreter.Args[Offset]);
      }

      internal bool IsAlias(out CmdParameterAttribute AliasType, out object value, string source = null) {
         foreach (CmdParameterAttribute cmdParameterAttribute in parameters) {
            if (BaseInterpreter.IsAlias(cmdParameterAttribute, out value, source ?? TopInterpreter.Args[Offset])) {
               AliasType = cmdParameterAttribute;
               return true;
            }
         }

         AliasType = null;
         value = null;
         return false;
      }
   }
}