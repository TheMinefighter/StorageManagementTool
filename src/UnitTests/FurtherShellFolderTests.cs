using System.Reflection;
using StorageManagementCore.Backend;
using Xunit;

namespace UnitTests {
	public class FurtherShellFolderTests {
		
		[Fact]
		public void ShellFolderIsDefined() {
			foreach (ShellFolder s in ShellFolder.AllShellFolders) {
				Assert.Equal(s.DefaultValue==null, s.Undefined);
			}
		}
	}
}