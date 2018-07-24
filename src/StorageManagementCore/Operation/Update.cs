using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace StorageManagementCore.Operation {
	public static class Update {
		private const string GitReleaseUrl = "https://api.github.com/repos/TheMinefighter/StorageManagementTool/releases";
		
		public static async Task<Stream> GetReleaseStream() {
			FormUrlEncodedContent RequestContent= new FormUrlEncodedContent(new KeyValuePair<string, string>[] {
				new KeyValuePair<string, string>("text", "This is a block of text"),
			});
			using (HttpClient client = new HttpClient()) {
				HttpResponseMessage data = await client.PostAsync(GitReleaseUrl, RequestContent);
			}
			
		}
	}
}