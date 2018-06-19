using System;

namespace StorageManagementCore.Operation {
	public static class CredentialsManager {
		/// <summary>
		///  Stores Credentials for later use when an Administartor Account is required
		/// </summary>
		private static bool _forAdmin;

		/// <summary>
		///  Stores Credentials for later use
		/// </summary>
		private static Credentials _credentials = new Credentials();

		public static EventHandler OnCredentialsChanged = (a, b) => { };

		public static bool AdminstratorCredentials => _forAdmin;
		public static bool HasCredentials => _credentials.Username == null;

		public static void DisposeCredentials() {
			_credentials.Dispose();
			_forAdmin = false;
			_credentials = new Credentials();
			OnCredentialsChanged(null, EventArgs.Empty);
		}

		public static (bool?, string) GetStoredCredentials() {
			if (_credentials.Username == null) {
				return (null, null);
			}

			return (_forAdmin, _credentials.Username);
		}

		/// <summary>
		///  Gives credentials meeting specified requirements
		/// </summary>
		/// <param name="adminNeeded">Whether the Credentials need to fit for an administrator Account</param>
		/// <param name="credentials">The Credentials requested</param>
		/// <returns>Whether the Operation were successful</returns>
		public static bool GetCredentials(bool adminNeeded, out Credentials credentials) {
			if (adminNeeded) {
				if (_forAdmin) {
					credentials = _credentials;
					return true;
				}
			}
			else {
				if (_credentials.Username != null) {
					credentials = _credentials;
					return true;
				}
			}

			credentials = new Credentials();
			EnterCredentialsDialog dialog = new EnterCredentialsDialog {Tag = new DialogReturnData(adminNeeded)};
			DisposeCredentials();
			dialog.ShowDialog();
			if (((DialogReturnData) dialog.Tag).IsAborted) {
				return false;
			}

			credentials = ((DialogReturnData) dialog.Tag).GivenCredentials;
			_credentials = ((DialogReturnData) dialog.Tag).GivenCredentials;
			if (((DialogReturnData) dialog.Tag).IsAdmin) {
				_forAdmin = true;
			}

			OnCredentialsChanged(null, EventArgs.Empty);
			return true;
		}

		/// <summary>
		///  Class for the the return data of the insert credentials dialog
		/// </summary>
		public class DialogReturnData {
			public bool IsAborted { get; set; }
			public Credentials GivenCredentials { get; set; }
			public bool IsAdmin { get; set; }
			public bool AdminRequired { get; set; }

			public DialogReturnData() { }

			public DialogReturnData(bool adminAccountRequired) {
				GivenCredentials = new Credentials();
				AdminRequired = adminAccountRequired;
				IsAborted = true;
			}
		}
	}
}