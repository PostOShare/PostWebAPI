using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PostWebApiCommon;
using PostWebApiCommon.Helpers;
using PostWebApiCommon.Models.DTO.Request;
using PostWebApiCommon.Models.DTO.Response;
using System.Net.Http.Json;
using System.Security.Claims;
using PostDto = PostWebApiCommon.Models.DTO.Request.PostDto;

namespace PostWebApiService.Services
{
    public class PostService : IPostService
    {
        private readonly IMongoCollection<PostDto> _postsCollection;
        private string _identityAPIBaseUrl;
        private string _validateAccessTokenUri;
        private IHttpClientHelper _httpClientHelper;
        private readonly ILogger<PostService> _logger;

        public PostService(IMongoDatabase database, IConfiguration configuration, IHttpClientHelper httpClientHelper, ILogger<PostService> logger)
        {
            _postsCollection = database.GetCollection<PostDto>("Posts");
            _identityAPIBaseUrl = configuration[Constants.IdentityAPIBaseUrl];
            _validateAccessTokenUri = configuration[Constants.ValidateAccessTokenEndpoint];
            _httpClientHelper = httpClientHelper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO> CreatePost(PostDto postDto)
        {
            try
            {
                _logger.LogInformation("Route: {method}, User: {username} | Creating post.",
                                   Constants.CreatePostRoute, postDto.PartitionKey);

                postDto.Id = $"post_{Guid.NewGuid()}";
                await _postsCollection.InsertOneAsync(postDto);
                
            }
            catch (Exception ex)
            {                
                _logger.LogError("Route: {method}, User: {username} | Exception occurred while saving post: {exception}",
                                   Constants.CreatePostRoute, postDto.PartitionKey, ex);
                throw;
            }

            _logger.LogInformation("Route: {method}, User: {username} | Post created successfully",
                                                   Constants.CreatePostRoute, postDto.PartitionKey);

            return new BaseResponseDTO
            {
                Result = true,
                ErrorCode = ErrorCodes.Success,
                ErrorDesc = string.Empty
            };
        }

        public async Task<BaseResponseDTO> DeletePost(string postId, string currentUserId)
        {
            try
            {
                _logger.LogInformation("Route: {method}, Post Id: {postId} | Validating post data",
                                                   Constants.DeletePostRoute, postId);

                var filter = Builders<PostDto>.Filter.Eq(p => p.Id, postId);
                var post = await _postsCollection.Find(filter).FirstOrDefaultAsync();

                if (post == null)
                {
                    _logger.LogInformation("Route: {method}, Post Id: {postId} | Post not found",
                                                   Constants.DeletePostRoute, postId);

                    return new BaseResponseDTO
                    {
                        Result = false,
                        ErrorCode = ErrorCodes.PostNotFound,
                        ErrorDesc = "Post not found."
                    };
                }

                _logger.LogInformation("Route: {method}, Post Id: {postId}, Username: {userId} | Identifying whether user is authorized to delete the post",
                                                   Constants.DeletePostRoute, postId, currentUserId);

                if (post.PartitionKey != currentUserId)
                {
                    _logger.LogInformation("Route: {method}, Post Id: {postId}, Username: {userId} | User is not authorized to delete this post",
                                                   Constants.DeletePostRoute, postId, currentUserId);

                    return new BaseResponseDTO
                    {
                        Result = false,
                        ErrorCode = ErrorCodes.UserNotAuthorizedToDeletePost,
                        ErrorDesc = "User is not authorized to delete this post."
                    };
                }

                _logger.LogInformation("Route: {method}, Post Id: {postId} | Deleting post",
                                                   Constants.DeletePostRoute, postId);

                var deleteResult = await _postsCollection.DeleteOneAsync(filter);

                if (deleteResult.DeletedCount == 0)
                {
                    _logger.LogInformation("Route: {method}, Post Id: {postId} | Failed to delete post",
                                                   Constants.DeletePostRoute, postId);

                    return new BaseResponseDTO
                    {
                        Result = false,
                        ErrorCode = ErrorCodes.FailedToDeletePost,
                        ErrorDesc = "Failed to delete post."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Route: {method}, Post Id: {postId} | Exception occurred while deleting post: {exception}",
                                   Constants.CreatePostRoute, postId, ex);
                throw;
            }

            _logger.LogInformation("Route: {method}, Post Id: {postId} | Post deleted successfully",
                                                   Constants.DeletePostRoute, postId);

            return new BaseResponseDTO
            {
                Result = true,
                ErrorCode = ErrorCodes.Success,
                ErrorDesc = string.Empty
            };
        }
    }
}