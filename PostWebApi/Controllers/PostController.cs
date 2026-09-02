using Microsoft.AspNetCore.Mvc;
using PostWebApiCommon.Models.DTO.Request;
using PostWebApiService.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace PostWebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        public IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("live")]
        public IActionResult Live()
        {
            return Ok("Post API is live");
        }

        /// <summary> 
        /// Saves a post to the database.
        /// </summary>
        /// <returns> 
        /// Created, data is invalid (Status BadRequest), or an internal error occurred 
        /// (Status InternalServerError)
        /// </returns>
        [HttpPost]
        [Route("create-post")]
        [SwaggerOperation("Saves a post to the database")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] PostDto newPost)
        {
            var response = await _postService.CreatePost(newPost);

            try
            {
                if (response.Result)
                {
                    return Created("create-post", response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new PostWebApiCommon.Models.DTO.Response.PostDto
                                      {
                                          Error = response.Error,
                                          Result = false
                                      });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new PostWebApiCommon.Models.DTO.Response.PostDto
                                  {
                                      Error = ex.Message,
                                      Result = false
                                  });
            }
        }
    }
}