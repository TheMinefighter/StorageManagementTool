using System;
using System.IO;
using System.Threading.Tasks;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;
using System.Net.Http;

namespace Test {
	internal class Program {
		public static async Task Main(string[] args) {
			using (Updater u= new Updater()) {
				await u.GetReleasesData();
			}
			//PagefileManagement.GetFutureFreeSpace(new PagefileSysConfiguration(){SystemManaged = true});
		}
	}
}