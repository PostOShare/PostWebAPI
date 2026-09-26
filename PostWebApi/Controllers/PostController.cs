using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostWebApiCommon.Models.DTO.Request;
using PostWebApiService.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Security.Claims;

namespace PostWebApi.Controllers
{
    [Authorize]
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
        /// Created, Unauthorized, data is invalid (Status BadRequest), or an internal error occurred 
        /// (Status InternalServerError)
        /// </returns>
        [HttpPost]
        [Route("create-post")]
        [SwaggerOperation("Saves a post to the database")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] PostDto newPost)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "Invalid token claims." });
            }

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
                                          ErrorDesc = response.ErrorDesc,
                                          ErrorCode = response.ErrorCode,
                                          Result = false
                                      });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new PostWebApiCommon.Models.DTO.Response.PostDto
                                  {
                                      ErrorDesc = ex.Message,
                                      ErrorCode = response.ErrorCode,
                                      Result = false
                                  });
            }
        }

        /// <summary> 
        /// Deletes a post from the database.
        /// </summary>
        /// <returns> 
        /// Ok, Unauthorized, data is invalid (Status BadRequest), or an internal error occurred 
        /// (Status InternalServerError)
        /// </returns>
        [HttpDelete("{postId}")]
        [SwaggerOperation("Deletes a post from the database")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PostDto>> DeletePost(string postId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "Invalid token claims." });
            }

            var response = await _postService.DeletePost(postId, currentUserId);

            try
            {
                if (response.Result)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new PostWebApiCommon.Models.DTO.Response.PostDto
                                      {
                                          ErrorDesc = response.ErrorDesc,
                                          ErrorCode = response.ErrorCode,
                                          Result = false
                                      });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new PostWebApiCommon.Models.DTO.Response.PostDto
                                  {
                                      ErrorDesc = ex.Message,
                                      ErrorCode = response.ErrorCode,
                                      Result = false
                                  });
            }
        }

        /// <summary> 
        /// Updates a post in the database.
        /// </summary>
        /// <returns> 
        /// Ok, Unauthorized, data is invalid (Status BadRequest), or an internal error occurred 
        /// (Status InternalServerError)
        /// </returns>
        [HttpPut("{postId}")]
        [SwaggerOperation("Updates a post in the database")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<PostDto>> UpdatePost(string postId, [FromBody] PostDto updatedPost)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "Invalid token claims." });
            }

            updatedPost.Id = postId;
            var response = await _postService.UpdatePost(postId, currentUserId, updatedPost);

            try
            {
                if (response.Result)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new PostWebApiCommon.Models.DTO.Response.PostDto
                                      {
                                          ErrorDesc = response.ErrorDesc,
                                          ErrorCode = response.ErrorCode,
                                          Result = false
                                      });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new PostWebApiCommon.Models.DTO.Response.PostDto
                                  {
                                      ErrorDesc = ex.Message,
                                      ErrorCode = response.ErrorCode,
                                      Result = false
                                  });
            }
        }
    }
}