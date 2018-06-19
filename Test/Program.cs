using System;
using System.Diagnostics;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
			Console.WriteLine(StorageManagementCore.Operation.PagefileManagement.GetSystemManaged(out bool val)+"with data" +val);
			//Console.ReadLine();
		}
	}
}