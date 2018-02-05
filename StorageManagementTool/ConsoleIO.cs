using System;

namespace StorageManagementTool
{
   /// <summary>
   ///    Class storing the Actions for Console Operations
   /// </summary>
   public class ConsoleIO
   {
      /// <summary>
      ///    Writes a message to Console
      /// </summary>
      public Action<string> WriteToConsole { get; set; }

      /// <summary>
      ///    Reads a line from Console
      /// </summary>
      public Func<string> ReadFromConsole { get; set; }

      /// <summary>
      ///    Writes a message to Console and a linebreak afterwards
      /// </summary>
      public Action<string> WriteLineToConsole { get; set; }

      /// <summary>
      ///    Sets the visibility of the Console Window
      /// </summary>
      public Action<bool> SetVisibiltyToConsole { get; set; }

      /// <summary>
      ///    Writes a message to Console and a linebreak afterwards
      /// </summary>
      /// <param name="message">The message to write to console</param>
      public static void WriteLine(string message)
      {
         Program.ConsoleIOObject.WriteLineToConsole(message);
      }

      /// <summary>
      ///    Reads a line from Console
      /// </summary>
      /// <returns>The line the user entered</returns>
      public static string ReadLine()
      {
         return Program.ConsoleIOObject.ReadFromConsole();
      }

      /// <summary>
      ///    Writes a message to Console
      /// </summary>
      /// <param name="message">The message to write to Console</param>
      public static void Write(string message)
      {
         Program.ConsoleIOObject.WriteToConsole(message);
      }

      /// <summary>
      ///    Sets the visibility of the Console Window
      /// </summary>
      /// <param name="visible">Whether the console window should be visible</param>
      public static void SetVisibility(bool visible)
      {
         Program.ConsoleIOObject.SetVisibiltyToConsole(visible);
      }
   }
}