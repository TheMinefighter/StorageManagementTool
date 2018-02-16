using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using static StorageManagementTool.GlobalizationRessources.OperatingMethodsStrings;

namespace StorageManagementTool
{
   public static partial class OperatingMethods
   {
      public static class SwapfileMethods
      {
         public enum SwapfileState
         {
            /// <summary>
            /// Active when swapfile is in its default state
            /// </summary>
            Standard,

            /// <summary>
            /// Active when swapfile is disabled
            /// </summary>
            Disabled,

            /// <summary>
            /// Active when swapfile were moved
            /// </summary>
            Moved
         }

         private static readonly string defaultSwapfileLocation = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys");
         private static readonly RegPath SwapfileControl = new RegPath(
            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
            "SwapFileControl");

         public static bool ChangeSwapfileStadium(SwapfileState currentState, bool forward, DriveInfo newLocation=null)
         {
           
            switch (currentState)
            {
               case SwapfileState.Standard when forward:
                  return Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 1, RegistryValueKind.DWord);
                  //Restart
               case SwapfileState.Standard when !forward:
                  MessageBox.Show(Error, SetStadium_ErrorNoneBeforeFirst, MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                  return false;
               case SwapfileState.Disabled when forward:
                  string SSDPath = defaultSwapfileLocation.Remove(1, 1);

                  if (newLocation==null)
                  {
                     if (MessageBox.Show(
                            Error, SetStadium_NoNewPathGiven,
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                     {
                       return ChangeSwapfileStadium(currentState, forward,newLocation);
                     }
                     else
                     {
                        return false;
                     }
                  }
                  FileInfo saveAs =new FileInfo(Path.Combine(newLocation.RootDirectory.FullName,"Swapfile.sys"));
                  if (!saveAs.Exists)
                  {
                     if (MessageBox.Show(
                            Error, SetStadium_NewInvalid,
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                     {
                       return ChangeSwapfileStadium(currentState,forward,newLocation);
                     }
                     else
                     {
                        return false;
                     }
                  }


                  string oldPath = defaultSwapfileLocation;
                  MoveFile(new FileInfo(oldPath), saveAs);
                  return Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 0, RegistryValueKind.DWord);
               case SwapfileState.Disabled when !forward:
                  Wrapper.FileAndFolder.DeleteFile(new FileInfo(defaultSwapfileLocation),false);
                  Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 1, RegistryValueKind.DWord);
                  break;
               case SwapfileState.Moved when forward:
                  MessageBox.Show(Error, SetStadium_ErrorNoneAfterLast, MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                  return false;
               case SwapfileState.Moved when !forward:
                  Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 0, RegistryValueKind.DWord);
                  break;
               default:
                  throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
            }

            return true;
         }

         //Legacy - Don't touch
         public static bool ChangeSwapfileStadium(int currentStadium, bool fwd)
         {
            if (currentStadium == 1 && !fwd)
            {
               MessageBox.Show(Error, SetStadium_ErrorNoneBeforeFirst, MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
               return false;
            }

            if (currentStadium == 4 && fwd)
            {
               MessageBox.Show(Error, SetStadium_ErrorNoneAfterLast, MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
               return false;
            }

            if (fwd)
            {
               switch (currentStadium)
               {
                  case 1:
                     try
                     {
                        Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 1, RegistryValueKind.DWord);
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
                               Error, SetStadium_NoNewPathGiven,
                               MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                        {
                           ChangeSwapfileStadium(currentStadium, fwd);
                        }
                        else
                        {
                           return false;
                        }
                     }

                     if (!new DirectoryInfo(Session.Singleton.CurrentConfiguration.DefaultHDDPath).Exists)
                     {
                        if (MessageBox.Show(
                               Error, "Es wurde ein ungültiger Pfad für den NewPath Speicherort eingegeben",
                               MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                        {
                           ChangeSwapfileStadium(currentStadium, fwd);
                        }
                        else
                        {
                           return false;
                        }
                     }

                     string newPath = Path.Combine(Session.Singleton.CurrentConfiguration.DefaultHDDPath,
                        HDDPath);
                     string oldPath = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys");
                     MoveFile(new FileInfo(oldPath), new FileInfo(newPath));
                     break;
                  case 3:
                     return Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 0, RegistryValueKind.DWord);
               }

               return true;
            }

            switch (currentStadium)
            {
               //Wrap
               case 2:
                  Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 1, RegistryValueKind.DWord);
                  break;
               case 3:
                  File.Delete(Path.Combine(Session.Singleton.CurrentConfiguration.DefaultHDDPath,
                     Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys").Remove(1, 1)));
                  break;
               case 4:
                  Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 0, RegistryValueKind.DWord);
                  break;
            }

            Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 1, RegistryValueKind.DWord);

            return false;
         }

         public static SwapfileState GetSwapfileState()
         {
            if (!Wrapper.RegistryMethods.GetRegistryValue(SwapfileControl, out object regValue))
            {
               return SwapfileState.Standard;
            }

            if (
               (uint?) regValue == null || (uint?) regValue == 1)
            {
               if (Wrapper.FileAndFolder.IsPathSymbolic(defaultSwapfileLocation))
               {
                  return SwapfileState.Moved;
               }
               else
               {
                  return SwapfileState.Standard;
               }
            }
            else
            {
               return SwapfileState.Disabled;
            }
         }
      }
   }
}