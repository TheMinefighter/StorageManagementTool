using Microsoft.Win32;

namespace StorageManagementTool
{
   /// <summary>
   ///    Class for storing a windows registry path
   /// </summary>
   public struct RegPath
   {
      /// <summary>
      ///    The where the value is stored
      /// </summary>
      public string RegistryKey;

      /// <summary>
      ///    The name of the value
      /// </summary>
      public string ValueName;

      public RegPath(string registryKey, string valueName)
      {
         RegistryKey = registryKey;
         ValueName = valueName;
      }

      public override string ToString()
      {
         return RegistryKey + '\\' + ValueName;
      }

//public RegPath()
      //{
      //}
   }
}