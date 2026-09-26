using PostWebApiCommon.Models.DTO.Response;

namespace PostWebApiService.Services
{
    public interface IPostService
    {
        Task<BaseResponseDTO> CreatePost(PostWebApiCommon.Models.DTO.Request.PostDto postDto);
        Task<BaseResponseDTO> DeletePost(string postId, string currentUserId);
        Task<BaseResponseDTO> UpdatePost(string postId, string currentUserId, PostWebApiCommon.Models.DTO.Request.PostDto postDto);
    }
}