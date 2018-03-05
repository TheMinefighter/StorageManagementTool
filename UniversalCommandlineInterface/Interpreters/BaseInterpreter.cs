using System.Collections.Generic;
using System.Linq;
using UniversalCommandlineInterface.Attributes;

namespace UniversalCommandlineInterface.Interpreters {
   public abstract class BaseInterpreter {
      public string Name { get; }
      public int Offset { get; internal set; }
      public CommandlineOptionInterpreter TopInterpreter { get; }
      public BaseInterpreter DirectParent { get; }
      public List<BaseInterpreter> ParentInterpreters { get; }

      public List<string> Path {
         get {
            if (Name == null) {
               return new List<string>();
            }

            List<string> tmpList = DirectParent.Path;
            tmpList.Add(Name);
            return tmpList;
         }
      }

      private BaseInterpreter() {
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns>Whether the end of the args has been reached</returns>
      public bool IncreaseOffset() {
         Offset++;
         return Offset == TopInterpreter.Args.Length;
      }

      protected BaseInterpreter(CommandlineOptionInterpreter top, int offset = 0) {
         Offset = offset;
         ParentInterpreters = new List<BaseInterpreter> {this};
         TopInterpreter = top;
         DirectParent = null;
      }


      protected BaseInterpreter(BaseInterpreter parent, string name, int offset = 0) {
         TopInterpreter = parent.TopInterpreter;
         DirectParent = parent;
         List<BaseInterpreter> parentInterpreters = parent.ParentInterpreters.ToList();
         parentInterpreters.Add(parent);
         ParentInterpreters = parentInterpreters;
         Offset = offset;
         Name = name;
      }

      internal abstract void PrintHelp();
      internal abstract bool Interpret(bool printErrors = true);


      public void PrintEror(string argName = null) {
         TopInterpreter.ConsoleIO.WriteToConsole(
            $"An error occurred while parsing argument {argName ?? Name} use {TopInterpreter.Options.PreferredArgumentPrefix}? for help");
      }

      internal bool IsParameterDeclaration(out CmdParameterAttribute found,
         IEnumerable<CmdParameterAttribute> possibleParameters, string search) {
         foreach (CmdParameterAttribute cmdParameterAttribute in possibleParameters) {
            if (IsParameterEqual(cmdParameterAttribute.Name, search)) {
               found = cmdParameterAttribute;
               return true;
            }
         }

         found = null;
         return false;
      }

      internal bool IsAlias(CmdParameterAttribute expectedAliasType, out object value, string source=null) {
         foreach (CmdParameterAliasAttribute cmdParameterAliasAttribute in expectedAliasType.ParameterAliases) {
            if (IsParameterEqual(cmdParameterAliasAttribute.Name, source?? TopInterpreter.Args[Offset])) {
               value = cmdParameterAliasAttribute.Value;
               return true;
            }
         }

         value = null;
         return false;
      }

      internal bool IsParameterEqual(string expected, string given) {
         return IsParameterEqual(expected, given, TopInterpreter.Options.CaseSensitiveArgumentChecking);
      }

      internal static bool IsParameterEqual(string expected, string given, bool caseSensitive) {
         if (expected == null) {
            return false;
         }

         if (caseSensitive) {
            given = given.ToLower();
            expected = expected.ToLower();
         }
         return '/' + expected == given || '-' + expected == given;
      }
   }
}