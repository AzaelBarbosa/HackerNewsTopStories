using HackerNewsTopStories.Dtos;

namespace HackerNewsTopStories.Services
{
    public interface IHackerNewsService
    {
        Task<List<StoryDto>> GetTopStoriesAsync(int count);
    }
}
