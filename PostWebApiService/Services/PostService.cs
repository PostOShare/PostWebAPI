using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PostWebApiCommon;
using PostWebApiCommon.Helpers;
using PostWebApiCommon.Models.DTO.Request;
using PostWebApiCommon.Models.DTO.Response;
using System.Net.Http.Json;
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
                _logger.LogInformation("Route: {method}, User: {username} | Validating access token: {accesstoken}",
                                   Constants.CreatePostRoute, postDto.PartitionKey, postDto.AccessToken);

                //Validate that user token is valid
                var validateTokenRequest = new ValidateTokenRequestDTO { AccessToken = postDto.AccessToken, RefreshToken = postDto.RefreshToken };
                var validateAccessTokenResponse = await _httpClientHelper.PostAsync<ValidateTokenRequestDTO, AuthResultDTO>(_identityAPIBaseUrl, _validateAccessTokenUri, JsonContent.Create(validateTokenRequest));

                if (validateAccessTokenResponse.Result)
                {
                    _logger.LogInformation("Route: {method}, User: {username} | Access token is valid. Creating post.",
                                   Constants.CreatePostRoute, postDto.PartitionKey);

                    postDto.Id = $"post_{Guid.NewGuid()}";
                    await _postsCollection.InsertOneAsync(postDto);
                }
                else
                {
                    _logger.LogInformation("Route: {method}, User: {username} | Access token is invalid.",
                                   Constants.CreatePostRoute, postDto.PartitionKey);

                    return new BaseResponseDTO
                    {
                        Result = false,
                        Error = "Invalid access token/ refresh token"
                    };
                }
            }
            catch (Exception ex)
            {                
                _logger.LogError("Route: {method}, User: {username} | Exception occurred while saving post: {exception}",
                                   Constants.CreatePostRoute, postDto.PartitionKey, ex);
                throw;
            }

            return new BaseResponseDTO
            {
                Result = true,
                Error = string.Empty
            };
        }
    }
}