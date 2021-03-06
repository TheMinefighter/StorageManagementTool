﻿using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using StorageManagementCore.Backend;
using StorageManagementCore.GlobalizationResources;

namespace StorageManagementCore.Operation {
	public static class Swapfile {
		/// <summary>
		///  Represents different states the swapfile can be in
		/// </summary>
		public enum SwapfileState {
			/// <summary>
			///  Active when swapfile is in its default state
			/// </summary>
			Standard,

			/// <summary>
			///  Active when swapfile is disabled
			/// </summary>
			Disabled,

			/// <summary>
			///  Active when swapfile were moved
			/// </summary>
			Moved,

			/// <summary>
			///  Never active, only for technical purposes
			/// </summary>
			None
		}

		private static readonly string DefaultSwapfileLocation =
			Environment.ExpandEnvironmentVariables(@"%SystemDrive%\Swapfile.sys");

		private static readonly RegistryValue SwapfileControl = new RegistryValue(
			@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
			"SwapFileControl");

		internal static FileInfo getSwapfilePath() => new FileInfo(FileAndFolder.GetRealPath(DefaultSwapfileLocation));

		/// <summary>
		///  Changes the Swapfile Stadium
		/// </summary>
		/// <param name="forward"></param>
		/// <param name="currentState">The current state of the swapfile</param>
		/// <param name="newLocation"></param>
		/// <returns></returns>
		public static bool ChangeSwapfileStadium(bool forward, SwapfileState currentState = SwapfileState.None,
			DriveInfo newLocation = null) {
			if (currentState == SwapfileState.None) {
				currentState = GetSwapfileState();
			}

			FileInfo defaultSwapFileInfo = new FileInfo(DefaultSwapfileLocation);
			switch (currentState) {
				case SwapfileState.Standard when forward:
					return RegistryMethods.SetRegistryValue(SwapfileControl, 0, RegistryValueKind.DWord);

				case SwapfileState.Standard when !forward:
					MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.SetStadium_ErrorNoneBeforeFirst,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					return false;

				case SwapfileState.Disabled when forward:
					if (newLocation is null) {
						if (MessageBox.Show(
							    OperatingMethodsStrings.SetStadium_NoNewPathGiven, OperatingMethodsStrings.Error,
							    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry) {
							return ChangeSwapfileStadium(forward, currentState, newLocation);
						}

						return false;
					}

					FileInfo saveAs = new FileInfo(Path.Combine(newLocation.RootDirectory.FullName, "Swapfile.sys"));
					if (!saveAs.Exists) {
						if (MessageBox.Show(
							    OperatingMethodsStrings.Error, OperatingMethodsStrings.SetStadium_NewInvalid,
							    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry) {
							return ChangeSwapfileStadium(forward, currentState, newLocation);
						}

						return false;
					}

					string oldPath = DefaultSwapfileLocation;
					if (defaultSwapFileInfo.Exists) {
						FileAndFolder.DeleteFile(defaultSwapFileInfo, false, false);
					}

					OperatingMethods.MoveFilePhysically(new FileInfo(oldPath), saveAs);
					return RegistryMethods.SetRegistryValue(SwapfileControl, 1, RegistryValueKind.DWord);

				case SwapfileState.Disabled when !forward:
					FileAndFolder.DeleteFile(defaultSwapFileInfo, false);
					RegistryMethods.SetRegistryValue(SwapfileControl, 1, RegistryValueKind.DWord);
					break;

				case SwapfileState.Moved when forward:
					MessageBox.Show(OperatingMethodsStrings.Error, OperatingMethodsStrings.SetStadium_ErrorNoneAfterLast,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					return false;

				case SwapfileState.Moved when !forward:
					RegistryMethods.SetRegistryValue(SwapfileControl, 0, RegistryValueKind.DWord);
					break;

				case SwapfileState.None:
					return false;
				default:
					throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
			}

			return true;
		}

		public static SwapfileState GetSwapfileState() {
			if (!RegistryMethods.GetRegistryValue(SwapfileControl, out object regValue,true)) {
				return SwapfileState.None;
			}

			// 1/Null --> swapfile enabled
			// 0 --> disabled
			if ((uint?) regValue == 0) {
				return SwapfileState.Disabled;
			}

			if (FileAndFolder.IsPathSymbolic(DefaultSwapfileLocation)) {
				return SwapfileState.Moved;
			}

			return SwapfileState.Standard;
		}

		public static string GetStateDescription(this SwapfileState state) {
			switch (state) {
				case SwapfileState.Standard:
					return OperatingMethodsStrings.GetDescription_Base + OperatingMethodsStrings.GetDescription_Standard;
				case SwapfileState.Disabled:
					return OperatingMethodsStrings.GetDescription_Base + OperatingMethodsStrings.GetDescription_Disabled;
				case SwapfileState.Moved:
					return OperatingMethodsStrings.GetDescription_Base + OperatingMethodsStrings.GetDescription_MovedNoAdmin;
				case SwapfileState.None:
					return OperatingMethodsStrings.GetDescription_Base + OperatingMethodsStrings.GetDescription_None;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}
	}
}