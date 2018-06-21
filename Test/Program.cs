using System;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using StorageManagementCore.Operation;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
//			Console.WriteLine(PagefileManagement.GetCurrentPagefileConfiguration(out PagefileSysConfiguration val));
//			Console.WriteLine(JsonConvert.SerializeObject(val));
	//		Console.WriteLine(JsonConvert.SerializeObject(FileSystem.Drives));
			//Console.ReadLine();
			RegistryValue registryValue = new RegistryValue(
				"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
				"ReleaseId");
			Console.Write(Environment.OSVersion.Version.Revision); 
		}
	}
}