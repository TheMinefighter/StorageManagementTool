using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using StorageManagementTool;

namespace StorageManagementToolLauncher
{
   /// <summary>
   ///    Launcher class
   /// </summary>
   internal class Program
   {
      /// <summary>
      ///    Entry point of the launcher
      /// </summary>
      /// <param name="args"></param>
      [STAThread]
      [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
      private static void Main(string[] args)
      {
         IntPtr handle = GetConsoleWindow();
         StorageManagementTool.Program.ConsoleIOObject = new ConsoleIO
         {
            WriteLineToConsole = Console.WriteLine,
            WriteToConsole = Console.Write,
            ReadFromConsole = Console.ReadLine,
            SetVisibiltyToConsole = x => ShowWindow(handle, x ? SW_SHOW : SW_HIDE)
         };
         StorageManagementTool.Program.Main(args);
      }

      #region From https://stackoverflow.com/a/3571628/6730162 access on 08.01.2018

      [DllImport("kernel32.dll")]
      private static extern IntPtr GetConsoleWindow();

      [DllImport("user32.dll")]
      private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

      private const int SW_HIDE = 0;
      private const int SW_SHOW = 5;

      #endregion
   }
}