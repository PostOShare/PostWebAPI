using Microsoft.AspNetCore.Mvc;

namespace PostWebApi.Controllers
{
    [Route("api/v1/post/")]
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("live")]
        public IActionResult Live()
        {
            return Ok("Post API is live");
        }
    }
}