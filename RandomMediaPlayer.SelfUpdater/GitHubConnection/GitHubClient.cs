using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RandomMediaPlayer.SelfUpdater.GitHubConnection
{
    internal class GitHubClient : HttpClient
    {
#pragma warning disable S1075 // URIs should not be hardcoded - i know, got any other ideas for this?
        private readonly static string GITHUB_RELEASE_URL = @"https://api.github.com/repos/KowalskiPiotr98/RandomMediaPlayer/releases/latest";
#pragma warning restore S1075 // URIs should not be hardcoded
        public GitHubClient(string version) : base()
        {
            DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("StartLauncher", version));
        }

        public async Task<T> GetLatestReleaseAsync<T>() where T : class
        {
            var response = await GetAsync(GITHUB_RELEASE_URL).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            return null;
        }
    }

    internal class GitHubWebClient : WebClient
    {
        public GitHubWebClient(string version) : base()
        {
            Headers.Add(HttpRequestHeader.UserAgent, $"StartLauncher/{version}");
        }
    }
}
