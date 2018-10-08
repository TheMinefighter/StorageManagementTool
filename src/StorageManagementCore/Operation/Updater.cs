using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using kalexi.Monads.Either.Code;
using Newtonsoft.Json;
using Octokit;
using StorageManagementCore.Backend;
using StorageManagementCore.Configuration;
using FileMode = System.IO.FileMode;

namespace StorageManagementCore.Operation {
	/// <summary>
	///  Provides functionality for automatic tests
	/// </summary>
	public static class Updater {
		//private const string RepositoryName = "PagesTest";
		private const string RepositoryName = "StorageManagementTool";
		private const string OwnerName = "TheMinefighter";
		private const string CrawlerName = "StorageManagementTool_UpdateCrawler";
		private const string UpdatePackageName = "UpdatePackage.zip";
		private const string ApiUrl = "https://api.github.com";

		public static void InvokeUpdateProcess(UpdateConfiguration config) {
			switch (config.Mode) {
				case UpdateMode.NoUpdates: return;
				case UpdateMode.DownloadAndInstallOnStartup:
					if (Session.Singleton.IsAdmin)
					{

						Update(config.UsePrereleases).ContinueWith(e => {
							if (e.Result == null) {
								RunUpdateInstaller();
							}
						});
					}
					break;
				case UpdateMode.DownloadOnStartupInstallNext:
					if (Directory.Exists(Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
						UpdateInstaller.Update.UpdateDataDirectory)))
					{
						RunUpdateInstaller();
					}
					else {
						if (Session.Singleton.IsAdmin)
						{
							Update(config.UsePrereleases);
						}
					}
					break;
				case UpdateMode.DownloadOnStartupInstallManual:
					if (Session.Singleton.IsAdmin)
					{
						Update(config.UsePrereleases);
					}
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static void RunUpdateInstaller() {//TODO Admin check
			Wrapper.ExecuteExecuteable(Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),"UpdateInstaller.exe"), "", true);
			Environment.Exit(0);
		}

		public static async Task<T> FirstOrDefaultAsync<T>(this IEnumerable<T> collection, Func<T, Task<bool>> predicate) {
			foreach (T item in collection) {
				bool isResult = await predicate(item).ConfigureAwait(false);
				if (isResult) {
					return item;
				}
			}

			return default(T);
		}

		public static async Task<Exception> Update(bool usePrereleases) {
			Either<Release, Exception> toUpdate = await ReleaseToUpdate(usePrereleases);
			if (toUpdate.IsRight) {
				return toUpdate.Right;
			}

			if (toUpdate.Left is null) {
				return new Exception("No new version found.");
			}

			return await DownloadUpdatePackageAsync(
				toUpdate.Left.Assets.First(x => x.State == "uploaded" && x.Name == UpdatePackageName),
				new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Directory.CreateSubdirectory("UpdateData").FullName);
		}

		internal static async Task<Either<IReadOnlyList<Release>, Exception>> GetReleasesData() {
			IReadOnlyList<Release> releases;
			try {
				releases = await GetGitHubClient().Repository.Release
					.GetAll(OwnerName, RepositoryName);
			}
			catch (Exception e) {
				return e;
			}

			return new Either<IReadOnlyList<Release>, Exception>(releases);
		}

		private static GitHubClient GetGitHubClient() => new GitHubClient(new ProductHeaderValue(CrawlerName, Program.VersionTag));

		/// <summary>
		///  Gets the release to update to
		/// </summary>
		/// <param name="releases">The <see cref="Release" />s available</param>
		/// <param name="usePrereleases">Whether to use Prereleases</param>
		/// <returns>The <see cref="Release" /> to update to, <see langword="null" /> if no UpdatePackage is available </returns>
		[CanBeNull]
		internal static async Task<Either<Release, Exception>> ReleaseToUpdate(bool usePrereleases) {
			GitHubClient client = GetGitHubClient();
			HttpClient hClient = new HttpClient {BaseAddress = new Uri("https://api.github.com")};
			hClient.DefaultRequestHeaders.Add("User-Agent", CrawlerName);
			Either<IReadOnlyList<Release>, Exception> releaseData = await GetReleasesData();
			if (releaseData.IsRight) {
				return releaseData.Right;
			}

			IReadOnlyList<RepositoryTag> tags;
			try {
				tags = await client.Repository.GetAllTags(OwnerName, RepositoryName);
			}
			catch (Exception e) {
				return e;
			}

			return await releaseData.Left.FirstOrDefaultAsync(async x => {
				if (!x.Assets.Any(y => y.State == "uploaded" && y.Name == UpdatePackageName)) {
					return false;
				}

				if (x.TagName == Program.VersionTag) {
					return false;
				}

				RepositoryTag t = tags.FirstOrDefault(y => y.Name == x.TagName);
				if (t is null) {
					return false;
				}

				HttpResponseMessage message = await hClient.GetAsync($"repos/{OwnerName}/{RepositoryName}/commits/{t.Commit.Sha}");
				if (message.StatusCode != HttpStatusCode.OK) {
					return false;
				}

				string messageContent = await message.Content.ReadAsStringAsync();
				InternalCommitRoot root = JsonConvert.DeserializeObject<InternalCommitRoot>(messageContent);
				if (root?.commit?.verification?.reason != "valid") {
					return false;
				}

				string commitName = root.commit?.message;
				if (commitName == null) {
					return false;
				}

				if (!commitName.EndsWith("release", StringComparison.OrdinalIgnoreCase)) {
					return false;
				}

				return !x.Draft && usePrereleases || !x.Prerelease;
			});
		}

		/// <summary>
		///  Extracts an <see cref="ZipArchive" /> to a specified path
		/// </summary>
		/// <param name="archive"> The <see cref="ZipArchive" /> to extract</param>
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
		///  Downloads an (zipped) update package, in extracted form, to the path defined
		/// </summary>
		/// <param name="source"> the <see cref="ReleaseAsset" /> of the Update package</param>
		/// <param name="unZipPath">Where to unzip the update package to</param>
		/// <returns><see langword="null" /> for success, otherwise the appropriate exception</returns>
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
// ReSharper disable InconsistentNaming
#pragma warning disable 649
		private class InternalVerification {
			public string reason;


			public bool verified;
		}

		private class InternalCommit {
			public string message;
			public InternalVerification verification;
		}

		private class InternalCommitRoot {
			public InternalCommit commit;
		}
		// ReSharper restore InconsistentNaming
#pragma warning restore 649
	}
}