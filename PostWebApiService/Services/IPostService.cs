using PostWebApiCommon.Models.DTO.Response;
using PostDto = PostWebApiCommon.Models.DTO.PostDto;

namespace PostWebApiService.Services
{
    public interface IPostService
    {
        Task<BaseResponseDTO> CreatePost(PostDto postDto, string currentUserId);
        Task<BaseResponseDTO> DeletePost(string postId, string currentUserId);
        Task<BaseResponseDTO> UpdatePost(string postId, string currentUserId, PostDto postDto);
        Task<PostResponseDTO> GetAllPostsByUserId(string currentUserId);
    }
}