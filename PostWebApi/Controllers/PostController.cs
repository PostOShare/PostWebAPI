using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostWebApiCommon;
using PostWebApiCommon.Models.DTO;
using PostWebApiCommon.Models.DTO.Response;
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
        /// Created, Unauthorized, data is invalid (Status BadRequest), Forbidden, or an internal error occurred 
        /// (Status InternalServerError)
        /// </returns>
        [HttpPost]
        [Route("create-post")]
        [SwaggerOperation("Saves a post to the database")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] PostDto newPost)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

                string? accessToken = await HttpContext.GetTokenAsync("access_token");

                #pragma warning disable CS8604 // Possible null reference argument
                var response = await _postService.CreatePost(newPost, currentUserId, accessToken);
                #pragma warning restore CS8604 // Possible null reference argument

                if (response.Result)
                {
                    return Created("create-post", response);
                }
                else if (response.ErrorCode.Equals(ErrorCodes.UsernameTokenError))
                {
                    return BadRequest(response);
                }
                else if (response.ErrorCode.Equals(ErrorCodes.UserNotAuthorizedToCreatePost))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new PostResponseDTO
                    {
                        ErrorDesc = response.ErrorDesc,
                        ErrorCode = response.ErrorCode,
                        Result = false
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new PostResponseDTO
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
                                  new PostResponseDTO
                                  {
                                      ErrorDesc = ex.Message,
                                      ErrorCode = ErrorCodes.InternalServerError,
                                      Result = false
                                  });
            }
        }

        /// <summary> 
        /// Deletes a post from the database.
        /// </summary>
        /// <returns> 
        /// Ok, Unauthorized, data is invalid (Status BadRequest), Forbidden, or an internal error occurred 
        /// (Status InternalServerError)
        /// </returns>
        [HttpDelete("{postId}")]
        [SwaggerOperation("Deletes a post from the database")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<PostDto>> DeletePost(string postId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

                string? accessToken = await HttpContext.GetTokenAsync("access_token");

                #pragma warning disable CS8604 // Possible null reference argument
                var response = await _postService.DeletePost(postId, currentUserId, accessToken);
                #pragma warning restore CS8604 // Possible null reference argument

                if (response.Result)
                {
                    return Ok(response);
                }
                else if (response.ErrorCode.Equals(ErrorCodes.UsernameTokenError))
                {
                    return BadRequest(response);
                }
                else if (response.ErrorCode.Equals(ErrorCodes.UserNotAuthorizedToCreatePost))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new PostResponseDTO
                    {
                        ErrorDesc = response.ErrorDesc,
                        ErrorCode = response.ErrorCode,
                        Result = false
                    });
                }
                else if (response.ErrorCode.Equals(ErrorCodes.PostNotFound))
                {
                    return NotFound(new PostResponseDTO
                    {
                        ErrorDesc = response.ErrorDesc,
                        ErrorCode = response.ErrorCode,
                        Result = false
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new PostResponseDTO
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
                                  new PostResponseDTO
                                  {
                                      ErrorDesc = ex.Message,
                                      ErrorCode = ErrorCodes.InternalServerError,
                                      Result = false
                                  });
            }
        }

        /// <summary> 
        /// Updates a post in the database.
        /// </summary>
        /// <returns> 
        /// Ok, Unauthorized, data is invalid (Status BadRequest), Forbidden, or an internal error occurred 
        /// (Status InternalServerError)
        /// </returns>
        [HttpPut("{postId}")]
        [SwaggerOperation("Updates a post in the database")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<PostDto>> UpdatePost(string postId, [FromBody] PostDto updatedPost)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

                updatedPost.Id = postId;
                string? accessToken = await HttpContext.GetTokenAsync("access_token");

                #pragma warning disable CS8604 // Possible null reference argument
                var response = await _postService.UpdatePost(postId, currentUserId, updatedPost, accessToken);
                #pragma warning restore CS8604 // Possible null reference argument

                if (response.Result)
                {
                    return Ok(response);
                }
                else if (response.ErrorCode.Equals(ErrorCodes.UsernameTokenError))
                {
                    return BadRequest(response);
                }
                else if (response.ErrorCode.Equals(ErrorCodes.UserNotAuthorizedToCreatePost))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new PostResponseDTO
                    {
                        ErrorDesc = response.ErrorDesc,
                        ErrorCode = response.ErrorCode,
                        Result = false
                    });
                }
                else if (response.ErrorCode.Equals(ErrorCodes.PostNotFound))
                {
                    return NotFound(new PostResponseDTO
                    {
                        ErrorDesc = response.ErrorDesc,
                        ErrorCode = response.ErrorCode,
                        Result = false
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new PostResponseDTO
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
                                  new PostResponseDTO
                                  {
                                      ErrorDesc = ex.Message,
                                      ErrorCode = ErrorCodes.InternalServerError,
                                      Result = false
                                  });
            }
        }

        /// <summary> 
        /// Gets all posts based on currentUserId
        /// </summary>
        /// <returns> 
        /// Ok, BadRequest, Unauthorized, Forbidden, or an internal error occurred (Status InternalServerError)
        /// </returns>
        [HttpGet("getposts-byuser")]
        [SwaggerOperation("Gets all posts based on currentUserId")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetAllPostsByUserId()
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

                string? accessToken = await HttpContext.GetTokenAsync("access_token");

                #pragma warning disable CS8604 // Possible null reference argument
                var response = await _postService.GetAllPostsByUserId(currentUserId, accessToken);
                #pragma warning restore CS8604 // Possible null reference argument

                if (response.ErrorCode.Equals(ErrorCodes.Success))
                {
                    return Ok(response);
                }
                else if (response.ErrorCode.Equals(ErrorCodes.UsernameTokenError))
                {
                    return BadRequest(response);
                }
                else if (response.ErrorCode.Equals(ErrorCodes.UserNotAuthorizedToCreatePost))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new PostResponseDTO
                    {
                        ErrorDesc = response.ErrorDesc,
                        ErrorCode = response.ErrorCode,
                        Result = false
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new PostResponseDTO
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
                                  new PostResponseDTO
                                  {
                                      ErrorDesc = ex.Message,
                                      ErrorCode = ErrorCodes.InternalServerError,
                                      Result = false
                                  });
            }
        }
    }
}