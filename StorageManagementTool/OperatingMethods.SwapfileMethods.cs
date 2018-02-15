using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using StorageManagementTool.GlobalizationRessources;

namespace StorageManagementTool
{
   public static partial class OperatingMethods
   {
      public static class SwapfileMethods{
         public enum SwapfileState
         {
            /// <summary>
            /// Active when swapfile is in its default state
            /// </summary>
            Start,
            /// <summary>
            /// Active when swapfile is disabled
            /// </summary>
            Disabled,
            /// <summary>
            /// Active when swapfile were moved
            /// </summary>
            Moved
         }
         public static bool ChangeSwapfileStadium(int currentStadium, bool fwd)
         {
            if (currentStadium == 1 && !fwd)
            {
               MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.SetStadium_ErrorNoneBeforeFirst, MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
               return false;
            }

            if (currentStadium == 4 && fwd)
            {
               MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.SetStadium_ErrorNoneAfterLast, MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
               return false;
            }

            if (fwd)
            {
               switch (Session.Singleton.Swapstadium)
               {
                  case 1:
                     try
                     {
                        Registry.SetValue(
                           @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                           "SwapFileControl", 1, RegistryValueKind.DWord);
                     }
                     catch (Exception)
                     {
                        return false;
                     }

                     break;
                  case 2:
                     string HDDPath = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys").Remove(1, 1);
                  
                     if (Session.Singleton.CurrentConfiguration.DefaultHDDPath == "")
                     {
                        if (MessageBox.Show(
                               OperatingMethodsStrings.Error, OperatingMethodsStrings.SetStadium_NoNewPathGiven,
                               MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                        {
                           ChangeSwapfileStadium(currentStadium, fwd);
                        }
                        else
                        {
                           return false;
                        }
                     }

                     try
                     {
                        new DirectoryInfo(Session.Singleton.CurrentConfiguration.DefaultHDDPath);
                     }
                     catch (Exception)
                     {
                        if (MessageBox.Show(
                               "Fehler", "Es wurde ein ungültiger Pfad für den NewPath Speicherort eingegeben",
                               MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                        {
                           ChangeSwapfileStadium(currentStadium, fwd);
                        }
                        else
                        {
                           return false;
                        }
                     }

                     string newPath =Path.Combine( Session.Singleton.CurrentConfiguration.DefaultHDDPath ,
                        HDDPath);
                     string oldPath = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys");
                     MoveFile(new FileInfo(oldPath), new FileInfo(newPath));
                     break;
                  case 3:
                     try
                     {
                        Registry.SetValue(
                           @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                           "SwapFileControl", 0, RegistryValueKind.DWord);
                        break;
                     }
                     catch (Exception)
                     {
                        return false;
                     }
               }

               return true;
            }

            switch (Session.Singleton.Swapstadium)
            {
               case 2:
                  Registry.SetValue(
                     @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                     "SwapFileControl", 1, RegistryValueKind.DWord);
                  break;
               case 3:
                  File.Delete(Path.Combine(Session.Singleton.CurrentConfiguration.DefaultHDDPath,
                     Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys").Remove(1, 1)));
                  break;
               case 4:
                  Registry.SetValue(
                     @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                     "SwapFileControl", 0, RegistryValueKind.DWord);
                  break;
            }

            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
               "SwapFileControl", 1, RegistryValueKind.DWord);

            return false;
         }
      }
   }
}