using System.Linq;
using StorageManagementCore.Backend;
using Xunit;

namespace UnitTests.ShellFolderTests {
	public class FurtherShellFolderTests {
		[Fact]
		private void ShellFolderIsDefined() {
			foreach (ShellFolder s in ShellFolder.AllShellFolders) {
				Assert.Equal(s.DefaultValue is null, s.Undefined);
			}
		}

		[Fact]
		private void ShellFolderIsUserSpecific() {
			foreach (ShellFolder s in ShellFolder.AllShellFolders.Where(x => x.Defined)) {
				Assert.NotNull(s.DefaultValue);
				Assert.Equal(s.DefaultValue.Contains("%USERPROFILE%") || s.DefaultValue.Contains("%USERNAME%"), s.IsUserSpecific);
			}
		}
	}
}