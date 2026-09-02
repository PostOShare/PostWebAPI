using PostDto = PostWebApiCommon.Models.DTO.Request.PostDto;
using PostWebApiCommon.Models.DTO.Response;
using MongoDB.Driver;

namespace PostWebApiService.Services
{
    public class PostService : IPostService
    {
        private readonly IMongoCollection<PostDto> _postsCollection;

        public PostService(IMongoDatabase database)
        {
            _postsCollection = database.GetCollection<PostDto>("Posts");
        }

        public async Task<BaseResponseDTO> CreatePost(PostDto postDto)
        {
            try
            {
                postDto.Id = $"post_{Guid.NewGuid()}";
                await _postsCollection.InsertOneAsync(postDto);                
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