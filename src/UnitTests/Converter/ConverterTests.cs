using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using StorageManagementCore.Backend;
using StorageManagementCore.WPFGUI.ValueConverters;
using Xunit;

namespace UnitTests.Converter {
	public class ConverterTests {
ShellFolderFilterConverter filter= new ShellFolderFilterConverter();
		NativeLanguageConverter native= new NativeLanguageConverter();
		
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		private void ShellFolderFilterTest(bool filtered) {
			object resultUnfiltered= filter.Convert(new object[] {ShellFolder.AllShellFolders, filtered}, typeof(IEnumerable), null, CultureInfo.CurrentUICulture);
			Assert.Equal(filtered, filter.ConvertBack(resultUnfiltered, new []{typeof(IReadOnlyCollection<ShellFolder>),typeof(bool)}, null, CultureInfo.CurrentUICulture)[1]);
		}
		//Theory not possible because DBNull.Value is not constant
		[Fact]
		private void NativeLanguageTest() {
			object[] cases = {
				DBNull.Value,
				CultureInfo.GetCultures(CultureTypes.SpecificCultures).First(),
				CultureInfo.GetCultures(CultureTypes.NeutralCultures).First()
			};
			foreach (object currentCase in cases) {
				object result = native.Convert(currentCase, typeof(string), null, CultureInfo.CurrentUICulture);	
				Assert.Equal(currentCase, native.ConvertBack(result, typeof(object),null,CultureInfo.CurrentUICulture));
			}
		}
	}
}