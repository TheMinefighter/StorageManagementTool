using System;
using System.ComponentModel;
using System.Security;

namespace StorageManagementCore.Operation
{
	/// <summary>
	///  Class for storing Windows Credentials
	/// </summary>
	public sealed class Credentials : IDisposable
	{
		[Browsable(false)] public SecureString Password;

		public string Username;

		public Credentials()
		{
			Password = new SecureString();
		}

		public void Dispose()
		{
			Password?.Dispose();
		}
	}
}