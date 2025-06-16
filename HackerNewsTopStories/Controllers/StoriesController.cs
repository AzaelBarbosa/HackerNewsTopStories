using HackerNewsTopStories.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsTopStories.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsService _service;

        public StoriesController(IHackerNewsService service)
        {
            _service = service;
        }

        [HttpGet("{count}")]
        public async Task<IActionResult> GetTopStories(int count)
        {
            if (count <= 0 || count > 100)
                return BadRequest("Count must be between 1 and 100");

            var stories = await _service.GetTopStoriesAsync(count);
            return Ok(stories);
        }
    }
}
