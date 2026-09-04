using Microsoft.Extensions.Configuration;
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

        public PostService(IMongoDatabase database, IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _postsCollection = database.GetCollection<PostDto>("Posts");
            _identityAPIBaseUrl = configuration[Constants.IdentityAPIBaseUrl];
            _validateAccessTokenUri = configuration[Constants.ValidateAccessTokenEndpoint];
            _httpClientHelper = httpClientHelper;
        }

        public async Task<BaseResponseDTO> CreatePost(PostDto postDto)
        {
            try
            {
                //Validate that user token is valid
                var validateTokenRequest = new ValidateTokenRequestDTO { AccessToken = postDto.AccessToken, RefreshToken = postDto.RefreshToken };
                var validateAccessTokenResponse = await _httpClientHelper.PostAsync<ValidateTokenRequestDTO, AuthResultDTO>(_identityAPIBaseUrl, _validateAccessTokenUri, JsonContent.Create(validateTokenRequest));

                if (validateAccessTokenResponse.Result)
                {
                    postDto.Id = $"post_{Guid.NewGuid()}";
                    await _postsCollection.InsertOneAsync(postDto);
                }
                else
                {
                    return new BaseResponseDTO
                    {
                        Result = false,
                        Error = "Invalid access token/ refresh token"
                    };
                }
            }
            catch (Exception ex)
            {
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