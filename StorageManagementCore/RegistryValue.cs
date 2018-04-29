﻿using System.IO;

namespace StorageManagementTool
{
   /// <summary>
   ///    Class for storing a windows registry path
   /// </summary>
   public struct RegistryValue
   {
      /// <summary>
      ///    The where the value is stored
      /// </summary>
      public string RegistryKey;

      /// <summary>
      ///    The name of the value
      /// </summary>
      public string ValueName;

      public RegistryValue(string registryKey, string valueName)
      {
         RegistryKey = registryKey;
         ValueName = valueName;
      }

      public static implicit operator RegistryValue((string, string) s)
      {
         return new RegistryValue(s.Item1, s.Item2);
      }

      public static implicit operator (string, string)(RegistryValue s)
      {
         return (s.RegistryKey, s.ValueName);
      }

      public override string ToString()
      {
         return RegistryKey + Path.DirectorySeparatorChar + ValueName;
      }
   }
}