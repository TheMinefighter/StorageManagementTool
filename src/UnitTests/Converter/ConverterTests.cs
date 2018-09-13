using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StorageManagementCore.Backend;
using StorageManagementCore.WPFGUI.ValueConverters;
using Xunit;

namespace UnitTests.Converter {
	public class ConverterTests {
		private readonly ShellFolderFilterConverter _filter = new ShellFolderFilterConverter();
		private readonly NativeLanguageConverter _language = new NativeLanguageConverter();
		private readonly ShellFolderLocalizedNameConverter _shellFolderConverter = new ShellFolderLocalizedNameConverter();


		[Theory, InlineData(true), InlineData(false)]
		private void ShellFolderFilterTest(bool filtered) {
			object resultUnfiltered = _filter.Convert(new object[] {ShellFolder.AllShellFolders, filtered}, typeof(IEnumerable),
				null, CultureInfo.CurrentUICulture);
			Assert.Equal(filtered,
				_filter.ConvertBack(resultUnfiltered, new[] {typeof(IReadOnlyCollection<ShellFolder>), typeof(bool)}, null,
					CultureInfo.CurrentUICulture)[1]);
		}

		[Theory, InlineData(1), InlineData(5)]
		private void ShellFolderConversion(int index) {
			object result = _shellFolderConverter.Convert(ShellFolder.AllShellFolders[index], typeof(string), null,
				CultureInfo.CurrentUICulture);
			Assert.Equal(ShellFolder.AllShellFolders[index],
				_shellFolderConverter.ConvertBack(result, typeof(ShellFolder), null, CultureInfo.CurrentUICulture));
		}

		//Theory not possible because DBNull.Value is not constant
		[Fact]
		private void NativeLanguageTest() {
			object[] cases = {
				"Lol that brings an exception",
				DBNull.Value,
				CultureInfo.GetCultures(CultureTypes.SpecificCultures).First(),
				CultureInfo.GetCultures(CultureTypes.NeutralCultures).First()
			};
			foreach (object currentCase in cases) {
				if (currentCase is string) {
					Assert.ThrowsAny<ArgumentException>(() =>
						_language.Convert(currentCase, typeof(string), null, CultureInfo.CurrentUICulture));
				}
				else {
					object result = _language.Convert(currentCase, typeof(string), null, CultureInfo.CurrentUICulture);
					Assert.Equal(currentCase, _language.ConvertBack(result, typeof(object), null, CultureInfo.CurrentUICulture));
				}
			}
		}
	}
}