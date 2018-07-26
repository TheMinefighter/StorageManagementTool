using StorageManagementCore.Backend;
using Xunit;

namespace UnitTests.ShellFolderTests {
	public class FurtherShellFolderTests {
		
		[Fact]
		private void ShellFolderIsDefined() {
			foreach (ShellFolder s in ShellFolder.AllShellFolders) {
				Assert.Equal(s.DefaultValue==null, s.Undefined);
			}
		}
	}
}