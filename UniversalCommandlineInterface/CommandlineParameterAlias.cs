﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace UniversalCommandlineInterface
{
   public class CommandlineParameterAlias
   {
      public CommandlineParameterAlias(string name, object value, string help="", IEnumerable<string> extendedHelp=null)
      {
         Name = name;
         this.value = value;
         this.help = help;
         this.extendedHelp = extendedHelp??new List<string>();
      }

      public string Name;

      public object value;
      public string help;
      public IEnumerable<string> extendedHelp;
   }
}