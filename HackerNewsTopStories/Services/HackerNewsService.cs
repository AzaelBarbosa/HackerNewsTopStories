using HackerNewsTopStories.Dtos;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace HackerNewsTopStories.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string BestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
        private const string API = "https://hacker-news.firebaseio.com/v0/beststories.json";

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<StoryDto>> GetTopStoriesAsync(int count)
        {
            var ids = await GetBestStoryIdsAsync();
            var tasks = ids.Take(100).Select(GetStoryByIdAsync); // limit to 100 to avoid overload
            var stories = await Task.WhenAll(tasks);

            return stories
                .Where(s => s != null)
                .OrderByDescending(s => s.Score)
                .Take(count)
                .ToList();
        }

        private async Task<List<int>> GetBestStoryIdsAsync()
        {
            if (!_cache.TryGetValue("BestStoryIds", out List<int> ids))
            {
                var response = await _httpClient.GetStringAsync(BestStoriesUrl);
                ids = JsonSerializer.Deserialize<List<int>>(response);
                _cache.Set("BestStoryIds", ids, TimeSpan.FromMinutes(5)); // cache for 5 minutes
            }
            return ids;
        }

        private async Task<StoryDto> GetStoryByIdAsync(int id)
        {
            var url = $"https://hacker-news.firebaseio.com/v0/item/{id}.json";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new StoryDto
            {
                Title = root.GetProperty("title").GetString(),
                Uri = root.TryGetProperty("url", out var urlProp) ? urlProp.GetString() : null,
                PostedBy = root.GetProperty("by").GetString(),
                Time = DateTimeOffset.FromUnixTimeSeconds(root.GetProperty("time").GetInt64()).UtcDateTime,
                Score = root.GetProperty("score").GetInt32(),
                CommentCount = root.TryGetProperty("descendants", out var commentsProp) ? commentsProp.GetInt32() : 0
            };
        }
    }
}
