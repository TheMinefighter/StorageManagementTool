using System.Threading.Tasks;
using StorageManagementCore.Operation;

//!So block 3 first, then 2 then 1
namespace Test {
	internal class Program {
		public static async Task Main(string[] args) {
//			User user = await new Octokit.GitHubClient(new ProductHeaderValue("TestCrawler")).User.Get("TheMinefighter");
			await Updater.Update(false);
		}
	}
}