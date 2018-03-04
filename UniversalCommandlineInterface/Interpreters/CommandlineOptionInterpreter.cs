using System;
using System.Collections.Generic;
using System.Reflection;
using UniversalCommandlineInterface.Attributes;

namespace UniversalCommandlineInterface {
   public class CommandlineOptionInterpreter {
      public string[] Args {
         get => _args;
         set {
            _args = value;
            ArgsLengthMinus1 = value.Length - 1;
         }
      }

      public ConsoleIO ConsoleIO ;
      internal int ArgsLengthMinus1; 
      public InterpretingOptions Options;
      private  string[] _args;

      public CommandlineOptionInterpreter(string[] args, InterpretingOptions options, ConsoleIO consoleIO = null) {
         Args = args;
         ConsoleIO = consoleIO??ConsoleIO.DefaultIO;
         Options = options;
      }

      public void Interpret<T>() {
         Interpret(typeof(T));
      }

      public void Interpret(Type baseContext) {
            ContextInterpreter contextInterpreter = new ContextInterpreter(this) {
               MyContextAttribute =
                  baseContext.GetCustomAttribute(typeof(CmdContextAttribute)) as CmdContextAttribute
            };
            contextInterpreter.MyContextAttribute.LoadChilds();
            contextInterpreter.Interpret();
         }
      }
   }