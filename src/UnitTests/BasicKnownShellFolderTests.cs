using System.Linq;
using System.Reflection;
using StorageManagementCore.Backend;
using Xunit;

namespace UnitTests {
	public class BasicKnownShellFolderTests {
		private FieldInfo[] fields;
		public BasicKnownShellFolderTests() {
			fields = typeof(ShellFolder.KnownShellFolders).GetFields();
		}

		[Fact]
		public void ShellFolderTypes() {
			foreach (FieldInfo fieldInfo in fields) {
				object value = fieldInfo.GetValue(null);
				Assert.IsType<ShellFolder>(value);
			}
		}

		[Fact]
		public void ShellFolderNames() {
			foreach (FieldInfo fieldInfo in fields) {
				object value= fieldInfo.GetValue(null);
				ShellFolder shellFolder = (ShellFolder) value;
				Assert.Equal(fieldInfo.Name,shellFolder.Name);
			}
		}
		[Fact]
		public void ShellfolderList() {
			Assert.Equal(fields.Select(x => x.GetValue(null)).Cast<ShellFolder>(), ShellFolder.AllShellFolders);
		}
	}
}