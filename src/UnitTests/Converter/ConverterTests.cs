using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using StorageManagementCore.Backend;
using StorageManagementCore.WPFGUI.ValueConverters;
using Xunit;

namespace UnitTests.Converter {
	public class ConverterTests {
ShellFolderFilterConverter filter= new ShellFolderFilterConverter();
		[Fact]
		private void ShellFolderFilterTest() {
			object resultUnfiltered= filter.Convert(new object[] {ShellFolder.AllShellFolders, true}, typeof(IEnumerable), null, CultureInfo.CurrentUICulture);
			Assert.Equal(filter.ConvertBack(resultUnfiltered, new []{typeof(IReadOnlyCollection<ShellFolder>),typeof(bool)}, null, CultureInfo.CurrentUICulture)[1],true);
		}
	}
}