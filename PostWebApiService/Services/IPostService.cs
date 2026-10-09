using PostWebApiCommon.Models.DTO.Response;
using PostDto = PostWebApiCommon.Models.DTO.PostDto;

namespace PostWebApiService.Services
{
    public interface IPostService
    {
        Task<BaseResponseDTO> CreatePost(PostDto postDto, string currentUserId, string accessToken);
        Task<BaseResponseDTO> DeletePost(string postId, string currentUserId, string accessToken);
        Task<BaseResponseDTO> UpdatePost(string postId, string currentUserId, PostDto postDto, string accessToken);
        Task<PostResponseDTO> GetAllPostsByUserId(string currentUserId, string accessToken);
    }
}