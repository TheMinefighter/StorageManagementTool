namespace StorageManagementCore.Operation {
	public class EnterCredentials {
		/// <summary>
		///  Stores Credentials for later use when an Administartor Account is required
		/// </summary>
		private static Credentials _forAdmin = new Credentials();

		/// <summary>
		///  Stores Credentials for later use
		/// </summary>
		private static Credentials _forStandard = new Credentials();
		
		/// <summary>
		///  Gives credentials meeting specified requirements
		/// </summary>
		/// <param name="adminNeeded">Whether the Credentials need to fit for an administrator Account</param>
		/// <param name="credentials">The Credentials requested</param>
		/// <returns>Whether the Operation were successful</returns>
		public static bool GetCredentials(bool adminNeeded, out Credentials credentials) {
			if (adminNeeded) {
				if (_forAdmin.Username != null) {
					credentials = _forAdmin;
					return true;
				}
			}
			else {
				if (_forStandard.Username != null) {
					credentials = _forStandard;
					return true;
				}
			}

			credentials = new Credentials();
			EnterCredentialsDialog dialog = new EnterCredentialsDialog {Tag = new DialogReturnData(adminNeeded)};
			dialog.ShowDialog();
			if (((DialogReturnData) dialog.Tag).IsAborted) {
				return false;
			}

			credentials = ((DialogReturnData) dialog.Tag).GivenCredentials;
			_forStandard = ((DialogReturnData) dialog.Tag).GivenCredentials;
			if (((DialogReturnData) dialog.Tag).IsAdmin) {
				_forAdmin = ((DialogReturnData) dialog.Tag).GivenCredentials;
			}

			return !((DialogReturnData) dialog.Tag).IsAborted;
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