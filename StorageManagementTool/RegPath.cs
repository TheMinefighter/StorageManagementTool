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
      public string RegistryKey { get; set; }

      /// <summary>
      ///    The name of the value
      /// </summary>
      public string ValueName { get; set; }

      public RegPath(string registryKey, string valueName)
      {
         RegistryKey = registryKey;
         ValueName = valueName;
      }

      //public RegPath()
      //{
      //}
   }
}