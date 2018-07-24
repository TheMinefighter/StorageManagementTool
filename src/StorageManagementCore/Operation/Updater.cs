using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Octokit;

namespace StorageManagementCore.Operation {
	public class Updater {
		private const string VersionTag = "1.1-b-2";
		private readonly GitHubClient _client = new GitHubClient(new ProductHeaderValue("StorageManagementToolUpdateCrawler"));
		private const string GitReleaseUrl = "https://api.github.com/repos/TheMinefighter/StorageManagementTool/releases";
		
		public async Task<IReadOnlyList<Release>> GetReleasesData() => await _client.Repository.Release.GetAll("TheMinefighter", "StorageManagementTool");

		public static Release ReleaseToUpdate(IReadOnlyList<Release> releases, bool acceptPrerelease) {
			Release ret = releases.First(x => !x.Draft && acceptPrerelease || !x.Prerelease);
			if (ret.Assets.Any(x=>x.State=="uploaded"&&x.Name=="UpdatePackage.zip")) {
				return ret;
			}
			else {
				return null;
			}
		}
	}
}