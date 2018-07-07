using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace Test {
	internal class Program {
		public static void Main(string[] args)
		{
			var Dta= CultureInfo.GetCultures(CultureTypes.AllCultures);
			Console.ReadLine();
			//PagefileManagement.GetFutureFreeSpace(new PagefileSysConfiguration(){SystemManaged = true});
		}
	}
}