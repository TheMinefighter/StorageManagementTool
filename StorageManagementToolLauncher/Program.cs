using System;
using System.Security.Permissions;
using System.Windows.Forms;
using UniversalCommandlineInterface;

namespace StorageManagementToolLauncher {
   /// <summary>
   ///    Launcher class
   /// </summary>
   internal class Program {
      /// <summary>
      ///    Entry point of the launcher
      /// </summary>
      /// <param name="args"></param>
      [STAThread]
      [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
      private static void Main(string[] args) {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         IntPtr handle = ConsoleIO.GetConsoleWindow();
         StorageManagementTool.Program.ConsoleIOObject = new ConsoleIO {
            WriteLineToConsole = Console.WriteLine,
            WriteToConsole = Console.Write,
            ReadFromConsole = Console.ReadLine,
            SetVisibiltyToConsole = x => ConsoleIO.ShowWindow(handle, x ? ConsoleIO.SW_SHOW : ConsoleIO.SW_HIDE)
         };

         StorageManagementTool.Program.Main(args);
      }
   }
}