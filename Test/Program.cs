using System;
using StorageManagementCore.Operation;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
			Console.WriteLine(PagefileManagement.GetSystemManaged(out bool val) + "with data" + val);
			//Console.ReadLine();
		}
	}
}