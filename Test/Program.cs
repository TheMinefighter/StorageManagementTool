using System;
using System.Diagnostics;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
			StorageManagementCore.Operation.PagefileManagement.GetSystemManaged(out bool val);
			Console.WriteLine(val);
			Console.ReadLine();
		}
	}
}