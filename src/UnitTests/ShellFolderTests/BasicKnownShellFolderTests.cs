using System.Linq;
using System.Reflection;
using Xunit;

namespace UnitTests.ShellFolderTests {
	public class BasicKnownShellFolderTests {
		private FieldInfo[] fields;

		public BasicKnownShellFolderTests() {
			fields = typeof(StorageManagementCore.Backend.ShellFolder.KnownShellFolders).GetFields();
		}

		[Fact]
		private void ShellFolderTypes() {
			foreach (FieldInfo fieldInfo in fields) {
				object value = fieldInfo.GetValue(null);
				Assert.IsType<StorageManagementCore.Backend.ShellFolder>(value);
			}
		}

		[Fact]
		private void ShellFolderNames() {
			foreach (FieldInfo fieldInfo in fields) {
				object value = fieldInfo.GetValue(null);
				StorageManagementCore.Backend.ShellFolder shellFolder = (StorageManagementCore.Backend.ShellFolder) value;
				Assert.Equal(fieldInfo.Name, shellFolder.Name);
			}
		}
	}
}