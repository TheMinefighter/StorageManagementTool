using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using static StorageManagementTool.GlobalizationRessources.OperatingMethodsStrings;

namespace StorageManagementTool
{
   public static partial class OperatingMethods
   {
      public static string GetStateDescription(this SwapfileMethods.SwapfileState state)
      {
         switch (state)
         {
            case SwapfileMethods.SwapfileState.Standard:
               return GetDescription_Base + GetDescription_Standard;
               break;
            case SwapfileMethods.SwapfileState.Disabled:
               return GetDescription_Base + GetDescription_Disabled;
               break;
            case SwapfileMethods.SwapfileState.Moved:
               return GetDescription_Base+ string.Format(GetDescription_Moved,SwapfileMethods.getSwapfilePath());
            case SwapfileMethods.SwapfileState.None:
               return GetDescription_Base + GetDescription_None;
               break;
            default:
               throw new ArgumentOutOfRangeException(nameof(state), state, null);
         }
      }
      public static class SwapfileMethods
      {
         /// <summary>
         /// Represents different states the swapfile can be in
         /// </summary>
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
            Moved,
            /// <summary>
            /// Never active, only for technical purposes
            /// </summary>
            None
         }

         internal static FileInfo getSwapfilePath()
         {
            return new FileInfo(Wrapper.FileAndFolder.GetRealPath(DefaultSwapfileLocation));
         }

         private static readonly string DefaultSwapfileLocation = Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys");
         private static readonly RegPath SwapfileControl = new RegPath(
            @"HKEY_LOCAL_MACHINE\SYSTEM\CbnurrentControlSet\Control\Session Manager\Memory Management",
            "SwapFileControl");

         /// <summary>
         /// Changes the Swapfile Stadium
         /// </summary>
         /// <param name="forward"></param>
         /// <param name="currentState">The current state of the swapfile</param>
         /// <param name="newLocation"></param>
         /// <returns></returns>
         public static bool ChangeSwapfileStadium(bool forward, SwapfileState currentState = SwapfileState.None,
            DriveInfo newLocation = null)
         {
            if (currentState == SwapfileState.None)
            {
               currentState = GetSwapfileState();
            }
            switch (currentState)
            {
               case SwapfileState.Standard when forward:
                  return Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 1, RegistryValueKind.DWord);

               case SwapfileState.Standard when !forward:
                  MessageBox.Show(Error, SetStadium_ErrorNoneBeforeFirst, MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                  return false;

               case SwapfileState.Disabled when forward:
                  if (newLocation == null)
                  {
                     if (MessageBox.Show(
                            Error, SetStadium_NoNewPathGiven,
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                     {
                        return ChangeSwapfileStadium(forward, currentState, newLocation);
                     }
                     else
                     {
                        return false;
                     }
                  }
                  FileInfo saveAs = new FileInfo(Path.Combine(newLocation.RootDirectory.FullName, "Swapfile.sys"));
                  if (!saveAs.Exists)
                  {
                     if (MessageBox.Show(
                            Error, SetStadium_NewInvalid,
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                     {
                        return ChangeSwapfileStadium(forward, currentState, newLocation);
                     }
                     else
                     {
                        return false;
                     }
                  }

                  string oldPath = DefaultSwapfileLocation;
                  MoveFile(new FileInfo(oldPath), saveAs);
                  return Wrapper.RegistryMethods.SetRegistryValue(SwapfileControl, 0, RegistryValueKind.DWord);

               case SwapfileState.Disabled when !forward:
                  Wrapper.FileAndFolder.DeleteFile(new FileInfo(DefaultSwapfileLocation), false);
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

         public static SwapfileState GetSwapfileState()
         {
            if (!Wrapper.RegistryMethods.GetRegistryValue(SwapfileControl, out object regValue))
            {
               return SwapfileState.Standard;
            }

            if ((uint?)regValue == null || (uint?)regValue == 1)
            {
               if (Wrapper.FileAndFolder.IsPathSymbolic(DefaultSwapfileLocation))
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