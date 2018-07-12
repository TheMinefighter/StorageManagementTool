using System;
using StorageManagementCore.Backend;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
			Console.WriteLine(SpecialFolders.GetSpecialFolderPath(AllShellFolders.SidebarParts.WindowsIdentifier));
			Console.WriteLine(new Guid(0xde61d971, 0x5ebc, 0x4f02, 0xa3, 0xa9, 0x6c, 0x82, 0x89, 0x5e, 0x5c, 0x04) ==
			                  new Guid("de61d971-5ebc-4f02-a3a9-6c82895e5c04"));
			Console.ReadLine();
			//PagefileManagement.GetFutureFreeSpace(new PagefileSysConfiguration(){SystemManaged = true});
		}
	}
}