namespace StorageManagementTool {
   /// <summary>
   ///    Class for storing a windows registry path
   /// </summary>
   public struct RegistryValue {
      /// <summary>
      ///    The where the value is stored
      /// </summary>
      public string RegistryKey;

      /// <summary>
      ///    The name of the value
      /// </summary>
      public string ValueName;

      public RegistryValue(string registryKey, string valueName) {
         RegistryKey = registryKey;
         ValueName = valueName;
      }

      public override string ToString() => RegistryKey + '\\' + ValueName;

//public RegistryValue()
      //{
      //}
   }
}