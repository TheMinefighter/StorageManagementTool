using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Octokit;
using FileMode = System.IO.FileMode;

namespace StorageManagementCore.Operation {
	public class Updater {
		private const string VersionTag = "1.1-b-2";
		private HttpClient _downloadClient;
		private readonly GitHubClient _client = new GitHubClient(new ProductHeaderValue("StorageManagementToolUpdateCrawler"));
		private const string GitReleaseUrl = "https://api.github.com/repos/TheMinefighter/StorageManagementTool/releases";

		public static async Task<Exception> Update(bool acceptPrereleases) {
			Updater u = new Updater();
			(IReadOnlyList<Release> releases, Exception releaseException) = await u.GetReleasesData();
			if (releaseException != null) {
				return releaseException;
			}

			Release toUpdate = ReleaseToUpdate(releases, acceptPrereleases);
			if (toUpdate == null) {
				return new Exception("No new version found.");
			}

			return await u.DownloadUpdatePackageAsync(toUpdate.Assets.First(x => x.State == "uploaded" && x.Name == "UpdatePackage.zip"),
				new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Directory.FullName);
		}

		public async Task<(IReadOnlyList<Release>, Exception)> GetReleasesData() {
			IReadOnlyList<Release> releases;
			try {
				releases = await _client.Repository.Release.GetAll("TheMinefighter", "StorageManagementTool");
			}
			catch (Exception e) {
				return (null, e);
			}

			return (releases, null);
		}

		[CanBeNull]
		private static Release ReleaseToUpdate([NotNull] [ItemNotNull] IEnumerable<Release> releases, bool acceptPrerelease) {
			//Tried to do signature checking, but API does not support that
			Release ret = releases.First(x => !x.Draft && acceptPrerelease || !x.Prerelease);
			if (ret.Assets.Any(x => x.State == "uploaded" && x.Name == "UpdatePackage.zip") && ret.TagName != Program.VersionTag) {
				return ret;
			}
			else {
				return null;
			}
		}

		public static async Task<Exception> ExtractArchiveAsync(ZipArchive archive, string path) {
			foreach (ZipArchiveEntry entry in archive.Entries) {
				string destinationPath = Path.GetFullPath(Path.Combine(path, entry.FullName));
				if (destinationPath.StartsWith(path, StringComparison.Ordinal)) {
					try {
						using (Stream destinationStream = File.Open(destinationPath, FileMode.CreateNew, FileAccess.Write)) {
							using (Stream entryStream = entry.Open()) {
								await entryStream.CopyToAsync(destinationStream);
							}
						}
					}
					catch (Exception e) {
						return e;
					}
				}
			}

			return null;
		}

		public async Task<Exception> DownloadUpdatePackageAsync(ReleaseAsset source, string unZipPath) {
			if (_downloadClient == null) {
				_downloadClient = new HttpClient();
			}

			Stream zipStream = null;
			try {
				zipStream = await _downloadClient.GetStreamAsync(source.BrowserDownloadUrl);
			}
			catch (Exception e) {
				zipStream?.Dispose();
				return e;
			}

			using (Stream innerZipStream = zipStream) {
				using (ZipArchive archive = new ZipArchive(innerZipStream, ZipArchiveMode.Read, false)) {
					return await ExtractArchiveAsync(archive, unZipPath);
				}
			}
		}
	}
}