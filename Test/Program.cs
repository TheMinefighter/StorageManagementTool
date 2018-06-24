using System;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
//			Console.WriteLine(PagefileManagement.GetCurrentPagefileConfiguration(out PagefileSysConfiguration val));
//			Console.WriteLine(JsonConvert.SerializeObject(val));
			//		Console.WriteLine(JsonConvert.SerializeObject(FileSystem.Drives));
			//Console.ReadLine();
//			RegistryValue registryValue = new RegistryValue(
//				"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
//				"ReleaseId");
			Console.WriteLine(new DriveInfo("C").Equals(new DriveInfo("C")));
			Console.Write(FileSystem.Drives.Single(x => x.Name.StartsWith("C")).RootDirectory.FullName
				.Equals(new DriveInfo("C").RootDirectory.FullName));
		}
	}
}