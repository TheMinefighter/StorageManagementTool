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
	/// <summary>
	/// Provides functionality for automatic tests
	/// </summary>
	public static class Updater {
		private const string RepositoryName = "StorageManagementTool";
		private const string OwnerName = "TheMinefighter";
		private const string CrawlerName = "StorageManagementToolUpdateCrawler";
		private const string UpdatePackageName = "UpdatePackage.zip";

		public static async Task<Exception> Update(bool usePrereleases) {
			(IReadOnlyList<Release> releases, Exception releaseException) = await GetReleasesData();
			if (releaseException != null) {
				return releaseException;
			}

			Release toUpdate = ReleaseToUpdate(releases, usePrereleases);
			if (toUpdate == null) {
				return new Exception("No new version found.");
			}

			return await DownloadUpdatePackageAsync(
				toUpdate.Assets.First(x => x.State == "uploaded" && x.Name == UpdatePackageName),
				new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Directory.CreateSubdirectory("UpdateData").FullName);
		}

		internal static async Task<(IReadOnlyList<Release>, Exception)> GetReleasesData() {
			IReadOnlyList<Release> releases;
			try {
				releases = await new GitHubClient(new ProductHeaderValue(CrawlerName,Program.VersionTag)).Repository.Release
					.GetAll(OwnerName, RepositoryName);
			}
			catch (Exception e) {
				return (null, e);
			}

			return (releases, null);
		}

		/// <summary>
		/// Gets the release to update to
		/// </summary>
		/// <param name="releases">The <see cref="Release"/>s available</param>
		/// <param name="usePrereleases">Whether to use Prereleases</param>
		/// <returns>The <see cref="Release"/> to update to, <see langword="null"/> if no UpdatePackage is available </returns>
		[CanBeNull]
		internal static Release ReleaseToUpdate([NotNull] [ItemNotNull] IEnumerable<Release> releases, bool usePrereleases) {
			//Tried to do signature checking, but API does not support that
			Release ret = releases.First(x => !x.Draft && usePrereleases || !x.Prerelease);
			if (ret.Assets.Any(x => x.State == "uploaded" && x.Name == UpdatePackageName) && ret.TagName != Program.VersionTag) {
				return ret;
			}
			else {
				return null;
			}
		}

		/// <summary>
		/// Extracts an <see cref="ZipArchive"/> to a specified path
		/// </summary>
		/// <param name="archive"> The <see cref="ZipArchive"/> to extract</param>
		/// <param name="path"> The path to extract the archive to</param>
		/// <returns></returns>
		internal static async Task<Exception> ExtractArchiveAsync(ZipArchive archive, string path) {
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

		/// <summary>
		/// Downloads an (zipped) update package, in extracted form, to the path defined
		/// </summary>
		/// <param name="source"> the <see cref="ReleaseAsset"/> of the Update package</param>
		/// <param name="unZipPath">Where to unzip the update package to</param>
		/// <returns><see langword="null"/> for success, otherwise the appropriate exception</returns>
		internal static async Task<Exception> DownloadUpdatePackageAsync(ReleaseAsset source, string unZipPath) {
			HttpClient downloadClient = new HttpClient();

			Stream zipStream = null;
			try {
				zipStream = await downloadClient.GetStreamAsync(source.BrowserDownloadUrl);
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