namespace StorageManagementTool
{
   /// <summary>
   /// Class for storing a windows registry path
   /// </summary>
    public class RegPath
   {
      public RegPath(string registryKey, string valueName) {
         this.RegistryKey = registryKey;
         this.ValueName = valueName; }

      public RegPath()
      {
         
      }
      /// <summary>
      /// The where the value is stored
      /// </summary>
      public string RegistryKey { get; set; }
      /// <summary>
      /// The name of the value
      /// </summary>
      public string ValueName { get; set; }
   }
}
