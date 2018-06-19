using System;
using Newtonsoft.Json;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
			Console.WriteLine(PagefileManagement.GetCurrentPagefileConfiguration(out PagefileSysConfiguration val));
			Console.WriteLine(JsonConvert.SerializeObject(val));
			//Console.ReadLine();
		}
	}
}