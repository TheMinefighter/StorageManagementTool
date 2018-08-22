using System.Reflection;
using StorageManagementCore.Backend;
using Xunit;

namespace UnitTests.ShellFolderTests {
	public class BasicKnownShellFolderTests {
		public BasicKnownShellFolderTests() => fields = typeof(ShellFolder.KnownShellFolders).GetFields();

		private readonly FieldInfo[] fields;

		[Fact]
		private void ShellFolderNames() {
			foreach (FieldInfo fieldInfo in fields) {
				object value = fieldInfo.GetValue(null);
				ShellFolder shellFolder = (ShellFolder) value;
				Assert.Equal(fieldInfo.Name, shellFolder.Name);
			}
		}

		[Fact]
		private void ShellFolderTypes() {
			foreach (FieldInfo fieldInfo in fields) {
				object value = fieldInfo.GetValue(null);
				Assert.IsType<ShellFolder>(value);
			}
		}
	}
}