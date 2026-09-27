using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PostWebApiCommon;
using PostWebApiCommon.Helpers;
using PostWebApiCommon.Models.DTO.Response;
using PostDto = PostWebApiCommon.Models.DTO.PostDto;

namespace PostWebApiService.Services
{
    public class PostService : IPostService
    {
        private readonly IMongoCollection<PostDto> _postsCollection;
        private readonly string _identityAPIBaseUrl;
        private readonly string _validateAccessTokenUri;
        private IHttpClientHelper _httpClientHelper;
        private readonly ILogger<PostService> _logger;

        public PostService(IMongoDatabase database, IConfiguration configuration, IHttpClientHelper httpClientHelper, ILogger<PostService> logger)
        {
            _postsCollection = database.GetCollection<PostDto>("Posts");
            _identityAPIBaseUrl = configuration[Constants.IdentityAPIBaseUrl]!;
            _validateAccessTokenUri = configuration[Constants.ValidateAccessTokenEndpoint]!;
            _httpClientHelper = httpClientHelper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO> CreatePost(PostDto postDto, string currentUserId)
        {
            try
            {
                if (postDto.PartitionKey != currentUserId)
                {
                    _logger.LogInformation("Route: {method}, Post Id: {postId}, Username: {userId} | User is not authorized to create this post",
                                                   Constants.CreatePostRoute, postDto.Id, currentUserId);

                    return new BaseResponseDTO
                    {
                        Result = false,
                        ErrorCode = ErrorCodes.UserNotAuthorizedToCreatePost,
                        ErrorDesc = "User is not authorized to create this post."
                    };
                }

                _logger.LogInformation("Route: {method}, User: {username} | Creating post.",
                                   Constants.CreatePostRoute, postDto.PartitionKey);

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
                                   Constants.DeletePostRoute, postId, ex);
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

        public async Task<BaseResponseDTO> UpdatePost(string postId, string currentUserId, PostDto updatedPost)
        {
            try
            {
                _logger.LogInformation("Route: {method}, Post Id: {postId} | Validating post data",
                                                   Constants.UpdatePostRoute, postId);

                var filter = Builders<PostDto>.Filter.Eq(p => p.Id, postId);
                var post = await _postsCollection.Find(filter).FirstOrDefaultAsync();

                if (post == null)
                {
                    _logger.LogInformation("Route: {method}, Post Id: {postId} | Post not found",
                                                   Constants.UpdatePostRoute, postId);

                    return new BaseResponseDTO
                    {
                        Result = false,
                        ErrorCode = ErrorCodes.PostNotFound,
                        ErrorDesc = "Post not found."
                    };
                }

                _logger.LogInformation("Route: {method}, Post Id: {postId}, Username: {userId} | Identifying whether user is authorized to delete the post",
                                                   Constants.UpdatePostRoute, postId, currentUserId);

                if (post.PartitionKey != currentUserId)
                {
                    _logger.LogInformation("Route: {method}, Post Id: {postId}, Username: {userId} | User is not authorized to delete this post",
                                                   Constants.UpdatePostRoute, postId, currentUserId);

                    return new BaseResponseDTO
                    {
                        Result = false,
                        ErrorCode = ErrorCodes.UserNotAuthorizedToDeletePost,
                        ErrorDesc = "User is not authorized to delete this post."
                    };
                }

                _logger.LogInformation("Route: {method}, Post Id: {postId} | Updating post",
                                                   Constants.UpdatePostRoute, postId);

                var updateResult = await _postsCollection.ReplaceOneAsync(filter, updatedPost);

                if (updateResult.ModifiedCount == 0)
                {
                    _logger.LogInformation("Route: {method}, Post Id: {postId} | Failed to update post",
                                                   Constants.UpdatePostRoute, postId);

                    return new BaseResponseDTO
                    {
                        Result = false,
                        ErrorCode = ErrorCodes.FailedToUpdatePost,
                        ErrorDesc = "Failed to update post."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Route: {method}, Post Id: {postId} | Exception occurred while updating post: {exception}",
                                   Constants.UpdatePostRoute, postId, ex);
                throw;
            }

            _logger.LogInformation("Route: {method}, Post Id: {postId} | Post updated successfully",
                                                   Constants.UpdatePostRoute, postId);

            return new BaseResponseDTO
            {
                Result = true,
                ErrorCode = ErrorCodes.Success,
                ErrorDesc = string.Empty
            };
        }

        public async Task<PostResponseDTO> GetAllPostsByUserId(string currentUserId)
        {
            try
            {
                _logger.LogInformation("Route: {method} | Validating post data", Constants.GetAllPostsByUserIdRoute);

                var filter = Builders<PostDto>.Filter.Eq(p => p.PartitionKey, currentUserId);
                var posts = await _postsCollection.Find(filter).ToListAsync();

                if (posts == null || posts.Count == 0)
                {
                    _logger.LogInformation("Route: {method} | No posts found for user {userId}",
                                                   Constants.GetAllPostsByUserIdRoute, currentUserId);

                    return new PostResponseDTO
                    {
                        Result = false,
                        ErrorCode = ErrorCodes.PostNotFound,
                        ErrorDesc = "Posts not found."
                    };
                }


                _logger.LogInformation("Route: {method} | Posts for user {userId} retrieved",
                                                   Constants.GetAllPostsByUserIdRoute, currentUserId);

                return new PostResponseDTO
                {
                    Result = true,
                    ErrorCode = ErrorCodes.Success,
                    ErrorDesc = string.Empty,
                    Posts = posts
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Route: {method}| Exception occurred while retrieving posts: {exception}",
                                   Constants.GetAllPostsByUserIdRoute, ex);
                throw;
            }
        }
    }
}