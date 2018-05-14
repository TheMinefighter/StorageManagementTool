using System.ComponentModel;
using System.Security;

namespace StorageManagementCore.Operation {
	/// <summary>
	///  Class for storing Windows Credentials
	/// </summary>
	public sealed class Credentials {
		[Browsable(false)] 
		public SecureString Password;

		public string Username;

		public Credentials() => Password = new SecureString();
	}
}