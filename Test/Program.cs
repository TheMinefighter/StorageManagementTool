using System;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
//			Console.WriteLine(PagefileManagement.GetCurrentPagefileConfiguration(out PagefileSysConfiguration val));
//			Console.WriteLine(JsonConvert.SerializeObject(val));
	//		Console.WriteLine(JsonConvert.SerializeObject(FileSystem.Drives));
			//Console.ReadLine();
		 	Console.Write(Environment.OSVersion.Version.Revision); 
		}
	}
}