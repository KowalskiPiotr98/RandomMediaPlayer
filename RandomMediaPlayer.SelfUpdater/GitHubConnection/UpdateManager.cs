using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RandomMediaPlayer.SelfUpdater.GitHubConnection
{
    public class UpdateManager : IDisposable
    {
        private static readonly string GITHUB_ASSET_NAME = @"RandomMediaPlayer-win64-installer.msi";

        private readonly GitHubClient client;
        private readonly string version;
        private bool updateAvailable;
        private GitHubReleaseModel cachedRelease;
        private bool disposedValue;

        public UpdateManager(string version)
        {
            this.version = version;
            client = new GitHubClient(version);
        }

        public void ClearCache()
        {
            cachedRelease = null;
            updateAvailable = false;
        }

        public async Task<bool> IsUpdateAvailableAsync(bool ignoreCache = false)
        {
            if (ignoreCache || cachedRelease is null)
            {
                cachedRelease = await client.GetLatestReleaseAsync<GitHubReleaseModel>().ConfigureAwait(false);
                updateAvailable = cachedRelease != null && cachedRelease.Name != version && cachedRelease.Name.StartsWith('v') && cachedRelease.Name.Count(s => s == '.') == 2;
            }
            return updateAvailable;
        }

        /// <summary>
        /// Attempts to install new update
        /// </summary>
        /// <returns>True if update started and application needs to close, false otherwise</returns>
        public async Task<bool> RunUpdaterAsync()
        {
            if (cachedRelease is null)
            {
                throw new InvalidOperationException($"{nameof(IsUpdateAvailableAsync)} must be called first");
            }
            if (!updateAvailable)
            {
                return false;
            }
            var asset = cachedRelease.Assets.FirstOrDefault(a => a.Name == GITHUB_ASSET_NAME);
            if (asset is null)
            {
                return false;
            }
            using var githubDownloader = new GitHubWebClient(version);
            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                await githubDownloader.DownloadFileTaskAsync(new Uri(asset.DownloadUrl), tempPath).ConfigureAwait(false);
            }
            catch (UriFormatException)
            {
                File.Delete(tempPath);
                return false;
            }
            try
            {
                File.Move(tempPath, $"{tempPath}.msi");
                tempPath += ".msi";
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = tempPath;
                p.StartInfo.UseShellExecute = true;
                _ = p.Start();
            }
            catch (Exception)
            {
                File.Delete(tempPath);
                return false;
            }
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
