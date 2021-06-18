using System.Text.Json.Serialization;

namespace RandomMediaPlayer.SelfUpdater.GitHubConnection
{
    internal class GitHubReleaseModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("assets")]
        public GitHubAssetModel[] Assets { get; set; }

    }

    internal class GitHubAssetModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("browser_download_url")]
        public string DownloadUrl { get; set; }
    }
}
